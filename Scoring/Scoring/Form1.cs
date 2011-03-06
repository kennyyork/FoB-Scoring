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
        enum GameState
        {
            None,
            Preliminary,
            Wildcard,
            SemiFinals,
            Finals
        }

        private GameState gameState = GameState.None;

        public Form1()
        {
            InitializeComponent();            
        }

        WebDisplay wd = new WebDisplay();

        private const string GAME_FILE_PATH = "game.txt";
        private const string TEAM_FILE_PATH = "teams.txt";
        private const string SCORES_FILE_PATH = "scores.txt";
        private const string ROUNDS_FILE_PATH = "rounds.txt";

        private int activeRound = -1;
        private int roundsPerTeam = 0;               

        private int prelimCount = 0;
        private const int WILDCARD_COUNT = 1;
        private const int SEMIFINAL_COUNT = 8;
        private const int FINAL_COUNT = 8;        

        private void Form1_Load(object sender, EventArgs e)
        {
            //center the nudPanel
            int offsetX = scoringInput1.Left + (scoringInput1.Width / 2) - (pnlScoreIn.Width / 2);
            int offsetY = scoringInput1.Top + scoringInput1.Height + 5;

            pnlScoreIn.Left = offsetX;
            pnlScoreIn.Top = offsetY;

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
                activeRound = scores.Max(s => s.Round.Number); //this should really be the max round - 1 + 1 to equal the index
                //activeRound = Math.Min(activeRound, rounds.Count - 1);
                nudRound.Value = activeRound + 1;
            }
            else if (gameState == GameState.Preliminary)
            {
                activeRound = 0;
            }

            UpdateUI();            
        }

        bool notebooksEntered = false;
        private void UpdateUI()
        {
            int prelimCount = roundsPerTeam * teams.Count / 4;

            btnPrelim.Enabled = gameState == GameState.None || gameState == GameState.Preliminary;
            btnWildcard.Enabled = gameState == GameState.Wildcard && (rounds.Count == prelimCount);
            btnSemi.Enabled = gameState == GameState.SemiFinals && (rounds.Count == prelimCount + 1);
            btnFinals.Enabled = gameState == GameState.Finals;

            btnCurrentSched.Enabled = rounds.Count > 0;

            if (activeRound < 0 || activeRound > rounds.Count)
            {
                pnlScoreIn.Enabled = false;
            }
            else
            {
                pnlScoreIn.Enabled = true;
                nudRound.Minimum = 1;
                nudRound.Maximum = Math.Min(activeRound + 1, rounds.Count);
            }

            if (nudRound.Value == (activeRound+1) )
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

        #region File Management

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
                while ((line = sr.ReadLine()) != null)
                {
                    if (line != string.Empty)
                    {
                        Team newT = Team.FromString(line);
                        if (teams.Any(t => newT.Name == t.Name || newT.Number == t.Number))
                        {
                            throw new InvalidOperationException(string.Format("Duplicate team on {0} or {1}", newT.Number, newT.Name));
                        }
                        teams.Add(newT);
                    }
                }
                sr.Close();
            }
            catch
            {
                teams.Clear();
                throw;
            }

            if( teams.Any( t => t.Notebook > 0 ) )
            {
                notebooksEntered = true;
            }
                
            prelimCount = (roundsPerTeam * teams.Count) / 4;

            try
            {
                sr = File.OpenText(ROUNDS_FILE_PATH);
                while ((line = sr.ReadLine()) != null)
                {
                    if (line != string.Empty)
                    {
                        Round newR = Round.FromString(teams, line);
                        if (rounds.Any(r => newR.Number == r.Number))
                        {
                            throw new InvalidOperationException(string.Format("Duplicate round on {0}", newR.Number));
                        }
                        rounds.Add(newR);
                    }
                }
                sr.Close();
            }
            catch (InvalidOperationException)
            {
                rounds.Clear();
                throw;
            }            
            catch(Exception ex)
            {
                rounds.Clear();
            }
            
            //are the rounds valid?
            if (rounds.Count > 0 && rounds.Count < prelimCount)
            {
                if (DialogResult.OK == MessageBox.Show("Invalid round data, clear existing file?", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
                {
                    rounds.Clear();
                    WriteRounds();
                }
                else
                {
                    throw new InvalidOperationException("Invalid rounds data");
                }
            }

            try
            {
                sr = File.OpenText(SCORES_FILE_PATH);
                while ((line = sr.ReadLine()) != null)
                {
                    if (line != string.Empty)
                    {
                        Score newS = Score.FromString(teams, rounds, line);
                        if (scores.Any(s => s.Round.Number == newS.Round.Number && s.Team.Number == newS.Team.Number))
                        {
                            throw new InvalidOperationException(string.Format("Duplicate score on {0} and {1}", newS.Round.Number, newS.Team.Name));
                        }
                        scores.Add(newS);
                    }
                }
                sr.Close();
            }
            catch (InvalidOperationException)
            {
                scores.Clear();
                throw;
            }
            catch
            {
                scores.Clear();
            }

            //figure out what state we are in
            int prelimScores = prelimCount * 4;
            if (scores.Count < prelimScores)
            {
                if (rounds.Count > 0)
                {
                    gameState = GameState.Preliminary;
                }
            }
            else if (scores.Count == (prelimScores))
            {
                gameState = GameState.Wildcard;
            }
            else if (scores.Count < (prelimScores + WILDCARD_COUNT + SEMIFINAL_COUNT))
            {
                gameState = GameState.SemiFinals;
            }
            else if (scores.Count < (prelimScores + WILDCARD_COUNT + SEMIFINAL_COUNT + FINAL_COUNT))
            {
                gameState = GameState.Finals;
            }
            else
            {
                throw new InvalidOperationException("Could not determine game state");
            }
        }
        
        private void WriteTeams()
        {
            Write(TEAM_FILE_PATH, teams);
        }

        private void WriteRounds()
        {
            Write(ROUNDS_FILE_PATH, rounds);            
        }

        private void AppendRound(Round r)
        {
            Append(ROUNDS_FILE_PATH, r);
        }

        private void WriteScores()
        {
            Write(SCORES_FILE_PATH, scores);
        }

        private void AppendScore(Score s)
        {
            Append(SCORES_FILE_PATH, s);
        }

        private void Append<T>(string file, T item)
        {
            StreamWriter sw = File.AppendText(file);
            sw.WriteLine(item);
            sw.Flush();
            sw.Close();
        }

        private void Write<T>(string file, IEnumerable<T> source)
        {
            StreamWriter sw = new StreamWriter(File.OpenWrite(file));
            foreach (var t in source)
            {
                sw.WriteLine(t);
            }
            sw.Flush();
            sw.Close();
        }

        #endregion

        private List<Round> GenerateSeeding(List<Team> teams, int gamesPerTeam, Round.Types type, bool backToBack)
        {
            //generate a random list of all teams
            //select 4 teams from that list wher not in last round
              //if 4 not available, re-add all teams and select where 1) not in current set and 2) not in last round
                //if 4 not available, select any needed

            int currentRound = rounds.Count + 1;

            StringBuilder debug = new StringBuilder();

            List<Round> newRounds = new List<Round>();
            int required = (teams.Count * gamesPerTeam) / 4;

            List<Team> availTeams = teams.Shuffle();
            while (newRounds.Count < required)
            {
                List<Team> set = availTeams.TakeAndRemove(4, t => t.LastRound != (currentRound-1));
                debug.AppendFormat("first set {0}  ",set.Count);
                foreach (var s in set) { debug.AppendFormat(" {0} ", s); }
                debug.AppendLine();

                if (set.Count < 4)
                {
                    debug.AppendFormat("failed to pull 4, only got {0}\n", set.Count);
                    availTeams = teams.Shuffle();
                    set.AddRange(availTeams.TakeAndRemove(4 - set.Count, t => (t.LastRound != (currentRound - 1)) && !set.Contains(t)));

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

                Round r = new Round(currentRound++, type, set[0], set[1], set[2], set[3]);
                newRounds.Add(r);                
            }

            return newRounds;
        }

        #region Score Display Handlers

        private void nudRound_ValueChanged(object sender, EventArgs e)
        {
            //load the scores
            if (scoringInput1.ScoresModified)
            {
                if ( DialogResult.Cancel == MessageBox.Show("Scores have been modified, discard?", "Discard Scores?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
                {
                    return;
                }
            }

            if (nudRound.Value <= rounds.Count)
            {
                scoringInput1.SetScores(rounds[(int)nudRound.Value - 1]);
            }
            UpdateUI();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            scoringInput1.CommitScores();
            foreach (var s in scoringInput1.CurrentScores)
            {                
                if (btnSubmit.Text == "Submit")
                {
                    scores.Add(s);
                    AppendScore(s);
                }
            }

            if (btnSubmit.Text == "Submit")
            {
                Round next1 = null, next2 = null;
                if ((activeRound + 1) < rounds.Count)
                {
                    next1 = rounds[activeRound + 1];
                }
                if ((activeRound + 2) < rounds.Count)
                {
                    next2 = rounds[activeRound + 2];
                }

                wd.LastRoundDisplay(rounds[activeRound], next1, next2);
                //wd.Show();
                
                ++activeRound;

                //end of available rounds reached
                if (activeRound >= rounds.Count) 
                {
                    AdvanceGameState();
                }
            }
            else //edit forces rewrite
            {
                WriteScores();
            }

            UpdateUI();

            if (activeRound < rounds.Count)
            {
                nudRound.Value = activeRound + 1;
            }
        }

        private void AdvanceGameState()
        {
            switch (gameState)
            {
                case GameState.Preliminary:
                    gameState = GameState.Wildcard;
                    break;
                case GameState.Wildcard:
                    gameState = GameState.SemiFinals;
                    break;
                case GameState.SemiFinals:
                    gameState = GameState.Finals;
                    break;
                default:
                    throw new InvalidOperationException("invalid game state");
            }
        }

        #endregion

        #region Web Handlers

        private void btnCurrentSched_Click(object sender, EventArgs e)
        {
            IEnumerable<Round> result = null;
            string title = "Unknown";            

            switch (gameState)
            {
                case GameState.Preliminary:
                    result = from r in rounds where r.Type == Round.Types.Preliminary select r;
                    title = "Prelimiary Rounds";
                    break;
                case GameState.Wildcard:
                    result = from r in rounds where r.Type == Round.Types.Wildcard select r;
                    title = "Wildcard Round";                    
                    break;      
                case GameState.SemiFinals:
                    result = from r in rounds where r.Type == Round.Types.Semifinals select r;
                    title = "Semifinal Rounds";
                    break;
                case GameState.Finals:
                    result = from r in rounds where r.Type == Round.Types.Finals select r;
                    title = "Finals Rounds";
                    break;      
            }

            if (result != null)
            {
                if (result.Count() > 0)
                {
                    wd.RoundDisplay(title, result);
                    wd.ShowDialog();
                }
                else
                {
                    MessageBox.Show(string.Format("Please generate {0}",title));                     
                }
            }
        }

        private void btnTeamScore_Click(object sender, EventArgs e)
        {            
            wd.TeamScoreDisplay(teams[0]);
            wd.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            wd.TeamRoundDisplay(teams[0]);
            wd.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            wd.LastRoundDisplay(null, rounds[1], null);
            wd.ShowDialog();
        }

        #endregion

        private void btnNotebook_Click(object sender, EventArgs e)
        {
            NotebookEntry ne = new NotebookEntry(teams);
            ne.ShowDialog();
            notebooksEntered = true;
        }

        #region Round Generation Button Actions

        private void btnPrelim_Click(object sender, EventArgs e)
        {
            try
            {
                if (rounds.Count > 0 || scores.Count > 0)
                {
                    if (MessageBox.Show("Warning:  All current round and score data will be lost", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                    {
                        return;
                    }

                    if (MessageBox.Show("Seriously:  All current round and score data will be lost, are you sure?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                    {
                        return;
                    }
                }

                rounds.Clear();
                scores.Clear();
                teams.ForEach(t => { t.Clear(); });

                rounds = GenerateSeeding(teams, roundsPerTeam, Round.Types.Preliminary, false);
                activeRound = 0;
                gameState = GameState.Preliminary;

                WriteRounds();

                UpdateUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnWildcard_Click(object sender, EventArgs e)
        {
            if (!notebooksEntered)
            {
                MessageBox.Show("Please enter notebook scores");
                return;
            }

            //TODO: This will break if there are less than 4 teams available for the wildcard

            //select the top 7 scores
            var topScores = from t in teams orderby t.TotalScore(Round.Types.Preliminary) descending select t;
            var topSeven = topScores.Take(7);

            var topNotebooks = from t in teams where !topSeven.Contains(t) orderby t.Notebook descending select t;
            var wildcards = topNotebooks.Take(4).ToList();

            Round r = new Round(rounds.Count + 1, Round.Types.Wildcard, wildcards[0], wildcards[1], wildcards[2], wildcards[3]);
            activeRound = rounds.Count;
            rounds.Add(r);
            AppendRound(r);

            UpdateUI();

            nudRound.Value = activeRound + 1;
        }

        private void btnSemi_Click(object sender, EventArgs e)
        {
            var scores = from t in teams orderby t.TotalScore(Round.Types.Preliminary) descending select t;
            var semi = scores.Take(7);

            var wildcard = rounds.Single(r => r.Type == Round.Types.Wildcard).Teams.OrderByDescending(t => t.TotalScore(Round.Types.Wildcard)).First();

            var semiTeams = semi.ToList();
            semiTeams.Add(wildcard);

            rounds.AddRange(GenerateSeeding(semiTeams, 6, Round.Types.Semifinals, false));
            WriteRounds();

            UpdateUI();

            nudRound.Value = activeRound + 1;
        }

        #endregion
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
