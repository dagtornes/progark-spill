using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill.GameObjects
{
    public interface IBehaviour
    {
        void decide(Entity me, GameState environment, float timedelta, GameStateStack states);
    }
}
