
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint1.Entities;

namespace Sprint1.Transformations
{
    public class EnemyMovement
    {
        private int offset = 0;
     
        private bool right;

        public EnemyMovement()
        {
            right = true;
            //moving = false;
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
            if (right && newPosition.X < 750) //750 is the end of the screen
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
            //entity.transformation = (new NullTransformation()).applyTransformation;
        }
     
    }
}
