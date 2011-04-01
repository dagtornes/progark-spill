using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace progarkspill.GameObjects.Behaviours
{
    class Creep : IBehaviour
    {
        public void decide(Entity me, GameState environment, float timedelta, GameStateStack states)
        {
            me.Physics.Velocity = environment.gameObjective().Physics.Position - me.Physics.Position;
            foreach (Entity player in environment.Players)
                if ((player.Physics.Position - me.Physics.Position).LengthSquared() < 90000) {
                    me.Physics.Velocity = player.Physics.Position - me.Physics.Position;

                }
        }
        public IBehaviour clone()
        {
            return (Creep)MemberwiseClone();
        }
    }
}
