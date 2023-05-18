using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.Commands
{
    internal class StartCommand : ICommand
    {
        Game1 reciever;

        public StartCommand(Game1 reciever)
        {
            this.reciever = reciever;
        }

        public void Execute()
        {
            reciever.StartGame();
        }
    }
}
