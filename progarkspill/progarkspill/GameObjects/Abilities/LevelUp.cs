using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace progarkspill.GameObjects.Abilities
{
    public class LevelUp : IAbility
    {
        public SharedContent.AbilityStats Stats { get; set; }

        public Entity ProjectilePrototype { get; set; }
        private IAbility target;
        private PlayerIndex control;
        private Buttons button;

        public LevelUp(IAbility other)
        {
            target = other;
        }
        public bool isReady(Entity me, GameState environment, float timedelta)
        {
            return me.Stats.Levels > 0 && GamePad.GetState(control).IsButtonDown(button);
        }

        public void fire(Entity me, GameState environment)
        {
            me.Stats.Levels -= 1;
            target.levelUp();
        }

        public void bind(PlayerIndex control, Buttons button)
        {
            this.control = control;
            this.button = button;
        }
        // Can't level up the level up button yo.
        public void levelUp()
        {

        }

        public IAbility clone()
        {
            return (LevelUp)MemberwiseClone();
        }
    }
}
