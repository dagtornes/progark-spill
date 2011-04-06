using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace progarkspill.GameObjects
{
    /// <summary>
    /// This class holds all physics information for an Entity and performs updates on it.
    /// </summary>
    public class Physics
    {
        public Vector2 Position { get; set; }
        private Vector2 velocity;
        /// <summary>
        /// SpeedModifier that is multiplied in with this entities Speed to determine how
        /// far it moved. This is reset to 1 every game tick - persistent slowing effects
        /// need to lower it for each game tick they want to slow the Entity.
        /// </summary>
        public float SpeedModifier;
        /// <summary>
        /// Velocity is a normalized direction vector.
        /// </summary>
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
        /// <summary>
        /// The facing this Entity has - in most cases the same as it's Velocity, but some
        /// objects never move and statically face in some direction.
        /// </summary>
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
            this.SpeedModifier = 1;
        }

        private float angle;
        private Vector2 orientation;

        public Physics clone()
        {
            return (Physics)MemberwiseClone();
        }
    }
}
