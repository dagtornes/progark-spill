using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedContent;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace progarkspill.GameObjects
{
    public class AutoAttack : IAbility
    {
        public AbilityStats Stats { get; set; }
        private PlayerIndex control;
        private Buttons binding;
        private bool bound = false;

        public bool triggered(Entity me, GameState environment, float timedelta)
        {
            Stats.CurrentCooldown -= timedelta;
            if (Stats.Level < 1)
                return false;
            if (!bound)
                return Stats.CurrentCooldown <= 0;
            else
            {
                bool cd = Stats.CurrentCooldown <= 0;
                return cd && (GamePad.GetState(control).IsButtonDown(binding));
            }
        }

        public void fire(Entity me, GameState environment)
        {
            
        }

        public void levelUp()
        {
            Stats.Level += 1;
        }

        public void bind(PlayerIndex control, Buttons button)
        {
            bound = true;
            binding = button;
            this.control = control;
        }
    }
}
