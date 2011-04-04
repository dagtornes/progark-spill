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

        protected bool bound = false;
        private PlayerIndex control;
        private Buttons binding;

        public virtual bool isReady(Entity me, GameState environment, float timedelta)
        {
            Stats.CurrentCooldown -= timedelta;
            return Stats.Level > 0 && bound && Stats.CurrentCooldown < 0 && GamePad.GetState(control).IsButtonDown(binding);
        }

        public virtual void fire(Entity me, GameState environment)
        {
            Entity bomb = new Entity(ProjectilePrototype);
            Stats.CurrentCooldown = Stats.Cooldown;
            bomb.Physics.Position = me.Source.Physics.Position;
            bomb.Status = new Statuses.ExpireCheck(Stats.Duration);
            bomb.Collidable.Radius = (int) Stats.Radius;
            bomb.CollisionHandler = new CollisionHandlers.GravityWell(Stats.Radius, Stats.EffectParam);
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
            Stats.Radius *= 1.10f;
        }

        public IAbility clone()
        {
            return (GravityWell)MemberwiseClone();
        }
    }
}
