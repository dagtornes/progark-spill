using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill.GameObjects
{
    public class BulletStatus : IStatus
    {
        public bool isAlive(Entity me)
        {
            return me.Physics.Position.Length() < 5000;
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
