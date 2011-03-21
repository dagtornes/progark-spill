using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace progarkspill.GameObjects
{
    public class Entity
    {
        public IBehaviour Behavior { get; set; }
        public CombatStats CombatStats { get; set; }
        public IRenderable Renderable { get; set; }
        public IStatus Status { get; set; }
        public ICollidable Collidable { get; set; }
        public ICollisionHandler CollisionHandler { get; set; }
        public Physics Physics { get; set; }
        public Entity Source { get; set; }
        public Statistics Stats { get; set; }

        public Entity()
        {
            CombatStats = new CombatStats();
            Status = new Status();
            Collidable = new Noncollidable();
            Physics = new Physics(200);
            Stats = new Statistics();
        }
        public void move(float timedelta)
        {
            Physics.Position += Physics.Velocity * Physics.Speed * timedelta;
            if (Physics.Velocity != Vector2.Zero)
            {
                Physics.Orientation = Physics.Velocity;
                Physics.Orientation.Normalize();
            }
        }


    }
}
