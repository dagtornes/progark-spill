using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill
{
    public interface IActive
    {
        bool isAlive(Entity me);
    }
}
