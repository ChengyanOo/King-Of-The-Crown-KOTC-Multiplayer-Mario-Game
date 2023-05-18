using Microsoft.Xna.Framework;
using Sprint1.Entities;
using Sprint1.Factories.SpriteFactories;
using Sprint1.States.PowerStates;
using Sprint1.Transformations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Sprint1.States.ActionStates
{
    internal class FallingState: ActionState
    {
        public FallingState(PlayerEntity entity, IActionState previousState) : base(entity, previousState)
        { }

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

        public override void Enter(IActionState previousState)
        {
            base.Enter(previousState);
            //entity.rigidbody.velocity = new Vector2(entity.rigidbody.velocity.X, );
        }
    }
}
