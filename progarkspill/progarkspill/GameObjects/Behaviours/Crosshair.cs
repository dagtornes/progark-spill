using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace progarkspill.GameObjects.Behaviours
{
    /// <summary>
    /// Crosshair is controlled by the player.
    /// </summary>
    public class Crosshair : Player
    {
        private static float maxDelta = 750;

        public override void decide(Entity me, GameState environment, float timedelta, GameStateStack states)
        {
            // Copy players speed, then add our own delta
            // HACK: We duplicate the players motion here, then add our own.  Not pretty.
            me.Physics.Position += me.Source.Physics.Velocity * me.Source.Physics.Speed * timedelta;
            Vector2 delta = me.Physics.Position - me.Source.Physics.Position;
            
            if (delta.LengthSquared() < maxDelta * maxDelta)
                me.Physics.Velocity = new Vector2(1, -1) * GamePad.GetState(control).ThumbSticks.Right;
            else
                me.Physics.Velocity = -delta;
        }
    }
}
