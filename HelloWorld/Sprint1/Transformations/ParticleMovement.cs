using Microsoft.Xna.Framework;
using Sprint1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.Transformations
{
    internal class ParticleMovement
    {
        private float dy;
        private float gravity;
        private Entity entity;
        private int Xoffset;
        private int Yoffset;
        private float anchor;

        public ParticleMovement(Entity entity, int Xoffset, int Yoffset, float anchor)
        {
            this.entity = entity;
            this.Xoffset = Xoffset;
            this.Yoffset = Yoffset;
            this.anchor = anchor;
            gravity = 9.8f;
            dy = 0;
        }

        public Vector2 applyTransformation(Vector2 position)
        {
            float deltaTime = Math.Abs((position.X-anchor)/Xoffset) * (float)0.016/4;
            Vector2 newPosition = position;

            //if position is not below the ground
            if (position.Y < 800)
            {
                //Multiply by delta timeSincePeak to account for frame drops/bad computers
                dy += gravity * deltaTime;
                newPosition.Y += dy * deltaTime;

                newPosition.X += Xoffset;
                newPosition.Y += Yoffset;
            }
            else
                entity.transformation = (new NullTransformation()).applyTransformation;
                entity.game.RemoveSprite(entity);

            return newPosition;
        }
    }
}
