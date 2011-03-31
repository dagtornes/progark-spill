using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedContent
{
    public class Statistics
    {
        public int Experience;
        public int Level;
        public int Kills;

        public Statistics clone()
        {
            return (Statistics)MemberwiseClone();
        }
    }
}
