using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scoring
{
    public partial class NotebookEntry : Form
    {        
        private List<Team> teams;

        public NotebookEntry(List<Team> teams)
        {
            if (teams == null) throw new ArgumentNullException();
            this.teams = teams;

            InitializeComponent();            
        }

        private void NotebookEntry_Load(object sender, EventArgs e)
        {
            dataGridView.AutoGenerateColumns = false;

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Number", HeaderText = "Team Number", ReadOnly = true });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Team Name", ReadOnly = true });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Notebook", HeaderText = "Notebook Score" });

            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;

            dataGridView.DataSource = teams;
        }
        
        private void btnOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void NotebookEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(System.IO.File.Open("teams.txt", System.IO.FileMode.Create)))
            {
                foreach (var t in teams)
                {
                    sw.WriteLine(t);
                }
            }
        }
    }
}
