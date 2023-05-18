using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Entities;
using Sprint1.Transformations;
using Sprint1.Entities.ItemEntities.Fireball;
using Sprint1.Factories.SpriteFactories;
using System.Diagnostics;

namespace Sprint1.States.PowerStates
{
    public class FireState : PowerState
    {
        private FireballPool fireballPool;
        public FireState(PlayerEntity entity, IPowerState previousState) : base(entity, previousState)
        {
           
        }

        public override void Enter(IPowerState previousState)
        {
            base.Enter(previousState);
            fireballPool = new FireballPool(entity.game, 2, SpriteEnum.particle | SpriteEnum.fireball, entity.layerDepth);
        }

        public override void Exit()
        {
            fireballPool = null;
        }

        public override void takeDamage()
        {
            toSmall();
        }
        

        public void Fire()
        {
            FireballEntity fireball = fireballPool.GetFireball();
            if (fireball != null)
            {
                fireball.rigidbody.position = entity.Position;
                fireball.rigidbody.velocity = new Vector2(200, 0);
                if (!entity.IsRight)
                {
                    fireball.rigidbody.velocity = new Vector2(-200, 0);
                }
                fireball.fireballPool = fireballPool;
                entity.rigidbody.isGrounded = false;
                entity.game.AddSprite(fireball);
            }
            
        }
    }
}