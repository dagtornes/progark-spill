using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace progarkspill.GameObjects.Behaviours
{
    public class RespawnPlayer : IBehaviour
    {
        Entity playerEntity;
        float time;

        public RespawnPlayer(Entity player, float time)
        {
            playerEntity = player;
            this.time = time;
        }

        public void respawn(GameState environment)
        {
            environment.addPlayer(playerEntity);
            playerEntity.CombatStats.Health = playerEntity.CombatStats.MaxHealth;
            playerEntity.Physics.Position = 50 * Vector2.One;
        }
        public void decide(Entity me, GameState environment, float timedelta, GameStateStack states)
        {
            time -= timedelta;
            if (time < 0)
            {
                respawn(environment);
                me.CombatStats.Health = -1;
            }
        }

        public IBehaviour clone()
        {
            return this;
        }
    }
}
