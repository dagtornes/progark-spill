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
            me.CombatStats.CurrentCooldown -= timedelta;
            if (me.CombatStats.CurrentCooldown <= 0)
            {
                // Find closest player and shoot, if in range
                me.CombatStats.CurrentCooldown = me.CombatStats.Cooldown;
            }

            Vector2 direction = environment.gameObjective().Physics.Position - me.Physics.Position;
            direction.Normalize();
            me.Physics.Velocity = direction;
        }
    }
}
