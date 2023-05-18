using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Sprint1.Sprites
{
    public interface ISprite
    {
        public Vector2 Position { get; set; }
        public bool IsVisible { get; set; }
        public bool IsRight { get; set; }
        public Color color { get; set; }
        public float layerDepth { get; set; }
        public int RightEdge { get; }
        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);
    }
}