using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Collisions;
using Sprint1.Entities;

namespace Sprint1.States.CrownStates
{
    public interface ICrownState
    {
        void toFloating();
        void toThrown();
        void toAttached(PlayerEntity playerEntity);
        
        void Enter(ICrownState previousState);
        void Exit();

        void collision(ICollidable collidee, int direction);
    }
}