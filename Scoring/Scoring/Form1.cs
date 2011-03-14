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
using System.Diagnostics;

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
            Finals,
            Complete
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
        private const int WILDCARD_ROUNDS = 1;
        private const int SEMIFINAL_ROUNDS = 12;
        private const int FINAL_ROUNDS = 4;        

        private void Form1_Load(object sender, EventArgs e)
        {
            StartServer();

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

        private void StartServer()
        {
            var p = Process.GetProcessesByName("mongoose-2.11");
            if (p.Length == 0)
            {                
                Process proc = Process.Start(@"html\mongoose-2.11.exe");                
            }
        }

        bool notebooksEntered = false;
        private void UpdateUI()
        {
            int prelimCount = roundsPerTeam * teams.Count / 4;

            btnPrelim.Enabled = gameState == GameState.None || gameState == GameState.Preliminary;
            btnWildcard.Enabled = gameState == GameState.Wildcard && (rounds.Count == prelimCount);
            btnSemi.Enabled = gameState == GameState.SemiFinals && (rounds.Count == prelimCount + 1);
            btnFinals.Enabled = gameState == GameState.Finals && (rounds.Count < (prelimCount + WILDCARD_ROUNDS + SEMIFINAL_ROUNDS));

            gbWeb.Enabled = gameState != GameState.None;
            gbPrint.Enabled = gameState != GameState.None;
            btnPrintFinal.Enabled = gameState == GameState.Complete;
            
            if (activeRound < 0 || activeRound > rounds.Count)
            {
                pnlScoreIn.Enabled = false;
            }
            else
            {
                pnlScoreIn.Enabled = true;                
                nudRound.Maximum = Math.Min(activeRound + 1, rounds.Count);
                nudRound.Minimum = 1;
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
            catch
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
            else if (scores.Count >= (prelimScores + (WILDCARD_ROUNDS * 4) + (SEMIFINAL_ROUNDS * 4)))
            {
                gameState = GameState.Finals;
            }
            else if (scores.Count < (prelimScores + (WILDCARD_ROUNDS * 4) + (SEMIFINAL_ROUNDS * 4)))
            {
                gameState = GameState.SemiFinals;
            }
            else
            {
                gameState = GameState.Complete;
            }
            //else
            //{
            //    throw new InvalidOperationException("Could not determine game state");
            //}
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

        [DebuggerDisplay("{Team.Name} = {Games},{LastRound}")]
        private class TeamWrapper
        {
            public Team Team;
            public int LastRound;
            public int Games;

            public TeamWrapper(Team t)
            {
                Team = t;
                LastRound = -1;
                Games = 0;
            }
            
            public static List<TeamWrapper> Wrap(IEnumerable<Team> teams)
            {
                List<TeamWrapper> wrapper = new List<TeamWrapper>();
                foreach (var t in teams)
                {
                    wrapper.Add(new TeamWrapper(t));
                }
                return wrapper;
            }
        }

        private List<Round> GenerateSeeding(List<Team> origTeams, int gamesPerTeam, Round.Types type, bool backToBack)
        {
            StringBuilder debug = new StringBuilder();
            
            //generate a random list of all teams
            //select 4 teams from that list wher not in last round
              //if 4 not available, re-add all teams and select where 1) not in current set and 2) not in last round
                //if 4 not available, select any needed

            int currentRound = rounds.Count + 1;
            List<TeamWrapper> teams = TeamWrapper.Wrap(origTeams);
            
            List<Round> output = new List<Round>();
            int required = (teams.Count * gamesPerTeam) / 4;

            List<TeamWrapper> availTeams = teams.Shuffle();
            while (output.Count < required)
            {
                List<TeamWrapper> set = availTeams.TakeAndRemove(4, t => t.LastRound != (currentRound - 1) && t.Games < gamesPerTeam);
                debug.AppendFormat("first set {0}  ",set.Count);
                foreach (var s in set) { debug.AppendFormat(" {0} ", s); }
                debug.AppendLine();

                if (set.Count < 4)
                {
                    debug.AppendFormat("failed to pull 4, only got {0}\n", set.Count);
                    availTeams = teams.Shuffle();
                    set.AddRange(availTeams.TakeAndRemove(4 - set.Count, t => (t.LastRound != (currentRound - 1) && t.Games < gamesPerTeam) && !set.Contains(t)));

                    debug.AppendFormat("second set {0}  ",set.Count);
                    foreach (var s in set) { debug.AppendFormat(" {0} ", s); }
                    debug.AppendLine();

                    if (set.Count < 4)
                    {
                        //is this our last round?
                        if (output.Count == (required - 1))
                        {
                            //TODO: fix our last round?
                            break;
                        }
                        else
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
                }

                set.ForEach(t => { ++t.Games; t.LastRound = currentRound; });
                Round r = new Round(currentRound++, type, from s in set select s.Team);
                output.Add(r);                
            }

            return output;
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
            List<Team> curTeams = new List<Team>();
            foreach (var s in scoringInput1.CurrentScores)
            {                
                if (btnSubmit.Text == "Submit")
                {
                    scores.Add(s);
                    AppendScore(s);
                }

                curTeams.Add(s.Team);
            }

            HtmlGenerator.TeamScoreDisplay(curTeams);

            if (btnSubmit.Text == "Submit")
            {                
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
            UpdateRoundWeb();

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
                case GameState.Finals:
                    gameState = GameState.Complete;
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
                    HtmlGenerator.RoundDisplay(HtmlGenerator.PageId.RoundFull, gameState.ToString(), result);
                    wd.DisplayPage(HtmlGenerator.GetPageId(HtmlGenerator.PageId.RoundFull));
                    wd.Show();
                }
                else
                {
                    MessageBox.Show(string.Format("Please generate {0}",title));                     
                }
            }
        }

        #endregion

        private void btnNotebook_Click(object sender, EventArgs e)
        {
            if (gameState != GameState.Preliminary)
            {
                if (DialogResult.No == MessageBox.Show("Seeding rounds are finished, are you sure you want to change notebook scores?", "Change Scores?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                {
                    return;
                }
            }

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
                UpdateAllWeb();                
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

            Round r = new Round(rounds.Count + 1, Round.Types.Wildcard, wildcards);
            activeRound = rounds.Count;
            rounds.Add(r);
            AppendRound(r);

            HtmlGenerator.RoundDisplay(HtmlGenerator.PageId.RoundFull, gameState.ToString(), new List<Round> { r });
            HtmlGenerator.TeamRoundsDisplay(teams);

            UpdateUI();

            nudRound.Value = activeRound + 1;
        }

        private void btnSemi_Click(object sender, EventArgs e)
        {
            //var scores = from t in teams orderby t.TotalScore(Round.Types.Preliminary) descending select t;
            //var semi = scores.Take(7);

            var semi = teams.OrderByDescending(t => t.TotalScore(Round.Types.Preliminary)).Take(7);

            var wildcard = rounds.Single(r => r.Type == Round.Types.Wildcard).Teams.OrderByDescending(t => t.TotalScore(Round.Types.Wildcard)).First();

            var semiTeams = semi.ToList();
            semiTeams.Add(wildcard);

            var newRounds = GenerateSeeding(semiTeams, 6, Round.Types.Semifinals, false);
            rounds.AddRange(newRounds);
            WriteRounds();

            HtmlGenerator.RoundDisplay(HtmlGenerator.PageId.RoundFull, gameState.ToString(), newRounds);
            HtmlGenerator.TeamRoundsDisplay(teams);

            UpdateUI();

            nudRound.Value = activeRound + 1;
        }

        private void btnFinals_Click(object sender, EventArgs e)
        {
            var finals = teams.OrderByDescending(t => t.TotalScore(Round.Types.Semifinals)).Take(4).ToList();

            Round r = new Round(activeRound + 1, Round.Types.Finals, finals);
            for (int i = 0; i < 4; ++i)
            {
                rounds.Add(r);
                AppendRound(r);

                r = new Round(activeRound + 2 + i, Round.Types.Finals, finals);
            }

            var finalSched = from r1 in rounds where r1.Type == Round.Types.Finals orderby r1.Number select r1;
            HtmlGenerator.RoundDisplay(HtmlGenerator.PageId.RoundFull, gameState.ToString(), finalSched);
            HtmlGenerator.TeamRoundsDisplay(teams);

            UpdateUI();

            nudRound.Value = activeRound + 1;
        }

        #endregion

        private Round.Types ConvertState(GameState state)
        {
            switch (state)
            {
                case GameState.Complete:
                case GameState.Finals:
                    return Round.Types.Finals;
                case GameState.Preliminary:
                case GameState.None:
                    return Round.Types.Preliminary;
                case GameState.Wildcard:
                    return Round.Types.Wildcard;
                case GameState.SemiFinals:
                    return Round.Types.Semifinals;
                default:
                    throw new ArgumentException();
            }
        }
        
        #region Printing        
        private void btnPrintSchedule_Click(object sender, EventArgs e)
        {            
            wd.DisplayPage(HtmlGenerator.GetPageId(HtmlGenerator.PageId.RoundFull));            
            wd.Print(false);            
        }

        private void btnPrintRefSheet_Click(object sender, EventArgs e)
        {
            string path = HtmlGenerator.GetPageId(HtmlGenerator.PageId.ScoringReferee);

            wd.DisplayPage(string.Format(path, "red"));
            wd.Print(false);

            wd.DisplayPage(string.Format(path, "green"));
            wd.Print(false);

            wd.DisplayPage(string.Format(path, "blue"));
            wd.Print(false);

            wd.DisplayPage(string.Format(path, "yellow"));
            wd.Print(false);
        }

        private void btnScoreSheets_Click(object sender, EventArgs e)
        {            
            wd.DisplayPage(HtmlGenerator.GetPageId(HtmlGenerator.PageId.ScoringMaster));
            wd.Print(false);
        }

        private void btnPrintBlank_Click(object sender, EventArgs e)
        {
            wd.DisplayPage(HtmlGenerator.GetPageId(HtmlGenerator.PageId.ScoringBlank));
            wd.Print(false);
        }

        private void btnPrintFinal_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Final Score not implemented");
            //wd.DisplayPage(HtmlGenerator.GetPageId(HtmlGenerator.PageId.ScoringBlank));
        }

        #endregion

        #region Web Generators
        private void btnWebSched_Click(object sender, EventArgs e)
        {
            var set = from r in rounds where r.Type == ConvertState(gameState) select r;
            HtmlGenerator.RoundDisplay(HtmlGenerator.PageId.RoundFull, gameState.ToString(), set);
        }

        private void btnWebRef_Click(object sender, EventArgs e)
        {
            var set = from r in rounds where r.Type == ConvertState(gameState) select r;
            HtmlGenerator.RefereeFieldSheets(set);
        }        

        private void btnWebScore_Click(object sender, EventArgs e)
        {
            var set = from r in rounds where r.Type == ConvertState(gameState) select r;
            HtmlGenerator.RefereeMasterSheets(set);
        }
        
        private void btnWebSchedDisplay_Click(object sender, EventArgs e)
        {
            HtmlGenerator.RoundDisplay(HtmlGenerator.PageId.RoundPartial, gameState.ToString(), rounds.Skip(activeRound).Take(8));            
        }

        private void btnWebLastRound_Click(object sender, EventArgs e)
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

            HtmlGenerator.LastRoundDisplay(rounds[activeRound], next1, next2);
        }

        private void btnWebAllScores_Click(object sender, EventArgs e)
        {
            HtmlGenerator.OverallScoresDisplay(teams, ConvertState(gameState));
        }

        private void UpdateAllWeb()
        {
            //printable
            btnWebSched_Click(null, EventArgs.Empty);            
            btnWebRef_Click(null, EventArgs.Empty);
            btnWebScore_Click(null, EventArgs.Empty);

            //display
            btnWebSchedDisplay_Click(null, EventArgs.Empty);
            btnWebLastRound_Click(null, EventArgs.Empty);
            btnWebAllScores_Click(null, EventArgs.Empty);

        }

        private void UpdateRoundWeb()
        {
            btnWebSchedDisplay_Click(null, EventArgs.Empty);
            btnWebLastRound_Click(null, EventArgs.Empty);
            btnWebAllScores_Click(null, EventArgs.Empty);
        }
        #endregion

        private void btnOpenWeb_Click(object sender, EventArgs e)
        {
            wd.Show();
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
