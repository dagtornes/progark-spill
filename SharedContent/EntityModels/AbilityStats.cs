using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedContent
{
    public class AbilityStats
    {
        public String Description;
        public int DamageType;
        public float Damage;
        public float Cooldown;
        public float CurrentCooldown;
        public float ProjectileVelocity;
        public int Level;
        public float Radius;
        public float Duration;

        public AbilityStats clone()
        {
            return (AbilityStats) MemberwiseClone();
        }
    }
}
