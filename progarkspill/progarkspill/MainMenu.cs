using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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

        public void tick(float timedelta) {

        }

    }
}
