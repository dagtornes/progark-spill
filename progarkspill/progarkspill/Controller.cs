using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace progarkspill
{
    /**
     * This class implements a few methods of the type
     * "WishIWereAMethodOnGamePad"
     */
    public class Controller
    {
        private static Dictionary<PlayerIndex, Dictionary<Buttons, float>> lastPressed;
        private static float now;
        static Controller()
        {
            now = 0.0f;
            lastPressed = new Dictionary<PlayerIndex, Dictionary<Buttons, float>>();
            foreach (PlayerIndex player in Enum.GetValues(typeof(PlayerIndex)))
            {
                lastPressed.Add(player, new Dictionary<Buttons, float>());
                foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
                {
                    float neg = float.NegativeInfinity;
                    lastPressed[player].Add(button, neg);
                }
            }
        }

        public static bool RecentlyPressed(PlayerIndex controller, Buttons button)
        {
            return RecentlyPressed(controller, button, 0.0f);
        }

        public static bool RecentlyPressed(PlayerIndex controller, Buttons button, float threshold)
        {
            GamePadState c = GamePad.GetState(controller);
            if ((lastPressed[controller][button] + threshold <= now) && c.IsButtonDown(button))
            {
                lastPressed[controller][button] = now;
                return true;
            }
            return false;
        }

        public static void Update(float dt)
        {
            now += dt;
        }
    }
}
