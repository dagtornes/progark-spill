using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace progarkspill
{
    class HitCircle : ICollidable
    {
        public int Radius { get; set; }

        public HitCircle(int radius)
        {
            Radius = radius;
        }

        public bool contains(Entity me, Vector2 point)
        {
            Vector2 difference = me.Position - point;
            return difference.LengthSquared() < this.Radius * this.Radius;
        }

        public bool intersects(Entity me, Entity other)
        {
            if (other.Collidable is HitCircle)
            {
                Vector2 difference = me.Position - other.Position;
                return difference.LengthSquared() < this.Radius * this.Radius +
                    other.Collidable.Radius * other.Collidable.Radius;
            }
            else
            {
                return false;
            }
        }
        // Implement check: Does ray (Direction vector) coming from origin hit me?
        public bool rayTrace(Entity me, Vector2 origin, Vector2 ray)
        {
            return false;
        }
    }
}
