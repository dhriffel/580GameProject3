using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _580GameProject3
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteSheet sheet;
        Player player;
        List<Raindrop> raindrops;
        IEnumerable<Sprite> raindropFrames;
        Random random;
        TimeSpan timer;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            raindrops = new List<Raindrop>();
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
            graphics.PreferredBackBufferWidth = 1500;
            graphics.PreferredBackBufferHeight = 1000;
            graphics.ApplyChanges();

            random = new Random(DateTime.Now.Second);
            timer = new TimeSpan(0);


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //player.LoadContent(Content);
            var t = Content.Load<Texture2D>("spriteSheet");
            sheet = new SpriteSheet(t, 400, 400);

            var playerFrames = from index in Enumerable.Range(4, 2) select sheet[index];
            raindropFrames = from index in Enumerable.Range(0, 4) select sheet[index];
            player = new Player(this, playerFrames);
            //raindrop = new Raindrop(raindropFrames, 100);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            player.Update(gameTime);

            timer += gameTime.ElapsedGameTime;
            if (timer.TotalSeconds >= 2)
            {
                raindrops.Add(new Raindrop(raindropFrames, random.Next(1500), random.Next(3)));
                timer = new TimeSpan(0);
            }
            foreach(Raindrop drop in raindrops)
            {
                drop.Update(gameTime);
                if (drop.Position.Y > graphics.PreferredBackBufferHeight)
                    drop.needsRemoved = true;
                if (drop.bounds.CollidesWith(player.bounds))
                    if (!player.IsFull())
                    {
                        drop.needsRemoved = true;
                        player.AddDrop();
                    }
            }

            raindrops.RemoveAll(drop => drop.needsRemoved == true);

            //raindrop.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            // TODO: Add your drawing code here
            player.Draw(spriteBatch);

            foreach (Raindrop drop in raindrops)
            {
                drop.Draw(spriteBatch);
            }
            //raindrop.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
