using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Sprites;
using Sprint1;
using Sprint1.Collisions;

namespace Sprint1.Commands
{
    internal class ToggleVisibleCollidersCommand : ICommand
    {
        private CollisionDetector2 reciever;
        
        public ToggleVisibleCollidersCommand(CollisionDetector2 reciever)
        {
            this.reciever = reciever;
        }

        public void Execute()
        {
            reciever.toggleVisibility();
        }
    }
}
