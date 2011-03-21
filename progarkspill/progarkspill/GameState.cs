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
        private List<Entity> gameObjects = new List<Entity>(); // These are objects with velocities and the like
       private GameStateStack stack;
        private Viewport view;
        public Texture2D BulletSprite { get; set; }

        public GameState()
        {
            this.view = new Viewport(Vector2.Zero, 500*(Vector2.One + 0.667f*Vector2.UnitX));
        }

        public GameState(GameStateStack stack)
        {
            this.stack = stack;
            this.view = new Viewport(Vector2.Zero, Vector2.One);
        }
        public void addGameObject(Entity gameObject)
        {
            gameObjects.Add(gameObject);
        }
        public void render(Renderer r)
        {
            r.begin(view);
            foreach (Entity gameObject in gameObjects)
            {
                r.render(gameObject.Renderable, gameObject.Physics);
            }
        }

        public void tick(float timedelta) 
        {
            // Behaviour pass
            foreach (Entity gameObject in gameObjects)
            {
                gameObject.Behavior.decide(gameObject, this, timedelta, null, null);
            }

            // Physics pass
            foreach (Entity gameObject in gameObjects)
            {
                view.fit(gameObject);
                gameObject.move(timedelta);
            }

            // Status pass
            List<Entity> deActivate = new List<Entity>();
            foreach (Entity gameObject in gameObjects)
                if (!gameObject.Status.isAlive(gameObject))
                    deActivate.Add(gameObject);
            foreach (Entity dead in deActivate)
                gameObjects.Remove(dead);
        }


    }
}
