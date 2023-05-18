using System;
using Microsoft.Xna.Framework;
using Sprint1.Audio;
using static Sprint1.States.PowerStates.PowerState;

namespace Sprint1.Trackers
{
    public class TimeTracker
    {
        public float time { get; set; }
        Game1 reciever;
        public delegate void TimeRanOutEventHandler(object source, EventArgs args);
        public event TimeRanOutEventHandler TimeRanOut;
        public event EventHandler<SoundEffectEventArgs> SetEffect;
        SoundEffectEventArgs SoundEffectArgs;
        public int limit = 400;
        public int warning_limit = 300;
        private bool isWarned;

        public TimeTracker(Game1 reciever)
        {
            this.reciever = reciever;
            isWarned = false;
        }

        public void Update(GameTime gameTime)
        {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (time > limit)
            {
                time = 0;
                onTimeRanOut();
                Console.WriteLine("one life has been deced(from timetracker)");
                reciever.ResetGame();
            }
            if (time > warning_limit && !isWarned)
            {
                SoundEffectArgs = new SoundEffectEventArgs { effect = "warning" };
                onSetEffect(SoundEffectArgs);
                reciever.timeTracker.SetEffect += reciever.audioManager.PlaySoundEffect;
                isWarned = true;
            }
            //Console.WriteLine("total timeSincePeak is: " + timeSincePeak);

        }

        protected virtual void onTimeRanOut()
        {
            if (TimeRanOut != null)
                TimeRanOut(this, EventArgs.Empty);
        }

        protected virtual void onSetEffect(SoundEffectEventArgs e)
        {
            EventHandler<SoundEffectEventArgs> handler = SetEffect;
            if (handler != null)
                handler(this, e);
        }

        public void Reset()
        {
            isWarned = false;
        }

    }
}

