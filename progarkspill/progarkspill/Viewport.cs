using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace progarkspill
{
    /**
     * This defines a mapping from some space to screen-space
     */
    public class Viewport
    {
        public Viewport(Vector2 corner, Vector2 size)
        {
            this.corner = corner;
            this.size = size;
        }

        /*
         * Maps a vector from this viewport to other.
         */
        public Vector2 mapTo(Vector2 from, Viewport other)
        {
            return other.size * ((from - corner) / size) + other.corner;
        }

        public Vector2 scaleTo(Viewport other)
        {
            return other.size / size;
        }

        public Vector2 corner, size;
    }
}
