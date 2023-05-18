using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Sprint1.Transformations
{
    public class NullTransformation
    {
        public Vector2 applyTransformation(Vector2 position)
        {
            return position;
        }
    }
}
