using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedContent
{
    public class CombatStats
    {
        public int MaxHealth;
        public int Health;

        public int Armor;
        public int Resistance;

        public float Damage;
        public int DamageType;

        public CombatStats clone()
        {
            return (CombatStats)MemberwiseClone();
        }
    }
}
