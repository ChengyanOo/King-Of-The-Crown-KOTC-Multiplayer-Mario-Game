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
using Sprint1.Physics;

namespace Sprint1.States.CrownStates
{

    public class ThrownCrownState : CrownState
    {
        float xSpeed;
        float ySpeed;

        public ThrownCrownState(CrownEntity entity, ICrownState previousState) : base(entity, previousState)
        {}

        public override void Enter(ICrownState previousState)
        {
            base.Enter(previousState);
            var random = new Random();
            int direction = (entity.playerEntity.IsRight) ? 1 : -1;
            xSpeed = direction*(5 + random.Next(10)/2);
            ySpeed = -(10 + random.Next(20)/2);
            //float ySpeed = -(5 + random.Next(10) / 2);
            //float xSpeed = direction * (2 + random.Next(4) / 2);

            entity.rigidbody = new Rigidbody(entity.game, entity.Position, new Vector2(xSpeed, ySpeed), 1);
            entity.playerEntity = null;
        }

        public override void collision (ICollidable collidee, int direction) 
        {
            if (collidee is PlayerEntity)
            {
                toAttached((PlayerEntity)collidee);
            }
            else
            {
                switch (direction)
                {
                    case 0:
                        entity.rigidbody.velocity = new Vector2(entity.rigidbody.velocity.X, -ySpeed);
                        break;
                    case 1:
                        entity.rigidbody.velocity = new Vector2(xSpeed, entity.rigidbody.velocity.Y);
                        break;
                    case 2:
                        entity.rigidbody.velocity = new Vector2(entity.rigidbody.velocity.X, ySpeed);
                        break;
                    case 3:
                        entity.rigidbody.velocity = new Vector2(-xSpeed, entity.rigidbody.velocity.Y);
                        break;
                }
            }
            correctPosition(collidee, direction);
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
                    entity.Position = new Vector2(entity.Position.X, entity.Position.Y + collisionRect.Height);
                    break;
                case 1:
                    entity.Position = new Vector2(entity.Position.X - collisionRect.Width, entity.Position.Y);
                    break;
                case 2:
                    entity.Position = new Vector2(entity.Position.X, entity.Position.Y - collisionRect.Height);
                    break;
                case 3:
                    entity.Position = new Vector2(entity.Position.X + collisionRect.Width, entity.Position.Y);
                    break;
            }
        }
    }
}