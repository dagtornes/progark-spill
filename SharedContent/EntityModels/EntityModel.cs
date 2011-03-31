using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SharedContent
{
    public class EntityModel
    {
        public Vector2 Position;
        public float Speed;

        public String CombatStatsAsset;
        public List<Ability> Abilities;
        public String BehaviourType;
        public String RenderableAsset;
        public String CollidableType;
        public int CollidableParam;
        public String CollisionHandlerType;
        public String StatusType;
    }
}
