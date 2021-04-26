using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vildmark;
using Vildmark.Maths;
using Vildmark.Windowing;

namespace LD48
{
    public class PlayerEntity : Entity
    {
        public float InputForce = 500;
        public float JumpForce => Level.Gravity;

        private IKeyboard keyboard;

        public PlayerEntity(Vector2 position, Vector2 size)
            : base(position, size)
        {
            keyboard = Game.Instance.Keyboard;
            Color = Color4.Red;
        }

        public override void Update(float delta)
        {
            Vector2 force = Vector2.Zero;

            if (keyboard.IsKeyDown(Keys.A))
            {
                force.X -= InputForce;
            }

            if (keyboard.IsKeyDown(Keys.D))
            {
                force.X += InputForce;
            }

            if (keyboard.IsKeyPressed(Keys.Space) && IsOnGround)
            {
                force.Y -= JumpForce * JumpForce;
            }

            if (force.LengthSquared > 0)
            {
                Velocity += force * delta;
            }
        }
    }
}
