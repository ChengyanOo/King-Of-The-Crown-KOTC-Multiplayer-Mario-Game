using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Entities;
using Sprint1.States.ActionStates;
using Sprint1.Collisions;
using Sprint1.Transformations;
using Sprint1.Factories.SpriteFactories;
using System.Diagnostics;
using Sprint1.Entities.ItemEntities.Fireball;
using Sprint1.States.BlockStates;
using Sprint1.Entities.ItemEntities;
using Sprint1.Trackers;
using Sprint1.Audio;

namespace Sprint1.States.EnemyStates
{

    public class EnemyGoombaState : EnemyState
    {
        private IEntity player;//Set to NUllEntity if not given Mario
        private float speed = 2;

        public EnemyGoombaState(EnemyEntity entity, IEnemyState previousState) : base(entity, previousState)
        {
            //player = new NullEntity();
        }

        public override void Enter(IEnemyState previousState)//pass in a IEntity instead of a PLayerEntity
        {
            //Debug.WriteLine(""+player.Position.X);
            entity.rigidbody.velocity = new Vector2(0, 0);
            base.Enter(previousState);
            this.IncScore += entity.game.pointTracker.IncScore;

        }

        public void moveGoomba()
        {
            int distance = (int)(player.Position.X - entity.Position.X);


            if (distance > 0)
            {
                //entity.transformation = (new EnemyMovement()).applyRightRun;
                entity.rigidbody.velocity = new Vector2(speed, 0);
            }
            else if (distance < 0)
            {
                //entity.transformation = (new EnemyMovement()).applyLeftRun;
                entity.rigidbody.velocity = new Vector2(-speed, 0);
            }
        }

        public void AddPlayer(PlayerEntity playerMario)
        {
            player = playerMario;
        }


        public override void Collision(ICollidable collidee, int direction)
        {
            if (collidee is PlayerEntity)
            {
                if (direction == 0 && ((((PlayerEntity)collidee).spriteType & SpriteEnum.player) == (SpriteEnum.player)))
                {
                    base.args = new SoundEffectEventArgs { effect = "stomp" };
                    onSetEffect(base.args);
                    base.SetEffect -= entity.game.audioManager.PlaySoundEffect;

                    PointEventArgs args = new PointEventArgs { PointValue = 100 };
                    onIncScore(args);
                    this.IncScore -= entity.game.pointTracker.IncScore;
                    entity.rigidbody.velocity = new Vector2(0, 0);
                    //goombaToDead();
                    entity.game.RemoveSprite(entity);
                }
            }
            else if (collidee is BlockEntity)
            {
                if((((BlockEntity)collidee).spriteType & SpriteEnum.allBlocks) != (SpriteEnum.block | SpriteEnum.hidden))
                {
                    if(direction == (int)CollisionDirection.Bottom)
                    {
                        entity.rigidbody.velocity = new Vector2(entity.rigidbody.velocity.X, 0);
                        entity.rigidbody.isGrounded = true;
                    }
                    else if(direction == (int)CollisionDirection.Top)
                    {
                        entity.rigidbody.velocity = new Vector2(entity.rigidbody.velocity.X, 0);
                    }
                    else if (direction == (int)CollisionDirection.Right)
                    {
                        entity.rigidbody.velocity = new Vector2(-speed, entity.rigidbody.velocity.Y);
                        entity.sprite.IsRight = false;
                    }
                    else if (direction == (int)CollisionDirection.Left)
                    {
                        entity.rigidbody.velocity = new Vector2(speed, entity.rigidbody.velocity.Y);
                        entity.sprite.IsRight = true;
                    }

                    correctPosition(collidee, direction);
                }
            }
            else if (collidee is EnemyEntity)
            {
                if(((((EnemyEntity)collidee).spriteType & SpriteEnum.allEnemies) == (SpriteEnum.enemy | SpriteEnum.shellKoopa)))
                {
                    entity.game.RemoveSprite(entity);
                }

                if(direction == 2)
                {
                    entity.rigidbody.velocity = new Vector2(entity.rigidbody.velocity.X, 0);
                    entity.rigidbody.isGrounded = true;
                }
                else if(direction == 0)
                {
                    entity.rigidbody.velocity = new Vector2(entity.rigidbody.velocity.X, 0);
                }
                else if (direction == 1)
                {
                    entity.rigidbody.velocity = new Vector2(-speed, entity.rigidbody.velocity.Y);
                    entity.sprite.IsRight = false;
                }
                else if (direction == 3)
                {
                    entity.rigidbody.velocity = new Vector2(speed, entity.rigidbody.velocity.Y);
                    entity.sprite.IsRight = true;
                }

                correctPosition(collidee, direction);
            }
            else if (collidee is ItemEntity)
            {
                if(direction == 2)
                {
                    entity.rigidbody.velocity = new Vector2(entity.rigidbody.velocity.X, 0);
                    entity.rigidbody.isGrounded = true;
                }
                else if(direction == 0)
                {
                    entity.rigidbody.velocity = new Vector2(entity.rigidbody.velocity.X, 0);
                }
                else if (direction == 1)
                {
                    entity.rigidbody.velocity = new Vector2(-speed, entity.rigidbody.velocity.Y);
                    entity.sprite.IsRight = false;
                }
                else if (direction == 3)
                {
                    entity.rigidbody.velocity = new Vector2(speed, entity.rigidbody.velocity.Y);
                    entity.sprite.IsRight = true;
                }

                correctPosition(collidee, direction);
            }
            else if (collidee is FireballEntity)
            {
                entity.rigidbody.velocity = new Vector2(0, 0);
                entity.game.RemoveSprite(entity);
            }
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
    }   
}
