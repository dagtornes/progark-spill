using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace progarkspill.GameObjects
{   // Special collidable essentially for noncollidables
    class Noncollidable : ICollidable
    {
        public int Radius
        {
            get { return 0; }
            set { }
        }
        public bool contains(Entity me, Vector2 point)
        {
            return false;
        }
        public bool intersects(Entity me, Entity other)
        {
            return false;
        }
        public bool rayTrace(Entity me, Vector2 origin, Vector2 ray)
        {
            return false;
        }
    }
}
