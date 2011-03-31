using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace progarkspill.GameObjects
{
    public class Crosshair : Player
    {

        public override void decide(Entity me, GameState environment, float timedelta, GameStateStack states)
        {
            me.Physics.Velocity = new Vector2(1, -1) * GamePad.GetState(control).ThumbSticks.Right;
            //me.Physics.Velocity += me.Source.Physics.Velocity;
        }
    }
}
