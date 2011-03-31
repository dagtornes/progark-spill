using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill.GameObjects
{
    public class BulletStatus : IStatus
    {
        private float radius;
        public BulletStatus(float radius) { this.radius = radius; }
        public BulletStatus() { this.radius = 1000; }

        public bool isAlive(Entity me)
        {
            return me.Physics.Position.Length() < radius;
        }
        public void kill(Entity me, Entity murderer)
        {

        }
        public Entity getKiller()
        {
            return null;
        }
    }
}
