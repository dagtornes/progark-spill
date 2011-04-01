using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill.GameObjects.Behaviours
{
    public class CreepSpawner : IBehaviour
    {
        private bool triggered = false;
        private bool bound = false;
        private float remainingTime;
        private float duration;

        private void bind(Entity me)
        {
            remainingTime = me.CombatStats.Armor;
            duration = me.CombatStats.Resistance;
            bound = true;
        }

        public void decide(Entity me, GameState environment, float timedelta, GameStateStack states)
        {
            if (!bound)
                bind(me);

            remainingTime -= timedelta;
            if (remainingTime < 0 && !triggered)
            {
                foreach (IAbility ability in me.Abilities)
                {
                    ability.levelUp();
                }
                triggered = true;
            }
            if (triggered)
            {
                duration -= timedelta;
            }
            if (duration < 0)
                me.CombatStats.Health = -1;
        }

        public IBehaviour clone()
        {
            return (CreepSpawner)MemberwiseClone();
        }
    }
}
