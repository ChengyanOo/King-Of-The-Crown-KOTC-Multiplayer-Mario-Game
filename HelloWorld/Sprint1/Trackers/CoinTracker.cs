using System;
using Microsoft.Xna.Framework;
using Sprint1.Audio;

namespace Sprint1.Trackers
{
    public class CoinTracker
    {
        public int coin { get; private set; }
        public event EventHandler<EventArgs> IncLife;

        public CoinTracker()
        {
            coin = 0;
        }

        public void AteCoin(object o, CoinEventArgs a)
        {
            coin += a.CoinValue; //temp value
            Console.WriteLine("Coin has been ate " + "curr coins: " + coin);
        }

        public void Update()
        {
            if(coin > 100)
            {
                onIncLife();
                coin = 0;
            }
        }

        protected virtual void onIncLife()
        {
            EventHandler<EventArgs> handler = IncLife;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
        public void Reset()
        {
            coin = 0;
        }
    }
    public class CoinEventArgs : EventArgs
    {
        public int CoinValue { get; set; }
    }
}

