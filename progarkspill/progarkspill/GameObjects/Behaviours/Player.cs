using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace progarkspill.GameObjects.Behaviours
{
    public class Player : DummyBehaviour
    {
        public PlayerIndex control;

        public override void decide(Entity me, GameState environment, float timedelta, GameStateStack states)
        {
            // Verify connection status
            GamePadState controller = GamePad.GetState(control);

            if (!controller.IsConnected)
            {
                pauseDisconnect(states); // May want to remember which player that paused game
                return;
            }

            setHeading(me, controller);

            // Pause menu:
            if (Controller.RecentlyPressed(control, Buttons.Start, 0.5f)) pauseDisconnect(states);
            if (Controller.RecentlyPressed(control, Buttons.Back, 0.5f)) states.pop();
        }


        public void setHeading(Entity me, GamePadState controller)
        {
            me.Physics.Orientation = me.Source.Physics.Position - me.Physics.Position;
            me.Physics.Velocity = (new Vector2(1, -1) * controller.ThumbSticks.Left);
        }

        public void pauseDisconnect(GameStateStack states)
        {
            states.push(new PauseMenu(states, control));
        }
    }
}
