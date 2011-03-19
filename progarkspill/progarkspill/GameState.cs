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
        }

        public GameState(GameStateStack stack)
        {
            this.stack = stack;
        }

        public void render(Renderer r)
        {
            foreach (Entity gameObject in gameObjects)
                gameObject.render(r);
        }

        public void tick(float timedelta) 
        {

            foreach (Entity gameObject in gameObjects)
            {
                gameObject.move(timedelta);
            }
            foreach (Player p in Players)
                p.update(timedelta, null, stack);
        }


    }
}
