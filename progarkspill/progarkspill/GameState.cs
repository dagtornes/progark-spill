using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace progarkspill
{
    class GameState : IGameState
    {
        public void render(Renderer r)
        {

        }

        public void tick(GameTime timedelta) 
        {
 
        }

        public IGameState nextState()
        {
            throw new NotImplementedException();
        }

    }
}
