using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharedContent;
using progarkspill.GameObjects;

namespace progarkspill
{
    public class Resources
    {
        public static void init(ContentManager content)
        {
            Resources.res = new Resources(content);
        }

        public static Texture2D getRes(string resname)
        {
            if (!Resources.res.resources.ContainsKey(resname))
                Resources.res.resources[resname] = Resources.res.content.Load<Texture2D>(resname);
            return Resources.res.resources[resname];
        }

        private Resources(ContentManager content)
        {
            this.resources = new Dictionary<string, Texture2D>();
            this.content = content;
            
        }

        public static Entity getPrototype(String name)
        {
            return new Entity((EntityModel)Resources.res.content.Load<EntityModel>("EntityPrototypes/" + name), Resources.res.content);
        }
        private static Resources res;
        private ContentManager content;
        private Dictionary<string, Texture2D> resources;
    }
}
