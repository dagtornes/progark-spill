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
            this.pie.Origin = new Vector2(0.0f, pie.Origin.Y);
            abilityColors.Add(1, Color.Green);     dirs.Add(1, Vector2.UnitY);
            abilityColors.Add(2, Color.Red);       dirs.Add(2, Vector2.UnitX);
            abilityColors.Add(3, Color.Blue);      dirs.Add(3, -Vector2.UnitX);
            abilityColors.Add(4, Color.Orange);    dirs.Add(4, -Vector2.UnitY);
        }
        public GameState(GameStateStack stack, SharedContent.LevelModel level)
           : this(stack)
        {
            foreach (SharedContent.CreepSpawner spawner in level.SpawnPoints)
                nonInteractives.Add(new Entity(spawner, Resources.res.content));
            gameObjectives.Add(Resources.getPrototype(level.GameObjectiveAsset));
            leveltime = leveltimeleft = level.Duration;
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
                    if (ability.isReady(gameObject, this, timedelta))
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
            ((Crosshair)crosshair.Behaviour).control = ((Player)player.Behaviour).control;
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

            renderTimeBar(r);
            Vector2 rpos = new Vector2(50, r.Screenspace.Size.Y / (players.Count + 1));
            foreach (Entity p in this.players)
            {
                renderCooldowns(r, p, rpos);
                rpos.Y += r.Screenspace.Size.Y / (players.Count + 1);
            }
        }

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
        private void grantXp(int amount)
        {
            foreach (Entity player in players)
                player.Stats.grantXp(amount / players.Count);
        }
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
