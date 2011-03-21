using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill.GameObjects
{
    public class BulletCollider : ICollisionHandler
    {
        public void collide(Entity me, Entity hostile)
        {
            // Turn off friendly fire
            if (me.Behavior is Player && hostile.Behavior is Player)
                return;
            // TODO: Disable friendly fire for creeps too
            int armor;
            if (me.CombatStats.DamageType == 0)
                armor = hostile.CombatStats.Armor;
            else
                armor = hostile.CombatStats.Resistance;
            float damage = me.CombatStats.Damage * (1 - armor / 1.0f);
            hostile.CombatStats.Health -= (int) damage;
            // Kill projectile after it's hit a creep
            me.CombatStats.Health = -1;
            if (hostile.CombatStats.Health < 0)
                // TODO: hostile killed by source, need some bookkeeping
                me.Stats.Kills += 1;
        }
    }
}
