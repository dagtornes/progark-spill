using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.Remoting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace progarkspill.GameObjects
{
    public class Entity
    {
        public IBehaviour Behaviour { get; set; }
        public SharedContent.CombatStats CombatStats { get; set; }
        public IRenderable Renderable { get; set; }
        public IStatus Status { get; set; }
        public ICollidable Collidable { get; set; }
        public ICollisionHandler CollisionHandler { get; set; }
        public Physics Physics { get; set; }
        public Entity Source { get; set; }
        public SharedContent.Statistics Stats { get; set; }
        public List<IAbility> Abilities { get; set; }

        public Entity()
        {
            CombatStats = new SharedContent.CombatStats();
            Status = new Status();
            Collidable = new NonCollidable();
            Physics = new Physics(200);
            Stats = new SharedContent.Statistics();
        }

        public Entity(SharedContent.EntityModel Model, ContentManager Content): this()
        {
            Physics = new Physics(Model.Speed);
            Physics.Position = Model.Position;
            CombatStats = Content.Load<SharedContent.CombatStats>(Model.CombatStatsAsset).clone();
            Abilities = new List<IAbility>();
            Assembly me = Assembly.GetEntryAssembly();
            foreach (SharedContent.Ability ability in Model.Abilities)
            {
                ObjectHandle abilityHandle = Activator.CreateInstanceFrom(me.CodeBase,
                    ability.AbilityTypeName);
                IAbility control = (IAbility) abilityHandle.Unwrap();
                control.Stats = Content.Load<SharedContent.AbilityStats>(ability.AbilityStatsAsset).clone(); ;
                Abilities.Add(control);
                if (ability.EntityModelAsset != null && ability.EntityModelAsset != "")
                {
                    control.ProjectilePrototype = new Entity(Content.Load<SharedContent.EntityModel>(ability.EntityModelAsset), Content);
                }
            }
            ObjectHandle collidableHandle = Activator.CreateInstanceFrom(me.CodeBase, Model.CollidableType);
            Collidable = (ICollidable) collidableHandle.Unwrap();
            if (Model.CollisionHandlerType != null && Model.CollisionHandlerType != "")
            {
                ObjectHandle handlerHandle = Activator.CreateInstanceFrom(me.CodeBase, Model.CollisionHandlerType);
                CollisionHandler = (ICollisionHandler)handlerHandle.Unwrap();
            }
            Renderable = new Sprite(Content.Load<Texture2D>(Model.RenderableAsset));
            ObjectHandle statusHandle = Activator.CreateInstanceFrom(me.CodeBase, Model.StatusType);
            Status = (IStatus)statusHandle.Unwrap();
            ObjectHandle behaviourHandle = Activator.CreateInstanceFrom(me.CodeBase, Model.BehaviourType);
            Behaviour = (IBehaviour)behaviourHandle.Unwrap();
        }
        public Entity(Entity other): this()
        {
            this.Physics = other.Physics.clone();
            this.Status = other.Status.clone();
            this.CombatStats = other.CombatStats.clone();
            this.Abilities = new List<IAbility>();
            foreach (IAbility ability in other.Abilities)
                this.Abilities.Add(ability.clone());
            this.Behaviour = other.Behaviour.clone();
            this.Stats = other.Stats.clone();
            this.Collidable = other.Collidable.clone();
            this.CollisionHandler = other.CollisionHandler;
            this.Renderable = other.Renderable.clone();
        }

        public void move(float timedelta)
        {
            Physics.Position += Physics.Velocity * Physics.Speed * timedelta;
            if (Physics.Velocity != Vector2.Zero)
            {
                Physics.Orientation = Physics.Velocity;
                Physics.Orientation.Normalize();
            }
        }


    }
}
