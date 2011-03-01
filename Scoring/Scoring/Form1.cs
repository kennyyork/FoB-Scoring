using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace Scoring
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();            
        }

        WebDisplay wd = new WebDisplay();

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

            UpdateUI();            
        }

        bool wildcardEntered = false;
        private void UpdateUI()
        {            
            btnWildcard.Enabled = (activeRound >= prelimCount) && wildcardEntered;
            btnSemi.Enabled = (activeRound >= (prelimCount + WILDCARD_COUNT));
            btnFinals.Enabled = (activeRound >= (prelimCount + WILDCARD_COUNT + SEMIFINAL_COUNT));

            if (activeRound == 0)
            {
                pnlScoreIn.Enabled = false;
            }
            else
            {
                pnlScoreIn.Enabled = true;
                nudRound.Minimum = 1;
                nudRound.Maximum = activeRound;
            }

            if (nudRound.Value == activeRound)
            {
                btnSubmit.Text = "Submit";
            }
            else if( nudRound.Value > 0 )
            {
                btnSubmit.Text = "Edit";
            }
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
            catch
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

            StringBuilder debug = new StringBuilder();

            List<Round> rounds = new List<Round>();
            int required = (teams.Count * gamesPerTeam) / 4;

            List<Team> availTeams = teams.Shuffle();
            while (rounds.Count < required)
            {                                   
                List<Team> set = availTeams.TakeAndRemove(4, t => t.LastRound != rounds.Count);
                debug.AppendFormat("first set {0}  ",set.Count);
                foreach (var s in set) { debug.AppendFormat(" {0} ", s); }
                debug.AppendLine();

                if (set.Count < 4)
                {
                    debug.AppendFormat("failed to pull 4, only got {0}\n", set.Count);
                    availTeams = teams.Shuffle();
                    set.AddRange(availTeams.TakeAndRemove(4 - set.Count, t => (t.LastRound != rounds.Count) && !set.Contains(t)));

                    debug.AppendFormat("second set {0}  ",set.Count);
                    foreach (var s in set) { debug.AppendFormat(" {0} ", s); }
                    debug.AppendLine();

                    if (set.Count < 4)
                    {
                        debug.AppendFormat("failed to pull 4, only got {0}\n", set.Count);

                        if (!backToBack)
                        {
                            throw new InvalidOperationException("unable to generate a seeding with the provided inputs");
                        }

                        set.AddRange(availTeams.TakeAndRemove(4 - set.Count, t => !set.Contains(t)));

                        debug.AppendFormat("final set {0}  ", set.Count);
                        foreach (var s in set) { debug.AppendFormat(" {0} ", s); }
                        debug.AppendLine();

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
                if (rounds.Count > 0)
                {
                    if (MessageBox.Show("Warning:  All current round data will be lost", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                    {
                        return;
                    }

                    if (MessageBox.Show("Seriously:  All current round data will be lost, are you sure?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                    {
                        return;
                    }
                }

                rounds = GenerateSeeding(teams, roundsPerTeam, false);
                activeRound = 1;
                UpdateUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void nudRound_ValueChanged(object sender, EventArgs e)
        {
            //load the scores
            if (scoringInput1.ScoresModified)
            {
                if (MessageBox.Show("Scores have been modified, discard?", "Discard Scores?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                {
                    return;
                }
            }

            scoringInput1.SetScores(rounds[(int)nudRound.Value]);
            UpdateUI();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            scoringInput1.CommitScores();
            if (btnSubmit.Text == "Submit")
            {
                ++activeRound;
            }

            UpdateUI();
            nudRound.Value = activeRound;
        }
    }

    public static class Extensions
    {
        public static List<T> Shuffle<T>(this List<T> list)
        {
            StringBuilder debug = new StringBuilder();

            List<T> temp = list.ToList();

            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            byte[] box = new byte[1];
            int k;
            T value;

            while (n > 1)
            {                
                do 
                    provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                
                k = (box[0] % n);
                n--;
                value = temp[k];
                temp[k] = temp[n];
                temp[n] = value;
            }

            //Random rng = new Random();
            //int n = temp.Count;
            //while (n > 1)
            //{
            //    int k = rng.Next(n);
            //    debug.AppendFormat("rng {0}\n", k);

            //    --n;
            //    T value = temp[k];
            //    temp[k] = temp[n];
            //    temp[n] = value;
            //}

            return temp;
        }

        public static List<T> TakeAndRemove<T>(this List<T> source, int count, Func<T, bool> predicate)
        {
            List<T> result = new List<T>();
            for (int i = 0; i < source.Count && result.Count < count;)
            {
                if (predicate(source[i]))
                {
                    result.Add(source[i]);
                    source.RemoveAt(i);
                    continue;
                }

                ++i;
            }

            return result;
        }
    }
}
