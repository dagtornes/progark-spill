using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace progarkspill.GameObjects
{
    public class Player : DummyBehaviour
    {
        public PlayerIndex control;

        public Player()
        {
        }
        public Player(PlayerIndex controller)
        {
            control = controller;
        }

        public override void decide(Entity me, GameState environment, float timedelta, GameStateStack states)
        {
            // Verify connection status
            GamePadState controller = GamePad.GetState(control);

            if (!controller.IsConnected)
            {
                pauseDisconnect(states); // May want to remember which player that paused game
                return;
            }

            if (controller.IsButtonDown(Buttons.LeftTrigger))
            {
                // Trigger selected action here 
                // Crosshair is located at me.Source.Physics
            }
            setHeading(me, controller);

            // Pause menu:
            if (controller.IsButtonDown(Buttons.Start)) pauseDisconnect(states);
            if (controller.IsButtonDown(Buttons.Back)) states.pop();
        }


        public void setHeading(Entity me, GamePadState controller)
        {
            me.Physics.Velocity = (new Vector2(1, -1) * controller.ThumbSticks.Left);
        }

        public void pauseDisconnect(GameStateStack states)
        {
            states.push(new PauseMenu(states, control));
        }
    }
}
