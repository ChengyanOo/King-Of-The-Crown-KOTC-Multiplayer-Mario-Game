using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.Sprites
{
    public class NullAnimation : ISpriteAnimation
    {
        private Rectangle sourceRectangle;
        public int Rows { get; set; }
        public int Columns { get; set; }

        public NullAnimation(Rectangle sourceRectangle)
        {
            this.sourceRectangle = sourceRectangle;
            this.Rows = 0;
            this.Columns = 0;
        }


        public Rectangle GetNextAnimationFrame(GameTime gameTime)
        {
            return sourceRectangle;
        }
    }
}
