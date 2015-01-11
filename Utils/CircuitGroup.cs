using CoPilot.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoPilot.Statistics.Utils
{
    public class CircuitGroup
    {
        public List<State> States { get; set; }
        public List<Double> Laps { get; set; }
        public Circuit Circuit { get; set; }
        public String Name { get; set; }

        public TimeSpan FastestLap
        {
            get
            {
                TimeSpan best = TimeSpan.MaxValue;
                foreach (var lap in Laps)
                {
                    TimeSpan lapTime = new TimeSpan(0, 0, 0, 0, (int)lap);
                    if (lapTime < best)
                    {
                        best = lapTime;
                    }
                }
                return best;
            }
        }
    }
}
