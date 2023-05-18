using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.States;
using Sprint1.Entities;

namespace Sprint1.Commands
{
    internal class TakeDamageCommand : ICommand
    {
        PlayerEntity reciever;

        public TakeDamageCommand(PlayerEntity reciever)
        {
            this.reciever = reciever;
        }

        public void Execute()
        {
            reciever.takeDamage();
        }
    }
}
