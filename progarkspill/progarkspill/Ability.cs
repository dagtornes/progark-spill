using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace progarkspill
{
    class Ability
    {
        String id;
        int cooldown;
        int level;

        public Ability(String name, EventQueue eq) // Probably add Event effect and stuff later on
        {
            id = name; // Identifier used in game data files
        }

        public void triggerIt(Vector2 direction, EventQueue eq, Entity hero) // Probably needs more params
        {

        }

        public void levelUp() // Increment some attribute on the ability by something
        {

        }
    }
}
