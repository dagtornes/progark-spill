using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace progarkspill
{
    class Renderer
    {
        public Renderer(GraphicsDeviceManager gdm)
        {
            this.gdm = gdm;
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

        public void renderMe(IRenderable entity)
        {
            Vector2 render_position = currentspace.mapTo(entity.getPosition(), screenspace);
        }

        private GraphicsDeviceManager gdm;
        private Viewport screenspace;
        private Viewport currentspace;
    }
}
