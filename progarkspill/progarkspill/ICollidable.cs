using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace progarkspill
{
    public interface ICollidable
    {
        bool contains(Entity me, Vector2 point);
        bool intersects(Entity me, Entity other);
        bool rayTrace(Entity me, Vector2 origin, Vector2 ray);
        int Radius { get; set; }
    }
}
