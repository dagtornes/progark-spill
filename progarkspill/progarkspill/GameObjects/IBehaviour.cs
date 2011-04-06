using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill.GameObjects
{
    /// <summary>
    /// Classes that implement this interface are for deciding what kind of actions
    /// an Entity *wants* to do, generally this means things such as setting speed and
    /// direction, or possibly deciding which abilites it wants to use. This can be used
    /// to implement Entities that home towards a target Entity, Entities that try to
    /// escape another or other patterns of movement.
    /// </summary>
    public interface IBehaviour
    {
        /// <summary>
        /// Update the intention of an Entity
        /// </summary>
        /// <param name="me">The Entity to update</param>
        /// <param name="environment">The GameState that contains this Entity</param>
        /// <param name="timedelta">Time since last update in seconds</param>
        /// <param name="states">GameStateStack that contains gameState (This is necessary to pause game)</param>
        void decide(Entity me, GameState environment, float timedelta, GameStateStack states);
        IBehaviour clone();
    }
}
