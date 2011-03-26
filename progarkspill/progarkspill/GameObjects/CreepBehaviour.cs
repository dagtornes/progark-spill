using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace progarkspill.GameObjects
{
    class CreepBehaviour : IBehaviour
    {
        public void decide(Entity me, GameState environment, float timedelta, GameStateStack states)
        {
            if (me.CombatStats.CurrentCooldown <= 0)
            {
                // Find closest player and shoot, if in range
                me.CombatStats.CurrentCooldown = me.CombatStats.Cooldown;
            }

            Vector2 direction = me.Physics.Position - environment.gameObjective().Physics.Position;
            direction.Normalize();
            me.Physics.Velocity = direction;
        }
    }
}
