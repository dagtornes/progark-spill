using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedContent;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace progarkspill.GameObjects
{
    public class AutoAttack : IAbility
    {
        public AbilityStats Stats { get; set; }
        public Entity ProjectilePrototype { get; set; }

        private PlayerIndex control;
        private Buttons binding;
        private bool bound = false;

        public virtual bool triggered(Entity me, GameState environment, float timedelta)
        {
            Stats.CurrentCooldown -= timedelta;
            if (Stats.Level < 1)
                return false;
            if (!bound)
                return Stats.CurrentCooldown <= 0;
            else
            {
                bool cd = (Stats.CurrentCooldown <= 0);
                return cd && (GamePad.GetState(control).IsButtonDown(binding));
            }
        }

        public virtual Vector2 Orientation(Entity me, GameState environment)
        {
            if (!bound)
                return me.Physics.Orientation;
            Vector2 orientation =  me.Source.Physics.Position - me.Physics.Position;
            orientation.Normalize();
            if (orientation == Vector2.Zero)
                orientation = me.Physics.Orientation;
            return orientation;

        }

        public void fire(Entity me, GameState environment)
        {
            Stats.CurrentCooldown = Stats.Cooldown;
            Entity projectile = new Entity(ProjectilePrototype);
            projectile.Physics.Position = me.Physics.Position;
            projectile.Physics.Velocity = Orientation(me, environment);
            projectile.Physics.Speed = Stats.ProjectileVelocity;
            projectile.CombatStats.Damage = Stats.Damage;
            projectile.CombatStats.DamageType = Stats.DamageType;
            projectile.Source = me;
            environment.addGameObject(projectile);
        }

        public void levelUp()
        {
            Stats.Level += 1;
        }

        public void bind(PlayerIndex control, Buttons button)
        {
            bound = true;
            binding = button;
            this.control = control;
        }

        public IAbility clone()
        {
            AutoAttack cloned = (AutoAttack)MemberwiseClone();
            cloned.Stats = Stats.clone();
            return cloned;
        }
    }
}
