using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill.GameObjects
{
    public class CombatStats
    {
        public int Health { get; set; } 
        public int Armor { get; set; }
        public int Resistance { get; set; }
        public float Cooldown { get; set; }
        public float CurrentCooldown { get; set; }
        public float Damage { get; set; }
        public float ProjectileVelocity { get; set; }
        public int DamageType { get; set; }

        public CombatStats()
        {
            Health = 1; // All objects are alive by default
        }

        public CombatStats copy()
        {
            return (CombatStats) this.MemberwiseClone();
        }

        public static CombatStats defaultShip()
        {
            CombatStats ship = new CombatStats();
            ship.Health = 100;
            ship.Armor = 15;
            ship.Resistance = 15;
            ship.Cooldown = 0.25f;
            ship.CurrentCooldown = 0;
            ship.DamageType = 0;
            ship.Damage = 8;
            ship.ProjectileVelocity = 500;
            return ship;
        }
    }
}
