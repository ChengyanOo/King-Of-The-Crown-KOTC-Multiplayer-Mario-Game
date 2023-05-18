using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Collisions;

namespace Sprint1.States.PowerStates
{
    public interface IPowerState
    {
        void toSmall();
        void toSuper();
        void toFire();
        void toDead();
        void takeDamage();
        void interactCrown();

        void Update(GameTime gameTime);
        void Enter(IPowerState previousState);
        void Exit();

        void Collision(ICollidable collidee, int direction);
    }
}