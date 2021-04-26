using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vildmark.Graphics.Rendering;
using Vildmark.Maths;
using Vildmark.Maths.Physics;

namespace LD48
{
    public abstract class Entity
    {
        public Level Level;
        public Vector2 Position;
        public Vector2 Size;
        public Vector2 Velocity;
        public bool IsGravityAffected = true;
        public bool IsKinematic = true;
        public bool IsCollidable = true;
        public Color4 Color = Color4.White;
        public float Bounce;
        public float Friction = 0.5f;
        public bool IsOnGround;

        public AABB2D CollisionBox => new(Position, Size);

        protected Entity(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        public virtual void Update(float delta)
        {
        }

        public virtual void Render(RenderContext2D renderContext)
        {
            renderContext.RenderRectangle(Position, Size, Color.ToVector());
        }
    }
}
