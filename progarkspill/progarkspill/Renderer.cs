using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace progarkspill
{
    public class Renderer
    {
        private Texture2D pixel;

        public Renderer(GraphicsDeviceManager gdm, Texture2D pixel)
        {
            this.gdm = gdm;
            this.pixel = pixel;
            Vector2 screensize = new Vector2(gdm.PreferredBackBufferWidth, gdm.PreferredBackBufferHeight);
            screenspace = new Viewport(Vector2.Zero, screensize);
            sb = new SpriteBatch(gdm.GraphicsDevice);
        }

        public void preRender()
        {
            gdm.GraphicsDevice.Clear(Color.CornflowerBlue);
        }

        /*
         * Sets the current Viewport, which will affect all renerMe() calls until a following end().
         * Viewports are mappings from a game-specific space to screen-space.
         */
        public void begin(Viewport view)
        {
            currentspace = view;
        }

        public void render(IRenderable entity, Vector2 pos, Vector2 dir)
        {
            float angle = (float)Math.Atan2(dir.Y, dir.X);
            render(entity, pos, angle);
        }

        public void render(IRenderable entity, Vector2 pos, float angle)
        {
            Vector2 render_position = currentspace.mapTo(pos, screenspace);
            Vector2 scale = currentspace.scaleTo(screenspace);
            
            sb.Begin();
            sb.Draw(entity.Texture, render_position, null, Color.White, angle, entity.Origin, scale, SpriteEffects.None, 0.0f);
            sb.End();
        }

        public void renderRect(Vector2 topleft, Vector2 bottomright, Color color)
        {
            topleft = currentspace.mapTo(topleft, screenspace);
            bottomright = currentspace.mapTo(bottomright, screenspace);

            float l = topleft.X, r = bottomright.X;
            float t = topleft.Y, b = bottomright.Y;

            sb.Begin();
            sb.Draw(this.pixel, topleft, null, color, 0.0f, Vector2.Zero, new Vector2(r - l, 1), SpriteEffects.None, 0.0f);
            sb.Draw(this.pixel, topleft + new Vector2(0, b - t), null, color, 0.0f, Vector2.Zero, new Vector2(r - l, 1), SpriteEffects.None, 0.0f);
            sb.End();
        }

        public void postRender()
        {

        }

        private GraphicsDeviceManager gdm;
        private Viewport screenspace;
        private Viewport currentspace;
        private SpriteBatch sb;
    }
}
