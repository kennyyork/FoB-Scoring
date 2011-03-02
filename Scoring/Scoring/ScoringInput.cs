using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scoring
{
    public partial class ScoringInput : UserControl
    {
        public ScoringInput()
        {
            InitializeComponent();            
        }

        private List<Label> lblTeams = new List<Label>();
        private List<Label> lblScores = new List<Label>();                
        private List<ComboBox> cbMarkers = new List<ComboBox>();
        private List<ComboBox> cbCarsGood = new List<ComboBox>();
        private List<ComboBox> cbCarsBad = new List<ComboBox>();
        private List<ComboBox> cbLogsGood = new List<ComboBox>();
        private List<ComboBox> cbLogsBad = new List<ComboBox>();
        private List<ComboBox> cbCoalGood = new List<ComboBox>();
        private List<ComboBox> cbCoalBad = new List<ComboBox>();
        private List<ComboBox> cbMultiplier = new List<ComboBox>();

        private bool modified = false;        
        Score[] scores = new Score[4];
        Round currentRound;

        public List<Score> CurrentScores
        {
            get
            {
                return scores.ToList();
            }
        }

        public void SetScores(Round round)
        {
            currentRound = round;

            //see if scores exist for this round, if not create them
            scores[0] = round.Red.GetScore(round) ?? new Score(round.Red,round);
            scores[1] = round.Green.GetScore(round) ?? new Score(round.Green, round);
            scores[2] = round.Blue.GetScore(round) ?? new Score(round.Blue, round);
            scores[3] = round.Yellow.GetScore(round) ?? new Score(round.Yellow, round);            

            for (int i = 0; i < 4; ++i)
            {
                lblTeams[i].Text = scores[i].Team.Name;
                                
                cbMarkers[i].SelectedIndex = cbMarkers[i].Items.IndexOf(scores[i].Markers);
                cbCarsGood[i].SelectedIndex = cbCarsGood[i].Items.IndexOf(scores[i].CarsGood);
                cbCarsBad[i].SelectedIndex = cbCarsBad[i].Items.IndexOf(scores[i].CarsBad);
                cbLogsGood[i].SelectedIndex = cbLogsGood[i].Items.IndexOf(scores[i].LogsGood);
                cbLogsBad[i].SelectedIndex = cbLogsBad[i].Items.IndexOf(scores[i].LogsBad);
                cbCoalGood[i].SelectedIndex = cbCoalGood[i].Items.IndexOf(scores[i].CoalGood);
                cbCoalBad[i].SelectedIndex = cbCoalBad[i].Items.IndexOf(scores[i].CoalBad);
                cbMultiplier[i].SelectedIndex = cbMultiplier[i].Items.IndexOf(scores[i].Multiplier);

                lblScores[i].Text = scores[i].TotalScore.ToString();
            }

            modified = false;
        }

        public bool ScoresModified { get { return modified; } }

        public void CommitScores()
        {
            if (!modified) return;

            for (int i = 0; i < 4; ++i)
            {
                scores[i].Markers = (int)cbMarkers[i].SelectedItem;
                scores[i].CarsGood = (int)cbCarsGood[i].SelectedItem;
                scores[i].CarsBad = (int)cbCarsBad[i].SelectedItem;
                scores[i].LogsGood = (int)cbLogsGood[i].SelectedItem;
                scores[i].LogsBad = (int)cbLogsBad[i].SelectedItem;
                scores[i].CoalGood = (int)cbCoalGood[i].SelectedItem;
                scores[i].CoalBad = (int)cbCoalBad[i].SelectedItem;
                scores[i].Multiplier = (double)cbMultiplier[i].SelectedItem;                               
            }

            modified = false;
        }

        private ComboBox BuildCombo(object[] values, int column, int row)
        {
            ComboBox b = new ComboBox();
            b.Items.AddRange(values);
            b.Dock = DockStyle.Fill;
            b.DropDownStyle = ComboBoxStyle.DropDownList;
            b.SelectedIndex = 0;
            b.Width = 10;
            b.SelectedValueChanged += new EventHandler(b_SelectedValueChanged);
           
            this.tableLayoutPanel1.Controls.Add(b, column, row);

            return b;
        }

        void b_SelectedValueChanged(object sender, EventArgs e)
        {
            modified = true;

            if (currentRound != null)
            {
                for (int i = 0; i < 4; ++i)
                {
                    int Markers = (int)cbMarkers[i].SelectedItem;
                    int CarsGood = (int)cbCarsGood[i].SelectedItem;
                    int CarsBad = (int)cbCarsBad[i].SelectedItem;
                    int LogsGood = (int)cbLogsGood[i].SelectedItem;
                    int LogsBad = (int)cbLogsBad[i].SelectedItem;
                    int CoalGood = (int)cbCoalGood[i].SelectedItem;
                    int CoalBad = (int)cbCoalBad[i].SelectedItem;
                    double Multiplier = (double)cbMultiplier[i].SelectedItem;

                    lblScores[i].Text = Score.CalcScore(Markers, CarsGood, CarsBad, LogsGood, LogsBad, CoalGood, CoalBad, Multiplier).ToString("0.00");
                }
            }
        }

        private Label BuildLabel(int column, int row)
        {
            Label l = new Label();
            l.Dock = DockStyle.Fill;
            l.TextAlign = ContentAlignment.MiddleCenter;
            this.tableLayoutPanel1.Controls.Add(l, column, row);
            return l;
        }

        private object[] ToObject<T>(T[] type)
        {
            object[] o = new object[type.Length];
            type.CopyTo(o, 0);
            return o;
        }

        private void ScoringInput_Load(object sender, EventArgs e)
        {
            object[] values;
            
            //teams            
            for (int i = 1; i <= 4; ++i)
            {
                lblTeams.Add(BuildLabel(0, i + 1));                
            }

            lblTeams[0].BackColor = Color.Red;
            lblTeams[1].BackColor = Color.Green;
            lblTeams[2].BackColor = Color.LightBlue;
            lblTeams[3].BackColor = Color.Yellow;

            //score
            for (int i = 1; i <= 4; ++i)
            {
                lblScores.Add(BuildLabel(9, i + 1));
            }

            //markers                        
            for (int i = 1; i <= 4; ++i)
            {
                cbMarkers.Add(BuildCombo(ToObject(Score.MARKERS), 1, i + 1));
            }

            //good cars
            values = ToObject(Score.CARS);
            for (int i = 1; i <= 4; ++i)
            {
                cbCarsGood.Add(BuildCombo(values, 2, i + 1));                
            }

            //bad cars
            for (int i = 1; i <= 4; ++i)
            {
                cbCarsBad.Add(BuildCombo(values, 3, i + 1));                
            }

            //good logs
            values = ToObject(Score.LOGS);
            for (int i = 1; i <= 4; ++i)
            {
                cbLogsGood.Add(BuildCombo(values, 4, i + 1));
            }
            //bad logs
            for (int i = 1; i <= 4; ++i)
            {
                cbLogsBad.Add(BuildCombo(values, 5, i + 1));
            }

            //good coal
            values = ToObject(Score.COAL);
            for (int i = 1; i <= 4; ++i)
            {
                cbCoalGood.Add(BuildCombo(values, 6, i + 1));
            }
            //bad coal
            for (int i = 1; i <= 4; ++i)
            {
                cbCoalBad.Add(BuildCombo(values, 7, i + 1));
            }

            //multiplier            
            for (int i = 1; i <= 4; ++i)
            {
                ComboBox b = BuildCombo(ToObject(Score.MUTILIERS), 8, i + 1);
                b.FormatString = "0.0";
                b.FormattingEnabled = true;                
                cbMultiplier.Add(b);
                
            }          

            modified = false;
        }
    }
}
