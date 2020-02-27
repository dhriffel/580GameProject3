using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace _580GameProject4
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteSheet sheet;
        SpriteFont font;
        Player player;
        List<Raindrop> raindrops;
        IEnumerable<Sprite> raindropFrames;
        Random random;
        TimeSpan timer, wellTimer;
        AxisList world;

        int score, highScore;
        Sprite wellFrame;
        BoundingRectangle wellSquare;
        int DRIPRATE;
        internal static int ScreenWidth;
        internal static int ScreenHeight;
        private Camera camera;

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

            ScreenHeight = graphics.PreferredBackBufferHeight;
            ScreenWidth = graphics.PreferredBackBufferWidth;

            wellSquare = new BoundingRectangle(-100, 600, 400, 400);
            random = new Random(DateTime.Now.Second);
            timer = new TimeSpan(0);
            wellTimer = new TimeSpan(0);

            score = 0; 
            highScore = 0;

            DRIPRATE = 2000;

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
            var t = Content.Load<Texture2D>("spriteSheet");
            sheet = new SpriteSheet(t, 400, 400);
            font = Content.Load<SpriteFont>("File");
            var playerFrames = from index in Enumerable.Range(4, 2) select sheet[index];
            raindropFrames = from index in Enumerable.Range(0, 4) select sheet[index];
            wellFrame = sheet[8];
            camera = new Camera();
            player = new Player(this, playerFrames);
            world = new AxisList();
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
            wellTimer += gameTime.ElapsedGameTime;
            if (timer.TotalMilliseconds >= DRIPRATE)
            {
                var drop = new Raindrop(raindropFrames, random.Next(500, 1500), random.Next(3));
                raindrops.Add(drop);
                world.AddGameObject(drop);
                timer = new TimeSpan(0);
                DRIPRATE = 400;
            }

            if(wellTimer.TotalSeconds >= 15)
            {
                
                if (score > highScore)
                    highScore = score;
                score = 0;
                wellTimer = new TimeSpan(0);
            }
            


            if (player.bounds.CollidesWith(wellSquare) & player.IsFull())
            {
                player.EmptyBucket();
                wellTimer = new TimeSpan(0);
                score++;
            }

            var platformQuery = world.QueryRange(player.bounds.Y, player.bounds.Y + player.bounds.Height);
            player.CheckForPlatformCollision(platformQuery);

            foreach (Raindrop drop in raindrops)
            {
                drop.Update(gameTime);
                world.UpdateGameObject(drop);
                if (drop.Position.Y > graphics.PreferredBackBufferHeight)
                    drop.needsRemoved = true;
                if (drop.needsRemoved == true)
                    world.RemoveGameObject(drop);
            }

            raindrops.RemoveAll(drop => drop.needsRemoved == true);

            camera.Follow(player);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGoldenrod);
            spriteBatch.Begin(transformMatrix: camera.Transform);

            // TODO: Add your drawing code here
            player.Draw(spriteBatch);
            wellFrame.Draw(spriteBatch, wellSquare, Color.White);

            foreach (Raindrop drop in raindrops)
            {
                drop.Draw(spriteBatch);
            }
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(font, $"Highscore: {highScore}", new Vector2(0, 10), Color.White);
            spriteBatch.DrawString(font, $"Score: {score}", new Vector2(0, 50), Color.White);
            spriteBatch.DrawString(font, $"Bucket Water Level: {(double)player.state.currentLevel / player.MAXFILLLEVEL*100}%",new Vector2(0,90), Color.White);
            spriteBatch.DrawString(font, $"Time: {(int)wellTimer.TotalSeconds}", new Vector2(0, 130), Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
