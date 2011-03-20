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
        public Sprite sprite;
        private ICollidable collidable = new Noncollidable();
        public CombatStats CombatState
        {
            get;
            set;
        }
        public float MaxSpeed { get; set; }
        public IActive Status { get; set; }

        public class Alive : IActive
        {
            public bool isAlive(Entity me)
            {
                return me.CombatState.Health > 1;
            }
        }
        public class InsideMap : IActive
        {
            public bool isAlive(Entity me)
            {
                // TODO: These numbers need to come from somewhere
                return me.Position.X <= 500 && me.Position.Y < 500 &&
                    me.Position.X >= 0 && me.Position.Y >= 0;
            }
        }
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
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
    
        public Entity Source { get; set; }

        public Entity(Vector2 position, Vector2 velocity, Texture2D texture)
        {
            sprite = new Sprite(texture);
            Velocity = velocity;
            this.position = position;
            CombatState = new CombatStats();
            Status = new Alive();
        }

        public Entity()
        {
            Velocity = new Vector2(0, 0);
            position = new Vector2(0, 0);
            Status = new Alive();
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
            Velocity = direction * MaxSpeed;
        }
        
    }
}
