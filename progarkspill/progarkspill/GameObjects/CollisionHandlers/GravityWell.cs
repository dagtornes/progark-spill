using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace progarkspill.GameObjects.CollisionHandlers
{
    public class GravityWell : ICollisionHandler
    {
        private float radius;
        private float pull;

        public GravityWell(float radius, float pull)
        {
            this.radius = radius;
            this.pull = pull;
        }
        public void collide(Entity me, Entity other)
        {
            Vector2 direction = other.Physics.Position - me.Physics.Position;
            float distance = direction.Length();
            direction.Normalize();
            float pullFactor = pull * (1 - distance / radius);
            if (distance < 25)
            {
                direction.Normalize();
                other.Physics.SpeedModifier = 0;
                return;
            }
            Vector2 newSpeed = other.Physics.Velocity * other.Physics.Speed - direction * pullFactor;
            other.Physics.SpeedModifier = newSpeed.Length() / other.Physics.Speed;
            other.Physics.Velocity = newSpeed;
        }
    }
}
