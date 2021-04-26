using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Vildmark;
using Vildmark.Graphics.Rendering;
using Vildmark.Maths;
using Vildmark.Maths.Physics;
using Vildmark.Windowing;

namespace LD48
{
    public class Game : VildmarkGame<Game>
    {
        private const float bounceAdjustSpeed = 0.1f;

        private static readonly Vector2 startPosition = new(100, 100);

        private RenderContext2D renderContext;
        private Level level;
        private PlayerEntity player;
        private DragInfo drag;

        protected override void InitializeWindowSettings(WindowSettings settings)
        {
            settings.Border = OpenTK.Windowing.Common.WindowBorder.Fixed;
            settings.Width = 1280;
            settings.Height = 720;
        }

        public override void Load()
        {
            renderContext = RenderContext2D.Create();
            level = new();

            level.AddEntity(player = new PlayerEntity(startPosition, new(50, 50)));
        }

        public override void Resize(int width, int height)
        {
            renderContext.Resize(width, height);
        }

        public override void Update(float delta)
        {
            level.Update(delta);

            renderContext.Camera.Transform.Y = level.CameraOffset;

            if (Mouse.IsMousePressed(MouseButton.Left))
            {
                drag = new(Mouse.Position, Mouse.Position);
            }
            else if (drag != null)
            {
                drag = drag with { End = Mouse.Position };

                if (Mouse.IsMouseReleased(MouseButton.Left))
                {
                    AABB2D dragAABB = GetDragAABB(drag);

                    AddPlatform(dragAABB);

                    drag = null;
                }
            }

            if (Mouse.IsMousePressed(MouseButton.Right))
            {
                foreach (var entity in GetEntitiesAtScreenPosition(Mouse.Position))
                {
                    level.RemoveEntity(entity);
                }
            }

            if (Mouse.Wheel.Y != 0)
            {
                foreach (var entity in GetEntitiesAtScreenPosition(Mouse.Position))
                {
                    entity.Bounce = MathHelper.Clamp(entity.Bounce + Mouse.Wheel.Y * bounceAdjustSpeed, 0, 1);
                }
            }

            if (Keyboard.IsKeyPressed(Keys.Tab))
            {
                player.Position = startPosition;
                player.Velocity = new Vector2();
            }
        }

        private void AddPlatform(AABB2D dragAABB)
        {
            if (dragAABB.Size.X == 0 || dragAABB.Size.Y == 0)
            {
                return;
            }

            level.AddEntity(new PlatformEntity(dragAABB.Position, dragAABB.Size));
        }

        public override void Render(float delta)
        {
            renderContext.Begin();

            level.Render(renderContext);

            if (drag is not null)
            {
                AABB2D dragAABB = GetDragAABB(drag);

                renderContext.RenderRectangle(dragAABB.Position, dragAABB.Size, Color4.Blue.ToVector());
            }

            renderContext.End();
        }

        private IEnumerable<Entity> GetEntitiesAtScreenPosition(Vector2 position)
        {
            foreach (var entity in level.Entities)
            {
                if (entity.CollisionBox.Contains(position + CameraOffset) && entity is not PlayerEntity)
                {
                    yield return entity;
                }
            }
        }

        private AABB2D GetDragAABB(DragInfo drag)
        {
            Vector2 min = MathsHelper.Min(drag.Start, drag.End);
            Vector2 max = MathsHelper.Max(drag.Start, drag.End);
            Vector2 size = max - min;

            return new AABB2D(min + CameraOffset, size);
        }

        private Vector2 CameraOffset => renderContext.Camera.Transform.Position.Xy;

        private record DragInfo(Vector2 Start, Vector2 End);
    }
}
