using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint1.Entities;
using Sprint1.Factories.SpriteFactories;
using Sprint1.Sprites;
using Sprint1.States.PowerStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Collisions;

namespace Sprint1.States.BlockStates
{
    public interface IBlockState
    {
        void toUsed();
        void toBrick();
        void toBumped();
        void toPrevious();

        void Update(GameTime gameTime);
        void Enter(IBlockState previousState);
        void Exit();

        void Collision(ICollidable collidee, int direction);
    }
}