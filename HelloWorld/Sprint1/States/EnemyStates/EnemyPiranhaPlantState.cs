using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Sprint1.Entities;
using Sprint1.Collisions;
using Sprint1.Entities.ItemEntities.Fireball;
using Sprint1.Trackers;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Sprint1.Physics;

namespace Sprint1.States.EnemyStates
{
    public class EnemyPiranhaPlantState: EnemyState
    {
        private IEntity player;//Set to NUllEntity if not given Mario
        private float speed = 2;

        private bool goingUp = true;
        private float anchor;
        private float peak;

        private float timeSincePeak;
        private float timeSinceAnchor;
        private bool atPeak = false;
        private bool atAnchor = false;
        public EnemyPiranhaPlantState(EnemyEntity entity, IEnemyState previousState) : base(entity, previousState)
        {
            this.anchor = entity.Position.Y;
            peak = entity.Position.Y - 90;
            timeSincePeak = 0;
            timeSinceAnchor = 0;
            entity.rigidbody = new Rigidbody(entity.game, entity.Position, new Vector2(0,0), 1, 0);
            entity.rigidbody.velocity = new Vector2(0, -speed);
        }

        public override void Enter(IEnemyState previousState)
        {
            base.Enter(previousState);
        }

        public override void Collision(ICollidable collidee, int direction)
        {
            if(collidee is FireballEntity || collidee is EnemyShellState)
            {
                PointEventArgs args = new PointEventArgs { PointValue = 200 };
                onIncScore(args);
                this.IncScore -= entity.game.pointTracker.IncScore;
                entity.game.RemoveSprite(entity);
            }
        }

        public void AddPlayer(PlayerEntity playerMario)
        {
            player = playerMario;
        }
        
        public override void Update(GameTime gameTime)
        {
            if (player == null | !MarioNearby())
            {
                if (atPeak)
                {
                    timeSincePeak += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (timeSincePeak > 1)
                    {
                        atPeak = false;
                        entity.rigidbody.velocity = new Vector2(0, speed);
                    }
                }
                else if (atAnchor)
                {
                    timeSinceAnchor += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (timeSinceAnchor > 3)
                    {
                        atAnchor = false;
                        entity.rigidbody.velocity = new Vector2(0, -speed);
                    }
                }
                else if(goingUp)
                {
                    if (entity.Position.Y < peak)
                    {
                        entity.Position = new Vector2(entity.Position.X,peak);
                        goingUp = false;
                        entity.rigidbody.velocity = Vector2.Zero;
                        timeSincePeak = 0;
                        atPeak = true;
                    }
                }
                else if (!goingUp)
                {
                    if (entity.Position.Y > anchor)
                    {
                        entity.Position = new Vector2(entity.Position.X, anchor);
                        goingUp = true;
                        entity.rigidbody.velocity = Vector2.Zero;
                        timeSinceAnchor = 0;
                        atAnchor = true;
                    }
                }
            }
        }

        private bool MarioNearby()
        {
            Rectangle entityCollider = entity.Collider; 
            entityCollider.Inflate(60, 30);
            entityCollider.Y += -30;

            return entityCollider.Intersects(player.Collider);
        }
    }
}
