using System;
using System.Timers;
namespace Sprint1.Yannstempclasses
{
	public class PreventSpamming
	{
        public Timer coolDownTimer;

        public float time;
		public bool isPressable;
        public PreventSpamming()
		{
            coolDownTimer = new Timer(500); //interval is 0.5s
            isPressable = true;
            coolDownTimer.Elapsed += EnterFrozenTime;
        }
        
        public void EnterFrozenTime(object o, ElapsedEventArgs e)
        {
            isPressable = true;
            coolDownTimer.Stop();
        }

        public void TimerStart()
        {
            isPressable = false;
            coolDownTimer.Start();
        }
    }
}

