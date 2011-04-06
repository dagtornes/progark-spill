using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill.GameObjects
{
    /// <summary>
    /// Used for book-keeping information and for implementing different kinds of
    /// ways to determine whether to discard an Entity.
    /// </summary>
    public interface IStatus
    {
        bool isAlive(Entity me, float timedelta);
        void kill(Entity me, Entity murderer);
        Entity getKiller();
        IStatus clone();
    }
}
