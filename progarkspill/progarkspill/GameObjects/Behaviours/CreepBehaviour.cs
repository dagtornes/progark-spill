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
            Vector2 direction = environment.gameObjective().Physics.Position - me.Physics.Position;
            direction.Normalize();
            me.Physics.Velocity = direction;
        }
    }
}
