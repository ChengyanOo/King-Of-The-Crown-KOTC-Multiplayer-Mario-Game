using Sprint1.Sprites;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sprint1.Factories.SpriteFactories;
using Sprint1.Factories.StateFactories;
using Microsoft.Xna.Framework;
using Sprint1.Collisions;
using Sprint1.Factories;
using Sprint1.Transformations;
using Sprint1.States.BlockStates;
using Sprint1.Physics;

namespace Sprint1.Entities.ItemEntities
{
    public class StarEntity : ItemEntity
    {
        private float speedX = 2;
        private float speedY = 20;
        public StarEntity(Game1 game, SpriteEnum spriteType) : base(game, spriteType)
        {
            Set(spriteType);
        }

        public StarEntity(Game1 game, SpriteEnum spriteType, Vector2 position, bool isRight, Color color, float layerDepth = 0) : base(game, spriteType, position, isRight, color, layerDepth)
        {
            this.rigidbody = new Rigidbody(game, this.Position, new Vector2(0, 0), 1);
            Set(spriteType);
        }

        public override void Set(SpriteEnum spriteType)
        {
            this.rigidbody = new Rigidbody(game, this.Position, new Vector2(0, 0), 1);
            setSprite(spriteType);

        }

        public override void Update(GameTime gameTime)
        {
            this.Position = transformation(this.Position);
            this.rigidbody.Update(gameTime);
            this.rigidbody.CheckMoving(this);
            base.Update(gameTime);
        }

        public override void OnCollisionEnter(ICollidable collidee, int direction)
        {
            if (collidee is PlayerEntity)
            {
                if ((((PlayerEntity)collidee).spriteType & SpriteEnum.player) == (SpriteEnum.player))
                {
                    this.rigidbody.velocity = new Vector2(0, 0);
                    this.game.RemoveSprite(this);
                }
            }
            else if (collidee is BlockEntity)
            {
                if((((BlockEntity)collidee).spriteType & SpriteEnum.allBlocks) != (SpriteEnum.block | SpriteEnum.hidden))
                {
                    if(direction == 2)
                    {
                        this.rigidbody.velocity = new Vector2(this.rigidbody.velocity.X, -speedY);
                        this.rigidbody.isGrounded = true;
                    }
                    else if(direction == 0)
                    {
                        this.rigidbody.velocity = new Vector2(this.rigidbody.velocity.X, 0);
                    }
                    else if (direction == 1)
                    {
                        this.rigidbody.velocity = new Vector2(-speedX, this.rigidbody.velocity.Y);
                        this.sprite.IsRight = false;
                    }
                    else if (direction == 3)
                    {
                        this.rigidbody.velocity = new Vector2(speedX, this.rigidbody.velocity.Y);
                        this.sprite.IsRight = true;
                    }

                    correctPosition(collidee, direction);
                }
            }
            else if (collidee is EnemyEntity || collidee is GoombaEnemyEntity)
            {
                if(direction == 2)
                {
                    this.rigidbody.velocity = new Vector2(this.rigidbody.velocity.X, -speedY);
                    this.rigidbody.isGrounded = true;
                }
                else if(direction == 0)
                {
                    this.rigidbody.velocity = new Vector2(this.rigidbody.velocity.X, 0);
                }
                else if (direction == 1)
                {
                    this.rigidbody.velocity = new Vector2(-speedX, this.rigidbody.velocity.Y);
                    this.sprite.IsRight = false;
                }
                else if (direction == 3)
                {
                    this.rigidbody.velocity = new Vector2(speedX, this.rigidbody.velocity.Y);
                    this.sprite.IsRight = true;
                }

                correctPosition(collidee, direction);
            }
            else if (collidee is ItemEntity)
            {
                if(direction == 2)
                {
                    this.rigidbody.velocity = new Vector2(this.rigidbody.velocity.X, -speedY);
                    this.rigidbody.isGrounded = true;
                }
                else if(direction == 0)
                {
                    this.rigidbody.velocity = new Vector2(this.rigidbody.velocity.X, 0);
                }
                else if (direction == 1)
                {
                    this.rigidbody.velocity = new Vector2(-speedX, this.rigidbody.velocity.Y);
                    this.sprite.IsRight = false;
                }
                else if (direction == 3)
                {
                    this.rigidbody.velocity = new Vector2(speedX, this.rigidbody.velocity.Y);
                    this.sprite.IsRight = true;
                }

                correctPosition(collidee, direction);
            }
        }

        private void setSprite(SpriteEnum spriteType)
        {
            if ((spriteType & SpriteEnum.item) == SpriteEnum.item)
            {
                SpriteEnum itemMushroom = SpriteEnum.allItems | spriteType;
                if (itemMushroom != SpriteEnum.item)
                {
                    base.Set(spriteType);
                }
            }
        }

        public void moveStar(PlayerEntity playerEntity)
        {
            int distance = (int)(playerEntity.Position.X - this.Position.X);


            if (distance > 0)
            {
                //entity.transformation = (new EnemyMovement()).applyRightRun;
                this.rigidbody.velocity = new Vector2(-speedX, speedY);
            }
            else if (distance < 0)
            {
                //entity.transformation = (new EnemyMovement()).applyLeftRun;
                this.rigidbody.velocity = new Vector2(speedX, speedY);
            }
        }

        private void correctPosition(ICollidable collidee, int direction)
        {
            Rectangle offset = this.game.GetCollider(this.spriteType, this.Position);
            Rectangle collisionRect = Rectangle.Intersect(offset, collidee.Collider);
            //Rectangle collisionRect = Rectangle.Intersect(entity.Collider, collidee.Collider);
            //entity.turnIdle();
            //entity.rigidbody.velocity = new Vector2(0, 0);
            switch (direction)
            {
                case 0:
                    this.Position = new Vector2(this.Position.X, this.Position.Y + collisionRect.Height);
                    break;
                case 1:
                    this.Position = new Vector2(this.Position.X - collisionRect.Width, this.Position.Y);
                    break;
                case 2:
                    this.Position = new Vector2(this.Position.X, this.Position.Y - collisionRect.Height);
                    break;
                case 3:
                    this.Position = new Vector2(this.Position.X + collisionRect.Width, this.Position.Y);
                    break;
            }
        }
    }
}
