using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.Sprites
{
    public class SpriteAnimation : ISpriteAnimation
    {
        private Point origin;
        private Point frameSize;
        private int rows;
        private int columns;
        private int currentFrame;
        private int totalFrames;
        private int millisecondsPerFrame;
        private int timeSinceLastFrame;

        public SpriteAnimation(Point origin, Point frameSize, int rows, int columns, int millisecondsPerFrame)
        {
            this.origin = origin;
            this.frameSize = frameSize;
            this.rows = rows;
            this.columns = columns;
            this.currentFrame = 0;
            this.totalFrames = rows * columns;
            this.millisecondsPerFrame = millisecondsPerFrame;
            this.timeSinceLastFrame = 0;
        }

        public int Rows
        {
            get { return rows; }

            set
            {
                rows = value;
                totalFrames = rows * columns;
            }
        }
        public int Columns
        {
            get { return columns; }

            set
            {
                columns = value;
                totalFrames = rows * columns;
            }
        }

        public Rectangle GetNextAnimationFrame(GameTime gameTime)
        {
            updateCurrentFrame(gameTime);
            return getSourceRectangle();
        }

        private void updateCurrentFrame(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                currentFrame++;
                if (currentFrame == totalFrames)
                {
                    currentFrame = 0;
                }
                timeSinceLastFrame = 0;
            }

        }

        private Rectangle getSourceRectangle()
        {
            int row = currentFrame / columns;
            int column = currentFrame % columns;
            Point currentAtlasPosition = new Point(origin.X + (frameSize.X * column), origin.Y + (frameSize.Y * row));

            return new Rectangle(currentAtlasPosition, frameSize);
        }
    }
}
