using Sprint1.Entities;
using Sprint1.Factories.SpriteFactories;
using Sprint1.Sprites;
using Sprint1.States.PowerStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Transformations;
using Sprint1.Collisions;
using Microsoft.Xna.Framework;
using Sprint1.Physics;


namespace Sprint1.States.BlockStates
{
    public class BumpedTransitionBlock : BlockState
    {
        private static float speed = 2;
        private static float offset = -10;
        private float anchor;
        private bool backtrack = false;

        public BumpedTransitionBlock(BlockEntity entity, IBlockState previousState) : base(entity, previousState)
        {
            Enter(previousState);
        }

        public override void Enter(IBlockState previousState)
        {
            base.Enter(previousState);
            anchor = entity.Position.Y;
            if (entity.rigidbody == null)
            {
                entity.rigidbody = new Rigidbody(entity.game, entity.Position, new Vector2(0, 0), 1, 0);
            }
            entity.rigidbody.velocity = new Vector2(0, -speed);
        }

        public override void Update(GameTime gameTime)
        {
            if (!backtrack)
            {
                if (entity.Position.Y < (anchor + offset))
                {
                    entity.rigidbody.velocity = new Vector2(0, speed);
                    backtrack = true;
                }
            }
            else if (entity.Position.Y >= anchor)
            {
                entity.Position = new Vector2(entity.Position.X, anchor);
                Exit();
            }
        }

        private bool checkOffset()
        {
            return (entity.Position.Y < (anchor + offset));
        }

        public override void Exit()
        {
            entity.rigidbody.velocity = new Vector2(0, 0);
            toPrevious();
        }
    }
}
