using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.States;
using Sprint1.Entities;

namespace Sprint1.Commands
{
    internal class ReleaseRightCommand : ICommand
    {
        PlayerEntity receiver;

        public ReleaseRightCommand(PlayerEntity receiver)
        {
            this.receiver = receiver;
        }

        public void Execute()
        {
            receiver.releaseRight();
        }
    }
}
