using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using progarkspill.GameObjects;
using SharedContent;
using progarkspill.GameObjects.Behaviours;

namespace progarkspill
{
    class MainMenu : IGameState
    {
        private GameStateStack states;
        private List<Entity> players;
        private List<SharedContent.LevelModel> levelProtos;
        private int selectedLevel;
        private List<PlayerIndex> controllers;
        private ContentManager Content;

        public bool tickDown { get { return false; } }
        public bool renderDown { get { return false; } }

        public MainMenu(GameStateStack gamestates, ContentManager Content)
        {
            states = gamestates;
            this.Content = Content;

            List<String> levels = Content.Load<List<String>>("Levels/Levels");
            levelProtos = new List<SharedContent.LevelModel>();
            foreach (String name in levels)
                levelProtos.Add(Content.Load<SharedContent.LevelModel>("Levels/" + name));
            selectedLevel = 0;

            controllers = new List<PlayerIndex>();
            if (GamePad.GetState(PlayerIndex.One).IsConnected) controllers.Add(PlayerIndex.One);
            if (GamePad.GetState(PlayerIndex.Two).IsConnected) controllers.Add(PlayerIndex.Two);
            if (GamePad.GetState(PlayerIndex.Three).IsConnected) controllers.Add(PlayerIndex.Three);
            if (GamePad.GetState(PlayerIndex.Four).IsConnected) controllers.Add(PlayerIndex.Four);
            players = new List<Entity>();
        }

        public void render(Renderer r) {
            r.beginScreen();
            Vector2 pos = 50 * Vector2.One;
            foreach (PlayerIndex p in controllers)
            {
                r.renderText(String.Format("Player {0} is connected", p), pos, Color.White, false);
                pos += 30 * Vector2.UnitY;
            }
            r.end();
        }

        private void addPlayer(PlayerIndex controller)
        {
            EntityModel playerPrototype = Content.Load<EntityModel>("EntityProtoTypes/PlayerPrototype");
            Entity player = new Entity(playerPrototype, Content);
            ((Player)player.Behaviour).control = controller;
            player.Abilities[0].bind(controller, Buttons.RightTrigger);
            player.Abilities[1].bind(controller, Buttons.A); // Gravity Well
            player.Abilities[2].bind(controller, Buttons.B); // Blink
            player.Abilities[3].bind(controller, Buttons.X); // SpeedBoost
            IAbility l1 = new GameObjects.Abilities.LevelUp(player.Abilities[1]);
            IAbility l2 = new GameObjects.Abilities.LevelUp(player.Abilities[2]);
            IAbility l3 = new GameObjects.Abilities.LevelUp(player.Abilities[3]);
            l1.bind(controller, Buttons.DPadDown);
            l2.bind(controller, Buttons.DPadRight);
            l3.bind(controller, Buttons.DPadLeft);
            player.Abilities.Add(l1);
            player.Abilities.Add(l2);
            players.Add(player);
        }

        public void tick(float timedelta) {
            if (Controller.RecentlyPressed(PlayerIndex.One, Buttons.Start, 0.5f))
            {
                //states.push(new GameState(states, levelProtos[0]));
                foreach (PlayerIndex controller in controllers)
                {
                    Console.WriteLine("Controller: {0}", controller);
                    addPlayer(controller);
                }
                states.push(GameState.Create(states, levelProtos[selectedLevel], players));
                players.Clear();
            }
            if (Controller.RecentlyPressed(PlayerIndex.One, Buttons.Back, 0.5f))
                states.pop();
            //if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back))
            //    states.pop();
            
        }

    }
}