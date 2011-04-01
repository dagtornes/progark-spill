using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using progarkspill.GameObjects;
using progarkspill.GameObjects.Behaviours;
using progarkspill.GameObjects.Renderables;

namespace progarkspill
{
    public class GameState : IGameState
    {
        private List<Entity> hostiles = new List<Entity>(); // All enemies
        private List<Entity> hostileProjectiles = new List<Entity>();
        private List<Entity> projectiles = new List<Entity>(); // Playerside projectiles
        private List<Entity> players = new List<Entity>(); // Players
        private List<Entity> nonInteractives = new List<Entity>(); // Events and creep spawners and the like
        private List<Entity> gameObjectives = new List<Entity>(); // Work this one out somewhere

        public List<Entity> newNoninteractives = new List<Entity>();

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
            // addPlayer(Player.createPlayer(PlayerIndex.One));
            gameObjectives.Add(Resources.getPrototype("GameObjective"));
            Entity playerOne = Resources.getPrototype("PlayerPrototype");
            ((Player)playerOne.Behaviour).control = PlayerIndex.One;
            addPlayer(playerOne);
            playerOne.Abilities[0].bind(PlayerIndex.One, Microsoft.Xna.Framework.Input.Buttons.RightTrigger);
            hostiles.Add(new Entity(Resources.getPrototype("StandardCreep")));
            hostiles.Add(new Entity(Resources.getPrototype("StandardCreep")));
            hostiles[1].Physics.Position = new Vector2(-500, -250);
            nonInteractives.Add(new Entity(Resources.getPrototype("CreepSpawner")));
            
        }
        public Entity gameObjective()
        {
            return gameObjectives[0];
        }

        private void behaviourTick(float timedelta)
        {
            behaviourTick(players, timedelta, projectiles);
            behaviourTick(hostiles, timedelta, hostileProjectiles);
            behaviourTick(projectiles, timedelta, projectiles);
            behaviourTick(nonInteractives, timedelta, hostiles); // nonInteractives may only spawn players
            nonInteractives.AddRange(newNoninteractives);
            newNoninteractives.Clear();
        }

        // Run a behaviour tick that is timedelta long, and let behaviour objects in gameObjects add
        // new gameobjects to destination if they wish
        private void behaviourTick(List<Entity> gameObjects, float timedelta, List<Entity> destination)
        {
            newObjects = new List<Entity>();
            foreach (Entity gameObject in gameObjects)
            {
                gameObject.Behaviour.decide(gameObject, this, timedelta, stack);
                foreach (IAbility ability in gameObject.Abilities)
                {
                    if (ability.triggered(gameObject, this, timedelta))
                        ability.fire(gameObject, this);
                }
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
            physicsTick(timedelta, hostileProjectiles);
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
            collisionCheck(hostileProjectiles, players); // Creep projectiles kill players
            collisionCheck(hostileProjectiles, gameObjectives);
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
            Entity crosshair = Resources.getPrototype("Crosshair");
            player.Source = crosshair;
            crosshair.Source = player;
            crosshair.Renderable.Tint = Color.Red;
            newNoninteractives.Add(crosshair);
            
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
            renderHBar(r, hostiles);
            render(r, nonInteractives);
            render(r, players);
            renderHBar(r, players);
            render(r, gameObjectives);
            renderHBar(r, gameObjectives);
            render(r, hostileProjectiles);
        }

        private void renderHBar(Renderer r, List<Entity> objects)
        {
            foreach (Entity e in objects)
            {
                if (e.CombatStats == null || e.Renderable == null) continue;
                float w = e.Renderable.Size.X;
                float health = 1.0f * e.CombatStats.Health / e.CombatStats.MaxHealth;

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
            physicsTick(timedelta);
            behaviourTick(timedelta);
            /*
            if (!(view.fit(players) || view.fit(hostiles)))
                view.shrink(-0.1f * timedelta);
            */
            updateViewport();
            collisionTick();

            statusCheck(hostiles); // Returns list of dead hostiles
            statusCheck(projectiles);
            statusCheck(gameObjectives); // Returns list of length > 0 if gameObjective is dead
            foreach (Entity player in statusCheck(players))
            {
                Entity respawner = new Entity();
                respawner.Behaviour = new RespawnPlayer(player, 7.5f + 7.5f * player.Stats.Level);
                respawner.CombatStats.Health = 1;
                nonInteractives.Add(respawner);
            }
            statusCheck(hostileProjectiles);
            statusCheck(nonInteractives);
            
        }

        private void updateViewport()
        {
            Vector2 pad = 150 * Vector2.One;
            float minSizeY = 1000;

            List<Vector2> positions = new List<Vector2>();
            foreach (Entity player in players) positions.Add(player.Physics.Position);
            positions.Add(gameObjective().Physics.Position);

            Vector2 min, max;
            Util.VectorUnion(positions, out min, out max);
            min -= pad;
            max += pad;
            if (max.Y - min.Y < minSizeY)
            {
                float delta = max.Y - min.Y;
                min.Y -= delta / 2;
                max.Y += delta / 2;
            }
            float aspect = view.Aspect;
            view.TopLeft = min;
            view.BottomRight = max;
            view.preserveAspect(aspect);
        }
    }
}
