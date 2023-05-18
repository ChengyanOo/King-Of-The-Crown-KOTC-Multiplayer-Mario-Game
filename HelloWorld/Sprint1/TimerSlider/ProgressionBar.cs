
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.IO;
using Sprint1.Trackers;



namespace Sprint1.TimerSlider
{
    public class ProgressionBar
    {
        protected readonly Texture2D background;
        protected readonly Texture2D foreground;
        protected readonly Vector2 position;
        protected readonly float maxValue;
        protected float currentValue;
        protected Rectangle part;
        protected static SpriteBatch spriteBatch;
        public int marioHoldTime;
        public int luigiHoldTime;

        protected  Game1 game1;//MIGHT USE LATER????

        public ProgressionBar(Texture2D bg, Texture2D fg, float max, Vector2 pos, Game1 game)
        {
            background = bg;
            foreground = fg;
            position = pos;
            maxValue = max;
            currentValue = max;
            //this.spriteBatch = spriteBatch;
            game1 = game;
        }
        public virtual void Update(float value)
        {
            currentValue = value;
            part.Width = (int)(currentValue/maxValue * foreground.Width);

        }

        public virtual void Draw()
        {
            game1.spriteBatch.Begin();
            game1.spriteBatch.Draw(background,position,Color.White);
            game1.spriteBatch.Draw(foreground, position, part, Color.White,0, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            //Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color backgroundColor, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth
            game1.spriteBatch.End();
        }

        public void TimeChange(object o, TimeEventArgs a)//make a luigi one
        {
            this.marioHoldTime = a.marioHoldTime;
           // this.luigiHoldTime = a.luigiHoldTime;
        }
        public void TimeChangeLuigi(object o, TimeEventArgsLuigi a)//make a luigi one
        {
            this.luigiHoldTime = a.luigiHoldTime;
            //Debug.WriteLine("HELLLLLLLLLLLLLLLLLLLLOOOOOOOOOOOOOOOOOO"+luigiHoldTime);
            // this.luigiHoldTime = a.luigiHoldTime;
        }
    }

    public class TimeEventArgs : EventArgs//add a second one
    {
        public int marioHoldTime { get; set; }
       // public int luigiHoldTime { get; set; }
    }

    public class TimeEventArgsLuigi : EventArgs//add a second one
    {
        public int luigiHoldTime { get; set; }
    }
}
