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
        private float time;
        private float dt;

        public Renderer(GraphicsDeviceManager gdm, ContentManager content)
        {
            this.gdm = gdm;
            //this.pixel = content.Load<Texture2D>("whitepixel");
            this.pixel = Resources.getRes("whitepixel");
            this.font = content.Load<SpriteFont>("default");
            Vector2 screensize = new Vector2(gdm.PreferredBackBufferWidth, gdm.PreferredBackBufferHeight);
            Console.WriteLine("Screensize: {0}", screensize);
            screenspace = new Viewport(Vector2.Zero, screensize);
            sb = new SpriteBatch(gdm.GraphicsDevice);
            this.time = 0.0f;
            this.dt = 0.0f;
        }

        public void preRender(float dt)
        {
            gdm.GraphicsDevice.Clear(Color.CornflowerBlue);
            this.time += dt;
            this.dt = dt;
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
            // position is given in screenspace
            Vector2 delta = scale * new Vector2(renderable.Texture.Width, renderable.Texture.Height);
            Vector2 begin = position;
            while (begin.X > 0) begin.X -= delta.X;
            while (begin.Y > 0) begin.Y -= delta.Y;
            //Console.WriteLine("Begin: {0}", begin);
            //Console.WriteLine("Screenspace: {0}", screenspace.Size);
            Vector2 rpos = begin;
            Vector2 size = screenspace.size;
            while (rpos.Y < screenspace.Size.Y)
            {
                while (rpos.X < screenspace.Size.X)
                {
                    sb.Draw(renderable.Texture, rpos, null, renderable.Tint, angle, Vector2.Zero, scale, SpriteEffects.None, 1.0f);
                    rpos.X += delta.X;
                }
                rpos.X = begin.X;
                rpos.Y += delta.Y;
            }
        }

        private void renderTiled2(IRenderable renderable, Vector2 position, float angle, Vector2 scale)
        {
            // sb.Draw(renderable.Texture, rpos + new Vector2(x, y) * scale * delta, null, Color.White, angle, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
            Vector2 delta = scale * new Vector2(renderable.Texture.Width, renderable.Texture.Height);
            Vector2 cnt = screenspace.Size / delta + 3*Vector2.One;

            int xx = (int)position.X / (int)delta.X + 1;
            int yy = (int)position.Y / (int)delta.Y + 1;
            Vector2 off = - new Vector2(xx * delta.X, yy * delta.Y);

            for (int x = 0; x != (int) cnt.X; ++x)
                for (int y = 0; y != (int) cnt.Y; ++y)
                    sb.Draw(renderable.Texture, off + position + new Vector2(x, y) * delta, null, renderable.Tint, angle, Vector2.Zero, scale, SpriteEffects.None, 1.0f);
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

        private void renderAnimated(IRenderable renderable, Vector2 pos, float angle, Vector2 scale)
        {
            int which = ((int) Math.Floor(this.time / renderable.FrameTime)) % renderable.Frames;
            int width = (int) renderable.Size.X / renderable.Frames;
            int height = (int) renderable.Size.Y;
            Rectangle frame = new Rectangle(width * which, 0, width, height);
            sb.Draw(renderable.Texture, pos, frame, renderable.Tint, angle, renderable.Origin, scale, SpriteEffects.None, 0.0f);
        }

        private void render(IRenderable entity, Vector2 pos, float angle)
        {
            Vector2 render_position = currentspace.mapTo(pos, screenspace);
            Vector2 scale = currentspace.scaleTo(screenspace);
            if (entity.Frames > 1)
                renderAnimated(entity, render_position, angle, scale);
            else if (entity.Tiled)
                renderTiled(entity, render_position, angle, scale);
            else
                sb.Draw(entity.Texture, render_position, null, entity.Tint, angle, entity.Origin, scale, SpriteEffects.None, 0.5f);
        }

        public void renderRectOutline(Vector2 topleft, Vector2 bottomright, Color color)
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

        public void renderRect(Vector2 topleft, Vector2 bottomright, Color color, bool filled)
        {
            if (filled)
                renderRectFilled(topleft, bottomright, color);
            else
                renderRectOutline(topleft, bottomright, color);
        }

        private void renderRectFilled(Vector2 topleft, Vector2 bottomright, Color color)
        {
            topleft = currentspace.mapTo(topleft, screenspace);
            bottomright = currentspace.mapTo(bottomright, screenspace);

            float l = topleft.X, r = bottomright.X;
            float t = topleft.Y, b = bottomright.Y;

            sb.Begin();
            sb.Draw(this.pixel, topleft, null, color, 0.0f, Vector2.Zero, new Vector2(1 + r - l, 1 + b - t), SpriteEffects.None, 0.0f);
            sb.End();
        }

        public void renderRect(Vector2 topleft, Vector2 bottomright, Color color)
        {
            renderRect(topleft, bottomright, color, false);
        }

        public void renderText(string text, Vector2 position, Color color)
        {
            renderText(text, position, color, true);
        }

        public void renderText(string text, Vector2 position, Color color, bool centered)
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
