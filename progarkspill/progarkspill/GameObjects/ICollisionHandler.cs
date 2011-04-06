using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill.GameObjects
{
    /// <summary>
    /// Used to perform actions in the event that two Entities collide.
    /// This is not used reflexively - me.collide(you) does not mean that
    /// you.collide(me)!
    /// </summary>
    public interface ICollisionHandler
    {
        /// <summary>
        /// Affect Entities in some manner.
        /// </summary>
        /// <param name="me"></param>
        /// <param name="other"></param>
        void collide(Entity me, Entity other);
    }
}
