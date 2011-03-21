using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SharedContent
{
    public class Stats
    {
        public int Health;
        public int Armor;
        public int Resistance;
        public float Cooldown;
        public float CurrentCooldown;
        public float Damage;
        public float ProjectileVelocity;
        public int DamageType;
    }
}
