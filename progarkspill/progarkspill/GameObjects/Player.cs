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

            me.CombatStats.CurrentCooldown -= timedelta; // Auto-attack cooldown

            if (!controller.IsConnected)
            {
                pauseDisconnect(states); // May want to remember which player that paused game
                return;
            }

            selectAction(controller);
            if (controller.IsButtonDown(Buttons.LeftTrigger))
            {
                // Trigger selected action here 
                // Crosshair is located at me.Source.Physics
            }

            if (controller.IsButtonDown(Buttons.RightTrigger))
            {
                shoot(me, environment, eq);
            }
            setHeading(me, controller);
        }

        public void shoot(Entity me, GameState environment, EventQueue eq)
        {
            // TODO: Make this use EventQueue instead of GameState directly when it's done?
            if (me.CombatStats.CurrentCooldown <= 0)
            {
                Entity projectile = new Entity();
                projectile.Source = me;
                projectile.Physics.Speed = me.CombatStats.ProjectileVelocity;
                projectile.Physics.Velocity = me.Physics.Orientation;
                projectile.Physics.Position = me.Physics.Position;
                projectile.Behavior = new BulletBehaviour();
                projectile.Collidable = new HitCircle(15);
                projectile.CollisionHandler = new BulletCollider();
                projectile.CombatStats = me.CombatStats;
                environment.addProjectile(projectile);
                me.CombatStats.CurrentCooldown = me.CombatStats.Cooldown;
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
            states.push(new PauseMenu(states, control));
        }
    }
}
