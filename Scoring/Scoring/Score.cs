using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scoring
{
    public class Score
    {
        public static readonly int[] MARKERS = new int[] { 0, 1, 2 };
        public static readonly int[] CARS = new int[] { 0, 1, 2 };
        public static readonly int[] LOGS = new int[] { 0, 1, 2, 3 };
        public static readonly int[] COAL = new int[] { 0, 1, 2, 4, 5 };
        public static readonly double[] MUTILIERS = new double[] { 1.0f, 1.1f, 1.2f, 1.5f };

        private const int MARKER_POINTS = 100;
        private const int CAR_POINTS = 50;
        private const int LOG_POINTS = 30;
        private const int COAL_POINTS = 20;

        public Team Team { get; private set; }

        public int Markers { get; set; }
        public int CarsGood { get; set; }
        public int CarsBad { get; set; }
        public int LogsGood { get; set; }
        public int LogsBad { get; set; }
        public int CoalGood { get; set; }
        public int CoalBad { get; set; }

        public double Multiplier { get; set; }

        public double GetScore() 
        {
            double result = Markers * MARKER_POINTS + Math.Max(0,(CarsGood * CAR_POINTS + LogsGood * LOG_POINTS + CoalGood * COAL_POINTS) * Multiplier - (CarsBad * CAR_POINTS + LogsBad * LOG_POINTS + CoalBad * COAL_POINTS));
            return result;
        }

        public Score(Team team)
        {
            this.Team = team;
            Multiplier = 1.0f;
        }

        public Score(Score score)
        {
            this.Team = score.Team;
            Markers = score.Markers;
            CarsGood = score.CarsGood;
            CarsBad = score.CarsBad;
            LogsGood = score.LogsGood;
            LogsBad = score.LogsBad;
            CoalGood = score.CoalGood;
            CoalBad = score.CoalBad;
            Multiplier = score.Multiplier;
        }
    }
}
