using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace progarkspill.Menus
{
    public class LoseMenu : IGameState
    {
        private GameStateStack stack;
        public LoseMenu(GameStateStack stack)
        {
            this.stack = stack;
        }

        public void render(Renderer r)
        {
            r.beginScreen();
            r.renderText("You lose...", 50 * Vector2.One, Color.White);
            r.end();
        }

        public void tick(float timedelta)
        {
            if (Controller.RecentlyPressed(PlayerIndex.One, Buttons.Back, 0.5f))
            {
                stack.pop();
            }
        }

        public bool renderDown
        {
            get { return false; }
        }

        public bool tickDown
        {
            get { return false; }
        }
    }
}
