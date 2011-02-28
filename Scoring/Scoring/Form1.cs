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

        private const string TEAM_FILE_PATH = "teams.txt";
        private const string SCORES_FILE_PATH = "scores.txt";
        private const string ROUNDS_FILE_PATH = "rounds.txt";        

        private void Form1_Load(object sender, EventArgs e)
        {
            CenterNud();

            //load existing            
            LoadFile();

            UpdateUI();            
        }


        private void UpdateUI()
        {
           
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

            try
            {
                StreamReader sr;
                string line;

                //read teams 
                sr = File.OpenText(TEAM_FILE_PATH);
                while( (line = sr.ReadLine()) != null )
                {
                    teams.Add(Team.FromString(line));                          
                }
                sr.Close();

                sr = File.OpenText(ROUNDS_FILE_PATH);
                while ((line = sr.ReadLine()) != null)
                {
                    rounds.Add(Round.FromString(teams,line));
                }
                sr.Close();

                sr = File.OpenText(SCORES_FILE_PATH);
                while ((line = sr.ReadLine()) != null)
                {
                    scores.Add(Score.FromString(teams,rounds,line));
                }
                sr.Close();

            }
            catch (Exception ex)
            {
                teams.Clear();
                rounds.Clear();
                scores.Clear();
                MessageBox.Show(ex.Message);
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
            int offsetX = scoringInput1.Left + (scoringInput1.Width / 2 ) - (nudRound.Width / 2);
            int offsetY = scoringInput1.Top + scoringInput1.Height + 5;

            nudRound.Left = offsetX;
            nudRound.Top = offsetY;
        }
    }
}
