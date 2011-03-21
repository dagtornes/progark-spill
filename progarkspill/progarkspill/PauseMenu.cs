using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace progarkspill
{
    class PauseMenu : IGameState
    {
        private GameStateStack owner;
        private PlayerIndex initiated;

        public PauseMenu(GameStateStack owner, PlayerIndex initiated)
        {
            this.owner = owner;
            this.initiated = initiated;
        }

        public void render(Renderer r)
        {
            r.begin(r.Screenspace);

            r.end();
        }

        public void tick(float timedelta)
        {
            GamePadState inputstate = GamePad.GetState(initiated);
            if (inputstate.IsButtonDown(Buttons.Back))
            {
                owner.pop();
            }
        }

        public bool renderDown
        {
            get { return true; }
        }

        public bool tickDown
        {
            get { return false; }
        }
    }
}
