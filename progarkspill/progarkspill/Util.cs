using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace progarkspill
{
    public class Util
    {
        /**
         * Returns two vectors (in min & max) which contain all input vectors.
         * param vectors MUST have at least one element.
         */
        public static void VectorUnion(List<Vector2> vectors, out Vector2 min, out Vector2 max)
        {
            float minx = vectors[0].X, miny = vectors[0].Y;
            float maxx = minx, maxy = miny;

            for (int i = 1; i != vectors.Count; ++i)
            {
                Vector2 v = vectors[i];
                minx = Math.Min(minx, v.X);
                maxx = Math.Max(maxx, v.X);
                miny = Math.Min(miny, v.Y);
                maxy = Math.Max(maxy, v.Y);
            }

            min = new Vector2(minx, miny);
            max = new Vector2(maxx, maxy);
        }
    }
}
