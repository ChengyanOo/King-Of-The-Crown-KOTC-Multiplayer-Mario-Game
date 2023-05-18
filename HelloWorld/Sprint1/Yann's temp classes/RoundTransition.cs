using System;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Timers;
using System.Threading;

namespace Sprint1.Yannstempclasses
{
    public class RoundTransition
    {
        private Game1 reciever;
        private Texture2D transitionScreen00;
        private Texture2D transitionScreen10;
        private Texture2D transitionScreen01;
        private Texture2D transitionScreen11;
        private Texture2D transitionScreen12;
        private Texture2D transitionScreen21;
        private Texture2D transitionScreen20;
        private Texture2D transitionScreen02;

        private System.Timers.Timer timer00;
        private System.Timers.Timer timer01;
        private System.Timers.Timer timer10;
        private System.Timers.Timer timer11;
        private System.Timers.Timer timer12;
        private System.Timers.Timer timer21;
        private System.Timers.Timer timer20;
        private System.Timers.Timer timer02;

        public bool displayed00;
        public bool displayed01;
        public bool displayed10;
        public bool displayed11;
        public bool displayed12;
        public bool displayed21;
        public bool displayed20;
        public bool displayed02;

        public RoundTransition(Game1 game)
        {
            this.reciever = game;
            timer00 = new System.Timers.Timer(2000);
            timer00.Elapsed += StopDisplay00;

            timer01 = new System.Timers.Timer(2000);
            timer01.Elapsed += StopDisplay01;

            timer10 = new System.Timers.Timer(2000);
            timer10.Elapsed += StopDisplay10;

            timer11 = new System.Timers.Timer(2000);
            timer11.Elapsed += StopDisplay11;

            timer12 = new System.Timers.Timer(2000);
            timer12.Elapsed += StopDisplay12;

            timer21 = new System.Timers.Timer(2000);
            timer21.Elapsed += StopDisplay21;

            timer20 = new System.Timers.Timer(2000);
            timer20.Elapsed += StopDisplay20;

            timer02 = new System.Timers.Timer(2000);
            timer02.Elapsed += StopDisplay02;

            displayed00 = true;
            displayed01 = true;
            displayed10 = true;
            displayed11 = true;
            displayed12 = true;
            displayed21 = true;
            displayed20 = true;
            displayed02 = true;
        }

        public void LoadRoundTransitionPics(ContentManager contentManager)
        {
            transitionScreen00 = contentManager.Load<Texture2D>("scores/luigi0_mario0");
            transitionScreen10 = contentManager.Load<Texture2D>("scores/luigi1_mario0");
            transitionScreen01 = contentManager.Load<Texture2D>("scores/luigi0_mario1");
            transitionScreen11 = contentManager.Load<Texture2D>("scores/luigi1_mario1");
            transitionScreen12 = contentManager.Load<Texture2D>("scores/luigi1_mario2");
            transitionScreen21 = contentManager.Load<Texture2D>("scores/luigi2_mario1");
            transitionScreen20 = contentManager.Load<Texture2D>("scores/luigi2_mario0");
            transitionScreen02 = contentManager.Load<Texture2D>("scores/luigi0_mario2");
            //transitionScreen00 = contentManager.Load<Texture2D>("frontProgress");
            //transitionScreen10 = contentManager.Load<Texture2D>("frontProgress");
            //transitionScreen01 = contentManager.Load<Texture2D>("frontProgress");
            //transitionScreen11 = contentManager.Load<Texture2D>("frontProgress");
            //transitionScreen12 = contentManager.Load<Texture2D>("frontProgress");
            //transitionScreen21 = contentManager.Load<Texture2D>("frontProgress");
            //transitionScreen20 = contentManager.Load<Texture2D>("frontProgress");
            //transitionScreen02 = contentManager.Load<Texture2D>("frontProgress");
        }

        public void Draw()
        {
            if (!displayed00)
            {
                reciever.spriteBatch.Begin();
                var screenCenter = new Vector2(reciever.GraphicsDevice.Viewport.Bounds.Width / 2, reciever.GraphicsDevice.Viewport.Bounds.Height / 2);
                var textureCenter = new Vector2(transitionScreen00.Width / 2, transitionScreen00.Height / 2);
                //reciever.spriteBatch.Draw(transitionScreen, screenCenter, Color.White);
                reciever.spriteBatch.Draw(transitionScreen00, screenCenter, null, Color.White, 0f, textureCenter, 1f, SpriteEffects.None, 1f);
                reciever.spriteBatch.End();
                timer00.Start();
            }
            else if (!displayed01)
            {
                reciever.spriteBatch.Begin();
                var screenCenter = new Vector2(reciever.GraphicsDevice.Viewport.Bounds.Width / 2, reciever.GraphicsDevice.Viewport.Bounds.Height / 2);
                var textureCenter = new Vector2(transitionScreen01.Width / 2, transitionScreen01.Height / 2);
                //reciever.spriteBatch.Draw(transitionScreen, screenCenter, Color.White);
                reciever.spriteBatch.Draw(transitionScreen01, screenCenter, null, Color.White, 0f, textureCenter, 1f, SpriteEffects.None, 1f);
                reciever.spriteBatch.End();
                timer01.Start();
            }
            else if (!displayed10)
            {
                reciever.spriteBatch.Begin();
                var screenCenter = new Vector2(reciever.GraphicsDevice.Viewport.Bounds.Width / 2, reciever.GraphicsDevice.Viewport.Bounds.Height / 2);
                var textureCenter = new Vector2(transitionScreen10.Width / 2, transitionScreen10.Height / 2);
                //reciever.spriteBatch.Draw(transitionScreen, screenCenter, Color.White);
                reciever.spriteBatch.Draw(transitionScreen10, screenCenter, null, Color.White, 0f, textureCenter, 1f, SpriteEffects.None, 1f);
                reciever.spriteBatch.End();
                timer10.Start();
            }
            else if (!displayed11)
            {
                reciever.spriteBatch.Begin();
                var screenCenter = new Vector2(reciever.GraphicsDevice.Viewport.Bounds.Width / 2, reciever.GraphicsDevice.Viewport.Bounds.Height / 2);
                var textureCenter = new Vector2(transitionScreen11.Width / 2, transitionScreen11.Height / 2);
                //reciever.spriteBatch.Draw(transitionScreen, screenCenter, Color.White);
                reciever.spriteBatch.Draw(transitionScreen11, screenCenter, null, Color.White, 0f, textureCenter, 1f, SpriteEffects.None, 1f);
                reciever.spriteBatch.End();
                timer11.Start();
            }
            else if (!displayed12)
            {
                reciever.spriteBatch.Begin();
                var screenCenter = new Vector2(reciever.GraphicsDevice.Viewport.Bounds.Width / 2, reciever.GraphicsDevice.Viewport.Bounds.Height / 2);
                var textureCenter = new Vector2(transitionScreen12.Width / 2, transitionScreen12.Height / 2);
                //reciever.spriteBatch.Draw(transitionScreen, screenCenter, Color.White);
                reciever.spriteBatch.Draw(transitionScreen12, screenCenter, null, Color.White, 0f, textureCenter, 1f, SpriteEffects.None, 1f);
                reciever.spriteBatch.End();
                timer12.Start();
            }
            else if (!displayed21)
            {
                reciever.spriteBatch.Begin();
                var screenCenter = new Vector2(reciever.GraphicsDevice.Viewport.Bounds.Width / 2, reciever.GraphicsDevice.Viewport.Bounds.Height / 2);
                var textureCenter = new Vector2(transitionScreen21.Width / 2, transitionScreen21.Height / 2);
                //reciever.spriteBatch.Draw(transitionScreen, screenCenter, Color.White);
                reciever.spriteBatch.Draw(transitionScreen21, screenCenter, null, Color.White, 0f, textureCenter, 1f, SpriteEffects.None, 1f);
                reciever.spriteBatch.End();
                timer21.Start();
            }
            else if (!displayed20)
            {
                reciever.spriteBatch.Begin();
                var screenCenter = new Vector2(reciever.GraphicsDevice.Viewport.Bounds.Width / 2, reciever.GraphicsDevice.Viewport.Bounds.Height / 2);
                var textureCenter = new Vector2(transitionScreen20.Width / 2, transitionScreen20.Height / 2);
                //reciever.spriteBatch.Draw(transitionScreen, screenCenter, Color.White);
                reciever.spriteBatch.Draw(transitionScreen20, screenCenter, null, Color.White, 0f, textureCenter, 1f, SpriteEffects.None, 1f);
                reciever.spriteBatch.End();
                timer20.Start();
            }
            else if (!displayed02)
            {
                reciever.spriteBatch.Begin();
                var screenCenter = new Vector2(reciever.GraphicsDevice.Viewport.Bounds.Width / 2, reciever.GraphicsDevice.Viewport.Bounds.Height / 2);
                var textureCenter = new Vector2(transitionScreen02.Width / 2, transitionScreen02.Height / 2);
                //reciever.spriteBatch.Draw(transitionScreen, screenCenter, Color.White);
                reciever.spriteBatch.Draw(transitionScreen02, screenCenter, null, Color.White, 0f, textureCenter, 1f, SpriteEffects.None, 1f);
                reciever.spriteBatch.End();
                timer02.Start();
            }
        }

        public void StopDisplay00(object o, ElapsedEventArgs e)
        {
            displayed00 = true;
            timer00.Stop();
        }

        public void StopDisplay01(object o, ElapsedEventArgs e)
        {
            displayed01 = true;
            timer01.Stop();
        }

        public void StopDisplay10(object o, ElapsedEventArgs e)
        {
            displayed10 = true;
            timer10.Stop();
        }

        public void StopDisplay11(object o, ElapsedEventArgs e)
        {
            displayed11 = true;
            timer11.Stop();
        }

        public void StopDisplay12(object o, ElapsedEventArgs e)
        {
            displayed12 = true;
            timer12.Stop();
        }

        public void StopDisplay21(object o, ElapsedEventArgs e)
        {
            displayed21 = true;
            timer21.Stop();
        }
        public void StopDisplay20(object o, ElapsedEventArgs e)
        {
            displayed20 = true;
            timer20.Stop();
        }
        public void StopDisplay02(object o, ElapsedEventArgs e)
        {
            displayed02 = true;
            timer02.Stop();
        }
    }
}

