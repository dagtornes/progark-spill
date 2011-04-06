using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace progarkspill
{
    /// <summary>
    /// This interface represents a possible state for the application to be in, and the
    /// methods that must be defined for the GameStateStack to be able to run the application
    /// in that state.
    /// </summary>
    public interface IGameState
    {
        /// <summary>
        /// Render all views of the IGameState to screen.
        /// </summary>
        /// <param name="r">The renderer to use for this process.</param>
        void render(Renderer r);
        /// <summary>
        /// Perform updates on IGameState that took place the last timedelta seconds.
        /// </summary>
        /// <param name="timedelta"></param>
        void tick(float timedelta);

        bool renderDown { get; }
        bool tickDown { get; }
    }
}
