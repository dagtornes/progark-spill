using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using progarkspill.GameObjects;

namespace progarkspill
{
    public class GameState : IGameState
    {
        private List<Entity> hostiles = new List<Entity>(); // All enemies and hostile projectiles
        private List<Entity> projectiles = new List<Entity>(); // Playerside projectiles
        private List<Entity> players = new List<Entity>(); // Players
        private List<Entity> nonInteractives = new List<Entity>(); // Events and creep spawners and the like
        private List<Entity> gameObjectives = new List<Entity>(); // Work this one out somewhere

        private GameStateStack stack;
        private Viewport view;
        public Texture2D BulletSprite { get; set; }
        private List<Entity> newObjects = new List<Entity>();
        private Sprite bgRenderable;

        public List<Entity> Players
        {
            get { return players; }
            set { players = value; }
        }
        
        public bool tickDown { get { return false; } }
        public bool renderDown { get { return false; } }

        public GameState(GameStateStack stack)
            : this()
        {
            this.stack = stack;
            this.bgRenderable = new Sprite(Resources.getRes("starfield"));
            this.bgRenderable.Tiled = true;
            this.view = new Viewport(Vector2.Zero, 500 * (Vector2.One + 0.667f * Vector2.UnitX));
        }

        public GameState()
        {
            this.view = new Viewport(Vector2.Zero, 500*(Vector2.One + 0.667f*Vector2.UnitX));
            // This needs to be fetched from data and tweaked loads 
            addPlayer(Player.createPlayer(PlayerIndex.One));

            gameObjectives.Add(new Entity());
            Entity objective = gameObjectives[0];
            Texture2D tex = Resources.getRes("bullet");
            this.BulletSprite = tex;
            objective.Renderable = new Sprite(Resources.getRes("objective"));
            objective.Physics.Position = new Vector2(250, 250);
            objective.CombatStats.Health = 1000;
            objective.Physics.Speed = 0;
            objective.Collidable = new HitCircle(objective.Renderable.Texture.Width / 2);
            Entity spawner = new Entity();
            Entity spawner2 = new Entity();
            nonInteractives.Add(spawner);
            nonInteractives.Add(spawner2);
            spawner.Physics.Position = new Vector2(750, 750);
            spawner2.Physics.Position = new Vector2(-300, -300);
            spawner2.CombatStats.Cooldown = 5;
            spawner.CombatStats.Cooldown = 5;
            Entity standardCreep = new Entity();
            standardCreep.CombatStats = CombatStats.defaultShip(); // Sanify
            standardCreep.Renderable = new Sprite(Resources.getRes("enemy1"));
            standardCreep.Collidable = new HitCircle(standardCreep.Renderable.Texture.Width / 2);
            standardCreep.CollisionHandler = new DamageCollisionHandler();
            standardCreep.Behaviour = new CreepBehaviour();
           
            spawner.Behaviour = new CreepSpawnBehaviour(standardCreep);
            spawner2.Behaviour = new CreepSpawnBehaviour(standardCreep);
            
        }

        public Entity gameObjective()
        {
            return gameObjectives[0];
        }

        private void behaviourTick(float timedelta)
        {
            behaviourTick(players, timedelta, projectiles);
            behaviourTick(hostiles, timedelta, hostiles);
            behaviourTick(projectiles, timedelta, projectiles);
            behaviourTick(nonInteractives, timedelta, hostiles); // nonInteractives may only spawn players
        }

        // Run a behaviour tick that is timedelta long, and let behaviour objects in gameObjects add
        // new gameobjects to destination if they wish
        private void behaviourTick(List<Entity> gameObjects, float timedelta, List<Entity> destination)
        {
            newObjects = new List<Entity>();
            foreach (Entity gameObject in gameObjects)
            {
                gameObject.Behaviour.decide(gameObject, this, timedelta, stack);
            }
            foreach (Entity newObject in newObjects)
                destination.Add(newObject);
        }

        private void physicsTick(float timedelta)
        {
            physicsTick(timedelta, players);
            physicsTick(timedelta, hostiles);
            physicsTick(timedelta, projectiles);
            physicsTick(timedelta, nonInteractives);
        }

        private void physicsTick(float timedelta, List<Entity> gameObjects)
        {
            foreach (Entity ent in gameObjects)
            {
                ent.move(timedelta);
            }
        }

        public void collisionTick()
        {
            collisionCheck(projectiles, hostiles); // Players kill hostiles
            collisionCheck(hostiles, gameObjectives); // Hostiles kill gameobjective
            collisionCheck(hostiles, players); // Hostiles kill players
            collisionCheck(players, nonInteractives); // Players gain powerups (?)

        }
        private void collisionCheck(List<Entity> colliders, List<Entity> obstacles)
        {
            foreach (Entity collider  in colliders)
            {
                foreach (Entity obstacle in obstacles)
                {
                    if (collider.Collidable.intersects(collider, obstacle))
                        collider.CollisionHandler.collide(collider, obstacle);
                }
            }
        }
 
        public void addGameObject(Entity gameObject)
        {
            newObjects.Add(gameObject);
        }

        public void addPlayer(Entity player)
        {
            players.Add(player);
        }

        private void render(Renderer r, List<Entity> gameObjects)
        {
            foreach (Entity gameObject in gameObjects)
            {
                if (gameObject.Renderable == null)
                    continue;
                r.render(gameObject.Renderable, gameObject.Physics);
            }
        }

        public void render(Renderer r)
        {
            r.begin(view);
            r.render(bgRenderable, new Physics(0.0f));
            render(r, projectiles);
            render(r, hostiles);
            render(r, nonInteractives);
            render(r, players);
            renderHBar(r, players);
            render(r, gameObjectives);
            renderHBar(r, gameObjectives);
        }

        private void renderHBar(Renderer r, List<Entity> objects)
        {
            foreach (Entity e in objects)
            {
                if (e.CombatStats == null || e.Renderable == null) continue;
                float w = e.Renderable.Size.X;
                float health = 1.0f * e.CombatStats.Health / CombatStats.defaultShip().Health;

                float top = e.Physics.Position.Y - e.Renderable.Size.Y / 2 - 15.0f;
                Vector2 topleft = new Vector2(e.Physics.Position.X - w / 2, top);
                Vector2 bottomRight = new Vector2(e.Physics.Position.X + w / 2, top + 10);
                Vector2 lastOne = new Vector2(topleft.X + health * w, top + 10);

                r.renderRect(topleft, bottomRight, Color.White);
                r.renderRect(topleft, lastOne, Color.Red);
            }
        }

        private List<Entity> statusCheck(List<Entity> gameObjects)
        {
            List<Entity> dead = new List<Entity>();
            foreach (Entity gameObject in gameObjects)
            {
                if (!gameObject.Status.isAlive(gameObject))
                {
                    dead.Add(gameObject);
                }
            }
            foreach (Entity gameObject in dead)
            {
                gameObjects.Remove(gameObject);
            }
            return dead;
        }

        public void tick(float timedelta) 
        {
            behaviourTick(timedelta);
            physicsTick(timedelta);
            if (!(view.fit(players) || view.fit(hostiles)))
                view.shrink(-0.1f * timedelta);
            collisionTick();

            statusCheck(hostiles); // Returns list of dead hostiles
            statusCheck(projectiles);
            statusCheck(gameObjectives); // Returns list of length > 0 if gameObjective is dead
            statusCheck(players);
        }


    }
}
