using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using progarkspill.GameObjects.Collidables;
using progarkspill.GameObjects.Renderables;

namespace progarkspill.GameObjects
{
    /// <summary>
    /// This book-keeping object is used for the state of multiple kinds of
    /// gameobjects. It uses a composition of classes to determine how to act.
    /// Some of these act as controllers only, some act as model only.
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// Controller. <see cref="IBehaviour"/>
        /// </summary>
        public IBehaviour Behaviour { get; set; }
        /// <summary>
        /// The model for health, armor and other combat engine related
        /// numbers.
        /// </summary>
        public SharedContent.CombatStats CombatStats { get; set; }
        /// <summary>
        /// Controller. <see cref="IRenderable"/>
        /// </summary>
        public IRenderable Renderable { get; set; }
        /// <summary>
        /// Controller. <see cref="IStatus"/>
        /// </summary>
        public IStatus Status { get; set; }
        /// <summary>
        /// Controller. <see cref="ICollidable"/>
        /// </summary>
        public ICollidable Collidable { get; set; }
        /// <summary>
        /// Controller. <see cref="ICollisionHandler"/>
        /// </summary>
        public ICollisionHandler CollisionHandler { get; set; }
        /// <summary>
        /// Model of physics (Location, speed...). <see cref="Physics"/>
        /// </summary>
        public Physics Physics { get; set; }
        /// <summary>
        /// The Entity that caused this Entity to come into existence. Not
        /// used by all Entities - check for null.
        /// </summary>
        public Entity Source { get; set; }
        /// <summary>
        /// Statistics about this Entity, such as level, stats, kills.
        /// </summary>
        public SharedContent.Statistics Stats { get; set; }
        /// <summary>
        /// The abilities this Entity can trigger. <see cref="IAbility"/>
        /// </summary>
        public List<IAbility> Abilities { get; set; }

        /// <summary>
        /// Initialize a default Entity that sets most fields to empty instances.
        /// </summary>
        public Entity()
        {
            CombatStats = new SharedContent.CombatStats();
            CombatStats.Health = 1; // Alive by default
            Status = new Statuses.Status();
            Collidable = new NonCollidable();
            Physics = new Physics(200);
            Stats = new SharedContent.Statistics();
            Abilities = new List<IAbility>();
        }
        /// <summary>
        /// Specialcase that creates an Entity that spawns creeps from a CreepSpawner.
        /// This loads necessary data from XNA content pipeline.
        /// </summary>
        /// <param name="Model"><see cref="SharedContent.CreepSpawner"/></param>
        /// <param name="Content">XNA content pipeline</param>
        public Entity(SharedContent.CreepSpawner Model, ContentManager Content)
            :
            this(Content.Load<SharedContent.EntityModel>("EntityPrototypes/CreepSpawner"), Content)
        {
            SharedContent.EntityModel prototype = Content.Load<SharedContent.EntityModel>(Model.CreepPrototypeAsset);
            Physics.Position = Model.Position;
            Behaviour = new Behaviours.CreepSpawner(Model.Start, Model.End);
            if (Model.RenderableAsset != null && Model.RenderableAsset != "")
                Renderable = new Sprite(Content.Load<Texture2D>(Model.RenderableAsset));

        }
        /// <summary>
        /// Loads a generic Entity from data coming from XNA content pipeline. This instanciates the classes named
        /// in the model using reflection.
        /// </summary>
        /// <param name="Model">The name of Model to load.</param>
        /// <param name="Content">Content pipeline to use.</param>
        public Entity(SharedContent.EntityModel Model, ContentManager Content): this()
        {
            // Hairy hacks to allow dynamic creation of classes from data files.
            Physics = new Physics(Model.Speed);
            Physics.Position = Model.Position;
            CombatStats = Content.Load<SharedContent.CombatStats>(Model.CombatStatsAsset).clone();
            Abilities = new List<IAbility>();
            Assembly me = Assembly.GetExecutingAssembly();
            foreach (SharedContent.Ability ability in Model.Abilities)
            {
                Object abilityHandle = me.CreateInstance("progarkspill.GameObjects.Abilities." +
                    ability.AbilityTypeName);
                IAbility control = (IAbility) abilityHandle;
                control.Stats = Content.Load<SharedContent.AbilityStats>(ability.AbilityStatsAsset).clone(); ;
                Abilities.Add(control);
                if (ability.EntityModelAsset != null && ability.EntityModelAsset != "")
                {
                    control.ProjectilePrototype = new Entity(Content.Load<SharedContent.EntityModel>(ability.EntityModelAsset), Content);
                }
            }
            Collidable = (ICollidable)me.CreateInstance("progarkspill.GameObjects.Collidables." +
                Model.CollidableType);
            if (Model.CollisionHandlerType != null && Model.CollisionHandlerType != "")
            {
                CollisionHandler = (ICollisionHandler)me.CreateInstance("progarkspill.GameObjects.CollisionHandlers." +
                    Model.CollisionHandlerType);
            }
            if (Model.RenderableAsset != null && Model.RenderableAsset != "")
                Renderable = new Sprite(Content.Load<Texture2D>(Model.RenderableAsset));
            Status = (IStatus)me.CreateInstance("progarkspill.GameObjects.Statuses." + 
                Model.StatusType);
            Behaviour = (IBehaviour)me.CreateInstance("progarkspill.GameObjects.Behaviours." +
                Model.BehaviourType);
            Stats.Level = Model.Level;
        }
        /// <summary>
        /// Perform a memberwise clone of another Entity.
        /// </summary>
        /// <param name="other">The entity to clone.</param>
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
            if (other.Renderable != null)
                this.Renderable = other.Renderable.clone();
        }
        /// <summary>
        /// Perform update of Physics belonging to this Entity.
        /// </summary>
        /// <param name="timedelta"></param>
        public void move(float timedelta)
        {
            Physics.Position += Physics.Velocity * Physics.Speed * Physics.SpeedModifier * timedelta;
            if (Physics.Velocity != Vector2.Zero)
            {
                Physics.Orientation = Physics.Velocity;
                Physics.Orientation.Normalize();
            }
            Physics.SpeedModifier = 1;
        }


    }
}
