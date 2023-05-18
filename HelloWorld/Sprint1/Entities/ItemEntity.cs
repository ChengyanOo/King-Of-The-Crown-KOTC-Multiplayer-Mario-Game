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
using Sprint1.States.EnemyStates;
using Sprint1.Factories;
using Sprint1.Transformations;
using Sprint1.Physics;
using Sprint1.States.BlockStates;
using Sprint1.Trackers;
using Sprint1.Audio;

namespace Sprint1.Entities
{
    public class ItemEntity : Entity
    {        
        public ItemEntity(Game1 game, SpriteEnum spriteType) : base(game, spriteType)
        {
           // this.itemColliderFactory = new ItemColliderFactory(this);
            
            this.rigidbody = new Rigidbody(game, this.Position, new Vector2(0, 0), 1, 0);
            Set(spriteType);
        }

        public ItemEntity(Game1 game, SpriteEnum spriteType, Vector2 position, bool isRight, Color color, float layerDepth = 0) : base(game, spriteType, position, isRight, color, layerDepth)
        {
           // this.itemColliderFactory = new ItemColliderFactory(this);
            
            this.rigidbody = new Rigidbody(game, this.Position, new Vector2(0, 0), 1, 0);
            Set(spriteType);
        }
        public override void OnCollisionEnter(ICollidable collidee, int direction)
        {
            if (collidee is PlayerEntity)
            {
                game.RemoveSprite(this);
            } 
            else
            {
                if (collidee is IEntity && ((((IEntity)collidee).spriteType & SpriteEnum.allBlocks) != (SpriteEnum.block | SpriteEnum.hidden)))
                {
                    correctPosition(collidee, direction);
                }
                
            }
        }

        public override void Set(SpriteEnum spriteType)
        {
            //enemyState = enemyStateFactory.Create(spriteType, enemyState);
            setSprite(spriteType);
            
           // itemColliderFactory.Create(this.spriteType, this.Position);
        }

        public override void Update(GameTime gameTime)
        {
            // itemColliderFactory.Create(this.spriteType, this.Position);
            if (rigidbody != null)
            {
                this.rigidbody.Update(gameTime);
                this.rigidbody.CheckMoving(this);
            }
            
            base.Update(gameTime);
        }


        private void setSprite(SpriteEnum spriteType)
        {
            if ((spriteType & SpriteEnum.item) == SpriteEnum.item)
            {
                SpriteEnum items = SpriteEnum.allItems | spriteType;
                if (items != SpriteEnum.item)
                {
                    base.Set(spriteType);
                }
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

