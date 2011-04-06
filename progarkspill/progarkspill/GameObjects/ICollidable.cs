using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace progarkspill.GameObjects
{
    /// <summary>
    /// Collision detection for some geometric shape.
    /// </summary>
    public interface ICollidable
    {
        /// <summary>
        /// Check whether this shape contains a point
        /// </summary>
        /// <param name="me">The Entity that has the position of the shape</param>
        /// <param name="point">The point to check</param>
        /// <returns>true if point is in shape, otherwise false</returns>
        bool contains(Entity me, Vector2 point);
        /// <summary>
        /// Check whether two Entities intersect each other using ICollidable
        /// </summary>
        /// <param name="me">Location data of this ICollidable</param>
        /// <param name="other">Location data of potential collidee</param>
        /// <returns>true if params intersect</returns>
        bool intersects(Entity me, Entity other);
        /// <summary>
        /// Check whether I am intersected by a line
        /// </summary>
        /// <param name="me">Location data of this ICollidable</param>
        /// <param name="origin">Origin of line</param>
        /// <param name="ray">Direction of line</param>
        /// <returns></returns>
        bool rayTrace(Entity me, Vector2 origin, Vector2 ray);
        /// <summary>
        /// Radius of this ICollidable (Set to 0 if that is a meaningless
        /// thing for this ICollidable to have);
        /// </summary>
        int Radius { get; set; }
        ICollidable clone();
    }
}
