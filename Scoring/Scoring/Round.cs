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
        public Types Type { get; private set; }
        public Team Red { get; private set; }
        public Team Green { get; private set; }
        public Team Blue { get; private set; }
        public Team Yellow { get; private set; }

        public Round(int number, Types type, Team red, Team green, Team blue, Team yellow)
        {
            Number = number;
            Type = type;
            Red = red;
            Green = green;
            Blue = blue;
            Yellow = yellow;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4},{5}",Number,(int)Type,Red.Number,Green.Name,Blue.Name,Yellow.Number);
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
