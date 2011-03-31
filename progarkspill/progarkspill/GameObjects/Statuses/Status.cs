﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill.GameObjects
{
    public class Status : IStatus
    {
        Entity killer = null;

        public bool isAlive(Entity me)
        {
            return me.CombatStats.Health > 0 && me.Physics.Position.LengthSquared() < 2000 * 2000;
        }
        public void kill(Entity me, Entity murderer)
        {
            killer = murderer;
        }
        public Entity getKiller()
        {
            return killer;
        }
    }
}