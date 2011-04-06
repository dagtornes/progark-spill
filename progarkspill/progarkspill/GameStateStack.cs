using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace progarkspill
{
    public class GameStateStack
    {
        private List<IGameState> gamestates = new List<IGameState>();

        public GameStateStack()
        {
            //push(new MainMenu(this));
        }
        /// <summary>
        /// Update the current gamestate.
        /// </summary>
        /// <param name="timedelta">Time since last update.</param>
        public void tick(float timedelta)
        {
            if (isEmpty()) return;

            gamestates[gamestates.Count - 1].tick(timedelta);

            int next = 2;
            while (next < gamestates.Count && gamestates[gamestates.Count - next].tickDown)
            {
                gamestates[gamestates.Count - next].tick(timedelta);
                next++;
            }
        }
        /// <summary>
        /// Take a look at which gamestate that will be updated next.
        /// </summary>
        /// <returns></returns>
        public IGameState peek()
        {
            return gamestates[gamestates.Count - 1];
        }
        /// <summary>
        /// Add a gamestate.
        /// </summary>
        /// <param name="state"></param>
        public void push(IGameState state)
        {
            gamestates.Add(state);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if no more states left, otherwise false</returns>
        public bool isEmpty()
        {
            return gamestates.Count <= 0;
        }
        /// <summary>
        /// Pop current running game state
        /// </summary>
        /// <returns></returns>
        public IGameState pop()
        {
            if (isEmpty()) return null;

            IGameState state = gamestates[gamestates.Count-1];
            
            gamestates.RemoveAt(gamestates.Count-1);
            return state;
        }
        /// <summary>
        /// Ask relevant gamestate to draw itself.
        /// </summary>
        /// <param name="r">Renderer to use</param>
        public void render(Renderer r)
        {
            if (isEmpty()) return;

            gamestates[gamestates.Count - 1].render(r);

            int next = 2;
            while (next < gamestates.Count && gamestates[gamestates.Count - next].renderDown)
            {
                gamestates[gamestates.Count - next].render(r);
                next++;
            }
        }
    }
}
