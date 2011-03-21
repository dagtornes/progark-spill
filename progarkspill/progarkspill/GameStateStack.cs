﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace progarkspill
{
    public class GameStateStack
    {
        private List<IGameState> gamestates = new List<IGameState>();

        public GameStateStack()
        {
            push(new MainMenu(this));
        }

        public void tick(float timedelta)
        {
            gamestates[gamestates.Count - 1].tick(timedelta);

            int next = 2;
            while (next < gamestates.Count && gamestates[gamestates.Count - next].tickDown)
            {
                gamestates[gamestates.Count - next].tick(timedelta);
                next++;
            }
        }

        public void push(IGameState state)
        {
            gamestates.Add(state);
        }
        public IGameState pop()
        {
            IGameState state = gamestates[gamestates.Count-1];
            gamestates.RemoveAt(gamestates.Count-1);
            return state;
        }

        public void render(Renderer r)
        {
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
