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
        public int LastRound { get; set; }
        public double Notebook { get; set; }

        private List<Score> scores;
        private List<Round> rounds;

        public Team(string name, int number)
        {
            Name = name;
            Number = number;
            Notebook = 0;

            scores = new List<Score>();
            rounds = new List<Round>();
        }

        public void AddScore(Score score)
        {
            scores.Add(score);
            LastRound = score.Round.Number;
        }

        public void AddRound(Round round)
        {

        }

        public override string ToString()
        {
            return string.Format("{0},{1}", Number, Name);
        }

        public static Team FromString(string line)
        {
            string[] split = line.Split(',');
            Team t = new Team(split[1], int.Parse(split[0]));
            return t;
        }

        //public static List<Team> ReadAll(string path)
        //{
        //    if (!File.Exists(path))
        //    {
        //        return null;
        //    }

        //    List<Team> teams = new List<Team>();

        //    try
        //    {                
        //        StreamReader sr = File.OpenText(path);
        //        string line;
        //        while ((line = sr.ReadLine()) != null)
        //        {
        //            string[] split = line.Split(',');
        //            Team t = new Team(split[1], int.Parse(split[0]));
        //            teams.Add(t);
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
            
        //    return teams;
        //}

        //public static void WriteAll(string path, IEnumerable<Team> teams)
        //{
        //    try
        //    {
        //        var fs = File.OpenWrite(path);
        //        StreamWriter sr = new StreamWriter(fs);
        //        foreach (var t in teams)
        //        {
        //            sr.WriteLine("{0},{1}", t.Number, t.Name);
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }            
        //}
    }
}
