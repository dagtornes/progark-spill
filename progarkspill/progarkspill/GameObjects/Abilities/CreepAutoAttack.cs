using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace progarkspill.GameObjects.Abilities
{
    public class CreepAutoAttack : AutoAttack
    {
        public override Vector2 Orientation(Entity me, GameState environment)
        {
            return me.Physics.Orientation;
        }
    }
}
