using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace progarkspill
{
    class Sprite : IRenderable
    {
        public Sprite(Texture2D texture)
        {
            this.texture = texture;
        }

        public Vector2 Position
        {
            get
            {
                return new Vector2(100, 100);
            }
        }

        public Texture2D getTexture()
        {
            return this.texture;
        }

        private Texture2D texture;
    }
}
