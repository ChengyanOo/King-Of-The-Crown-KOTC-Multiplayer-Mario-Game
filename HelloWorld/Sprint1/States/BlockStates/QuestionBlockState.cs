using Sprint1.Entities;
using Sprint1.Factories.SpriteFactories;
using Sprint1.Sprites;
using Sprint1.States.PowerStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Transformations;
using Sprint1.Collisions;
using Microsoft.Xna.Framework;


namespace Sprint1.States.BlockStates
{
    public class QuestionBlockState : BlockState
    {
        public QuestionBlockState(BlockEntity entity, IBlockState previousState) : base(entity, previousState)
        { }

        public override void Collision(ICollidable collidee, int direction)
        {
            if (collidee is PlayerEntity && direction == 2)
            {
                toBumped();
            }
        }
    }
}
