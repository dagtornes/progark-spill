using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace progarkspill
{
    public interface IGameState
    {
        void render(Renderer r);
        
        void tick(float timedelta);

        bool renderDown { get; }
        bool tickDown { get; }
    }
}
