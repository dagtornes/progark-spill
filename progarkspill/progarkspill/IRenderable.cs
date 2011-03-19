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
        Vector2 Position { get; }
        Texture2D getTexture();
    }
}
