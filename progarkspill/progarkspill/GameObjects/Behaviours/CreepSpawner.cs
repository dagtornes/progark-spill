using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill.GameObjects.Behaviours
{
    /// <summary>
    /// Creep spawners become active after Start seconds of a game,
    /// and can spawn creeps until End seconds after the start of a game.
    /// </summary>
    public class CreepSpawner : IBehaviour
    {
        private float Start;
        private float End;
        private bool triggered = false;

        public CreepSpawner()
        {
        }

        public CreepSpawner(float Start, float End)
        {
            this.Start = Start;
            this.End = End;
        }
        
        public void decide(Entity me, GameState environment, float timedelta, GameStateStack states)
        {
            Start -= timedelta;
            End -= timedelta;
            if (Start < 0 && !triggered)
            {
                triggered = true;
                foreach (IAbility ability in me.Abilities)
                    ability.levelUp();
            }
            if (End < 0)
                me.CombatStats.Health = -1;
        }

        public IBehaviour clone()
        {
            return (CreepSpawner)MemberwiseClone();
        }
    }
}
