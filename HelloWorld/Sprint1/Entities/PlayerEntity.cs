using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.States.ActionStates;
using Sprint1.States.PowerStates;
using Sprint1.Sprites;
using Sprint1.Factories;
using Sprint1.Factories.SpriteFactories;
using Sprint1.Transformations;
using Sprint1.Factories.StateFactories;
using Sprint1.Collisions;
using System.Diagnostics;
using Microsoft.VisualBasic;
using Sprint1.Physics;
using Sprint1.Trackers;
//using System.Numerics;

namespace Sprint1.Entities
{
    public class PlayerEntity : Entity
    {
        private PowerStateFactory powerStateFactory; 
        private ActionStateFactory actionStateFactory;
        private IPowerState powerState;
        private IActionState actionState;
        private PowerState pState;
        private ActionState aState;
        public bool PoweredUp { get; set; }
        public bool canSteal;
        public float cooldownTime;
        public event EventHandler<PositionEventArgs> ChangingPosition;
        PositionEventArgs PositionEventArgs;
        public CrownEntity crownEntity { get; set; }

        public PlayerEntity(Game1 game, SpriteEnum spriteType) : base(game, spriteType)
        {
            this.powerStateFactory = new PowerStateFactory(this);
            this.actionStateFactory = new ActionStateFactory(this);
            this.rigidbody = new Rigidbody(game ,this.Position, new Vector2(0, 0), 1);
            ResetCooldown();
            canSteal = true;
            Set(spriteType);
        }

        public PlayerEntity(Game1 game, SpriteEnum spriteType, Vector2 position, bool isRight, Color color, float layerDepth = 0) : base(game, spriteType, position, isRight, color, layerDepth)
        {
            this.powerStateFactory = new PowerStateFactory(this);
            this.actionStateFactory = new ActionStateFactory(this);
            this.rigidbody = new Rigidbody(game, this.Position, new Vector2(0, 0), 1);
            this.rigidbody.Moving += this.SetPosition;
            ResetCooldown();
            canSteal = true;
            Set(spriteType);
        }

        public override Vector2 Position
        {
            get { return sprite.Position; }
            set
            { 
                sprite.Position = value;
                sprite.Position = Vector2.Clamp(sprite.Position, new Vector2(48,0), new Vector2(1148, 532));
                collider.Location = colliderOffset + new Point((int)sprite.Position.X, (int)sprite.Position.Y);
            }
        }

        public override void Set(SpriteEnum spriteType)
        {
            setState(spriteType);
            setSprite(spriteType);
        }

        public void Set(IActionState actionState, IPowerState powerState)
        {          
            Set(actionState);
            Set(powerState);
        }

        public void Set(IActionState actionState)
        {
            if (actionState is CrouchingState)
            {
                Set(SpriteEnum.player | SpriteEnum.crouching);
            }
            else if (actionState is FallingState)
            {
                Set(SpriteEnum.player | SpriteEnum.falling);
            } 
            else if (actionState is IdleState)
            {
                Set(SpriteEnum.player | SpriteEnum.idle);
            }
            else if (actionState is JumpingState)
            {
                Set(SpriteEnum.player | SpriteEnum.jumping);
            }
            else if (actionState is RunningState)
            {
                Set(SpriteEnum.player | SpriteEnum.running);
            }
        }

        public void Set(IPowerState powerState)
        {
            if (powerState is DeadState)
            {
                Set(SpriteEnum.player | SpriteEnum.dead);
            }
            else if (powerState is SmallState)
            {
                Set(SpriteEnum.player | SpriteEnum.small);
            }
            else if (powerState is SuperState)
            {
                Set(SpriteEnum.player | SpriteEnum.super);
            }
            else if (powerState is FireState)
            {
                Set(SpriteEnum.player | SpriteEnum.fire);
            }
        }

        public override void Update(GameTime gameTime)
        {
            this.rigidbody.Update(gameTime);
            this.rigidbody.CheckMoving(this);
            this.checkBottom();
            if(this.rigidbody.velocity.Y > 0 && !(powerState is DeadState))
            {
                actionState.toFalling();
            }

            CooldownTimer(gameTime);

            powerState.Update(gameTime);
            base.Update(gameTime);
        }

        private void CooldownTimer(GameTime gameTime)
        {
            if (cooldownTime > 0)
            {
                cooldownTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                //Debug.WriteLine(cooldownTime);
            }
            else
            {
                canSteal = true;
            }
        }

        public void ResetCooldown()
        {
            canSteal = false;
            cooldownTime = 1f;
        }

        private void checkBottom()
        {
            if (Position.Y >= game.getScreenDimensions().Y-90)
            {
                turnDead();
            }
        }

        public override void OnCollisionEnter(ICollidable collidee, int direction)
        {
            powerState.Collision(collidee, direction);
        }

        public void faceLeft()
        {
            actionState.faceLeft();
        }

        public void faceRight()
        {
            actionState.faceRight();
        }

        public void faceUp()
        {
            actionState.faceUp();
        }

        public void faceDown()
        {
            actionState.faceDown();
        }

        public void releaseLeft()
        {
            actionState.releaseLeft();
        }

        public void releaseRight()
        {
            actionState.releaseRight();
        }

        public void releaseUp()
        {
            actionState.releaseUp();
        }

        public void releaseDown()
        {
            actionState.releaseDown();
        }

        public void turnIdle()
        {
            actionState.toIdle();
        }

        public void turnFalling()
        {
            actionState.toFalling();
        }
        
        public void turnJumping()
        {
            actionState.toJumping();
        }

        public void turnRunning()
        {
            actionState.toRunning();
        }

        public void takeDamage()
        {
            powerState.takeDamage();
        }

        public void turnDead()
        {
            powerState.toDead();
        }

        public void turnSmall()
        {
            powerState.toSmall();
        }

        public void turnSuper()
        {
            powerState.toSuper();
        }

        public void turnFire()
        {
            powerState.toFire();
        }

        public void Fire()
        {
            if (powerState is FireState)
            {
                ((FireState)powerState).Fire();
            }
        }

        public void interactCrown()
        {
            powerState.interactCrown();
        }

        private void setSprite(SpriteEnum spriteType)
        {
            if ((spriteType & SpriteEnum.player) == SpriteEnum.player)
            {
                SpriteEnum action = SpriteEnum.allActions & spriteType;
                SpriteEnum power = SpriteEnum.allPowers & spriteType;
                if (action != SpriteEnum.player && power != SpriteEnum.player)
                {
                    base.Set(spriteType);
                }
                else if (action != SpriteEnum.player)
                {
                    base.Set(spriteType | (this.spriteType & SpriteEnum.allPowers));
                }
                else if (power != SpriteEnum.player)
                {
                    base.Set(spriteType | (this.spriteType & SpriteEnum.allActions));
                }
            }
        }

        private void setState(SpriteEnum spriteType)
        {
            IActionState previousActionState = actionState;
            IPowerState previousPowerState = powerState;

            IActionState newActionState = actionStateFactory.Create(spriteType, actionState); 
            if (newActionState != null)
            {
                actionState = newActionState;
                aState = (ActionState)actionState; //"aState" is a placeholder variable
                aState.SetEffect += game.audioManager.PlaySoundEffect;
            }
            IPowerState newPowerState = powerStateFactory.Create(spriteType, powerState); 
            if (newPowerState != null)
            {
                
                powerState = newPowerState; //publisher
                pState = (PowerState)powerState;
                pState.SetEffect += game.audioManager.PlaySoundEffect;
                /*
                pState = (PowerState)powerState; //"pState" is a placeholder variable
                pState.Killed += game.lifeTracker.DecreaseLife;
                pState.AteMushroom += game.lifeTracker.IncreaseLife;
                //pState.SetEffect += game.soundEffectTracker.PlayEffect;
                pState.SetEffect += game.audioManager.PlaySoundEffect;
                pState.SetCoin += game.coinTracker.AteCoin;
                Console.WriteLine("--------------new-state-------------------");
                */
            }

            if ((spriteType & SpriteEnum.player) == SpriteEnum.player)
            {
                if ((spriteType & SpriteEnum.allPowers) != SpriteEnum.player)
                {
                    if (previousActionState != null)
                    {
                        previousPowerState.Exit();
                    }
                    powerState.Enter(previousPowerState);
                } 

                if ((spriteType & SpriteEnum.allActions) != SpriteEnum.player || newActionState is DeadActionState)
                {
                    if (previousActionState != null)
                    {
                        previousActionState.Exit();
                    }
                    actionState.Enter(previousActionState);
                } 
                else if (previousActionState is DeadActionState && !(newActionState is DeadActionState))
                {
                    actionState.Exit();
                } 
            }
        }

        public void SetPosition(object o, EventArgs a)
        {
            PositionEventArgs = new PositionEventArgs { PositionValue = this.Position };
            onChangingPosition(PositionEventArgs);
        }

        protected virtual void onChangingPosition(PositionEventArgs e)
        {
            EventHandler<PositionEventArgs> handler = ChangingPosition;
            if (handler != null)
                handler(this, e);
        }

    }

    public class PositionEventArgs : EventArgs
    {
        public Vector2 PositionValue { get; set; }
    }
}