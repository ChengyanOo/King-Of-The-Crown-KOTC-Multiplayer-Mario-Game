using Sprint1.States.ActionStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Entities;
using Sprint1.Transformations;
using Microsoft.Xna.Framework;
using Sprint1.States.PowerStates;
using Sprint1.Sprites;
using Sprint1.Factories.SpriteFactories;
using Sprint1.Collisions;
using System.Diagnostics;
using Sprint1.Trackers;

namespace Sprint1.States.BlockStates
{
    public class SpawnDecorator : IBlockState
    {
        private IBlockState blockState;
        private List<SpriteEnum> itemList;
        private BlockEntity entity;
        private bool onCollision;
        private bool flag;

        private float time;
        private int count = 0;
        private float currentTime = 0;

        public SpawnDecorator(IBlockState blockState, BlockEntity entity, List<SpriteEnum> itemList)
        {
            this.blockState = blockState;
            this.itemList = itemList;
            this.entity = entity;
            this.onCollision = true;
            this.flag = false;
        }

        public SpawnDecorator(IBlockState blockState, BlockEntity entity, List<SpriteEnum> itemList, float time)
        {
            this.blockState = blockState;
            this.itemList = itemList;
            this.entity=entity;
            this.time = time;
            this.onCollision = false;
            this.flag = false;
        }

        public void toUsed()
        {
            blockState.toUsed();
        }
        public void toBrick()
        {
            blockState.toBrick();
        }
        public void toBumped()
        {
            entity.Set((IBlockState)(new BumpedTransitionBlock(entity, this)));
        }
        public void toPrevious()
        {
            entity.Set((IBlockState)blockState);
        }

        public void Update(GameTime gameTime) 
        {
            blockState.Update(gameTime);
            if (!onCollision)
            {
                timeSpawn(gameTime);
            } 
            else if (flag)
            {
                Debug.WriteLine("Collision Spawn");
                collisionSpawn();
                flag = false;
            }
        }
        public void Enter(IBlockState previousState) 
        {
            blockState.Enter(previousState);
        }
        public void Exit() 
        {
            blockState.Exit();
        }

        public void Collision(ICollidable collidee, int direction)
        {
            blockState.Collision(collidee, direction);
            if (onCollision && direction == 2 && collidee is PlayerEntity)
            {
                toBumped();
                flag = true;
            }
        }

        private void collisionSpawn()
        {
            if (itemList != null && itemList.Count > 0)
            {
                IEntity spawnedEntity = new SpawnTransitionEntity(entity.game, itemList[0], entity.Position, true, entity.color, entity.layerDepth + 0.1f, entity, 0);
                entity.game.AddSprite(spawnedEntity);
                itemList.RemoveAt(0);
            
                if (itemList.Count == 0)
                {
                    toUsed();
                }
            }
            else
            {
                toUsed();
            }
        }

        private void timeSpawn(GameTime gameTime)
        {
            if (itemList != null && itemList.Count > 0)
            {
                currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (currentTime > time)
                {
                    IEntity spawnedEntity = new SpawnTransitionEntity(entity.game, itemList[count++], entity.Position, true, entity.color, entity.layerDepth + 0.1f, entity, 0);
                    entity.game.AddSprite(spawnedEntity);
                    if (count >= itemList.Count)
                    {
                        count = 0;
                    }
                    currentTime = 0;
                }    
            }
        }
    }
}

