using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _580GameProject3
{
    class Raindrop : IBoundable
    {
        float scale = 0.15f;

        public BoundingRectangle Bounds;
        public BoundingRectangle bounds => Bounds;

        Sprite[] sprite;

        Vector2 origin = new Vector2(200,400);

        public Vector2 Position;

        float velocity;

        int velocityModifier;

        TimeSpan timer;

        int currentFrame = 0;

        const int ANIMATION_FRAME_RATE = 124;

        public bool needsRemoved = false;


        /// <summary>
        /// Constructs a new platform
        /// </summary>
        /// <param name="bounds">The platform's bounds</param>
        /// <param name="sprite">The platform's sprite</param>
        public Raindrop(IEnumerable<Sprite> frames, int x, int currentFrame)
        {
            this.sprite = frames.ToArray();
            this.currentFrame = currentFrame;
            Position = new Vector2(x, 0);
            Bounds = new BoundingRectangle(Position.X - (int)(origin.X * scale), Position.Y - (int)(origin.Y * scale), 40, 40);
            timer = new TimeSpan(0);
            velocityModifier = currentFrame;

        }

        public void Update(GameTime gameTime)
        {

            velocity = velocity + (velocityModifier *0.15f);
            Position.Y += velocity;// Position.Y + velocity;
            Bounds.Y += velocity;

            timer += gameTime.ElapsedGameTime;

            while (timer.TotalMilliseconds > ANIMATION_FRAME_RATE)
            {
                // increase by one frame
                currentFrame++;
                // reduce the timer by one frame duration
                timer -= new TimeSpan(0, 0, 0, 0, ANIMATION_FRAME_RATE);
            }

            currentFrame %= 4;
        }
        /// <summary>
        /// Draws the platform
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch to render to</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            sprite[currentFrame].Draw(spriteBatch, Position, Color.White, 0, origin, scale, SpriteEffects.None, 1);
            //sprite[currentFrame].Draw(spriteBatch, bounds, Color.White);
        }
    }
}
