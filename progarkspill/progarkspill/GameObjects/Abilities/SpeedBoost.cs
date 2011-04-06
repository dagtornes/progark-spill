using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill.GameObjects.Abilities
{
    /// <summary>
    /// Boosts the speed of its entitiy for a short while.
    /// </summary>
    public class SpeedBoost : GravityWell
    {
        private float remaining = 0;

        public override bool isReady(Entity me, GameState environment, float timedelta)
        {
            if (remaining > 0)
            {
                me.Physics.SpeedModifier = Stats.EffectParam;
                remaining -= timedelta;
            } 
            if (base.isReady(me, environment, timedelta))
                return true;
            return Stats.CurrentCooldown < 0 && !(me.Behaviour is Behaviours.Player);
        }

        public override void fire(Entity me, GameState environment)
        {
            Stats.CurrentCooldown = Stats.Cooldown;
            remaining = Stats.Duration;
        }


        public override void levelUp()
        {
            Stats.EffectParam += 0.1f;
        }

    }
}
