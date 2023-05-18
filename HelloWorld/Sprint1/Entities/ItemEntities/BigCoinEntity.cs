using Microsoft.Xna.Framework;
using Sprint1.Collisions;
using Sprint1.Factories.SpriteFactories;

using Sprint1.Factories;
using Sprint1.States.BlockStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Transformations;
using Sprint1.Physics;
using Sprint1.Trackers;

namespace Sprint1.Entities.ItemEntities
{
    public class BigCoinEntity : ItemEntity
    {
        private float speed = 200;
        public event EventHandler<PointEventArgs> IncScore;

        public BigCoinEntity(Game1 game, SpriteEnum spriteType) : base(game, spriteType)
        {
            Set(spriteType);
            this.IncScore += game.pointTracker.IncScore;
        }

        public BigCoinEntity(Game1 game, SpriteEnum spriteType, Vector2 position, bool isRight, Color color, float layerDepth = 0) : base(game, spriteType, position, isRight, color, layerDepth)
        {
            this.rigidbody = new Rigidbody(game, this.Position, new Vector2(0, 0), 1, 0);
            Set(spriteType);
            this.IncScore += game.pointTracker.IncScore;
        }
        /// <summary>
        /// Makes the entity have the correct state and sprite for a given SpriteEnum
        /// </summary>
        /// <param name="spriteType">A SpriteEnum specifying the correct sprite type</param>
        public override void Set(SpriteEnum spriteType)
        {
            this.rigidbody = new Rigidbody(game, this.Position, new Vector2(0, 0), 1, 0);
            setSprite(spriteType);
        }

        public override void OnCollisionEnter(ICollidable collidee, int direction)
        {
            if (collidee is PlayerEntity)
            {
                PointEventArgs args = new PointEventArgs { PointValue = 100 };
                onIncScore(args);
                game.RemoveSprite(this);
            } 
        }

        public override void Update(GameTime gameTime)
        {
            this.Position = transformation(this.Position);
            this.rigidbody.Update(gameTime);
            this.rigidbody.CheckMoving(this);
            base.Update(gameTime);
        }

        private void setSprite(SpriteEnum spriteType)
        {
            if ((spriteType & SpriteEnum.block) == SpriteEnum.block)
            {
                SpriteEnum block = SpriteEnum.allBlocks | spriteType;
                if (block != SpriteEnum.block)
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
            this.rigidbody.velocity = new Vector2(0, 0);
            switch (direction)
            {
                case 0:
                    this.rigidbody.position = new Vector2(this.Position.X, this.Position.Y + collisionRect.Height);
                    this.Position = new Vector2(this.Position.X, this.Position.Y + collisionRect.Height);
                    this.rigidbody.velocity = new Vector2(speed, 0);
                    break;
                case 1:
                    this.rigidbody.position = new Vector2(this.Position.X - collisionRect.Width, this.Position.Y);
                    this.Position = new Vector2(this.Position.X, this.Position.Y - collisionRect.Height);
                    this.rigidbody.velocity = new Vector2(-speed, 0);
                    break;
                case 2:
                    this.rigidbody.position = new Vector2(this.Position.X, this.Position.Y - collisionRect.Height);
                    this.Position = new Vector2(this.Position.X, this.Position.Y - collisionRect.Height);
                    this.rigidbody.velocity = new Vector2(speed, 0);
                    break;
                case 3:
                    this.rigidbody.position = new Vector2(this.Position.X + collisionRect.Width, this.Position.Y);
                    this.Position = new Vector2(this.Position.X, this.Position.Y - collisionRect.Height);
                    this.rigidbody.velocity = new Vector2(speed, 0);
                    break;
            }
        }



        protected virtual void onIncScore(PointEventArgs e)
        {
            EventHandler<PointEventArgs> handler = IncScore;
            if (handler != null)
                handler(this, e);
        }

    }
}
