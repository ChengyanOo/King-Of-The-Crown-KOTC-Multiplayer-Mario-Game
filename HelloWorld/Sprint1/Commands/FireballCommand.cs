using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.States;
using Sprint1.Entities;

namespace Sprint1.Commands
{
    internal class FireballCommand : ICommand
    {
        PlayerEntity receiver;

        public FireballCommand(PlayerEntity receiver)
        {
            this.receiver = receiver;
        }
        
        public void Execute()
        {
            receiver.Fire();
        }
    }
}
