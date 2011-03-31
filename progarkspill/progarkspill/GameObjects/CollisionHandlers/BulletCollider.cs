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
            if (me.Behaviour is Player && hostile.Behaviour is Player)
                return;
            // TODO: Disable friendly fire for creeps too
            int armor;
            if (me.CombatStats.DamageType == 0)
                armor = hostile.CombatStats.Armor;
            else
                armor = hostile.CombatStats.Resistance;
            float damage = me.CombatStats.Damage * (1 - armor / 1.0f);
            hostile.CombatStats.Health -= (int) me.CombatStats.Damage;
            // Kill projectile after it's hit a creep
            me.CombatStats.Health = -1;
            if (hostile.CombatStats.Health < 0)
            {
                me.Stats.Kills += 1;
                hostile.Status.kill(hostile, me);
            }
        }
    }
}
