using Sprint1.States.GameState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.Commands
{
    internal class PauseCommand : ICommand
    {
        Game1 reciever;

        public PauseCommand(Game1 reciever)
        {
            this.reciever = reciever;
            //isPaused = false;
        }

        public void Execute()
        {
            reciever.changeIsPaused();
        }
    }
}
