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

        public void SetScores(Score team1, Score team2, Score team3, Score team4)
        {
            Score[] test = new Score[] { team1, team2, team3, team4 };

            for (int i = 0; i < 4; ++i)
            {
                lblTeams[i].Text = test[i].TeamName;
                
                cbMarkers[i].SelectedIndex = cbMarkers[i].Items.IndexOf(test[i].Markers);
                cbCarsGood[i].SelectedIndex = cbCarsGood[i].Items.IndexOf(test[i].CarsGood);
                cbCarsBad[i].SelectedIndex = cbCarsBad[i].Items.IndexOf(test[i].CarsBad);
                cbLogsGood[i].SelectedIndex = cbLogsGood[i].Items.IndexOf(test[i].LogsGood);
                cbLogsBad[i].SelectedIndex = cbLogsBad[i].Items.IndexOf(test[i].LogsBad);
                cbCoalGood[i].SelectedIndex = cbCoalGood[i].Items.IndexOf(test[i].CoalGood);
                cbCoalBad[i].SelectedIndex = cbCoalBad[i].Items.IndexOf(test[i].CoalBad);
                cbMultiplier[i].SelectedIndex = cbMultiplier[i].Items.IndexOf(test[i].Multiplier);
                
                lblScores[i].Text = test[i].GetScore().ToString();
            }
        }

        private ComboBox BuildCombo(object[] values, int column, int row)
        {
            ComboBox b = new ComboBox();
            b.Items.AddRange(values);
            b.Dock = DockStyle.Fill;
            b.DropDownStyle = ComboBoxStyle.DropDownList;
            b.SelectedIndex = 0;
            b.Width = 10;
           
            this.tableLayoutPanel1.Controls.Add(b, column, row);

            return b;
        }

        private Label BuildLabel(int column, int row)
        {
            Label l = new Label();
            l.Dock = DockStyle.Fill;
            l.TextAlign = ContentAlignment.MiddleCenter;
            this.tableLayoutPanel1.Controls.Add(l, column, row);
            return l;
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

            //markers
            values = new object[] { 0, 1, 2 };
            for (int i = 1; i <= 4; ++i)
            {
                cbMarkers.Add(BuildCombo(values, 1, i + 1));
            }

            //good cars
            values = new object[] { 0, 1, 2 };
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
            values = new object[] { 0, 1, 2, 3 };
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
            values = new object[] { 0, 1, 2, 3, 4, 5 };
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
            values = new object[] { 1.0f, 1.1f, 1.2f, 1.5f };
            for (int i = 1; i <= 4; ++i)
            {
                cbMultiplier.Add(BuildCombo(values,8, i + 1));
            }

            //score
            for (int i = 1; i <= 4; ++i)
            {
                lblScores.Add(BuildLabel(9, i + 1));
            }
        }
    }
}
