using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace progarkspill
{
    public interface IRenderable
    {
        Vector2 Origin { get; }
        Texture2D Texture { get; }
        float Depth { get; set; }
        bool Tiled { get; set; }
        Vector2 Size { get; }
        Vector2 Scale { get; set; }
    }
}
