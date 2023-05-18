using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.Commands
{
    internal class ResetCommand : ICommand
    {
        Game1 reciever;

        public ResetCommand(Game1 reciever)
        {
            this.reciever = reciever;
        }

        public void Execute()
        {
            reciever.ResetGame();
            reciever.isGameOver = false;
            reciever.isWon = false;
            reciever.timeTracker.time = 0;
        }
    }
}
