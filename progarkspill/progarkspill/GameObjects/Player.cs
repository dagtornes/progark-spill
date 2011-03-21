using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace progarkspill.GameObjects
{
    public class Player : IBehaviour
    {
        PlayerIndex control;
        Buttons action = Buttons.A;

        public Player(PlayerIndex controller)
        {
            control = controller;
        }

        public void decide(Entity me, GameState environment, float timedelta, GameStateStack states, EventQueue eq)
        {
            // Verify connection status
            GamePadState controller = GamePad.GetState(control);
            if (!controller.IsConnected)
            {
                pauseDisconnect(states); // May want to remember which player that paused game
                return;
            }
            me.CombatStats.CurrentCooldown -= timedelta; // Auto-attack cooldown

            selectAction(controller);
            if (controller.IsButtonDown(Buttons.LeftTrigger))
            {
                // Trigger selected action here 
                // Crosshair is located at me.Source.Physics
            }

            if (controller.IsButtonDown(Buttons.RightTrigger))
            {
                shoot(me, eq);
            }
            setHeading(me, controller);
        }

        public void shoot(Entity me, EventQueue eq)
        {
            // TODO: Make this use EventQueue instead of GameState directly when it's done?
            if (me.CombatStats.CurrentCooldown <= 0)
            {
                Vector2 direction = me.Physics.Orientation;
            }
            
        }
        public void setHeading(Entity me, GamePadState controller)
        {
            me.Physics.Velocity = (new Vector2(1, -1) * controller.ThumbSticks.Left);
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
