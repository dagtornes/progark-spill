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

        public GameState()
        {
            players.Add(new Player(PlayerIndex.One));
        }
        public void render(Renderer r)
        {
            foreach (Entity gameObject in gameObjects)
                if (gameObject is IRenderable)
                    r.renderMe((IRenderable) gameObject);

        }

        public void tick(GameTime timedelta) 
        {

            foreach (Entity gameObject in gameObjects)
            {
                gameObject.move(timedelta);
            }
        }


    }
}
