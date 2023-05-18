using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Entities;
using Sprint1.Transformations;

namespace Sprint1.States.PowerStates
{
    public class SuperState : PowerState
    {
        public SuperState(PlayerEntity entity, IPowerState previousState) : base(entity, previousState)
        { }

        public override void takeDamage()
        {
            toSmall();
        }
    }
}