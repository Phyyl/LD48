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
    public class PlatformEntity : Entity
    {
        public PlatformEntity(Vector2 position, float width)
            : base(position, new(width, Textures.PlatformCenter.Height * Game.PixelScale))
        {
            IsKinematic = false;
            IsGravityAffected = false;
        }

        public override void Render(RenderContext2D renderContext)
        {
            float leftWidth = Textures.PlatformLeft.Width * Game.PixelScale;
            float centerWidth = Size.X - (Textures.PlatformLeft.Width + Textures.PlatformRight.Width) * Game.PixelScale;

            renderContext.RenderRectangle(Position, Textures.PlatformLeft.Size * Game.PixelScale, Color.ToVector(), Textures.PlatformLeft);
            renderContext.RenderRectangle(Position + new Vector2(leftWidth, 0), new Vector2(centerWidth, Textures.PlatformCenter.Height * Game.PixelScale), Color.ToVector(), Textures.PlatformCenter);
            renderContext.RenderRectangle(Position + new Vector2(leftWidth + centerWidth, 0), Textures.PlatformRight.Size * Game.PixelScale, Color.ToVector(), Textures.PlatformRight);
        }
    }
}
