using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scoring
{
    public class Score
    {
        public string TeamName { get; set; }

        public int Markers { get; set; }
        public int CarsGood { get; set; }
        public int CarsBad { get; set; }
        public int LogsGood { get; set; }
        public int LogsBad { get; set; }
        public int CoalGood { get; set; }
        public int CoalBad { get; set; }

        public double Multiplier { get; set; }

        public double GetScore() { return 0.0; }
    }
}
