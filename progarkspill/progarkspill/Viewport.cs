using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using progarkspill.GameObjects;

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
            this.minSizeY = 500;
        }

        public Viewport(Viewport other)
        {
            this.corner = other.corner;
            this.size = other.size;
            this.minSizeY = other.minSizeY;
        }

        public Vector2 Size
        {
            get { return this.size; }
            set { this.size = value; }
        }
        public Vector2 TopLeft
        {
            get { return this.corner; }
            set { this.corner = value; }
        }
        public Vector2 BottomRight
        {
            get { return this.corner + this.size; }
            set { this.size = value - this.corner; }
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

        public void shrink(float amnt)
        {
            if (size.Y <= minSizeY) return;
            Vector2 delta = amnt * size;
            corner -= 0.5f * delta;
            size += delta;
        }

        public bool fit(Entity ent)
        {
            bool didFit = false;
            float aspect = Aspect;

            float dx = 0.0f, dy = 0.0f;
            if (ent.Physics.Position.X < corner.X) dx = ent.Physics.Position.X - corner.X;
            if (ent.Physics.Position.Y < corner.Y) dy = ent.Physics.Position.Y - corner.Y;

            corner += new Vector2(dx, dy);
            size -= new Vector2(dx, dy);

            didFit = dx != 0.0 || dy != 0.0;
            dx = 0; dy = 0;
            if (ent.Physics.Position.X > (corner + size).X) dx = ent.Physics.Position.X - (corner + size).X;
            if (ent.Physics.Position.Y > (corner + size).Y) dy = ent.Physics.Position.Y - (corner + size).Y;

            size += new Vector2(dx, dy);
            didFit = (dx != 0.0 || dy != 0.0) || didFit;

            preserveAspect(aspect);

            return didFit;
        }
        public bool fit(List<Entity> entities)
        {
            bool didFit = false;
            foreach (Entity ent in entities)
                didFit = fit(ent) || didFit;
            return didFit;
        }

        public void preserveAspect(float aspect)
        {
            Vector2 oldsize = size;
            if (size.X / size.Y > aspect)
                size.Y = (size.X / aspect);
            else
                size.X = aspect * size.Y;
            Vector2 delta = size - oldsize;
            corner -= 0.5f * delta;
        }

        public float Aspect
        {
            get { return size.X / size.Y; }
        }

        public Vector2 corner, size;
        private float minSizeY;
    }
}
