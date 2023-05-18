using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Sprites;

namespace Sprint1.Commands
{
    internal class ToggleCommand : ICommand
    {
        ISprite reciever;

        public ToggleCommand(ISprite reciever)
        {
            this.reciever = reciever;
        }

        public void Execute()
        {
            reciever.IsVisible = !reciever.IsVisible;
        }
    }
}
