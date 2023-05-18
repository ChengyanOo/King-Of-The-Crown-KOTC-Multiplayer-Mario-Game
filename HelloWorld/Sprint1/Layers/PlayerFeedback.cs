using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Sprint1.Sprites;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sprint1.Entities;
using System.Diagnostics;
using Sprint1.Audio;
using System.Reflection.Metadata;
using Sprint1.Factories.SpriteFactories;
using Sprint1.TimerSlider;

namespace Sprint1.Scrolling
{
    public class PlayerFeedback
    {
        protected SpriteBatch spriteBatch;
        protected SpriteFont font;
        protected Game1 game1;

        //UNUSED
       // private ProgressionBar progressionBar;
       // private ProgressBarAnimated progressBarAnimated;
        //private ProgressionBar progressionBarLuigi;
        //private ProgressBarAnimated progressBarAnimatedLuigi;

        private int height = 50;
        //UNUSED, for spacing
        private int screenWidth;

        public PlayerFeedback(SpriteBatch spriteBatch, SpriteFont font, Game1 game)
        {
            this.spriteBatch = spriteBatch;
            this.font = font;
            game1 = game;

            screenWidth = game.GraphicsDevice.Viewport.Width;
        }

        //UNUSED , trying to add progression bar here
      /*  public PlayerFeedback(SpriteBatch spriteBatch, SpriteFont font, Game1 game, Texture2D front, Texture2D back) : this(spriteBatch, font, game)
        {
            progressionBar = new(back, front, 0, new(screenWidth / 12, height + font.LineSpacing), game);
            progressBarAnimated = new(back, front, 5, new(screenWidth / 12, height + font.LineSpacing), game);

            progressionBarLuigi = new(back, front, 0, new(630, height + font.LineSpacing), game);
            progressBarAnimatedLuigi = new(back, front, 5, new(10 * screenWidth / 12, height + font.LineSpacing), game);
        }
      */
        public void Draw(float health, int points, int time)
        {
            Color color = Color.Firebrick;
            //string decimalLength = String.Format("{0:000000}", points);

            spriteBatch.Begin();

            spriteBatch.DrawString(font, "MARIO", new Vector2(127, height), color);
            spriteBatch.DrawString(font, "CROWNED", new Vector2(357, height), color); ;
            spriteBatch.DrawString(font, "" + game1.roundTracker.marioWinningRounds, new Vector2(395, height + font.LineSpacing), color);


            spriteBatch.DrawString(font, "ROUND", new Vector2(580, height), color);
            spriteBatch.DrawString(font, "" + game1.roundTracker.totalRounds, new Vector2(610, height + font.LineSpacing), color);

            spriteBatch.DrawString(font, "CROWNED", new Vector2(800, height), color);
            spriteBatch.DrawString(font, "" + game1.roundTracker.luigiWinningRounds, new Vector2(835, height + font.LineSpacing), color);
            spriteBatch.DrawString(font, "LUIGI", new Vector2(1027, height), color);

            /*
            //UNUSED, trying to add progression bar here
            if(progressionBar != null)
            {
                progressionBar.Draw();
                progressBarAnimated.Draw();
                progressionBarLuigi.Draw();
                progressBarAnimatedLuigi.Draw();
            }
            */

            spriteBatch.End();
        }

        //UNUSED, trying to add progression bar here
      /*
        public void Update(float time)
        {
            if(progressBarAnimated != null)
            {
                progressionBar.Update(time);
                progressBarAnimated.Update(progressionBar.marioHoldTime);
                progressBarAnimatedLuigi.Update(progressionBarLuigi.luigiHoldTime);
            }
        }
      */
    }
}