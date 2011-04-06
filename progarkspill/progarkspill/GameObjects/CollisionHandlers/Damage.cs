using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill.GameObjects.CollisionHandlers
{
    /// <summary>
    /// Possibly redundant - does the same thing as Bullet but used by slightly different
    /// entities.
    /// </summary>
    public class Damage : ICollisionHandler
    {
        public virtual void collide(Entity me, Entity other)
        {
            int resistance;
            if (me.CombatStats.DamageType == 0)
                resistance = other.CombatStats.Armor;
            else
                resistance = other.CombatStats.Resistance;
            other.CombatStats.Health -= (int) (me.CombatStats.Damage * (1 - resistance / 100.0));
            if (other.CombatStats.Health <= 0)
                other.Status.kill(other, me.Source);
            me.CombatStats.Health = -1; // Kill self on crash
        }
    }
}
