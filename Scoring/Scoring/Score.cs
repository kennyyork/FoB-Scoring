using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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
        public Round Round { get; private set; }

        public int Markers { get; set; }
        public int CarsGood { get; set; }
        public int CarsBad { get; set; }
        public int LogsGood { get; set; }
        public int LogsBad { get; set; }
        public int CoalGood { get; set; }
        public int CoalBad { get; set; }

        public double Multiplier { get; set; }

        public double TotalScore
        {            
            //double result = Markers * MARKER_POINTS + Math.Max(0,(CarsGood * CAR_POINTS + LogsGood * LOG_POINTS + CoalGood * COAL_POINTS) * Multiplier - (CarsBad * CAR_POINTS + LogsBad * LOG_POINTS + CoalBad * COAL_POINTS));
            get { return CalcScore(Markers, CarsGood, CarsBad, LogsGood, LogsBad, CoalGood, CoalBad, Multiplier); }
        }

        public static double CalcScore(int markers, int carsGood, int carsBad, int logsGood, int logsBad, int coalGood, int coalBad, double multiplier)
        {
            return markers * MARKER_POINTS + Math.Max(0, (carsGood * CAR_POINTS + logsGood * LOG_POINTS + coalGood * COAL_POINTS) * multiplier - (carsBad * CAR_POINTS + logsBad * LOG_POINTS + coalBad * COAL_POINTS));
        }

        public Score(Team team, Round round)
        {
            this.Team = team;
            Multiplier = 1.0f;
            Round = round;
            team.AddScore(this);
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

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9:0.0}", Team.Number, Round.Number, Markers, CarsGood, CarsBad, LogsGood, LogsBad, CoalGood, CoalBad, Multiplier);
        }

        public static Score FromString(List<Team> teams, List<Round> rounds, string line)
        {
            string[] split = line.Split(',');

            Team t1 = teams.Single(t => t.Number == int.Parse(split[0]));
            Round r1 = rounds.Single( r => r.Number == int.Parse(split[1]));

            Score s = new Score(t1, r1);
            s.Markers = int.Parse(split[2]);
            s.CarsGood = int.Parse(split[3]);
            s.CarsBad = int.Parse(split[4]);
            s.LogsGood = int.Parse(split[5]);
            s.LogsBad = int.Parse(split[6]);
            s.CoalGood = int.Parse(split[7]);
            s.CoalBad = int.Parse(split[8]);
            s.Multiplier = double.Parse(split[9]);

            return s;
        }

        //public static List<Score> ReadAll(string path)
        //{
        //    if (!File.Exists(path))
        //    {
        //        return null;
        //    }

        //    List<Score> teams = new List<Score>();

        //    try
        //    {
        //        StreamReader sr = File.OpenText(path);
        //        string line;
        //        while ((line = sr.ReadLine()) != null)
        //        {
        //            string[] split = line.Split(',');
        //            //round,team,scores...
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }

        //    return teams;
        //}
    }
}
