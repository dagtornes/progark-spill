using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace progarkspill.GameObjects.Abilities
{
    /// <summary>
    /// This is used by creeps to determine where to shoot.
    /// <see cref="AutoAttack"/>
    /// </summary>
    public class CreepAutoAttack : AutoAttack
    {
        public override Vector2 Orientation(Entity me, GameState environment)
        {
            return me.Physics.Orientation;
        }
    }
}
