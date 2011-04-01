using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace progarkspill.GameObjects.Renderables
{
    public class Sprite : IRenderable
    {
        public Sprite(Texture2D texture)
        {
            this.texture = texture;
            this.position = Vector2.Zero;
            this.angle = 0.0f;
            this.origin = new Vector2(0.5f * texture.Width, 0.5f * texture.Height);
            this.depth = 0.0f;
            this.tiled = false;
            this.Scale = Vector2.One;
            this.Tint = Color.White;
        }

        public bool Tiled { get; set; }
        public float Depth { get; set; }

        public Color Tint { get; set; }

        public Vector2 Position
        {
            get;
            set;
        }

        public float Angle
        {
            get;
            set;
        }

        public Vector2 Direction
        {
            set {
                angle = (float) Math.Atan2(value.Y, value.X);
            }
        }

        public Texture2D Texture
        {
            get
            {
                return texture;
            }
        }

        public Vector2 Origin
        {
            get { return this.origin; }
        }

        public Vector2 Size
        {
            get { return new Vector2(Texture.Width, Texture.Height); }
        }

        public Vector2 Scale { get; set; }

        private bool tiled;
        private float depth;
        private Vector2 origin;
        private Vector2 position;
        private float angle;
        private Texture2D texture;

        public IRenderable clone()
        {
            return (IRenderable)MemberwiseClone();
        }
    }
}
