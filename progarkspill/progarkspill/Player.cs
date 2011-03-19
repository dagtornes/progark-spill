using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace progarkspill
{
    class Player
    {
        PlayerIndex control;
        Ship hero; 
        Buttons action = Buttons.A;
        Dictionary<Buttons, Ability> actions; // Where to get this from?
       
        public Player(PlayerIndex controller)
        {
            control = controller;
            hero = new Ship();
        }

        public void update(GameTime timedelta, EventQueue eq, GameStateStack states)
        {
            // Verify connection status
            GamePadState controller = GamePad.GetState(control);
            if (!controller.IsConnected)
            {
                pauseDisconnect(states); // May want to remember which player that paused game
                return;
            }
            selectAction(controller);
            if (controller.IsButtonDown(Buttons.LeftTrigger))
            {
                Ability triggered;
                if (actions.TryGetValue(action, out triggered))
                    // Player aims with right trigger
                    triggered.triggerIt(controller.ThumbSticks.Right, eq, hero); 
            }
            setHeading(controller);
        }

        public void setHeading(GamePadState controller)
        {
            hero.Velocity = controller.ThumbSticks.Left;
            hero.Velocity.X *= hero.maxSpeed;
            hero.Velocity.Y *= hero.maxSpeed;
        }

        public void selectAction(GamePadState controller)
        {
            // LT triggers an action chosen at some point by using one of A, B, X, Y
            if (controller.IsButtonDown(Buttons.A))
                action = Buttons.A;
            if (controller.IsButtonDown(Buttons.B))
                action = Buttons.B;
            if (controller.IsButtonDown(Buttons.X))
                action = Buttons.X;
            if (controller.IsButtonDown(Buttons.Y))
                action = Buttons.Y;
        }
        public void pauseDisconnect(GameStateStack states)
        {

        }
    }
}
