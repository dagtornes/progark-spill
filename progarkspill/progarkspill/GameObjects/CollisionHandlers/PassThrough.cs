using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill.GameObjects.CollisionHandlers
{
    public class PassThrough : Damage
    {
        ISet<Entity> collided = new HashSet<Entity>();

        public override void collide(Entity me, Entity other)
        {
            if (collided.Contains(other))
                return;
            base.collide(me, other);
            collided.Add(other);
            me.CombatStats.Health = 1;
        }
    }
}
