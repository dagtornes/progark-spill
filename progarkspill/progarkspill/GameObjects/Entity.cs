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
            CombatStats.Health = 1; // Alive by default
            Status = new Statuses.Status();
            Collidable = new NonCollidable();
            Physics = new Physics(200);
            Stats = new SharedContent.Statistics();
            Abilities = new List<IAbility>();
        }
        public Entity(SharedContent.CreepSpawner Model, ContentManager Content)
            :
            this(Content.Load<SharedContent.EntityModel>("EntityPrototypes/CreepSpawner"), Content)
        {
            Abilities[0].Stats.Cooldown = Model.Cooldown;
            SharedContent.EntityModel prototype = Content.Load<SharedContent.EntityModel>(Model.CreepPrototypeAsset);
            Abilities[0].ProjectilePrototype = new Entity(prototype, Content);
            Physics.Position = Model.Position;
            Behaviour = new Behaviours.CreepSpawner(Model.Start, Model.End);
            if (Model.RenderableAsset != null && Model.RenderableAsset != "")
                Renderable = new Sprite(Content.Load<Texture2D>(Model.RenderableAsset));

        }
        public Entity(SharedContent.EntityModel Model, ContentManager Content): this()
        {
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
            if (other.Renderable != null)
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
