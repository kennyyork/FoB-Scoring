using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scoring
{
    public class Round
    {
        public enum Types
        {
            Preliminary,
            Wildcard,
            Semifinals,
            Finals
        }

        public int Number { get; private set; }
        public Types Type { get;  set; }

        public const int RED = 0;
        public const int GREEN = 1;
        public const int BLUE = 2;
        public const int YELLOW = 3;

        public IList<Team> Teams
        {
            get { return teams; }
        }
        
        public Team Red 
        {
            get { return teams[RED]; }
            private set { teams[RED] = value; }
        }

        public Team Green
        {
            get { return teams[GREEN]; }
            private set { teams[GREEN] = value; }
        }

        public Team Blue
        {
            get { return teams[BLUE]; }
            private set { teams[BLUE] = value; }
        }

        public Team Yellow
        {
            get { return teams[YELLOW]; }
            private set { teams[YELLOW] = value; }
        }

        private List<Team> teams;

        public Round(int number, Types type, Team red, Team green, Team blue, Team yellow)
        {
            teams = new List<Team>();

            Number = number;
            Type = type;
            teams.Add(red);
            teams.Add(green);
            teams.Add(blue);
            teams.Add(yellow);

            red.AddRound(this);
            green.AddRound(this);
            blue.AddRound(this);
            yellow.AddRound(this);            
        }

        public string TeamColor(Team team)
        {
            int i = teams.IndexOf(team);
            switch(i)
            {
                case RED:
                    return "Red";
                case GREEN:
                    return "Green";
                case BLUE:
                    return "Blue";
                case YELLOW:
                    return "Yellow";
                default:
                    return "Unknown Error";
            }
        }

        public Score GetScore(int color)
        {
            return teams[color].GetScore(this);
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4},{5}",Number,(int)Type,Red.Number,Green.Number,Blue.Number,Yellow.Number);
        }

        public static Round FromString(List<Team> teams, string line)
        {
            try
            {
                string[] split = line.Split(',');
                int number = int.Parse(split[0]);
                Types type = (Types)int.Parse(split[1]);

                int id = int.Parse(split[2]);
                Team t1 = teams.Single(t => t.Number == id);
                id = int.Parse(split[3]);
                Team t2 = teams.Single(t => t.Number == id);
                id = int.Parse(split[4]);
                Team t3 = teams.Single(t => t.Number == id);
                id = int.Parse(split[5]);
                Team t4 = teams.Single(t => t.Number == id);

                Round r = new Round(number, type, t1, t2, t3, t4);
                return r;
            }
            catch
            {
                throw;
            }
        }
    }
}
