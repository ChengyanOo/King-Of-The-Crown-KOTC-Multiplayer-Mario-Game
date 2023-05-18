using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint1.Entities;

namespace Sprint1.Transformations
{
    public class Bump 
    {
        private Entity entity;
        private float offset;
        private float anchor;
        private bool isUp = true;

        public Bump(Entity entity, float offset, float anchor)
        {
            this.entity = entity;
            this.offset = offset;
            this.anchor = anchor;
        }

        public Vector2 applyTransformation(Vector2 position)
        {
            Vector2 newPosition = position;
            
            if (isUp)
            {
                newPosition.Y--;
                if (newPosition.Y < anchor - offset)
                {
                    isUp = false;
                }
            }
            else
            {
                if (position.Y < anchor)
                {
                    newPosition.Y++;
                }
                else
                {
                    entity.transformation = (new NullTransformation()).applyTransformation;
                }
            }
            return newPosition;
        }
    }
}
