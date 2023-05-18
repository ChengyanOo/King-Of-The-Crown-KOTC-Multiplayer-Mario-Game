using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Entities;
using Sprint1.Transformations;
using Sprint1.Collisions;

namespace Sprint1.States.PowerStates
{
    public class DeadState : PowerState
    {
        public float totalTime;
        public static float endTime = 1;

        public DeadState(PlayerEntity entity, IPowerState previousState) : base(entity, previousState)
        {
            totalTime = 0;
        }

        public override void Enter(IPowerState previousState)
        {
            totalTime = 0;
            if (entity.crownEntity != null)
            {
                throwCrown();
            }
            base.Enter(previousState);
        }

        public override void takeDamage()
        { }

        /*
        public override void Collision(ICollidable collidee, int direction) { }
        */

        public override void Update(GameTime gameTime)
        {
            totalTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (totalTime > endTime)
            {
                entity.rigidbody.velocity = new Vector2(0, entity.rigidbody.velocity.Y);
                toSmall();
                entity.turnIdle();
            }
            base.Update(gameTime);
        }
    }
}