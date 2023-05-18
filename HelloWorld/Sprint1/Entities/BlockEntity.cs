using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.States.BlockStates;
using Sprint1.Factories.StateFactories;
using Sprint1.Factories.SpriteFactories;
using Sprint1.Collisions;

namespace Sprint1.Entities
{
    public class BlockEntity : Entity
    { 
        private BlockStateFactory blockStateFactory;
        private IBlockState blockState;
        private BlockState bState;

        public BlockEntity(Game1 game, SpriteEnum spriteType) : base(game, spriteType)
        {
            blockStateFactory = new BlockStateFactory(this);
            Set(spriteType);
        }

        public BlockEntity(Game1 game, SpriteEnum spriteType, Vector2 position, bool isRight, Color color, float layerDepth = 0) : base(game, spriteType, position, isRight, color, layerDepth)
        {
            blockStateFactory = new BlockStateFactory(this);
            Set(spriteType);
        }
        /// <summary>
        /// Makes the entity have the correct state and sprite for a given SpriteEnum
        /// </summary>
        /// <param name="spriteType">A SpriteEnum specifying the correct sprite type</param>
        public void Set(IBlockState blockState)
        {
            this.blockState = blockState;
        }
        
        public override void Set(SpriteEnum spriteType)
        {
            blockState = blockStateFactory.Create(spriteType, blockState);
            bState = (BlockState)blockState;
            bState.SetEffect += game.audioManager.PlaySoundEffect;
            setSprite(spriteType);
        }

        public override void OnCollisionEnter(ICollidable collidee, int direction)
        {
            blockState.Collision(collidee, direction);
        }

        public override void Update(GameTime gameTime)
        {
            if (rigidbody != null)
            {
                this.rigidbody.Update(gameTime);
                this.rigidbody.CheckMoving(this);
            }
            
            blockState.Update(gameTime);
            base.Update(gameTime);
        }

        private void setSprite(SpriteEnum spriteType)
        {
            if ((spriteType & SpriteEnum.block) == SpriteEnum.block)
            {
                SpriteEnum block = SpriteEnum.allBlocks | spriteType;
                if (block != SpriteEnum.block)
                {
                    base.Set(spriteType);
                }
            }
        }

        public void turnSpawning(List<SpriteEnum> sprites)
        {    
            blockState = new SpawnDecorator(blockState, this, sprites);
        }

        public void turnSpawning(List<SpriteEnum> sprites, float time)
        {    
            blockState = new SpawnDecorator(blockState, this, sprites, time);
        }
    }
}