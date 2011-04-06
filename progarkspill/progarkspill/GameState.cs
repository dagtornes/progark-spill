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
using SharedContent;
using progarkspill.GameObjects.Statuses;
using progarkspill.Menus;

namespace progarkspill
{
    /// <summary>
    /// This class encapsulates the running game, keeping track of all
    /// players, projectiles, enemies and gameobjectives.
    /// </summary>
    public class GameState : IGameState
    {
        private List<Entity> hostiles = new List<Entity>(); // All enemies
        private List<Entity> hostileProjectiles = new List<Entity>();
        private List<Entity> projectiles = new List<Entity>(); // Playerside projectiles
        private List<Entity> players = new List<Entity>(); // Players
        private List<Entity> nonInteractives = new List<Entity>(); // Events and creep spawners and the like
        private List<Entity> gameObjectives = new List<Entity>(); // Work this one out somewhere

        public List<Entity> newNoninteractives = new List<Entity>(); // Buffer for entities that need to be added

        private GameStateStack stack; // My source
        private Viewport view;
        private List<Entity> newObjects = new List<Entity>(); // Buffer for Entities that need to be added
        private Sprite bgRenderable; // Background

        private float leveltimeleft;
        private float leveltime;

        private Sprite pie = new Sprite(Resources.getRes("pie"));
        private Sprite ring = new Sprite(Resources.getRes("ring"));
        private Dictionary<int, Color> abilityColors = new Dictionary<int,Color>();
        private Dictionary<int, Vector2> dirs = new Dictionary<int, Vector2>();
        
        public List<Entity> Players
        {
            get { return players; }
            set { players = value; }
        }

        public static GameState Create(GameStateStack stack, SharedContent.LevelModel level, List<Entity> players)
        {
            GameState ret = new GameState(stack, level);
            foreach (Entity player in players)
                ret.addPlayer(player);
            return ret;
        }
        
        public bool tickDown { get { return false; } }
        public bool renderDown { get { return false; } }

        /// <summary>
        /// Create a GameState without initializing a level in it.
        /// </summary>
        /// <param name="stack">The GameStateStack to which this GameState belongs</param>
        public GameState(GameStateStack stack)
            : this()
        {
            this.stack = stack;
            this.bgRenderable = new Sprite(Resources.getRes("starfield"));
            this.bgRenderable.Tiled = true;
            this.view = new Viewport(Vector2.Zero, 500 * (Vector2.One + 0.667f * Vector2.UnitX));
        }
        /// <summary>
        /// Initialize a GameState without initializing anything in it.
        /// </summary>
        public GameState()
        {
            this.view = new Viewport(Vector2.Zero, 500*(Vector2.One + 0.667f*Vector2.UnitX));
            this.pie.Origin = new Vector2(0.0f, pie.Origin.Y);
            abilityColors.Add(1, Color.Green);     dirs.Add(1, Vector2.UnitY);
            abilityColors.Add(2, Color.Red);       dirs.Add(2, Vector2.UnitX);
            abilityColors.Add(3, Color.Blue);      dirs.Add(3, -Vector2.UnitX);
            abilityColors.Add(4, Color.Orange);    dirs.Add(4, -Vector2.UnitY);
        }
        /// <summary>
        /// Initialize a GameState to a level as defined by the level model.
        /// </summary>
        /// <param name="stack"></param>
        /// <param name="level"></param>
        public GameState(GameStateStack stack, SharedContent.LevelModel level)
           : this(stack)
        {
            foreach (SharedContent.CreepSpawner spawner in level.SpawnPoints)
                nonInteractives.Add(new Entity(spawner, Resources.res.content));
            gameObjectives.Add(Resources.getPrototype(level.GameObjectiveAsset));
            leveltime = leveltimeleft = level.Duration;
        }
        /// <summary>
        /// TODO: Implement support for multiple gameobjectives
        /// </summary>
        /// <returns>The current game objective</returns>
        public Entity gameObjective()
        {
            return gameObjectives[0];
        }
        /// <summary>
        /// Give all Entities the opportunity to decide where they want to go and what
        /// they want to do in the game world at this point. This includes things such
        /// as spawning new Entities or updating internal state such as cooldowns.
        /// </summary>
        /// <param name="timedelta">Time passed since last update in seconds</param>
        private void behaviourTick(float timedelta)
        {
            behaviourTick(players, timedelta, projectiles);
            behaviourTick(hostiles, timedelta, hostileProjectiles);
            behaviourTick(projectiles, timedelta, projectiles);
            behaviourTick(nonInteractives, timedelta, hostiles); // nonInteractives may only spawn players
            nonInteractives.AddRange(newNoninteractives);
            newNoninteractives.Clear();
        }

        /// <summary>
        /// Iterates over Entities, let them add new entities to destination if they require
        /// spawning or the like.
        /// </summary>
        /// <param name="gameObjects">The Entities that need to take some decisions</param>
        /// <param name="timedelta">Time passed since last update in seconds</param>
        /// <param name="destination">The list of Entities to which gameObjects may spawn new Entities to.</param>
        private void behaviourTick(List<Entity> gameObjects, float timedelta, List<Entity> destination)
        {
            newObjects = new List<Entity>();
            foreach (Entity gameObject in gameObjects)
            {
                gameObject.Behaviour.decide(gameObject, this, timedelta, stack);
                foreach (IAbility ability in gameObject.Abilities)
                {
                    if (ability.isReady(gameObject, this, timedelta))
                        ability.fire(gameObject, this);
                }
            }
            foreach (Entity newObject in newObjects)
                destination.Add(newObject);
        }
        /// <summary>
        /// Update the positions of all Entities - let them move around in the world according to their
        /// current direction, speed and speed modifiers.
        /// </summary>
        /// <param name="timedelta">Time passed since last update in seconds</param>
        private void physicsTick(float timedelta)
        {
            physicsTick(timedelta, players);
            physicsTick(timedelta, hostiles);
            physicsTick(timedelta, projectiles);
            physicsTick(timedelta, nonInteractives);
            physicsTick(timedelta, hostileProjectiles);
        }
        /// <summary>
        /// Iterate across gameObjects, moving as defined by their physics.
        /// </summary>
        /// <param name="timedelta">Time passed since last update in seconds</param>
        /// <param name="gameObjects">The Entities to update.</param>
        private void physicsTick(float timedelta, List<Entity> gameObjects)
        {
            foreach (Entity ent in gameObjects)
            {
                ent.move(timedelta);
            }
        }
        /// <summary>
        /// Perform collision detection between Entities.
        /// </summary>
        public void collisionTick()
        {
            collisionCheck(projectiles, hostiles); // Players kill hostiles
            collisionCheck(hostiles, gameObjectives); // Hostiles kill gameobjective
            collisionCheck(hostiles, players); // Hostiles kill players
            collisionCheck(hostileProjectiles, players); // Creep projectiles kill players
            collisionCheck(hostileProjectiles, gameObjectives);
            collisionCheck(players, nonInteractives); // Players gain powerups (?)

        }
        /// <summary>
        /// Check for collisions between colliders and obstacles, and delegate
        /// action to collisionhandlers in the event that there are collisions.
        /// THIS METHOD IS NOT REFLEXIVE: a.collide(b) does not imply b.collide(a)
        /// </summary>
        /// <param name="colliders"></param>
        /// <param name="obstacles"></param>
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
        /// <summary>
        /// Add an Entity to the GameState. Depending on the current state of the
        /// gameloop, this may end up in different places - hostiles may only add to
        /// hostile projectiles, nonInteractives can add to hostiles, players can add
        /// to projectiles and so on.
        /// </summary>
        /// <param name="gameObject"></param>
        public void addGameObject(Entity gameObject)
        {
            newObjects.Add(gameObject);
        }
        /// <summary>
        /// Add a player to this game.
        /// </summary>
        /// <param name="player">The player Entity to add.</param>
        public void addPlayer(Entity player)
        {
            Entity crosshair = Resources.getPrototype("Crosshair");
            player.Source = crosshair;
            ((Crosshair)crosshair.Behaviour).control = ((Player)player.Behaviour).control;
            crosshair.Source = player;
            crosshair.Renderable.Tint = Color.Red;
            newNoninteractives.Add(crosshair);
            
            players.Add(player);
        }
        /// <summary>
        /// Renders all entities in param to renderer.
        /// </summary>
        /// <param name="r">Renderer to use.</param>
        /// <param name="gameObjects">Entities to render.</param>
        private void render(Renderer r, List<Entity> gameObjects)
        {
            foreach (Entity gameObject in gameObjects)
            {
                if (gameObject.Renderable == null)
                    continue;
                r.render(gameObject.Renderable, gameObject.Physics);
            }
        }
        /// <summary>
        /// Renders this gamestate.
        /// </summary>
        /// <param name="r">Renderer to use.</param>
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

            renderTimeBar(r);
            Vector2 rpos = new Vector2(50, r.Screenspace.Size.Y / (players.Count + 1));
            foreach (Entity p in this.players)
            {
                renderCooldowns(r, p, rpos);
                rpos.Y += r.Screenspace.Size.Y / (players.Count + 1);
            }

            int playerNo = 1;

            foreach (Entity p in players)
            {
                Vector2 scorePos = new Vector2(r.Screenspace.Size.X - 215, 50 + playerNo * 25);
                r.renderText("Player " + playerNo++ + " kills: " + p.Stats.Kills, scorePos, Color.White);
            }
        }
        /// <summary>
        /// Render the ability cooldowns belonging to player.
        /// </summary>
        /// <param name="r">Renderer to use</param>
        /// <param name="player">Player whose cooldowns to render</param>
        /// <param name="rpos">Position to render them too</param>
        private void renderCooldowns(Renderer r, Entity player, Vector2 rpos)
        {
            int end = Math.Max(player.Abilities.Count, 4);
            for (int a = 1; a != end; ++a)
            {
                if (player.Abilities[a].Stats == null) continue;
                float s = 1 - Math.Max(player.Abilities[a].Stats.CurrentCooldown / player.Abilities[a].Stats.Cooldown, 0);
                this.pie.Scale = s * Vector2.One;
                this.pie.Tint = abilityColors[a];
                r.render(pie, rpos, dirs[a]);
            }

            for (int i = 0; i != player.Stats.Levels; ++i)
            {
                float s = (float)Math.Pow(1.1f, i);
                this.ring.Scale = s * Vector2.One;
                r.render(this.ring, rpos, Vector2.UnitX);
            }
            this.ring.Scale = Vector2.One;
        }
        /// <summary>
        /// Render the amount of time left of current level.
        /// </summary>
        /// <param name="r">Renderer to use.</param>
        private void renderTimeBar(Renderer r)
        {
            r.beginScreen();
            float left = 0.2f * r.Screenspace.Size.X;
            float right = 0.8f * r.Screenspace.Size.X;
            float top = 0.025f * r.Screenspace.Size.Y;
            float bottom = 0.05f * r.Screenspace.Size.Y;
            r.renderRect(new Vector2(left, top), new Vector2(right, bottom), Color.White);
            float timeleft = (right - left) * this.leveltimeleft / this.leveltime + left;
            r.renderRect(new Vector2(left, top), new Vector2(timeleft, bottom), Color.White, true);
        }
        /// <summary>
        /// Render the health bars that belong to objects.
        /// </summary>
        /// <param name="r">Renderer to use.</param>
        /// <param name="objects">Entities with health and maximum health.</param>
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

                r.renderRect(topleft, bottomRight, Color.Red);
                r.renderRect(topleft, lastOne, Color.Green, true);
            }
        }
        /// <summary>
        /// Perform a status check to remove all dead Entities from param.
        /// </summary>
        /// <param name="gameObjects">Entities to filter.</param>
        /// <param name="timedelta">Time since last update</param>
        /// <returns>All dead Entities</returns>
        private List<Entity> statusCheck(List<Entity> gameObjects, float timedelta)
        {
            List<Entity> dead = new List<Entity>();
            foreach (Entity gameObject in gameObjects)
            {
                if (!gameObject.Status.isAlive(gameObject, timedelta))
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
        /// <summary>
        /// Spawn explosion
        /// </summary>
        /// <param name="pos">The position to spawn an explosion.</param>
        private void explode(Vector2 pos)
        {
            Entity explosion = new Entity();
            explosion.Behaviour = new DummyBehaviour();
            explosion.Status = new ExpireCheck(1.8f);
            explosion.Physics = new Physics(0);
            explosion.Physics.Position = pos;
            Sprite expl = new Sprite(Resources.getRes("TheExplosion"));
            expl.Frames = 18;
            expl.FrameTime = 0.1f;
            explosion.Renderable = expl;
            newNoninteractives.Add(explosion);
        }
        /// <summary>
        /// Perform a game update. The algorithm for this is as follows:</br>
        /// - check if the game is ended
        /// - if not, move all entities according to physics
        /// - let entities decide what to do next
        /// - resize area of gamestate that is rendered
        /// - collision detection and collision handling
        /// - removal of dead creeps
        /// -- add xp to players for dead creeps
        /// -- render explosions where creeps died
        /// - removal of other non-player entities
        /// - start respawn timer for dead players if any
        /// </summary>
        /// <param name="timedelta">Time since last update.</param>
        public void tick(float timedelta) 
        {
            this.leveltimeleft -= timedelta;
            if (this.leveltimeleft < 0.0f && hostiles.Count == 0)
            {
                // You've won, congratulations.
                this.stack.pop();
                this.stack.push(new WinMenu(this.stack));
            }
            physicsTick(timedelta);
            behaviourTick(timedelta);
            
            updateViewport();
            collisionTick();

            List<Entity> dead = statusCheck(hostiles, timedelta); // Returns list of dead hostiles
            foreach (Entity d in dead)
            {
                explode(d.Physics.Position);
                grantXp(d.Stats.Level * 50);
            }
             // Returns list of dead hostiles
            statusCheck(projectiles, timedelta);
            if (statusCheck(gameObjectives, timedelta).Count > 0)
            {
                this.stack.pop();
                this.stack.push(new LoseMenu(this.stack));
            }
            foreach (Entity player in statusCheck(players, timedelta))
            {
                Entity respawner = new Entity();
                respawner.Behaviour = new RespawnPlayer(player, 7.5f + 7.5f * player.Stats.Level);
                respawner.CombatStats.Health = 1;
                nonInteractives.Add(respawner);
            }
            statusCheck(hostileProjectiles, timedelta);
            statusCheck(nonInteractives, timedelta);
            
        }
        /// <summary>
        /// Hands out experience that is shared between all players.
        /// </summary>
        /// <param name="amount">The amount of experience to hand out</param>
        private void grantXp(int amount)
        {
            foreach (Entity player in players)
                player.Stats.grantXp(amount / players.Count);
        }
        /// <summary>
        /// Resize area to render to fit all 'interesting' Entities into screen.
        /// </summary>
        private void updateViewport()
        {
            Vector2 pad = 150 * Vector2.One;
            
            List<Vector2> positions = new List<Vector2>();
            foreach (Entity player in players)
            {
                positions.Add(player.Physics.Position);
                positions.Add(player.Source.Physics.Position);
            }
            positions.Add(gameObjective().Physics.Position);
            positions.Add(-100 * Vector2.One);
            positions.Add(100 * Vector2.One);
            Vector2 min, max;
            Util.VectorUnion(positions, out min, out max);
            min -= pad;
            max += pad;
            
            float aspect = view.Aspect;
            view.TopLeft = min;
            view.BottomRight = max;
            view.preserveAspect(aspect);
        }
    }
}
