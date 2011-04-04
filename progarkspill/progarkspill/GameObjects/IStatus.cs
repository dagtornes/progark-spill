using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill.GameObjects
{
    public interface IStatus
    {
        bool isAlive(Entity me, float timedelta);
        void kill(Entity me, Entity murderer);
        Entity getKiller();
        IStatus clone();
    }
}
