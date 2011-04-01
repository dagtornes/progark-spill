﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill.GameObjects
{
    /**
     * This status class represents any entity owned by another (me.Source),
     * and should have the same lifetime as its owner.
     */
    public class Owned : IStatus
    {
        public bool isAlive(Entity me)
        {
            return me.Source.Status.isAlive(me.Source);
        }

        public void kill(Entity me, Entity murderer)
        {
            
        }

        public Entity getKiller()
        {
            return null;
        }

        public IStatus clone()
        {
            return this;
        }
    }
}
