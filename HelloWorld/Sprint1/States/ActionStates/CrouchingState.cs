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
    internal class CrouchingState: ActionState
    {
        public CrouchingState(PlayerEntity entity, IActionState previousState) : base(entity, previousState)
        { }

        public override void faceUp() 
        {
            toIdle();
            entity.transformation = (new TransformationMovements()).applyIdle;
        }
        public override void faceLeft()
        {
            if (!(entity.sprite.IsRight))
            {
                toRunning();
            }
            else
            {
                entity.sprite.IsRight = false;
            }
        }
        public override void faceRight()
        {
            if (entity.sprite.IsRight)
            {
                toRunning();
            }
            else
            {
                entity.sprite.IsRight = true;
            }
        }

        public override void faceDown()
        {
          toFalling();
        }

        public override void Enter(IActionState previousState)
        {
            base.Enter(previousState);
            //entity.transformation = (new NullTransformation()).applyTransformation;
            //entity.transformation = new TransformationMovements().applyDown;//This will fix it but he can move and crouch
            
        }
    }
}
