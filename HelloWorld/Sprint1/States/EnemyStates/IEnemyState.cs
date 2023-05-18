using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Collisions;
using Sprint1.States.ActionStates;

namespace Sprint1.States.EnemyStates
{
    public interface IEnemyState
    {
        void goombaToDead();
        void Enter(IEnemyState previousState);
        void Exit();
        void Collision(ICollidable collidee, int direction);
        void Update(GameTime gameTime);
    }
}