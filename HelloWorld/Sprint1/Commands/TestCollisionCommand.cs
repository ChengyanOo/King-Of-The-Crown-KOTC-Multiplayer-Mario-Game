using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.States;
using Sprint1.Entities;
using Sprint1.Collisions;

namespace Sprint1.Commands
{
    internal class TestCollisionCommand : ICommand
    {
        private IEntity reciever;
        private ICollidable collidee;
        private int direction;

        public TestCollisionCommand(IEntity reciever, ICollidable collidee, int direction)
        {
            this.reciever = reciever;
            this.collidee = collidee;
            this.direction = direction;
        }

        public void Execute()
        {
            reciever.OnCollisionEnter(collidee, direction);
        }
    }
}
