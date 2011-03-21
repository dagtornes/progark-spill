using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill.GameObjects
{
    public class Statistics
    {
        public int Experience { get; set; }
        public int Level { get; set; }
        public int Kills { get; set; }

        public Statistics()
        {
            Kills = 0;
            Experience = 0;
            Level = 0;
        }
    }
}
