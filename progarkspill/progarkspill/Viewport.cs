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

        public void fit(Entity ent)
        {
            float aspect = Aspect;

            float dx = 0.0f, dy = 0.0f;
            if (ent.Position.X < corner.X) dx = ent.Position.X - corner.X;
            if (ent.Position.Y < corner.Y) dy = ent.Position.Y - corner.Y;

            corner += new Vector2(dx, dy);
            size -= new Vector2(dx, dy);

            if (ent.Position.X > (corner + size).X) dx = ent.Position.X - (corner + size).X;
            if (ent.Position.Y > (corner + size).Y) dy = ent.Position.Y - (corner + size).Y;

            size += new Vector2(dx, dy);

            preserveAspect(aspect);
        }

        public void preserveAspect(float aspect)
        {
            if (size.X / size.Y > aspect)
                size.Y = (size.X / aspect);
            else
                size.X = aspect * size.Y;
        }

        public float Aspect
        {
            get { return size.X / size.Y; }
        }

        public Vector2 corner, size;
    }
}
