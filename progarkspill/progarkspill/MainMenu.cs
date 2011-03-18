using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace progarkspill
{
    class MainMenu : IGameState
    {
        private GameStateStack states;
        
        public MainMenu(GameStateStack gamestates)
        {
            states = gamestates;
        }
        public void render(Renderer r) {
            
        }

        public void tick(GameTime timedelta) {

        }

    }
}
