using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Sprint1.Sprites
{
    public class Sprite : ISprite
    {
        private Texture2D texture;
        public Vector2 Position { get; set; }
        private Rectangle destinationRectangle;
        public Rectangle sourceRectangle;
        public Color color { get; set; }
        private ISpriteAnimation animation;
        public bool IsVisible { get; set; }
        public bool IsRight { get; set; } 
        public float layerDepth { get; set; }

        public int RightEdge { get => destinationRectangle.Right; }

        //Non animated Sprite
        public Sprite(Texture2D texture, Point origin, Point spriteSize, Vector2 position, bool isVisible, bool isRight, Color color, float layerDepth)
        {
            this.texture = texture;
            this.Position = position;
            this.destinationRectangle = new Rectangle(position.ToPoint(), spriteSize);
            this.sourceRectangle = new Rectangle(origin, spriteSize);
            this.color = color;
            this.IsVisible = isVisible;
            this.IsRight = isRight;
            this.animation = new NullAnimation(sourceRectangle);
            this.layerDepth = layerDepth;
        }
        //Animated Sprite
        public Sprite(Texture2D texture, Point origin, Point spriteSize, int rows, int columns, int millisecondsPerFrame,
            Vector2 position, bool isVisible, bool isRight, Color color, float layerDepth) : this(texture, origin, spriteSize, position, isVisible, isRight, color, layerDepth)
        {
            this.animation = new SpriteAnimation(origin, spriteSize, rows, columns, millisecondsPerFrame);
        }

        public void Update(GameTime gameTime)
        {
            if (IsVisible)
            {
                sourceRectangle = animation.GetNextAnimationFrame(gameTime);
                destinationRectangle.Location = Position.ToPoint();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                if (IsRight)
                {
                    spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, 0, new Vector2(0,0), 0, layerDepth);
                }
                else
                {
                    spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, 0, new Vector2(0,0), SpriteEffects.FlipHorizontally, layerDepth);
                }
                
            }
        }
    }
}
