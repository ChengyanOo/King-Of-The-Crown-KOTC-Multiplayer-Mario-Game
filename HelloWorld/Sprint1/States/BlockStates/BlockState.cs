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
using Sprint1.Trackers;
using Sprint1.Audio;

namespace Sprint1.States.BlockStates
{
    public abstract class BlockState : IBlockState
    {
        protected BlockEntity entity;
        protected IBlockState previousState;
        public event EventHandler<SoundEffectEventArgs> SetEffect;
        protected SoundEffectEventArgs args;

        public BlockState(BlockEntity entity, IBlockState previousState)
        {
            this.entity = entity; 
            this.previousState = previousState;
        }

        public void toUsed()
        {
            ///entity.Set(SpriteEnum.block | SpriteEnum.used);
        }
        public void toBrick()
        {
            entity.Set(SpriteEnum.block | SpriteEnum.brick);
        }
        public void toBumped()
        {
            entity.Set(new BumpedTransitionBlock(entity, this));
        }
        public void toPrevious()
        {
            entity.Set(previousState);
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Enter(IBlockState previousState) 
        {
            this.previousState = previousState;
        }
        public virtual void Exit() { }

        public virtual void Collision(ICollidable collidee, int direction)
        { }

        protected virtual void onSetEffect(SoundEffectEventArgs e)
        {
            EventHandler<SoundEffectEventArgs> handler = SetEffect;
            if (handler != null)
                handler(this, e);
        }
    }
}
