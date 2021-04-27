using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD48
{
    public class PowerupEntity : Entity
    {
        public PowerupEntity(Vector2 position, Vector2 size)
            :base(position, size)
        {
            IsCollidable = false;
            IsKinematic = false;
            IsGravityAffected = false;
            Color = Color4.Yellow;
        }
    }
}
