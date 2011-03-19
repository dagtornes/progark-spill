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
        Entity hero; 
        Buttons action = Buttons.A;
        GameState game;
        Dictionary<Buttons, Ability> actions; // Where to get this from?

        public Player(GameState game, PlayerIndex controller)
        {
            this.control = controller;
            this.game = game;
        }
        public Player(PlayerIndex controller)
        {
            control = controller;
        }

        public Entity Hero
        {
            get { return hero; }
            set { hero = value; }
        }

        public void update(float timedelta, EventQueue eq, GameStateStack states)
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

            if (controller.IsButtonDown(Buttons.RightShoulder))
            {
                // Shoot in direction ship is facing
            }
            setHeading(controller);
        }

        public void setHeading(GamePadState controller)
        {
            hero.setHeading(controller.ThumbSticks.Left);
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
            System.Console.WriteLine("Controlled is disconnected.");
        }
    }
}
