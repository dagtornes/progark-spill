using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace progarkspill.GameObjects.Collidables
{
    class HitCircle : ICollidable
    {
        public int Radius { get; set; }

        public bool contains(Entity me, Vector2 point)
        {
            if (Radius == 0)
                Radius = me.Renderable.Texture.Width / 2;
            Vector2 difference = me.Physics.Position - point;
            return difference.LengthSquared() < this.Radius * this.Radius;
        }

        public bool intersects(Entity me, Entity other)
        {
            if (Radius == 0)
                Radius = me.Renderable.Texture.Width / 2;

            if (other.Collidable is HitCircle)
            {
                Vector2 difference = me.Physics.Position - other.Physics.Position;
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
            float c;
            if (Radius == 0)
                Radius = me.Renderable.Texture.Width / 2;

            Vector2.Dot(ref origin, ref origin, out c);
            c -= Radius * Radius;
            float b;
            Vector2.Dot(ref origin, ref ray, out b);
            float a;
            Vector2.Dot(ref ray, ref ray, out a);

            return b * b - 4 * a * c >= 0.0f;
        }
        public ICollidable clone()
        {
            return (ICollidable)MemberwiseClone();
        }
    }
}
