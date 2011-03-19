using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace progarkspill
{
    public class GameStateStack
    {
        public LinkedList<IGameState> gamestates = new LinkedList<IGameState>();

        public GameStateStack()
        {
            push(new MainMenu(this));
        }

        public void tick(float timedelta)
        {
            IGameState state = pop();
            state.tick(timedelta);
        }

        public void push(IGameState state)
        {
            gamestates.AddFirst(state);
        }
        public IGameState pop()
        {
            IGameState state = gamestates.First.Value;
            gamestates.RemoveFirst();
            return state;
        }

        public void render(Renderer r)
        {
            gamestates.First.Value.render(r);
        }
    }
}
