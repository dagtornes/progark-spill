using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace progarkspill.GameObjects.Abilities
{
    public class Blink : GravityWell
    {
        public override bool isReady(Entity me, GameState environment, float timedelta)
        {
            if (base.isReady(me, environment, timedelta))
                return true;
            return Stats.CurrentCooldown < 0 && !(me.Behaviour is Behaviours.Player);
        }

        public override void fire(Entity me, GameState environment)
        {
            Vector2 direction;
            if (me.Behaviour is Behaviours.Player) // Player, so blink in direction of cursor
            {
                direction = me.Source.Physics.Position - me.Physics.Position;
                if (direction.LengthSquared() < Stats.EffectParam * Stats.EffectParam)
                {
                    me.Physics.Position += direction;
                    me.Source.Physics.Position += direction;
                }
                else
                {
                    direction.Normalize();
                    me.Source.Physics.Position += direction * Stats.EffectParam;
                }
            }
            else
            {
                direction = me.Physics.Velocity;
                me.Physics.Position += direction * Stats.EffectParam;
            }
            Stats.CurrentCooldown = Stats.Cooldown;
        }

        public override void levelUp()
        {
            Stats.EffectParam += 50;
        }
    }
}
