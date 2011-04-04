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
        public int Levels = 0;

        public Statistics clone()
        {
            return (Statistics)MemberwiseClone();
        }

        public void grantXp(int amount)
        {
            Experience += amount;
            if (Experience > xpForNextLevel())
            {
                Levels += 1;
                Experience -= xpForNextLevel();
                Level += 1;
            }
        }

        public int xpForNextLevel()
        {
            return Level * 500;
        }
    }
}
