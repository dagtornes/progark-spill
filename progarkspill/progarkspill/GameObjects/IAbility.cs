using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using SharedContent;

namespace progarkspill.GameObjects
{
    /// <summary>
    /// The interface implemented by abilities that "do stuff" ie. shooting,
    /// teleporting or bombing.
    /// </summary>
    public interface IAbility
    {
        /// <summary>
        /// The state associated with this ability
        /// </summary>
        AbilityStats Stats { get; set; }
        /// <summary>
        /// If this ability spawns projectiles, it copies a prototype into the world
        /// </summary>
        Entity ProjectilePrototype { get; set; }
        /// <summary>
        /// Check whether this ability should be used
        /// </summary>
        /// <param name="me">The entity to which the ability belongs</param>
        /// <param name="environment">The environment of said ability</param>
        /// <param name="timedelta">Time since last update in seconds</param>
        /// <returns>true if ability can be used, otherwise false</returns>
        bool isReady(Entity me, GameState environment, float timedelta);
        /// <summary>
        /// Cause whatever effect this ability has when used.
        /// </summary>
        /// <param name="me">The Entity which the abilities belongs to (Useful for
        /// position data)</param>
        /// <param name="environment">The environment in which Entity resides</param>
        void fire(Entity me, GameState environment);
        /// <summary>
        /// Bind this ability to an Xbox 360 controller.
        /// </summary>
        /// <param name="control">The player who gets to control this ability</param>
        /// <param name="button">The button on the controller it is bound to</param>
        void bind(PlayerIndex control, Buttons button);
        /// <summary>
        /// Level up the ability, making it more powerful.
        /// </summary>
        void levelUp();
        IAbility clone();
    }
}
