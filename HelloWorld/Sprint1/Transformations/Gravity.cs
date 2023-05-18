/*
using Microsoft.Xna.Framework;
using Sprint1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.Transformations
{
    internal class Gravity
    {
        private float gravity;
        private Entity entity;

        //note this class uses gravity

        public Gravity(Entity entity, float force, float angle)
        {
            this.entity = entity;
            this.force = force;
            this.angle = angle;
            gameTime = new GameTime();
            timeSincePeak = 0;
            gravity = -9.8f;
            y = 0;
            x = 0;
        }

        public Vector2 applyTransformation(Vector2 position)
        {
            Vector2 newPosition = position;
            timeSincePeak += deltaTime;

            //if position is not below the ground
            if (position.Y < 480)
            {
                //calculate x and y components of force to get position, converting to radians also.
                x = position.X + ((float)MathF.Cos((float)((Math.PI / 180) * angle)) * force) * timeSincePeak;
                newPosition.X = x * deltaTime;

                y = position.Y + ((float)MathF.Sin((float)((Math.PI / 180) * angle)) * force) * timeSincePeak - (gravity * (timeSincePeak * timeSincePeak / 2));
                newPosition.Y = y * deltaTime;
            }
            else
                entity.transformation = (new NullTransformation()).applyTransformation;

            return newPosition;
        }
    }
}
*/