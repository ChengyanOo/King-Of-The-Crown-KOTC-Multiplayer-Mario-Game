using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.Sprites
{
    public interface ISpriteAnimation
    {
        int Rows { get; set; }
        int Columns { get; set; }
        Rectangle GetNextAnimationFrame(GameTime gameTime);
    }
}

