using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SharedContent
{
    public class CreepSpawner
    {
        public String CreepPrototypeAsset;
        public Vector2 Position;
        public String RenderableAsset;

        // Start spawning creeps after Start seconds
        public float Start;
        // Stop spawning creeps after Start + End seconds
        public float End;
        // Spawn creeps every cooldown seconds
        public float Cooldown;
    }
}
