using _580GameProject4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _580GameProject3
{
    class NotFull : Fullness
    {
        public NotFull(Fullness state)
        {
            currentLevel = state.currentLevel;
            player = state.player;
            Initialize();
        }
        public NotFull(Player player)
        {
            this.player = player;
            Initialize();
        }

        private void Initialize()
        {
            currentLevel = 0;
            isFull = false;
        }
        public override void AddDrop()
        {
            currentLevel+= 1;
            FullnessCheck();
        }
        public override void FullnessCheck()
        {
            if(currentLevel == 10)
            {
                player.state = new Full(this);
            }

        }
    }
}
