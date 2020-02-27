using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _580GameProject3
{
    class Full : Fullness
    {
        public Full(Fullness state)
        {
            currentLevel = state.currentLevel;
            player = state.player;
            Initialize();
        }

        private void Initialize()
        {
            currentLevel = 10;
            isFull = true;
        }
        public override void AddDrop()
        {
            return;
        }
        public override void FullnessCheck()
        {
            if (currentLevel < 10)
            {
                player.state = new NotFull(this);
            }

        }
    }
}


