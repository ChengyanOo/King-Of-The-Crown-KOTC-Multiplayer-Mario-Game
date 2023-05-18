using Sprint1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Factories.SpriteFactories;
using Sprint1.Transformations;
using Microsoft.Xna.Framework;

namespace Sprint1.States.ActionStates
{
    internal class JumpingState: ActionState
    {
        public JumpingState(PlayerEntity entity, IActionState previousState) : base(entity, previousState)
        { }

        public override void faceDown()
        {
            toFalling();
        }
        public override void faceLeft()
        {
            entity.rigidbody.velocity = new Vector2(-2, entity.rigidbody.velocity.Y);
            entity.sprite.IsRight = false;
        }
        public override void faceRight()
        {
            entity.rigidbody.velocity = new Vector2(2, entity.rigidbody.velocity.Y);
            entity.sprite.IsRight = true;
        }

        public override void releaseLeft()
        {
            if (entity.rigidbody.velocity.X < 0)
            {
                entity.rigidbody.velocity = new Vector2(0, entity.rigidbody.velocity.Y);
            }
        }

        public override void releaseRight()
        {
           if (entity.rigidbody.velocity.X > 0)
            {
                entity.rigidbody.velocity = new Vector2(0, entity.rigidbody.velocity.Y);
            }
        }

        public override void releaseUp()
        {
            entity.rigidbody.velocity = new Vector2(entity.rigidbody.velocity.X, 0);
            toFalling();
        }

        public override void Enter(IActionState previousState)
        {
            base.Enter(previousState);
            entity.rigidbody.velocity = new Vector2(entity.rigidbody.velocity.X, -20);
        }
    }
}
