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
using Sprint1.Entities.ItemEntities.Fireball;
using Sprint1.Audio;


namespace Sprint1.Entities.ItemEntities
{
    public class GreenMushroomEntity : ItemEntity
    {
        private float speed = 2;
        public event EventHandler<SoundEffectEventArgs> SetEffect;
        SoundEffectEventArgs SoundEffectArgs;
        public GreenMushroomEntity(Game1 game, SpriteEnum spriteType) : base(game, spriteType)
        {
            this.rigidbody = new Rigidbody(game, this.Position, new Vector2(0, 0), 1);
            Set(spriteType);
            this.SetEffect += game.audioManager.PlaySoundEffect;
        }

        public GreenMushroomEntity(Game1 game, SpriteEnum spriteType, Vector2 position, bool isRight, Color color, float layerDepth = 0) : base(game, spriteType, position, isRight, color, layerDepth)
        {
            this.rigidbody = new Rigidbody(game, this.Position, new Vector2(0, 0), 1);
            Set(spriteType);
            this.SetEffect += game.audioManager.PlaySoundEffect;

            SoundEffectArgs = new SoundEffectEventArgs { effect = "powerup_appears" };
            onSetEffect(SoundEffectArgs);
        }

        public override void Set(SpriteEnum spriteType)
        {

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
                    //entity.transformation = (new NullTransformation()).applyTransformation;
                    this.rigidbody.velocity = new Vector2(0, 0);
                    this.game.RemoveSprite(this);
                }
            }
            else if (collidee is BlockEntity)
            {
                if((((BlockEntity)collidee).spriteType & SpriteEnum.allBlocks) != (SpriteEnum.block | SpriteEnum.hidden))
                {
                    if(direction == (int)CollisionDirection.Bottom)
                    {
                        this.rigidbody.velocity = new Vector2(this.rigidbody.velocity.X, 0);
                        this.rigidbody.isGrounded = true;
                    }
                    else if(direction == (int)CollisionDirection.Top)
                    {
                        this.rigidbody.velocity = new Vector2(this.rigidbody.velocity.X, 0);
                    }
                    else if (direction == (int)CollisionDirection.Right)
                    {
                        this.rigidbody.velocity = new Vector2(-speed, this.rigidbody.velocity.Y);
                        this.sprite.IsRight = false;
                    }
                    else if (direction == (int)CollisionDirection.Left)
                    {
                        this.rigidbody.velocity = new Vector2(speed, this.rigidbody.velocity.Y);
                        this.sprite.IsRight = true;
                    }

                    correctPosition(collidee, direction);
                }
            }
            else if (collidee is EnemyEntity)
            {
                if(((((EnemyEntity)collidee).spriteType & SpriteEnum.allEnemies) == (SpriteEnum.enemy | SpriteEnum.shellKoopa)))
                {
                    this.game.RemoveSprite(this);
                }

                if(direction == 2)
                {
                    this.rigidbody.velocity = new Vector2(this.rigidbody.velocity.X, 0);
                    this.rigidbody.isGrounded = true;
                }
                else if(direction == 0)
                {
                    this.rigidbody.velocity = new Vector2(this.rigidbody.velocity.X, 0);
                }
                else if (direction == 1)
                {
                    this.rigidbody.velocity = new Vector2(-speed, this.rigidbody.velocity.Y);
                    this.sprite.IsRight = false;
                }
                else if (direction == 3)
                {
                    this.rigidbody.velocity = new Vector2(speed, this.rigidbody.velocity.Y);
                    this.sprite.IsRight = true;
                }

                correctPosition(collidee, direction);
            }
            else if (collidee is ItemEntity)
            {
                if(direction == 2)
                {
                    this.rigidbody.velocity = new Vector2(this.rigidbody.velocity.X, 0);
                    this.rigidbody.isGrounded = true;
                }
                else if(direction == 0)
                {
                    this.rigidbody.velocity = new Vector2(this.rigidbody.velocity.X, 0);
                }
                else if (direction == 1)
                {
                    this.rigidbody.velocity = new Vector2(-speed, this.rigidbody.velocity.Y);
                    this.sprite.IsRight = false;
                }
                else if (direction == 3)
                {
                    this.rigidbody.velocity = new Vector2(speed, this.rigidbody.velocity.Y);
                    this.sprite.IsRight = true;
                }

                correctPosition(collidee, direction);
            }
            else if (collidee is FireballEntity)
            {
                this.rigidbody.velocity = new Vector2(0, 0);
                this.game.RemoveSprite(this);
            }

            correctPosition(collidee, direction);
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

        public void moveMushroom(PlayerEntity playerEntity)
        {
            int distance = (int)(playerEntity.Position.X - this.Position.X);


            if (distance > 0)
            {
                //entity.transformation = (new EnemyMovement()).applyRightRun;
                this.rigidbody.velocity = new Vector2(0, speed);
                this.rigidbody.velocity = new Vector2(-speed, 0);
            }
            else if (distance <= 0)
            {
                //entity.transformation = (new EnemyMovement()).applyLeftRun;
                this.rigidbody.velocity = new Vector2(0, speed);
                this.rigidbody.velocity = new Vector2(speed, 0);
               
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

        protected virtual void onSetEffect(SoundEffectEventArgs e)
        {
            EventHandler<SoundEffectEventArgs> handler = SetEffect;
            if (handler != null)
                handler(this, e);
        }
    }
}

