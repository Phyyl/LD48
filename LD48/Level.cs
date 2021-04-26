using LD48;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Vildmark.Graphics.Rendering;
using Vildmark.Maths;
using Vildmark.Maths.Physics;

namespace LD48
{
    public class Level
    {
        private const float platformFriction = 0.1f;

        private readonly List<Entity> entities = new();

        public IEnumerable<Entity> Entities => entities.ToArray();

        public float Gravity => 200;

        public float CameraSpeed { get; private set; } = 50;

        public float CameraOffset { get; private set; }

        public void AddEntity(Entity entity)
        {
            entity.Level = this;
            entities.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            entities.RemoveAll(e => e == entity);
        }

        public void Update(float delta)
        {
            foreach (var entity in entities)
            {
                entity.Update(delta);
            }

            foreach (var entity in entities)
            {
                UpdatePhysics(entity, delta);
            }

            CameraOffset += CameraSpeed * delta;
        }

        public void Render(RenderContext2D renderContext)
        {
            foreach (var entity in entities)
            {
                entity.Render(renderContext);
            }
        }

        private void UpdatePhysics(Entity entity, float delta)
        {
            if (entity.IsGravityAffected)
            {
                entity.Velocity.Y += Gravity * delta;
            }

            if (entity.Velocity.LengthSquared == 0)
            {
                return;
            }

            if (entity.IsCollidable)
            {
                Vector2 movement = entity.Velocity * delta;
                List<EntityCollision> previousCollisions = new();

                for (int i = 0; i < 2 && movement.Length > 0 && (i == 0 || previousCollisions.Count > 0); i++)
                {
                    Vector2 currentMovement = movement;
                    float friction = previousCollisions.Count > 0 ? platformFriction : 0;
                    float inverseFriction = 1 - friction * delta;

                    EntityCollision[] closestCollisions = GetClosestCollisions(entity, movement);

                    if (closestCollisions?.Length > 0)
                    {
                        EntityCollision entityCollision = closestCollisions[0];

                        currentMovement = entityCollision.Movement;

                        movement -= currentMovement;
                        movement *= Vector2.One - entityCollision.Normal.Abs();

                        HandleCollisions(entity, closestCollisions);

                        previousCollisions.AddRange(closestCollisions);
                    }

                    entity.Position += currentMovement * inverseFriction;
                    entity.Velocity *= inverseFriction;
                }

                entity.IsOnGround = previousCollisions.Any(c => c.Face == AABBFace.Top);
            }
            else
            {
                entity.Position += entity.Velocity * delta;
            }

            EntityCollision ExecuteCollision(Vector2 movement, Entity other)
            {
                var collision = CollisionDetection.IntersectMovingAABBToAABB(entity.CollisionBox, movement, other.CollisionBox);

                if (collision == null)
                {
                    return null;
                }

                return new EntityCollision(collision, other);
            }

            EntityCollision[] GetClosestCollisions(Entity entity, Vector2 movement)
            {
                return entities
                    .Where(e => e != entity && e.IsCollidable)
                    .Select(e => ExecuteCollision(movement, e))
                    .Where(e => e is not null)
                    .GroupBy(c => c.Movement.Length)
                    .OrderBy(g => g.Key)
                    .FirstOrDefault()
                    ?.ToArray();
            }
        }

        private void HandleCollisions(Entity entity, EntityCollision[] collisions)
        {
            if (collisions is null || collisions.Length == 0)
            {
                return;
            }

            AABB2DIntersectionResult collision = collisions[0];

            if (collision.Movement.LengthSquared == 0 && collisions.Any(c => c.Face == AABBFace.Top) && entity.IsOnGround)
            {
                entity.Velocity.Y = 0;
                return;
            }

            float bounce = collisions.Average(c => c.Other.Bounce);

            switch (collision.Face)
            {
                case AABBFace.Left:
                    entity.Velocity.X *= -bounce;
                    break;
                case AABBFace.Right:
                    entity.Velocity.X *= -bounce;
                    break;
                case AABBFace.Bottom:
                    entity.Velocity.Y *= -bounce;
                    break;
                case AABBFace.Top:
                    entity.Velocity.Y *= -bounce;
                    break;
            }
        }

        private class EntityCollision : AABB2DIntersectionResult
        {
            public new Entity Other { get; }

            public EntityCollision(Vector2 position, Vector2 movement, AABBFace face, Entity other)
                : base(position, movement, face, other.CollisionBox)
            {
                Other = other;
            }

            public EntityCollision(AABB2DIntersectionResult collision, Entity other)
                : this(collision.Position, collision.Movement, collision.Face, other)
            {
            }
        }
    }
}
