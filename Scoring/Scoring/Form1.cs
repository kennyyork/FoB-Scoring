using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Scoring
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();            
        }

        private const string GAME_FILE_PATH = "game.txt";
        private const string TEAM_FILE_PATH = "teams.txt";
        private const string SCORES_FILE_PATH = "scores.txt";
        private const string ROUNDS_FILE_PATH = "rounds.txt";

        private int activeRound = 0;
        private int roundsPerTeam = 0;       

        private int prelimCount = 0;
        private const int WILDCARD_COUNT = 1;
        private const int SEMIFINAL_COUNT = 8;
        private const int FINAL_COUNT = 8;

        private Round.Types gameState = Round.Types.Semifinals;

        private void Form1_Load(object sender, EventArgs e)
        {
            CenterNud();

            //load existing            
            try
            {
                LoadFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (scores.Count > 0)
            {
                activeRound = scores.Max(s => s.Round.Number);
            }

            if (activeRound == 0)
            {
                pnlScoreIn.Enabled = false;
            }
            else
            {
                pnlScoreIn.Enabled = true;
                nudRound.Minimum = 0;
                nudRound.Maximum = activeRound;
            }
           
            UpdateUI();            
        }

        bool wildcardEntered = false;
        private void UpdateUI()
        {            
            btnWildcard.Enabled = (activeRound >= prelimCount) && wildcardEntered;
            btnSemi.Enabled = (activeRound >= (prelimCount + WILDCARD_COUNT));
            btnFinals.Enabled = (activeRound >= (prelimCount + WILDCARD_COUNT + SEMIFINAL_COUNT));
        }

        private List<Team> teams = new List<Team>();
        private List<Round> rounds = new List<Round>();
        private List<Score> scores = new List<Score>();

        private void LoadFile()
        {
            if (teams.Count > 0)
            {
                DialogResult dr = MessageBox.Show("All current data will be lost, continue?", "Caution", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.Cancel) return;
            }

            teams.Clear();
            rounds.Clear();
            scores.Clear();
            
            StreamReader sr;
            string line;
            
            try
            {                
                //read global game 
                line = File.ReadAllText(GAME_FILE_PATH);
                string[] split = line.Split(',');
                roundsPerTeam = int.Parse(split[0]);                
            }
            catch
            {
                throw;
            }

            try
            {
                //read teams 
                sr = File.OpenText(TEAM_FILE_PATH);
                while( (line = sr.ReadLine()) != null )
                {
                    teams.Add(Team.FromString(line));                          
                }
                sr.Close();
            }
            catch
            {
                teams.Clear();
                throw;
            }
                
            prelimCount = (roundsPerTeam * teams.Count) / 4;

            try
            {
                sr = File.OpenText(ROUNDS_FILE_PATH);
                while ((line = sr.ReadLine()) != null)
                {
                    rounds.Add(Round.FromString(teams,line));
                }
                sr.Close();
            }
            catch
            {                
                rounds.Clear();
                throw;
            }

            try
            {
                sr = File.OpenText(SCORES_FILE_PATH);
                while ((line = sr.ReadLine()) != null)
                {
                    scores.Add(Score.FromString(teams,rounds,line));
                }
                sr.Close();
            }
            catch (Exception ex)
            {
                scores.Clear();
                throw;
            }
        }
        //if (File.Exists(TEAM_FILE_PATH))
        //    {
        //        DialogResult dr = MessageBox.Show("File exists, overwrite?", "Caution", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
        //        if (dr == DialogResult.Cancel) return;
        //    }
        private void WriteTeams()
        {
            StreamWriter sw = new StreamWriter(File.OpenWrite(TEAM_FILE_PATH));
            foreach (var t in teams)
            {
                sw.WriteLine(t);
            }
        }

        private void WriteRounds()
        {
            StreamWriter sw = new StreamWriter(File.OpenWrite(ROUNDS_FILE_PATH));
            foreach (var t in rounds)
            {
                sw.WriteLine(t);
            }
        }

        private void WriteScores()
        {
            StreamWriter sw = new StreamWriter(File.OpenWrite(SCORES_FILE_PATH));
            foreach (var t in scores)
            {
                sw.WriteLine(t);
            }
        }

        private void CenterNud()
        {
            int offsetX = scoringInput1.Left + (scoringInput1.Width / 2 ) - (pnlScoreIn.Width / 2);
            int offsetY = scoringInput1.Top + scoringInput1.Height + 5;

            pnlScoreIn.Left = offsetX;
            pnlScoreIn.Top = offsetY;
        }

        private List<Round> GenerateSeeding(List<Team> teams, int gamesPerTeam, bool backToBack)
        {
            //generate a random list of all teams
            //select 4 teams from that list wher not in last round
              //if 4 not available, re-add all teams and select where 1) not in current set and 2) not in last round
                //if 4 not available, select any needed

            List<Round> rounds = new List<Round>();
            int required = (teams.Count * gamesPerTeam) / 4;

            while (rounds.Count < required)
            {                   
                List<Team> availTeams = teams.Shuffle();
                List<Team> set = availTeams.TakeAndRemove(4, t => t.LastRound != rounds.Count - 1);
                if (set.Count < 4)
                {
                    availTeams = teams.Shuffle();
                    set.AddRange(availTeams.TakeAndRemove(4 - set.Count, t => (t.LastRound != rounds.Count - 1) && !set.Contains(t)));

                    if (set.Count < 4)
                    {
                        if (!backToBack)
                        {
                            throw new InvalidOperationException("unable to generate a seeding with the provided inputs");
                        }

                        set.AddRange(availTeams.TakeAndRemove(4 - set.Count, t => !set.Contains(t)));
                        if (set.Count < 4)
                        {
                            throw new InvalidOperationException("unable to generate a seeding with the provided inputs");
                        }
                    }
                }

                Round r = new Round(rounds.Count + 1, Round.Types.Preliminary, set[0], set[1], set[2], set[3]);
                rounds.Add(r);
            }


            return rounds;
        }

        private void btnPrelim_Click(object sender, EventArgs e)
        {
            try
            {
                rounds = GenerateSeeding(teams, roundsPerTeam, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    public static class Extensions
    {
        public static List<T> Shuffle<T>(this List<T> list)
        {
            List<T> temp = list.ToList();

            Random rng = new Random();
            int n = temp.Count;
            while (n > 1)
            {
                int k = rng.Next(n);
                --n;
                T value = temp[k];
                temp[k] = temp[n];
                temp[n] = value;
            }

            return temp;
        }

        public static List<T> TakeAndRemove<T>(this List<T> source, int count, Func<T, bool> predicate)
        {
            List<T> result = new List<T>();
            for (int i = 0; i < source.Count && result.Count < count; ++i)
            {
                if (predicate(source[i]))
                {
                    result.Add(source[i]);
                    source.RemoveAt(i);
                    continue;
                }
            }

            return result;
        }
    }
}
