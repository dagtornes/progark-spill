using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace progarkspill.GameObjects
{
    public class CreepAutoAttack : AutoAttack
    {
        public override Vector2 Orientation(Entity me, GameState environment)
        {
            Random rgen = new Random();
            Vector2 orientation = environment.Players[rgen.Next(environment.Players.Count)].Physics.Position - me.Physics.Position;
            orientation.Normalize();
            return orientation;
        }
    }
}
