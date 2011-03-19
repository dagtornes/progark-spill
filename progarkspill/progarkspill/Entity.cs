using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace progarkspill
{
    class Entity
    {
        public Vector2 Velocity;
        public Vector2 position;

        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        public Entity(Vector2 position, Vector2 velocity)
        {
            Velocity = velocity;
            this.position = position;
        }

        public Entity()
        {
            Velocity = new Vector2(0, 0);
            position = new Vector2(0, 0);
        }

        public void move(GameTime timedelta)
        {
            position.X += Velocity.X * timedelta.ElapsedGameTime.Milliseconds;
            position.Y += Velocity.Y * timedelta.ElapsedGameTime.Milliseconds;
        }

        
    }
}
