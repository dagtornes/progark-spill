using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace progarkspill
{
    class GameState : IGameState
    {
        private List<Entity> gameObjects = new List<Entity>(); // These are objects with velocities and the like
        private List<Player> players = new List<Player>(); // These are objects that need updating to let player control
        private GameStateStack stack;
        private Viewport view;

        public List<Player> Players
        {
            get { return players; }
            set {
                foreach (Player p in value)
                    gameObjects.Add(p.Hero);
                foreach (Player p in players)
                    gameObjects.Remove(p.Hero);
                players = value; 
            }
        }
        public GameState()
        {
            this.view = new Viewport(Vector2.Zero, 500*(Vector2.One + Vector2.UnitX));
        }

        public GameState(GameStateStack stack)
        {
            this.stack = stack;
            this.view = new Viewport(Vector2.Zero, Vector2.One);
        }

        public void render(Renderer r)
        {
            r.begin(view);
            foreach (Entity gameObject in gameObjects)
                gameObject.render(r);
        }

        public void tick(float timedelta) 
        {

            foreach (Entity gameObject in gameObjects)
            {
                view.fit(gameObject);
                gameObject.move(timedelta);
            }
            foreach (Player p in Players)
                p.update(timedelta, null, stack);
        }


    }
}
