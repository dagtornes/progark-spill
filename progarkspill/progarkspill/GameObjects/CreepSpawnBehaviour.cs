using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace progarkspill.GameObjects
{
    public class CreepSpawnBehaviour : IBehaviour
    {
        private Entity Prototype { get; set; }

        public CreepSpawnBehaviour(Entity creepPrototype)
        {
            Prototype = creepPrototype;
        }

        public void decide(Entity me, GameState environment, float timedelta, GameStateStack states)
        {
            me.CombatStats.CurrentCooldown -= timedelta;
            if (me.CombatStats.CurrentCooldown <= 0)
            {
                Entity newCreep = new Entity();
                // This should be done by some factory or other
                // Alternatively, make sure that actual copying is taking place
                newCreep.Physics.Position = me.Physics.Position;
                newCreep.Behaviour = Prototype.Behaviour;
                newCreep.CollisionHandler = Prototype.CollisionHandler;
                newCreep.Renderable = Prototype.Renderable;
                newCreep.Source = me;
                newCreep.Status = Prototype.Status;
                newCreep.Collidable = Prototype.Collidable;
                newCreep.CombatStats = Prototype.CombatStats;
                me.CombatStats.CurrentCooldown = me.CombatStats.Cooldown;
                environment.addGameObject(newCreep);
            }
        }
    }
}
