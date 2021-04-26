using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vildmark.Maths.Physics;

namespace LD48
{
    public class PlatformEntity : Entity
    {
        public PlatformEntity(Vector2 position, Vector2 size) 
            : base(position, size)
        {
            IsKinematic = false;
            IsGravityAffected = false;
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            Color = new(Bounce, Bounce, Bounce, 1);
        }
    }
}
