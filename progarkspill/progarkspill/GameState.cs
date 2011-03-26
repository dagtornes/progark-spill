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
        private Entity gameObjective; // Work this one out somewhere

        private GameStateStack stack;
        private Viewport view;
        public Texture2D BulletSprite { get; set; }
        private List<Entity> newObjects = new List<Entity>();

        public List<Entity> Players
        {
            get { return players; }
            set { players = value; }
        }
        
        public bool tickDown { get { return false; } }
        public bool renderDown { get { return false; } }

        public GameState()
        {
            this.view = new Viewport(Vector2.Zero, 500*(Vector2.One + 0.667f*Vector2.UnitX));
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
                gameObject.Behavior.decide(gameObject, this, timedelta, stack);
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
                ent.move(timedelta);
        }

        public GameState(GameStateStack stack)
        {
            this.stack = stack;
            this.view = new Viewport(Vector2.Zero, 500 * (Vector2.One + 0.667f * Vector2.UnitX));
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
                r.render(gameObject.Renderable, gameObject.Physics);
            }
        }

        public void render(Renderer r)
        {
            r.begin(view);
            render(r, projectiles);
            render(r, hostiles);
            render(r, nonInteractives);
            render(r, players);
        }

        public void tick(float timedelta) 
        {
            behaviourTick(timedelta);
            physicsTick(timedelta);
            
            // Collision pass
            // Status pass

        }


    }
}
