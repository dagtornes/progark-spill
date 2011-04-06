using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill.GameObjects.Abilities
{
    public class LaserAttack : AutoAttack
    {
        public override void levelUp()
        {
            base.levelUp();
            Stats.Damage += 5;
        }
    }
}
