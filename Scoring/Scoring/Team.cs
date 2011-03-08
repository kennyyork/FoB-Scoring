﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Scoring
{
    public class Team : System.ComponentModel.INotifyPropertyChanged
    {
        public string Name { get; private set; }
        public int Number { get; private set; }        
        private double notebook;

        public double Notebook 
        {
            get { return notebook; }
            set
            {
                notebook = value;
                NotifyPropertyChanged("Notebook");
            }
        }

        public List<Round> Rounds { get { return rounds; } }
        public List<Score> Scores { get { return scores; } }

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
            if (scores.Contains(score))
            {
                throw new InvalidOperationException("Score already exists in collection");
            }

            scores.Add(score);
        }

        public void AddRound(Round round)
        {            
            if (rounds.Contains(round))
            {
                throw new InvalidOperationException("Round already exists in collection");
            }

            rounds.Add(round);
        }

        public double TotalScore(Round.Types type)
        {
            var result = from s in scores where s.Round.Type == type select s;
            double sum = 0;
            foreach (var s in result)
            {
                sum += s.TotalScore;
            }

            return sum;
        }

        public Score GetScore(Round round)
        {
            return scores.SingleOrDefault(s => s.Round == round);
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2}", Number, Name, Notebook);
        }

        public static Team FromString(string line)
        {
            string[] split = line.Split(',');
            Team t = new Team(split[1], int.Parse(split[0]));
            if (split.Length == 3)
            {
                t.Notebook = int.Parse(split[2]);
            }
            return t;
        }

        public void Clear()
        {
            rounds.Clear();
            scores.Clear();
        }

        #region INotifyPropertyChanged Members

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string name)
        {
            var p = PropertyChanged;
            if (p != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(name));
            }
        }

        #endregion
    }
}
