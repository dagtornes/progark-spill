using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace progarkspill.GameObjects
{
    public class Physics
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; } 
        public Vector2 Orientation { get; set; }
        public float Speed { get; set; }

        public Physics(float Speed)
        {
            Position = new Vector2(0, 0);
            Velocity = new Vector2(0, 0);
            Orientation = Vector2.UnitX;
            this.Speed = Speed;
        }
    }
}
