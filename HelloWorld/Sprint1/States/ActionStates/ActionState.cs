using Sprint1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.States.PowerStates;
using Sprint1.Factories.SpriteFactories;
using Sprint1.Trackers;
using Sprint1.Audio;


namespace Sprint1.States.ActionStates
{
    public abstract class ActionState: IActionState
    {
        protected PlayerEntity entity;
        protected IActionState previousState;
        public event EventHandler<SoundEffectEventArgs> SetEffect;
        SoundEffectEventArgs args;

        public ActionState(PlayerEntity entity, IActionState previousState)
        {
            this.entity = entity;
            this.previousState = previousState;
        }

        public void toIdle()
        {
            entity.Set(SpriteEnum.player | SpriteEnum.idle);
        }
        public void toCrouching()
        {
            if ((entity.spriteType & SpriteEnum.allPowers) != (SpriteEnum.player | SpriteEnum.small))
            {
                entity.Set(SpriteEnum.player | SpriteEnum.crouching);
            }
        }
        public void toJumping()
        {
            entity.Set(SpriteEnum.player | SpriteEnum.jumping);
            if (entity.PoweredUp)
            {
                args = new SoundEffectEventArgs { effect = "jumpsuper" };
            }
            else
            {
                args = new SoundEffectEventArgs { effect = "jumpsmall" };
            }
            onSetEffect(args);
            this.SetEffect -= entity.game.audioManager.PlaySoundEffect;
        }
        public void toFalling()
        {
            entity.Set(SpriteEnum.player | SpriteEnum.falling);
        }
        public void toRunning()
        { 
            entity.Set(SpriteEnum.player | SpriteEnum.running);
        }
        public void toPrevious()
        {
            entity.Set(previousState);
        }

        public virtual void faceLeft() { }
        public virtual void faceRight() { }
        public virtual void faceUp() { }
        public virtual void faceDown() { } 
        
        public virtual void releaseLeft() { }
        public virtual void releaseRight() { }
        public virtual void releaseUp() { }
        public virtual void releaseDown() { }
        
        public virtual void Enter(IActionState previousState)
        {
            this.previousState = previousState;
        }

        public virtual void Exit()
        { }

        protected virtual void onSetEffect(SoundEffectEventArgs e)
        {
            EventHandler<SoundEffectEventArgs> handler = SetEffect;
            if (handler != null)
                handler(this, e);
        }
    }
}
