using _580GameProject4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _580GameProject3
{
    public abstract class Fullness
    {
        public Player player { get; set; }
        public int currentLevel { get; set; }
        public bool isFull;

        public abstract void AddDrop();
        public void EmptyDrops() {
            currentLevel = 0;
            isFull = false;
            player.state = new NotFull(this);
        }
        public abstract void FullnessCheck();
    }
}
