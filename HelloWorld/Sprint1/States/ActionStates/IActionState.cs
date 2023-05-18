using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint1.Collisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.States.ActionStates
{
    public interface IActionState
    {
        void toIdle();
        void toCrouching();
        void toJumping();
        void toFalling();
        void toRunning();
        void toPrevious();

        void faceLeft();
        void faceRight();
        void faceUp();
        void faceDown();
        
        void releaseLeft();
        void releaseRight();
        void releaseUp();
        void releaseDown();

        void Enter(IActionState previousState);
        void Exit();
    }
}