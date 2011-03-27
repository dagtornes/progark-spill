using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using progarkspill.GameObjects;
using Microsoft.Xna.Framework.Content;

namespace progarkspill
{
    public class Renderer : IDisposable
    {
        private Texture2D pixel;
        private SpriteFont font;

        public Renderer(GraphicsDeviceManager gdm, ContentManager content)
        {
            this.gdm = gdm;
            //this.pixel = content.Load<Texture2D>("whitepixel");
            this.pixel = Resources.getRes("whitepixel");
            this.font = content.Load<SpriteFont>("default");
            Vector2 screensize = new Vector2(gdm.PreferredBackBufferWidth, gdm.PreferredBackBufferHeight);
            screenspace = new Viewport(Vector2.Zero, screensize);
            sb = new SpriteBatch(gdm.GraphicsDevice);
        }

        public void preRender()
        {
            gdm.GraphicsDevice.Clear(Color.CornflowerBlue);
        }

        /*
         * Sets the current Viewport, which will affect all render() calls until a following end().
         * Viewports are mappings from a game-specific space to screen-space.
         */
        public void begin(Viewport view)
        {
            currentspace = view;
        }

        private void renderTiled(IRenderable renderable, Vector2 position, float angle, Vector2 scale)
        {
            // sb.Draw(renderable.Texture, rpos + new Vector2(x, y) * scale * delta, null, Color.White, angle, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
            Vector2 delta = scale * new Vector2(renderable.Texture.Width, renderable.Texture.Height);
            Vector2 cnt = screenspace.size / delta + 2*Vector2.One;

            int xx = (int)position.X / (int)delta.X + 1;
            int yy = (int)position.Y / (int)delta.Y + 1;
            Vector2 off = - new Vector2(xx * delta.X, yy * delta.Y);

            for (int x = 0; x != (int) cnt.X; ++x)
                for (int y = 0; y != (int) cnt.Y; ++y)
                    sb.Draw(renderable.Texture, off + position + new Vector2(x, y) * delta, null, Color.White, angle, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }

        public void beginScreen()
        {
            begin(screenspace);
        }

        /*
         * Renders the renderable to screen.
         * Gets position and rotation from the transform.
         */
        public void render(IRenderable renderable, Physics transform)
        {
            sb.Begin();
            render(renderable, transform.Position, transform.Angle);
            sb.End();
        }

        private void render(IRenderable entity, Vector2 pos, Vector2 dir)
        {
            sb.Begin();
            float angle = (float)Math.Atan2(dir.Y, dir.X);
            render(entity, pos, angle);
            sb.End();
        }

        private void render(IRenderable entity, Vector2 pos, float angle)
        {
            Vector2 render_position = currentspace.mapTo(pos, screenspace);
            Vector2 scale = currentspace.scaleTo(screenspace);
            
            if (entity.Tiled)
                renderTiled(entity, render_position, angle, scale);
            else
                sb.Draw(entity.Texture, render_position, null, Color.White, angle, entity.Origin, scale, SpriteEffects.None, 0.0f);
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
            sb.Draw(this.pixel, topleft, null, color, 0.0f, Vector2.Zero, new Vector2(1, b - t), SpriteEffects.None, 0.0f);
            sb.Draw(this.pixel, topleft + new Vector2(r - l, 0), null, color, 0.0f, Vector2.Zero, new Vector2(1, b - t), SpriteEffects.None, 0.0f);
            sb.End();
        }

        public void renderText(string text, Vector2 position, Color color, bool centered = true)
        {
            sb.Begin();
            Vector2 origin = Vector2.Zero;
            if (centered)
                origin = 0.5f * new Vector2(0, 0);
            sb.DrawString(this.font, text, position, color, 0.0f, origin, Vector2.One, SpriteEffects.None, 0.0f);
            sb.End();
        }

        public void postRender()
        {

        }

        public Viewport Screenspace { get { return screenspace; } }

        private GraphicsDeviceManager gdm;
        private Viewport screenspace;
        private Viewport currentspace;
        private SpriteBatch sb;

        public void Dispose()
        {
            sb.Dispose();
        }

        public void end()
        {
            
        }
    }
}
