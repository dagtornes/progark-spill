using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill.GameObjects
{
    public interface IStatus
    {
        bool isAlive(Entity me);
        void kill(Entity me, Entity murderer);
        Entity getKiller();
    }
}
