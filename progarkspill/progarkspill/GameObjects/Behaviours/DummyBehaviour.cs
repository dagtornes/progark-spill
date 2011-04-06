using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill.GameObjects.Behaviours
{
    /// <summary>
    /// This behaviour does not do anything, but implements a clone method that works for
    /// all other behaviours.
    /// </summary>
    public class DummyBehaviour : IBehaviour
    {
        public virtual void decide(Entity me, GameState environment, float timedelta, GameStateStack states)
        {
        }

        public IBehaviour clone()
        {
            return (IBehaviour)MemberwiseClone();
        }
    }
}
