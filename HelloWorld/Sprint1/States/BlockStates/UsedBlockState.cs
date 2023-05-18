using Sprint1.Entities;
using Sprint1.Factories.SpriteFactories;
using Sprint1.Sprites;
using Sprint1.States.PowerStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.States.BlockStates
{
  
    public class UsedBlockState : BlockState
    {
        public UsedBlockState(BlockEntity entity, IBlockState previousState) : base(entity, previousState)
        { }

        // TODO
    }
}
