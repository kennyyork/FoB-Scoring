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
            get { return teamList; }
        }
        
        public Team Red 
        {
            get { return teamList[RED]; }
            private set { teamList[RED] = value; }
        }

        public Team Green
        {
            get { return teamList[GREEN]; }
            private set { teamList[GREEN] = value; }
        }

        public Team Blue
        {
            get { return teamList[BLUE]; }
            private set { teamList[BLUE] = value; }
        }

        public Team Yellow
        {
            get { return teamList[YELLOW]; }
            private set { teamList[YELLOW] = value; }
        }

        private List<Team> teamList;

        //public Round(int number, Types type, Team red, Team green, Team blue, Team yellow)
        //{
        //    teamList = new List<Team>();

        //    Number = number;
        //    Type = type;
        //    teamList.Add(red);
        //    teamList.Add(green);
        //    teamList.Add(blue);
        //    teamList.Add(yellow);

        //    red.AddRound(this);
        //    green.AddRound(this);
        //    blue.AddRound(this);
        //    yellow.AddRound(this);            
        //}

        public Round(int number, Types type, IEnumerable<Team> teams)
        {
            teamList = teams.ToList();            
            Number = number;
            Type = type;

            foreach (var t in teamList)
            {
                t.AddRound(this);
            }            
        }

        public string TeamColor(Team team)
        {
            int i = teamList.IndexOf(team);
            return TeamColor(i);
        }

        public static string TeamColor(int color)
        {            
            switch(color)
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
            return teamList[color].GetScore(this);
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

                List<Team> roundT = new List<Team>();

                int id = int.Parse(split[2]);
                roundT.Add(teams.Single(t => t.Number == id));
                id = int.Parse(split[3]);
                roundT.Add(teams.Single(t => t.Number == id));
                id = int.Parse(split[4]);
                roundT.Add(teams.Single(t => t.Number == id));
                id = int.Parse(split[5]);
                roundT.Add(teams.Single(t => t.Number == id));

                Round r = new Round(number, type, roundT);
                return r;
            }
            catch
            {
                throw;
            }
        }
    }
}
