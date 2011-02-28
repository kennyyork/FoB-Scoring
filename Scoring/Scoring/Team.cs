using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Scoring
{
    public class Team
    {
        public string Name { get; private set; }
        public int Number { get; private set; }

        private List<Score> scores;
        private List<Round> rounds;

        public Team(string name, int number)
        {
            Name = name;
            Number = number;

            scores = new List<Score>();
            rounds = new List<Round>();
        }

        public static List<Team> ReadAll(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            List<Team> teams = new List<Team>();

            try
            {                
                StreamReader sr = File.OpenText(path);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] split = line.Split(',');
                    Team t = new Team(split[1], int.Parse(split[0]));
                    teams.Add(t);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            
            return teams;
        }

        public static void WriteAll(string path, IEnumerable<Team> teams)
        {

        }
    }
}
