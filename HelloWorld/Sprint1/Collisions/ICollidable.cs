using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Entities;

namespace Sprint1.Collisions
{
    public interface ICollidable
    {
        /*
         * Directions (counter clockwise):
         * 0 = up
         * 1 = right
         * 2 = down
         * 3 = left
         */
        Rectangle Collider { get; set; }
        void OnCollisionEnter(ICollidable collision, int direction);
        void DrawCollider(SpriteBatch spriteBatch);
    }
}