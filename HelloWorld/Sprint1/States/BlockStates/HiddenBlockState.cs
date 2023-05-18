using Sprint1.Entities;
using Sprint1.Factories.SpriteFactories;
using Sprint1.Sprites;
using Sprint1.States.PowerStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Collisions;

namespace Sprint1.States.BlockStates
{
    public class HiddenBlockState : BlockState
    {
        public HiddenBlockState(BlockEntity entity, IBlockState previousState) : base(entity, previousState)
        {}

        public override void Collision(ICollidable collidee, int direction)
        {
            if (direction == 2 && collidee is PlayerEntity)
            {
                if ((((PlayerEntity)collidee).spriteType & SpriteEnum.allPowers) == (SpriteEnum.player | SpriteEnum.small))
                {
                    toBrick();
                }
            }
        }
    }
}
