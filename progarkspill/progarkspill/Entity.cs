using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace progarkspill
{
    public class Entity
    {
        public Vector2 Velocity;
        public Vector2 position;
        private Sprite sprite;
        private float maxSpeed = 200;
        private ICollidable collidable = new Noncollidable();

        public Vector2 Position
        {
            get
            {
                return position;
            }
        }
        public ICollidable Collidable
        {
            get
            {
                return collidable;
            }
            set
            {
                collidable = value;
            }
        }

        public Entity(Vector2 position, Vector2 velocity, Texture2D texture)
        {
            sprite = new Sprite(texture);
            Velocity = velocity;
            this.position = position;
        }

        public Entity()
        {
            Velocity = new Vector2(0, 0);
            position = new Vector2(0, 0);
        }

        public void render(Renderer renderer)
        {
            renderer.render(sprite, position, Velocity);
        }

        public void move(float timedelta)
        {
            position += Velocity * timedelta;
        }

        public void setHeading(Vector2 direction)
        {
            System.Console.WriteLine("setHeading is called.");
            Velocity = direction * maxSpeed;
        }
        
    }
}
