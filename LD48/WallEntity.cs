using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vildmark.Graphics.Rendering;
using Vildmark.Maths;

namespace LD48
{
    public class WallEntity : Entity
    {
        public WallEntity(float x)
            : base(new Vector2(x,-10000), new Vector2(Textures.Wall.Width * Game.PixelScale, 10000000))
        {
            IsKinematic = false;
            IsGravityAffected = false;
        }

        public override void Render(RenderContext2D renderContext)
        {
            renderContext.RenderRectangle(Position, Size, Color.ToVector(), Textures.Wall);
        }
    }
}
