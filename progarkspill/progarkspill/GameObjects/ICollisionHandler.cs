using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill.GameObjects
{
    public interface ICollisionHandler
    {
        void collide(Entity me, Entity other);
    }
}
