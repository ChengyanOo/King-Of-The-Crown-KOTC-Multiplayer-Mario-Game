
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint1.Entities;
using Sprint1.Trackers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.States.GameState
{
    public class GameOverState
    {
        //protected Game1 game1;
        protected GraphicsDevice graphics;
        protected SpriteBatch spriteBatch;
        protected Game1 game;
        protected SpriteFont font;
        public GameOverState(GraphicsDevice g1, SpriteBatch spriteBatch, SpriteFont font, Game1 game)
        {
            graphics = g1;
            this.spriteBatch = spriteBatch;
            this.font = font;
        }

        public void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.Black);
            graphics.Clear(Color.Black);
            spriteBatch.Begin();
            if (!game.isMarioWon)
            {
                spriteBatch.DrawString(font, "Game Over Mario! :(", new Vector2(300, 100), Color.White);
            }
            else
            {
                spriteBatch.DrawString(font, "Game Over Luigi! :(", new Vector2(300, 100), Color.White);
            }
           
            spriteBatch.DrawString(font, "Try Again(R)?", new Vector2(280, 200), Color.White);
            spriteBatch.DrawString(font, "Quit and Exit(Q)?", new Vector2(380, 200), Color.White);
            spriteBatch.End();
            //base.Draw(gameTime);
        }

        

    }
}
