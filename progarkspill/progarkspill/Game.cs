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
        List<Player> players = new List<Player>();
        Entity corner;
        Stats statsTests;


        public Game()
        {
            Content.RootDirectory = "Content";
            states.push(new GameState(states));
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
            System.Console.WriteLine(statsTests.Armor);
           
            Texture2D tex = Content.Load<Texture2D>("ship9km");
            Texture2D tex2 = Content.Load<Texture2D>("bullet");

            // Behaviour for Entities
            Player playerOne = new Player(PlayerIndex.One);
            Player playerTwo = new Player(PlayerIndex.Two);
            ((GameState) states.peek()).BulletSprite = tex2;

            Entity p1 = new Entity();
            p1.Behaviour = playerOne;
            p1.CombatStats = CombatStats.defaultShip();
            p1.Physics = new Physics(200);
            p1.Renderable = new Sprite(tex);
            p1.Status = new Status();

            ((GameState)states.peek()).addPlayer(p1);

            corner = new Entity();
            corner.Physics = new Physics(0);
            corner.Physics.Position = new Vector2(0,0);
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
