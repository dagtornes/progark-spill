using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill.GameObjects.Statuses
{
    /// <summary>
    /// This default status checks whether the object has health left, and exists within
    /// the current "interesting" area of the game.
    /// </summary>
    public class Status : IStatus
    {
        Entity killer = null;

        public bool isAlive(Entity me, float timedelta)
        {
            return me.CombatStats.Health > 0 && me.Physics.Position.LengthSquared() < 2000 * 2000;
        }
        public void kill(Entity me, Entity murderer)
        {
            if (murderer != null)
                murderer.Stats.Kills++;

            killer = murderer;
        }
        public Entity getKiller()
        {
            return killer;
        }
        public IStatus clone()
        {
            return (IStatus)MemberwiseClone();
        }
    }
}
