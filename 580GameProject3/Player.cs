using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _580GameProject3;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _580GameProject4
{
    public class Player
    {
        Game1 game;

        float scale = 0.25f;

        Sprite[] sprite;

        Vector2 origin = new Vector2(200, 400);

        public Vector2 Position;

        public BoundingRectangle bounds;

        float velocity;

        public int fillLevel = 0;

        public int MAXFILLLEVEL = 10;

        int fullCheck = 0;

        public Fullness state;

        public Player(Game1 game, IEnumerable<Sprite> frames)
        {
            this.sprite = frames.ToArray();
            this.game = game;
            Position = new Vector2(game.GraphicsDevice.Viewport.Width / 2, game.GraphicsDevice.Viewport.Height);
            bounds = new BoundingRectangle(Position.X - (int)(origin.X * scale), Position.Y - (int)(origin.Y * scale), 100, 100);
            state = new NotFull(this);
        }

        public bool IsFull()
        {
            return state.isFull;
        }
        public void AddDrop()
        {
            state.AddDrop();
        }
        public void EmptyBucket()
        {
            state.EmptyDrops();
        }

        public void CheckForPlatformCollision(IEnumerable<IBoundable> drops)
        {
            Debug.WriteLine($"Checking collisions against {drops.Count()} drops");

            foreach (Raindrop drop in drops)
            {
                if (bounds.CollidesWith(drop.bounds) & drop.needsRemoved == false & !state.isFull)
                {

                    drop.needsRemoved = true;
                    //AddDrop();
                    state.AddDrop();
                    

                }
            }
        }

        public void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();


            // Horizontal movement
            if (keyboard.IsKeyDown(Keys.Left))
            {
                velocity -= 2;
                //Position.X -= speed;
                //bounds.X -= speed;
            }
            else if (keyboard.IsKeyDown(Keys.Right))
            {
                velocity += 2;
                //Position.X += speed;
                //bounds.X += speed;
            }

            Position.X += velocity;
            bounds.X += velocity;
            //bounds.X = Position.X;
            velocity = velocity / (1.1f + state.currentLevel * 0.02f);

            if (bounds.X + bounds.Width >= game.GraphicsDevice.Viewport.Width)
            {
                Position.X = game.GraphicsDevice.Viewport.Width - bounds.Width / 2;
                bounds.X = game.GraphicsDevice.Viewport.Width - bounds.Width;
                velocity = 0;
            }
            if (bounds.X <= 0)
            {
                Position.X = bounds.Width / 2;
                bounds.X = 0;
                velocity = 0;
            }


            if (fillLevel >= MAXFILLLEVEL)
                fullCheck = 1;

            Debug.WriteLine($"Bucket's fill level is {state.currentLevel}");
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            sprite[fullCheck].Draw(spriteBatch, Position, Color.White, 0, origin, 0.25f, SpriteEffects.None, 1);
            //sprite[fullCheck].Draw(spriteBatch, bounds, Color.White);
        }
    }
}
