using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill.GameObjects.Statuses
{
    /// <summary>
    /// This status is used by bullets - they die when going out of the
    /// relevant area of the game.
    /// </summary>
    public class BulletStatus : IStatus
    {
        private float radius;
        public BulletStatus(float radius) { this.radius = radius; }
        public BulletStatus() { this.radius = 1000; }

        public bool isAlive(Entity me, float timedelta)
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
        public IStatus clone()
        {
            return (BulletStatus)MemberwiseClone();
        }
    }
}
