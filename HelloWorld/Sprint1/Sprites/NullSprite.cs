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
    public class NullSprite : ISprite
    {
        public Vector2 Position { get; set; }
        public Rectangle Collider { get; set; }
        public bool IsVisible { get; set; }
        public bool IsRight { get; set; }
        public Color color { get; set; }
        public float layerDepth { get; set; }
        public int RightEdge { get => 0; }
        public NullSprite ()
        {
            Position = new Vector2(0, 0);
            IsVisible = false;
            IsRight = true;
            color = Color.White;
            layerDepth = 0;
        }

        public void Update(GameTime gameTime) { }
        public void Draw(SpriteBatch spriteBatch) { }
        public void DrawCollider(SpriteBatch spriteBatch) { }
    }
}