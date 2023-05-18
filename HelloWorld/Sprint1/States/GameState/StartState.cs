using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.States.GameState
{
    public class StartState
    {
        protected GraphicsDevice graphics;

        protected SpriteBatch spriteBatch;

        protected SpriteFont font;

        protected Game1 game;
        public StartState(GraphicsDevice g1, SpriteBatch spriteBatch, SpriteFont font, Game1 game)
        {
            graphics = g1;
            this.spriteBatch = spriteBatch;
            this.font = font;
            this.game = game;
        }

        public void Draw(GameTime gameTime)
        {
            graphics.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(game.startScreen, new Vector2(0, 0), Color.White);

            spriteBatch.DrawString(font, "Press Backspace to Start!", new Vector2(500, 635), Color.White);
            //spriteBatch.DrawString(font, "Quit and Exit(Q)?", new Vector2(550, 600), Color.White);
            spriteBatch.End();
        }

        public void StartScreen(GameTime gameTime)
        {
            Draw(gameTime);
        }

    }
}
