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
using Sprint1.Factories.SpriteFactories;
using Sprint1.Transformations;
using System.Diagnostics;

namespace Sprint1.States.EnemyStates
{
    public class EnemyShellState : EnemyState
    {
        float hideTime = 5f; //every  5s.
        float currentTime = 0f;
        Boolean hit = false;
        private float speed = 2;

        public EnemyShellState(EnemyEntity entity, IEnemyState previousState) : base(entity, previousState)
        { }

        public override void Enter(IEnemyState previousState)
        {
            base.Enter(previousState);
        }

        public override void Collision(ICollidable collidee, int direction)
        {
            hit = true;
            if (collidee is PlayerEntity)
            {
                if (direction == 1 && ((((PlayerEntity)collidee).spriteType & SpriteEnum.player) == (SpriteEnum.player)))
                {
                    entity.rigidbody.velocity = new Vector2(-speed, 0);
                }
                else if (direction == 3 && ((((PlayerEntity)collidee).spriteType & SpriteEnum.player) == (SpriteEnum.player)))
                {
                    entity.rigidbody.velocity = new Vector2(speed, 0);
                }
            }

            if (collidee is BlockEntity)
            {
                if (direction == 1 && (((BlockEntity)collidee).spriteType & SpriteEnum.allBlocks) == (SpriteEnum.block | SpriteEnum.stair))
                {
                    //entity.transformation = (new EnemyMovement()).applyLeftRun;
                    entity.rigidbody.velocity = new Vector2(-speed, 0);
                }
                else if (direction == 3 && (((BlockEntity)collidee).spriteType & SpriteEnum.allBlocks) == (SpriteEnum.block | SpriteEnum.stair))
                {
                    entity.rigidbody.velocity = new Vector2(speed, 0);
                }

                if (direction == 0 | ((((BlockEntity)collidee).spriteType & SpriteEnum.allBlocks) != (SpriteEnum.block | SpriteEnum.hidden)))
                {
                    correctPosition(collidee, direction);
                }

                if (direction == 2 && ((((BlockEntity)collidee).spriteType & SpriteEnum.allBlocks) != (SpriteEnum.block | SpriteEnum.hidden)))
                {
                    correctPosition(collidee, direction);
                }
            }
        }
        public override void Update(GameTime gameTime)
        {
            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (!hit && currentTime >= hideTime)
            {
                toKoopa();
                currentTime = 0;
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