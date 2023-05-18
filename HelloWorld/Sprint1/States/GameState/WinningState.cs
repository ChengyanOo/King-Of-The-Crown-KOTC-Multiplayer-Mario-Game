
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Sprint1.Entities;
using Sprint1.Trackers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.States.GameState
{
    public class WinningState
    {
        //protected Game1 game1;
        protected GraphicsDevice graphics;

        protected SpriteBatch spriteBatch;

        protected SpriteFont font;

        protected Game1 game;
        public WinningState( GraphicsDevice g1, SpriteBatch spriteBatch, SpriteFont font, Game1 game)
        {
            graphics = g1;
            this.spriteBatch = spriteBatch;
            this.font = font;
            this.game = game;
        }
        // protected override void Update(GameTime gameTime)
        //{


        // }
        /*
         protected override void LoadContent()
         {
             spriteBatch = new SpriteBatch(GraphicsDevice);

         }
        */
        public void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.Black);
            graphics.Clear(Color.Black);
            spriteBatch.Begin();
            if (game.isMarioWon)
            {
                //spriteBatch.DrawString(font, "Congratulations Mario!", new Vector2(300, 100), Color.White);
                spriteBatch.Draw(game.marioWins,new Vector2(0,0),Color.White);
            }
            else
            {
                //spriteBatch.DrawString(font, "Congratulations Luigi!", new Vector2(300, 100), Color.White);
                spriteBatch.Draw(game.luigiWins, new Vector2(0, 0), Color.White);
            }
            
            spriteBatch.DrawString(font, "Play Again?(R)", new Vector2(350, 600), Color.White);
            spriteBatch.DrawString(font, "Quit and Exit(Q)?", new Vector2(550, 600), Color.White);
            spriteBatch.End();
            //base.Draw(gameTime);
        }

        public void WinScreen(GameTime gameTime)
        {
            MediaPlayer.IsMuted = true;
            Draw(gameTime);
        }

        public int PointsToAdd(Entity entity)
        {
            int points = 0;
            int flagPoleHeight = 576 - (int)entity.Position.Y;
            if (flagPoleHeight >= 0 && flagPoleHeight <= 48)
            {
                points = 100;
            }
            else if (flagPoleHeight > 48 && flagPoleHeight <= 174)
            {
                points = 400;
            }
            else if (flagPoleHeight > 174 && flagPoleHeight <= 246)
            {
                points = 800;
            }
            else if (flagPoleHeight > 246 && flagPoleHeight <= 384)
            {
                points = 2000;
            }
            else if (flagPoleHeight > 384 && flagPoleHeight <= 432)
            {
                points = 4000;
            }
            Debug.WriteLine("Points: " + points);
            int multiplier = 3;
            float timeRemaining = 400 - entity.game.GetTimeRemaining();
            points += ((int)(timeRemaining * multiplier));

            return points;
        }

       
    }
}


