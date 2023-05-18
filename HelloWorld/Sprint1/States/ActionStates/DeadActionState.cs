using Microsoft.Xna.Framework;
using Sprint1.Entities;
using Sprint1.Factories.SpriteFactories;
using Sprint1.States.PowerStates;
using Sprint1.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.States.ActionStates
{
    internal class DeadActionState: ActionState
    {
        public DeadActionState(PlayerEntity entity, IActionState previousState) : base(entity, previousState)
        { }

        public override void Enter(IActionState previousState)
        {
            entity.rigidbody.velocity = Vector2.Zero;
            base.Enter(previousState);
        }

        public override void Exit()
        {
            //toPrevious();
            base.Exit();
        }
    }
}
