using System;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using Sprint1.Entities;
using Sprint1.TimerSlider;

namespace Sprint1.Trackers
{
    public class HoldTimeTracker
    {
        public int MarioHoldTime { get; private set; }
        public int LuigiHoldTime { get; private set; }
        private int RoundWinningTime;
        private System.Timers.Timer MarioTimer;
        private System.Timers.Timer LuigiTimer;
        public event EventHandler<EventArgs> MarioWon;
        public event EventHandler<EventArgs> LuigiWon;
        public event EventHandler<EventArgs> DisplayRoundTransition;

        public event EventHandler<TimeEventArgs> TimeChanging;
        public event EventHandler<TimeEventArgsLuigi> TimeChangingLuigi;
        TimeEventArgs timeEventArgs;
        TimeEventArgsLuigi timeEventArgsLuigi;

        private Game1 reciever;

        public HoldTimeTracker(Game1 game)
        {
            MarioHoldTime = 0;
            LuigiHoldTime = 0;

            MarioTimer = new System.Timers.Timer(1000); //interval is 1s
            LuigiTimer = new System.Timers.Timer(1000); //interval is 1s
            RoundWinningTime = 30; //set 50s as the winning time for each round

            LuigiTimer.Elapsed += LuigiHolding;
            MarioTimer.Elapsed += MarioHolding;

            reciever = game;


            this.TimeChangingLuigi += reciever.progressionBarLuigi.TimeChangeLuigi;
            this.TimeChanging += reciever.progressionBar.TimeChange;

            this.MarioWon += reciever.roundTracker.MarioWon;
            this.LuigiWon += reciever.roundTracker.LuigiWon;

            reciever.roundTracker.AllTimerStop += this.AllTimerStop;
        }

        public void MarioHolding(object o, ElapsedEventArgs e)
        {
            timeEventArgs = new TimeEventArgs
            {
                marioHoldTime = this.MarioHoldTime
            };

            Debug.WriteLine("HELLO MARIO GOT THIS");
            onTimeChanging(timeEventArgs);

            MarioHoldTime++;
            Console.WriteLine("(from HoldTimeTracker)MarioHoldTime is: " + MarioHoldTime);
            if (MarioHoldTime >= RoundWinningTime)
            {
                MarioHoldTime = 0;
                LuigiHoldTime = 0;
                onMarioWon();
            }
        }

        public void LuigiHolding(object o, ElapsedEventArgs e)
        {
            timeEventArgsLuigi = new TimeEventArgsLuigi
            {
                luigiHoldTime = this.LuigiHoldTime
            };
            onTimeChangingLuigi(timeEventArgsLuigi);
            Debug.WriteLine("HELLO LUIGI GOT THIS");

            LuigiHoldTime++;
            Console.WriteLine("(from HoldTimeTracker)LuigiHoldTime is: " + LuigiHoldTime);
            if (LuigiHoldTime >= RoundWinningTime)
            {
                MarioHoldTime = 0;
                LuigiHoldTime = 0;
                onLuigiWon();
            }
        }


        public void MarioTimerStart()
        {
            MarioTimer.Start();
        }

        public void LuigiTimerStart()
        {
            LuigiTimer.Start();
        }

        public void MarioTimerPause()
        {
            MarioTimer.Stop();
        }

        public void LuigiTimerPause()
        {
            LuigiTimer.Stop();
        }

        public void MarioTimerReset()
        {
            MarioHoldTime = 0;
        }

        public void LuigiTimerReset()
        {
            LuigiHoldTime = 0;
        }

        protected virtual void onMarioWon()
        {
            EventHandler<EventArgs> handler = MarioWon;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        protected virtual void onLuigiWon()
        {
            EventHandler<EventArgs> handler = LuigiWon;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        protected virtual void onTimeChanging(TimeEventArgs e)
        {
            EventHandler<TimeEventArgs> handler = TimeChanging;
            if (handler != null)
                handler(this, e);
        }

        protected virtual void onTimeChangingLuigi(TimeEventArgsLuigi e)
        {
            EventHandler<TimeEventArgsLuigi> handler = TimeChangingLuigi;
            if (handler != null)
                handler(this, e);
        }

        public void AllTimerStop(object o, EventArgs e)
        {
            MarioTimerPause();
            LuigiTimerPause();
            MarioHoldTime = 0;
            LuigiHoldTime = 0;
        }
    }
}
