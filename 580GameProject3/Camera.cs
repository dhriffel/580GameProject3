using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _580GameProject4
{
    public class Camera
    {
        public Matrix Transform { get; private set; }

        public void Follow(Player target)
        {
            //Transform = 
                
            var position = Matrix.CreateTranslation(-target.Position.X, -target.Position.Y, 0);
            var offset = Matrix.CreateTranslation(Game1.ScreenWidth / 2, Game1.ScreenHeight, 0);

            Transform = position * offset;
        }
    }
}
