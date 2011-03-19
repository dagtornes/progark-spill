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

namespace progarkspill
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        Renderer renderer;
        GameState state = new GameState(); // TODO: Factor out into GameStateStack
        GraphicsDeviceManager gdm;
        Viewport view;
        Entity ent;
        List<Player> players = new List<Player>();

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
            
            renderer = new Renderer(gdm);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
           
            Texture2D tex = Content.Load<Texture2D>("ship9km");
            ent = new Entity(Vector2.Zero, Vector2.One, tex);
            Player playerOne = new Player(state, PlayerIndex.One);
            playerOne.Hero = ent;
            Player two = new Player(state, PlayerIndex.Two);
            two.Hero = new Entity(new Vector2(100, 0), Vector2.Zero, tex);
            players = new List<Player>();
            players.Add(playerOne);
            players.Add(two);
            state.Players = players;
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            base.Update(gameTime);
            float seconds = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            state.tick(seconds);
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
            state.render(renderer);
        }
    }
}
