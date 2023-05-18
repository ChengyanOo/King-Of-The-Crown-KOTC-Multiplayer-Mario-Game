using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.Trackers
{
    public class PointTracker
    {
        public int points { get; private set; }

        public PointTracker()
        {
            points = 0;
        }

        public void IncScore(object o, PointEventArgs a)
        {
            points += a.PointValue;
            //Debug.WriteLine("Score has been increased by: " + a.PointValue + " curr points: " + points);
        }

        public void SetScore(object o, PointEventArgs a)
        {
            points = a.PointValue;
            //Debug.WriteLine("Score has been decreased by: " + a.PointValue + " curr points: " + points);
        }

        public void Reset()
        {
            points = 0;
        }

    }

    public class PointEventArgs : EventArgs
    {
        public int PointValue { get; set; }
    }
}
