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
            Hero.CombatState.CurrentCooldown -= timedelta;

            selectAction(controller);
            if (controller.IsButtonDown(Buttons.LeftTrigger))
            {
                // Trigger selected action here 
            }

            if (controller.IsButtonDown(Buttons.LeftThumbstickDown))
            {
                // Shoot in direction ship is facing
                Vector2 direction = controller.ThumbSticks.Left;
                shoot(direction, eq);
            }
            setHeading(controller);
        }

        public void shoot(Vector2 direction, EventQueue eq)
        {
            // TODO: Make this use EventQueue instead of GameState directly when it's done?
            if (Hero.CombatState.CurrentCooldown <= 0)
            {
                Hero.CombatState.CurrentCooldown = Hero.CombatState.Cooldown;
                Entity bullet = new Entity();
                bullet.MaxSpeed = Hero.CombatState.ProjectileVelocity;

                bullet.setHeading(direction * new Vector2(1, -1));
                System.Console.WriteLine("Sent projectile towards " + direction + " MaxSpeed is " + bullet.MaxSpeed);
                bullet.Status = new Entity.InsideMap();
                bullet.Collidable = new HitCircle(10);
                bullet.Position = Hero.Position;
                
                game.addProjectile(bullet);
            }
            
        }
        public void setHeading(GamePadState controller)
        {
            hero.setHeading(new Vector2(1, -1) * controller.ThumbSticks.Left);
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
