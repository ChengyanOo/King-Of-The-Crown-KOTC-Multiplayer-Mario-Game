
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Sprint1.Transformations
{
    public class TransformationMovements
    {

        private bool moving = false;
        private bool right;
        private int offset = 0;

        public TransformationMovements()
        {
            right = true;
            //moving = false;
           
        }

        public Vector2 applyJump(Vector2 position)
        {
            Vector2 newPosition = position;
            if (newPosition.Y > 0 )
            {
                newPosition.Y-=offset;
            }
            //moving = true;
          
            return newPosition;
        }

        public Vector2 applyLeftRun(Vector2 position)
        {
            Vector2 newPosition = position;
            if (right && newPosition.X > 0)
            {
                newPosition.X-=offset;
            }
           // moving = true;

            return newPosition;
        }

        public Vector2 applyRightRun(Vector2 position)
        {
            Vector2 newPosition = position;
            if (right)//can change x direction
            {
                newPosition.X+=offset;
            }
            //moving = true;

            return newPosition;
        }
        public Vector2 applyIdle(Vector2 position)
        {
            Vector2 newPosition = position;
           // moving = false;
            return newPosition;
        }
        public Vector2 applyDown(Vector2 position)
        {
            Vector2 newPosition = position;
            newPosition.Y+=offset;
            
            //moving = true;

            return newPosition;
        }

        public Boolean isMoving()
        {
            return moving;
        }
    }
}

