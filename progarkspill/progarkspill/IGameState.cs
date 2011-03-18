using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace progarkspill
{
    interface IGameState
    {
        void render(Renderer r);

        void tick(GameTime timedelta);
    }
}
