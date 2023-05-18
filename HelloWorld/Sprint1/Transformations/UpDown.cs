using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Sprint1.Transformations
{
    public class UpDown 
    {
        private int count;
        private bool up;

        public UpDown()
        {
            up = true;
            count = 0;
        }

        public Vector2 applyTransformation(Vector2 position)
        {
            Vector2 newPosition = position;
            if (up)
            {
                newPosition.Y++;
            } else
            {
                newPosition.Y--;
            }
            count++;
            if (count%15 == 0)
            {
                up = !up;
            }
            return newPosition;
        }
    }
}
