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
        private Vector2 velocity;
        public Vector2 Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                if (value != Vector2.Zero)
                    value.Normalize();
                velocity = value;
            }

        }
        public Vector2 Orientation
        {
            get { return orientation; }
            set { this.orientation = value; this.orientation.Normalize(); angle = (float) Math.Atan2(value.Y, value.X); }
        }
        public float Angle { get { return angle; } }
        public float Speed { get; set; }

        public Physics(float Speed)
        {
            Position = new Vector2(0, 0);
            Velocity = new Vector2(0, 0);
            Orientation = Vector2.UnitX;
            this.Speed = Speed;
        }

        private float angle;
        private Vector2 orientation;

        public Physics clone()
        {
            return (Physics)MemberwiseClone();
        }
    }
}
