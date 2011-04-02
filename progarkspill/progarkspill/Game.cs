using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using progarkspill.GameObjects;
using SharedContent;
using progarkspill.GameObjects.Behaviours;

namespace progarkspill
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        Renderer renderer;
        GameStateStack states = new GameStateStack();
        GraphicsDeviceManager gdm;
        Viewport view;
        Entity ent;
        EntityModel playerPrototype;
        List<Player> players = new List<Player>();
        Stats statsTests;


        public Game()
        {
            Content.RootDirectory = "Content";
            gdm = new GraphicsDeviceManager(this);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            Resources.init(Content);
            //states.push(new GameState(states, Content.Load<SharedContent.LevelModel>("Levels/DemoLevel")));
            states.push(new MainMenu(states, Content));
            renderer = new Renderer(gdm, Content);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            statsTests = Content.Load<Stats>("InitialCombatStats");
            playerPrototype = Content.Load<EntityModel>("EntityProtoTypes/PlayerPrototype");
            Entity player = new Entity(playerPrototype, Content);
            player.Abilities[0].bind(PlayerIndex.One, Buttons.LeftTrigger);
            System.Console.WriteLine(statsTests.Armor);
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (states.isEmpty())
                this.Exit();

            // TODO: Add your update logic here
            base.Update(gameTime);
            float seconds = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            states.tick(seconds);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            renderer.preRender();
            // TODO: Add your drawing code here
            base.Draw(gameTime);
            states.render(renderer);
        }
    }
}
