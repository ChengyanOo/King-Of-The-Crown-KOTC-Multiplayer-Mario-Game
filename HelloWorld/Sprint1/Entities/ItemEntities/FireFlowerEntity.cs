using Microsoft.Xna.Framework;
using Sprint1.Collisions;
using Sprint1.Factories.SpriteFactories;
using Sprint1.Factories.StateFactories;
using Sprint1.Factories;
using Sprint1.States.BlockStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.Entities.ItemEntities
{
    public class FireFlowerEntity : ItemEntity
    {
      

        public FireFlowerEntity(Game1 game, SpriteEnum spriteType) : base(game, spriteType)
        {
            
            Set(spriteType);
        }

        public FireFlowerEntity(Game1 game, SpriteEnum spriteType, Vector2 position, bool isRight, Color color, float layerDepth = 0) : base(game, spriteType, position, isRight, color, layerDepth)
        {
           
            Set(spriteType);
        }
        /// <summary>
        /// Makes the entity have the correct state and sprite for a given SpriteEnum
        /// </summary>
        /// <param name="spriteType">A SpriteEnum specifying the correct sprite type</param>
        public override void Set(SpriteEnum spriteType)
        {
           
            setSprite(spriteType);
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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private void setSprite(SpriteEnum spriteType)
        {
            if ((spriteType & SpriteEnum.item) == SpriteEnum.item)
            {
                SpriteEnum item = SpriteEnum.allItems | spriteType;
                if (item != SpriteEnum.item)
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
