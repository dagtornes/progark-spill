using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill.GameObjects.Statuses
{
    public class ExpireCheck : IStatus
    {
        public float duration;

        public ExpireCheck(float duration)
        {
            this.duration = duration;
        }

        public bool isAlive(Entity me, float timedelta)
        {
            duration -= timedelta;
            return duration >= 0;
        }

        public void kill(Entity me, Entity murderer)
        {
            ;
        }

        public Entity getKiller()
        {
            return null;
        }

        public IStatus clone()
        {
            return (ExpireCheck)MemberwiseClone();
        }
    }
}
