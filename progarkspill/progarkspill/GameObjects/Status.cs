using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill.GameObjects
{
    public class Status : IStatus
    {
        public bool isAlive(Entity me)
        {
            return true;
        }
    }
}
