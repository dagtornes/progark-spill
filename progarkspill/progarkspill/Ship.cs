using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace progarkspill
{
    class Ship : Entity
    {
        public float maxSpeed = 2;
        


        public void setHeading(Vector2 direction)
        {
            Velocity = direction * maxSpeed;
        }
    }

}
