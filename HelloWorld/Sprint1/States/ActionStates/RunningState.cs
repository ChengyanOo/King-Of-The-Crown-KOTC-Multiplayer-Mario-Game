using Microsoft.Xna.Framework;
using Sprint1.Entities;
using Sprint1.Factories.SpriteFactories;
using Sprint1.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.States.ActionStates
{
    internal class RunningState : ActionState
    {

        private float speed = 4;
        public RunningState(PlayerEntity entity, IActionState previousState) : base(entity, previousState)
        { }
        
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

        public override void releaseLeft()
        {
            if (entity.rigidbody.velocity.X < 0)
            {
                toIdle();
            }
        }

        public override void releaseRight()
        {
            if (entity.rigidbody.velocity.X > 0)
            {
                toIdle();
            }
        }

        public override void Enter(IActionState previousState)
        {
            base.Enter(previousState);
            //if () {
            //this.transformation = (new UpDown()).applyTransformation;
            if (!(entity.sprite.IsRight))
            {
                //entity.transformation = (new TransformationMovements()).applyLeftRun;
                entity.rigidbody.velocity = new Vector2(-speed, entity.rigidbody.velocity.Y);
            }
            else
            {
                //entity.transformation = (new TransformationMovements()).applyRightRun;
                entity.rigidbody.velocity = new Vector2(speed, entity.rigidbody.velocity.Y);
            }

            //make sure to have the right sprites
            //Add the transformations
            //somewhere else register keypresses to stop the character from moving to idle

        }
    }
}
