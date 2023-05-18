using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Sprint1.Transformations
{
    public class LeftRight 
    {
        private int count;
        private bool right;

        public LeftRight()
        {
            right = true;
            count = 0;
        }

        public Vector2 applyTransformation(Vector2 position)
        {
            Vector2 newPosition = position;
            if (right)
            {
                newPosition.X++;
            } else
            {
                newPosition.X--;
            }
            count++;
            if (count%100 == 0)
            {
                right = !right;
            }
            return newPosition;
        }
    }
}
