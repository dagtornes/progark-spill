using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using SharedContent;

namespace progarkspill.GameObjects
{
    public interface IAbility
    {
        AbilityStats Stats { get; set; }

        bool triggered(Entity me, GameState environment, float timedelta);
        void fire(Entity me, GameState environment);
        void bind(PlayerIndex control, Buttons button);
        void levelUp();
        IAbility clone();
    }
}
