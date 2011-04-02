using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SharedContent
{
    public class LevelModel
    {
        public String GameObjectiveAsset; // Which GameObjective to use
        public Vector2 GameObjectivePosition;

        public List<CreepSpawner> SpawnPoints; // Which spawn points to use
    }
}
