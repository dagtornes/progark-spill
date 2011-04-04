using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace progarkspill.GameObjects.Abilities
{
    public class GravityWell : IAbility
    {
        public SharedContent.AbilityStats Stats { get; set; }

        public Entity ProjectilePrototype { get; set; }

        private bool bound = false;
        private PlayerIndex control;
        private Buttons binding;
        private float radius = 400;
        private float duration = 15;
        private float pull = 500;

        public bool isReady(Entity me, GameState environment, float timedelta)
        {
            Stats.CurrentCooldown -= timedelta;
            return Stats.Level > 0 && bound && Stats.CurrentCooldown < 0 && GamePad.GetState(control).IsButtonDown(binding);
        }

        public void fire(Entity me, GameState environment)
        {
            Entity bomb = new Entity(ProjectilePrototype);
            Stats.CurrentCooldown = Stats.Cooldown;
            bomb.Physics.Position = me.Source.Physics.Position;
            bomb.Status = new Statuses.ExpireCheck(duration);
            bomb.Collidable.Radius = (int) radius;
            bomb.CollisionHandler = new CollisionHandlers.GravityWell(radius, pull);
            environment.addGameObject(bomb);
        }

        public void bind(Microsoft.Xna.Framework.PlayerIndex control, Microsoft.Xna.Framework.Input.Buttons button)
        {
            bound = true;
            this.control = control;
            this.binding = button;
        }

        public void levelUp()
        {
            Stats.Level += 1;
            radius *= 1.10f;
        }

        public IAbility clone()
        {
            return (GravityWell)MemberwiseClone();
        }
    }
}
