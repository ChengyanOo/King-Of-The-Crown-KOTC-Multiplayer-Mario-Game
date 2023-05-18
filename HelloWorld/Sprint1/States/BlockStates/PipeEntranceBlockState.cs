﻿using Sprint1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.States.BlockStates
{
    public class PipeEntranceBlockState: BlockState
    {

        public PipeEntranceBlockState(BlockEntity entity, IBlockState previousState) : base(entity, previousState)
        {
            
        }
    }
}
