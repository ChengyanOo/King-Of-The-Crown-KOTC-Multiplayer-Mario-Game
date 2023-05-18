using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Entities;
using Sprint1.States.ActionStates;
using Sprint1.Collisions;
using Sprint1.Factories.SpriteFactories;
using Sprint1.States.EnemyStates;
using Sprint1.Trackers;
using Sprint1.Audio;
using System.Net.NetworkInformation;

namespace Sprint1.States.PowerStates
{

    public abstract class PowerState : IPowerState
    {
        protected PlayerEntity entity;
        protected IPowerState previousState;

        //public delegate void TimeRunOutEventHandler(object source, EventArgs args);
        //public delegate void EatMushroomEventHandler(object source, EventArgs args);

        //public event TimeRunOutEventHandler TimeRanOut;
        //public event EatMushroomEventHandler AteMushroom;
        public event EventHandler<EventArgs> TimeRanOut;
        public event EventHandler<EventArgs> AteMushroom;
        public event EventHandler<EventArgs> Killed;
        public event EventHandler<PointEventArgs> IncScore;

        public event EventHandler<SoundEffectEventArgs> SetEffect;
        SoundEffectEventArgs SoundEffectArgs;
        public event EventHandler<CoinEventArgs> SetCoin;
        CoinEventArgs CoinArgs;

        public PowerState(PlayerEntity entity, IPowerState previousState)
        {
            this.entity = entity;
            this.previousState = previousState;
            this.IncScore += entity.game.pointTracker.IncScore;
        }

        public void toDead()
        {
            entity.Set(SpriteEnum.player | SpriteEnum.dead);
            /*
            onKilled();
            this.Killed -= entity.game.lifeTracker.DecreaseLife;
            SoundEffectArgs = new SoundEffectEventArgs { effect = "mariodie" };
            onSetEffect(SoundEffectArgs);
            this.SetEffect -= entity.game.audioManager.PlaySoundEffect;
            */
        }
        public void toSmall()
        {
            entity.PoweredUp = false;
            if ((entity.spriteType & SpriteEnum.allActions) == (SpriteEnum.player | SpriteEnum.crouching))
            {
                entity.Set(SpriteEnum.player2 | SpriteEnum.small | SpriteEnum.idle);
            }
            else
            {
                entity.Set(SpriteEnum.player | SpriteEnum.small);
            }
        }
        public void toSuper()
        {
            entity.PoweredUp = true;
            entity.Set(SpriteEnum.player | SpriteEnum.super);
        }
        public void toFire()
        {
            entity.PoweredUp = true;
            entity.Set(SpriteEnum.player | SpriteEnum.player1 | SpriteEnum.fire);
        }

        public virtual void Update(GameTime gameTime) { }

        public abstract void takeDamage();
        public virtual void interactCrown()
        {
            if (entity.crownEntity == null)
            {
                stealCrown();
            }
            else
            {
                throwCrown();
            }
        }

        protected virtual void stealCrown()
        {

            //check if we can steal
            if (entity.canSteal)
            {
                SoundEffectArgs = new SoundEffectEventArgs { effect = "failToSteal" };
                onSetEffect(SoundEffectArgs);

                //create fake collider to check if there is a player nearby
                Rectangle fakeCollider = new Rectangle((int)entity.Position.X - 32, (int)entity.Position.Y - 32, 128, 128);
                //check if there is a player nearby
                foreach (PlayerEntity player in entity.game.playerList)
                {
                    if (fakeCollider.Intersects(player.Collider))
                    {
                        //if there is a player nearby, check if they have a crown
                        if (player.crownEntity != null && player != entity)
                        {
                            //if they have a crown, steal it
                            player.crownEntity.turnAttached(entity);
                            entity.crownEntity = player.crownEntity;
                            player.crownEntity = null;

                        }
                    }
                }

            }
            entity.ResetCooldown();

        }

        protected virtual void throwCrown()
        {
            entity.crownEntity.turnThrown();
            entity.crownEntity = null;
        }

        public virtual void Enter(IPowerState previousState)
        {
            this.previousState = previousState;
        }
        public virtual void Exit() { }

        public virtual void Collision(ICollidable collidee, int direction)
        {
            if ((entity.spriteType & SpriteEnum.allPowers) != (SpriteEnum.player | SpriteEnum.dead))
            {
                if (collidee is CrownEntity && entity.crownEntity == null)
                {
                    if ((((CrownEntity)collidee).spriteType & SpriteEnum.attached) != (SpriteEnum.attached))
                    {
                        entity.crownEntity = (CrownEntity)collidee;
                    }
                    else if (((CrownEntity)collidee).playerEntity == entity)
                    {
                        entity.crownEntity = (CrownEntity)collidee;
                    }
                } 
                else if (collidee is ItemEntity)
                {
                    if ((((ItemEntity)collidee).spriteType & SpriteEnum.allItems) == (SpriteEnum.item | SpriteEnum.redMushroom))
                    {
                        addPoints(1000);
                        toSuper();
                    }
                    else if ((((ItemEntity)collidee).spriteType & SpriteEnum.allItems) == (SpriteEnum.item | SpriteEnum.fireFlower))
                    {
                        addPoints(1000);
                        toFire();
                    }
                    else if ((((ItemEntity)collidee).spriteType & SpriteEnum.allItems) == (SpriteEnum.item | SpriteEnum.star))
                    {
                        addPoints(1000);
                    }
                    else if ((((ItemEntity)collidee).spriteType & SpriteEnum.allItems) == (SpriteEnum.item | SpriteEnum.bigCoin))
                    {

                        //Debug.WriteLine("Hello there");
                        SoundEffectArgs = new SoundEffectEventArgs { effect = "coin" };
                        onSetEffect(SoundEffectArgs);
                        addPoints(200);
                        CoinArgs = new CoinEventArgs { CoinValue = 5 };
                        onSetCoin(CoinArgs);
                    }
                    else if ((((ItemEntity)collidee).spriteType & SpriteEnum.allItems) == (SpriteEnum.item | SpriteEnum.greenMushroom))
                    {
                        onAteMushroom();
                        this.AteMushroom -= entity.game.lifeTracker.IncreaseLife;
                    }
                }
                else if (collidee is EnemyEntity)
                {
                    if (direction != 2 && ((((EnemyEntity)collidee).spriteType & SpriteEnum.allEnemies) == (SpriteEnum.enemy | SpriteEnum.koopa)))
                    {
                        //takeDamage();
                        correctPosition(collidee, direction);
                    }
                    else if (direction != 2 && ((((EnemyEntity)collidee).spriteType & SpriteEnum.allEnemies) == (SpriteEnum.enemy | SpriteEnum.goomba)))
                    {
                        takeDamage();
                        correctPosition(collidee, direction);
                    }
                    else if (direction != 2 && ((((EnemyEntity)collidee).spriteType & SpriteEnum.allEnemies) == (SpriteEnum.enemy | SpriteEnum.shellKoopa)) && ((((EnemyEntity)collidee).rigidbody.velocity != new Vector2(0, 0))))
                    {
                        takeDamage();
                        correctPosition(collidee, direction);
                    }
                    else
                    {
                        entity.rigidbody.velocity = new Vector2(0, -10);
                    }
                }

                else if (collidee is BlockEntity)
                {
                    if (SpriteEnum.hazard == (((IEntity)collidee).spriteType & SpriteEnum.hazard))
                    {
                        takeDamage();
                        
                        if (direction == 1)
                        {
                            entity.rigidbody.velocity = new Vector2(1, 0);
                        }
                        else if (direction == 3)
                        {
                            entity.rigidbody.velocity = new Vector2(-1, 0);
                        }
                        else if (direction == 0)
                        {
                            entity.rigidbody.velocity = Vector2.Zero;
                        }
                        else
                        {
                            if (entity.IsRight)
                            {
                                entity.rigidbody.velocity = new Vector2(-2, -5);
                            }
                            else
                            {
                                entity.rigidbody.velocity = new Vector2(2, -5);
                            }
                        }
                        correctPosition(collidee, direction);
                    }
                    else if (!(direction != 0 && ((((BlockEntity)collidee).spriteType & SpriteEnum.allBlocks) == (SpriteEnum.block | SpriteEnum.hidden))))
                    {
                        if (direction == 2)
                        {
                            if ((((BlockEntity)collidee).spriteType & SpriteEnum.jumpPad) == SpriteEnum.jumpPad)
                            {
                                entity.turnJumping();
                                entity.rigidbody.velocity = new Vector2(entity.rigidbody.velocity.X, -30);
                            }
                            else
                            {
                                if (entity.rigidbody.velocity.X == 0)
                                {
                                    entity.turnIdle();
                                }
                                else
                                {
                                    entity.turnRunning();
                                }

                                entity.rigidbody.velocity = new Vector2(entity.rigidbody.velocity.X, Math.Min(0, entity.rigidbody.velocity.Y));
                                entity.rigidbody.isGrounded = true;
                            }

                        }
                        else if (direction == 0)
                        {
                            entity.rigidbody.velocity = new Vector2(entity.rigidbody.velocity.X, 0);
                        }
                        else
                        {
                            entity.rigidbody.velocity = new Vector2(0, entity.rigidbody.velocity.Y);
                        }
                        correctPosition(collidee, direction);
                    }
                }
            }
        }

        private void addPoints(int points)
        {
            PointEventArgs args = new PointEventArgs { PointValue = points };
            onIncScore(args);
            this.IncScore -= entity.game.pointTracker.IncScore;
        }


        private void correctPosition(ICollidable collidee, int direction)
        {
            Rectangle offset = entity.game.GetCollider(entity.spriteType, entity.Position);
            Rectangle collisionRect = Rectangle.Intersect(offset, collidee.Collider);
            //Rectangle collisionRect = Rectangle.Intersect(entity.Collider, collidee.Collider);
            //entity.turnIdle();
            //entity.rigidbody.velocity = new Vector2(0, 0);
            switch (direction)
            {
                case 0:
                    //entity.rigidbody.position = new Vector2(entity.Position.X, entity.Position.Y + collisionRect.Height);
                    entity.Position = new Vector2(entity.Position.X, entity.Position.Y + collisionRect.Height);
                    break;
                case 1:
                    //entity.rigidbody.position = new Vector2(entity.Position.X - collisionRect.Width, entity.Position.Y);
                    entity.Position = new Vector2(entity.Position.X - collisionRect.Width, entity.Position.Y);
                    break;
                case 2:
                    //entity.rigidbody.position = new Vector2(entity.Position.X, entity.Position.Y - collisionRect.Height);
                    entity.Position = new Vector2(entity.Position.X, entity.Position.Y - collisionRect.Height);
                    break;
                case 3:
                    //entity.rigidbody.position = new Vector2(entity.Position.X + collisionRect.Width, entity.Position.Y);
                    entity.Position = new Vector2(entity.Position.X + collisionRect.Width, entity.Position.Y);
                    break;
            }
        }


        protected virtual void onTimeRunOut()
        {
            EventHandler<EventArgs> handler = TimeRanOut;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        protected virtual void onKilled()
        {
            EventHandler<EventArgs> handler = Killed;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }


        protected virtual void onAteMushroom()
        {
            EventHandler<EventArgs> handler = AteMushroom;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        protected virtual void onIncScore(PointEventArgs e)
        {
            EventHandler<PointEventArgs> handler = IncScore;
            if (handler != null)
                handler(this, e);
        }

        protected virtual void onSetEffect(SoundEffectEventArgs e)
        {
            EventHandler<SoundEffectEventArgs> handler = SetEffect;
            if (handler != null)
                handler(this, e);
        }

        protected virtual void onSetCoin(CoinEventArgs e)
        {
            EventHandler<CoinEventArgs> handler = SetCoin;
            if (handler != null)
                handler(this, e);
        }
    }
}