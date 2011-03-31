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
                bool cd = Stats.CurrentCooldown <= 0;
                return cd && (GamePad.GetState(control).IsButtonDown(binding));
            }
        }

        public void fire(Entity me, GameState environment)
        {
            Entity projectile = new Entity(ProjectilePrototype);
            projectile.Physics.Position = me.Physics.Position;
            projectile.Physics.Velocity = me.Physics.Position - me.Source.Physics.Position;
            projectile.Physics.Velocity.Normalize();
            if (projectile.Physics.Velocity == Vector2.Zero)
                projectile.Physics.Velocity = me.Physics.Orientation;
            projectile.CombatStats.Damage = Stats.Damage;
            projectile.CombatStats.DamageType = Stats.DamageType;
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
