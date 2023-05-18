using Microsoft.Xna.Framework;
using Sprint1.Collisions;
using Sprint1.Factories.SpriteFactories;
using Sprint1.Factories.StateFactories;
using Sprint1.Physics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.Entities.ItemEntities.Fireball
{
    public class FireballEntity : Entity
    {
        public FireballPool fireballPool;
        public FireballEntity(Game1 game, SpriteEnum spriteType) : base(game, spriteType)
        {
            this.rigidbody = new Rigidbody(game, Position, new Vector2(0, 0), 1);
            Set(spriteType);
        }
        public FireballEntity(Game1 game, SpriteEnum spriteType, Vector2 position, bool isRight, Color color, float layerDepth = 0) : base(game, spriteType, position, isRight, color, layerDepth)
        {
            this.rigidbody = new Rigidbody(game, Position, new Vector2(0, 0), 1);
            
            Set(spriteType);
        }

        public override void Set(SpriteEnum spriteType)
        {
            setSprite(spriteType);
        }

        public override void Update(GameTime gameTime)
        {
            //Debug.WriteLine(this.rigidbody.isGrounded);
            this.rigidbody.Update(gameTime);
            this.Position = this.rigidbody.position;
            //OnScreenEdge();
            base.Update(gameTime);
        }

        private void setSprite(SpriteEnum spriteType)
        {
            if ((spriteType & SpriteEnum.particle) == SpriteEnum.particle)
            {
                SpriteEnum particle = spriteType & SpriteEnum.allParticles;
                if (particle != SpriteEnum.particle)
                {
                    base.Set(spriteType);
                }
            }
        }

        public void OnScreenEdge() {
                
            if (this.rigidbody.position.X < 0 || this.rigidbody.position.X > game.getScreenDimensions().X)
            {   
                if(fireballPool != null)
                    fireballPool.ReturnFireball(this);
            }
        }

        public override void OnCollisionEnter(ICollidable collidee, int direction)
        {
            /*
            if (collidee is PlayerEntity)
            {
                game.RemoveSprite(this);
                //remove one life from mario
            }*/
            if (collidee is BlockEntity | collidee is ItemEntity)
            {
                //TODO: bounce randomly maybe?
                if (direction == 1)
                {
                    if(fireballPool != null)
                    {
                        fireballPool.ReturnFireball(this);
                    }
                    else
                    {
                        game.RemoveSprite(this);
                    }

                    //this.rigidbody.velocity = new Vector2(-1, 0);
                    //this.sprite.IsRight = false;
                }
                else if (direction == 3)
                {
                    //Despawn fireball
                    if (fireballPool != null)
                    {
                        fireballPool.ReturnFireball(this);
                    }
                    else
                    {
                        game.RemoveSprite(this);
                    }                    //this.rigidbody.velocity = new Vector2(1, 0);
                    //this.sprite.IsRight = true;
                }
                else if(direction == 2)
                {
                    this.rigidbody.velocity = new Vector2(this.rigidbody.velocity.X, -200);
                }
            }
            
        }
    }
}
