using Sprint1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Transformations;
using Sprint1.Factories.SpriteFactories;
using Microsoft.Xna.Framework;

namespace Sprint1.States.ActionStates
{
    internal class IdleState: ActionState
    {
        // private bool moving;
       // TransformationMovements t1;
        public IdleState(PlayerEntity entity, IActionState previousState) : base(entity, previousState)
        {
            //     moving= new TransformationMovements().isMoving();
            //TransformationMovements t1 = new TransformationMovements();
        }

        public override void faceLeft()
        {
            entity.IsRight = false;
            toRunning();
        }
        public override void faceRight()
        {
            entity.IsRight = true;
            toRunning();
        }
        public override void faceUp()
        {
            toJumping();
        }    

        public override void Enter(IActionState previousState)
        {
            base.Enter(previousState);
            //if () {
            //this.transformation = (new UpDown()).applyTransformation;
            //entity.transformation = (new NullTransformation()).applyTransformation;
            entity.rigidbody.velocity = new Vector2(0, entity.rigidbody.velocity.Y);
            //}

            //make sure to have the right sprites
            //Add the transformations
            //somewhere else register keypresses to stop the character from moving to idle

        }
    }
}

