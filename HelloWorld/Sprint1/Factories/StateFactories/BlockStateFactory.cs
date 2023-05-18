using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Entities;
using Sprint1.States.BlockStates;
using Sprint1.Factories.SpriteFactories;
using Sprint1.Sprites;

namespace Sprint1.Factories.StateFactories
{
    public class BlockStateFactory
    {
        private BlockEntity entity;

        public BlockStateFactory(BlockEntity entity)
        {
            this.entity = entity;
        }

        public IBlockState Create(SpriteEnum spriteType, IBlockState previousBlockState)
        {
            IBlockState blockState = null;
            if ((SpriteEnum.block & spriteType) == SpriteEnum.block)
            {
                SpriteEnum block = (SpriteEnum.allBlocks & spriteType);
                if (block != SpriteEnum.block)
                {
                    switch (block)
                    {
                        case SpriteEnum.block | SpriteEnum.brick:
                            blockState = new BrickBlockState(entity, previousBlockState);
                            break;
                        case SpriteEnum.block | SpriteEnum.hidden:
                            blockState = new HiddenBlockState(entity, previousBlockState);
                            break;
                        case SpriteEnum.block | SpriteEnum.floor:
                        case SpriteEnum.block | SpriteEnum.floor | SpriteEnum.top:
                        case SpriteEnum.block | SpriteEnum.floor | SpriteEnum.bot:
                            blockState = new FloorBlockState(entity, previousBlockState);
                            break;
                        case SpriteEnum.block | SpriteEnum.stair:
                            blockState = new StairBlockState(entity, previousBlockState);
                            break;
                        default:
                            blockState = new FloorBlockState(entity, previousBlockState);
                            break;
                    }
                }
            }

            blockState ??= new FloorBlockState(entity, previousBlockState);

            return blockState;
        }
    }
}