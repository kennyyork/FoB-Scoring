using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web.UI.HtmlControls;
using Microsoft.Win32;
using System.IO;
using System.Threading;

namespace Scoring
{
    public partial class WebDisplay : Form
    {
        public WebDisplay(string cssPath)
        {                        
            InitializeComponent();
            ReloadCSS(cssPath);
        }

        public void ReloadCSS(string cssPath)
        {
            string css = File.ReadAllText(cssPath);
            css = css.Replace("{", "{{");
            css = css.Replace("}", "}}");
            HTML_BASE = string.Format(HTML_TEMPLATE, css);
        }

        private const string HTML_TEMPLATE = @"<html>
<head>
<style type=""text/css"">
{0}
</style>
</head>
<body>{{0}}
</body>
</html>";
        private string HTML_BASE = string.Empty;

        private const string TABLE = @"<table id=""the_table"" class=""{0}"">
{1}
</table>";
        private const string TABLE_TITLE = @"<tr><th class=""table_title"" colspan=""100"">{0}</th></tr>
";
        
        private const string ROUND_HEADER = @"<tr><th class=""round_number"">Round</th><th class=""round_red"">Red</th><th class=""round_green"">Green</th><th class=""round_blue"">Blue</th><th class=""round_yellow"">Yellow</th></tr>
";
        private const string ROUND_ROW_COLOR = @"<tr><td class=""round_number"">{0}</td><td class=""round_red"">{1}</td><td class=""round_green"">{2}</td><td class=""round_blue"">{3}</td><td class=""round_yellow"">{4}</td></tr>
";
        private const string ROUND_ROW_NO_COLOR = @"<tr><td class=""round_number"">{0}</td><td class=""round_plain"">{1}</td><td class=""round_plain"">{2}</td><td class=""round_plain"">{3}</td><td class=""round_plain"">{4}</td></tr>
";        

        public void RoundDisplay(string title, List<Round> rounds, bool withColor)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(TABLE_TITLE, title);
            sb.Append(ROUND_HEADER);

            string template = withColor ? ROUND_ROW_COLOR : ROUND_ROW_NO_COLOR;
            foreach (var r in rounds)
            {
                //sb.AppendFormat(ROUND_ROW, r.Number, r.Red, r.Green, r.Blue, r.Yellow);
                sb.AppendFormat(template, r.Number, r.Red.Name, r.Green.Name, r.Blue.Name, r.Yellow.Name);
            }

            string table = string.Format(TABLE, "round_table", sb.ToString());
            string document = string.Format(HTML_BASE, table);
            webBrowser.DocumentText = document;
        }

        private const string TEAM_ROUND_HEADER = @"<tr><th class=""round_number"">Round</th><th class=""round_color"">Color</th></tr>
";
        private const string TEAM_ROUND_ROW = @"<tr><td class=""round_number"">{0}</td><td class=""round_{1}"">{2}</td></tr>
";

        public void TeamRoundDisplay(Team team)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(TABLE_TITLE, "Team Schedule: " + team.Name);
            sb.Append(TEAM_ROUND_HEADER);

            foreach (var r in team.Rounds)
            {
                string color = r.TeamColor(team);
                sb.AppendFormat(TEAM_ROUND_ROW, r.Number, color.ToLower(), color);
            }

            string table = string.Format(TABLE, "team_round_table", sb.ToString());
            string document = string.Format(HTML_BASE, table);
            webBrowser.DocumentText = document;
        }

        private const string TEAM_SCORE_SECTION_HEADER = @"<tr><th colspan=""100"" class=""score_section"">{0}</th></tr>
";
        private const string TEAM_SCORE_HEADER = @"<tr><td class=""score_number"">Round</td><td class=""score_markers"">Markers</td><td class=""score_goodcars"">Good Cars</td><td class=""score_badcars"">Bad Cars</td><td class=""score_goodlogs"">Good Logs</td><td class=""score_badlogs"">Bad Logs</td><td class=""score_goodcoal"">Good Coal</td><td class=""score_badcoal"">Bad Coal</td><td class=""score_multiplier"">Multiplier</td><td class=""score_round"">Score</td><td class=""score_total"">Total Score</td></tr>
";
        private const string TEAM_SCORE_ROW = @"<tr><td class=""score_number"">{0}</td><td class=""score_markers"">{1}</td><td class=""score_goodcars"">{2}</td><td class=""score_badcars"">{3}</td><td class=""score_goodlogs"">{4}</td><td class=""score_badlogs"">{5}</td><td class=""score_goodcoal"">{6}</td><td class=""score_badcoal"">{7}</td><td class=""score_multiplier"">{8}</td><td class=""score_round"">{9}</td><td class=""score_total"">{10}</td></tr>
";

        public void TeamScoreDisplay(Team team)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(TABLE_TITLE, "Team Scores: " + team.Name);            

            for (int i = 0; i < 4; ++i)
            {
                double sum = 0;
                var scores = from s in team.Scores where s.Round.Type == (Round.Types)i orderby s.Round.Number select s;
                if (scores.Count() > 0)
                {
                    sb.AppendFormat(TEAM_SCORE_SECTION_HEADER, ((Round.Types)i).ToString());
                    sb.Append(TEAM_SCORE_HEADER);
                    foreach (var s in scores)
                    {
                        sb.AppendFormat(TEAM_SCORE_ROW, s.Round.Number, s.Markers, s.CarsGood, s.CarsBad, s.LogsGood, s.LogsBad, s.CoalGood, s.CoalBad, s.Multiplier, s.GetScore(), sum);
                        sum += s.GetScore();
                    }
                }                
            }

            string table = string.Format(TABLE, "team_score_table", sb.ToString());
            string document = string.Format(HTML_BASE, table);
            webBrowser.DocumentText = document;
        }



        private void WaitForComplete()
        {
            while (webBrowser.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
        }

        public void WriteHTML(string path)
        {
            WaitForComplete();
            
            try
            {
                File.WriteAllText(path, webBrowser.DocumentText);
            }
            catch
            {
                throw;
            }
        }

        public void Print()
        {
            WaitForComplete();

            string keyName = @"Software\Microsoft\Internet Explorer\PageSetup";
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName, true))
            {
                if (key != null)
                {
                    string old_footer = (string)key.GetValue("footer");
                    string old_header = (string)key.GetValue("header");
                    key.SetValue("footer", "");
                    key.SetValue("header", "");
                    webBrowser.ShowPrintPreviewDialog();
                    //webBrowser.Print();      
                    key.SetValue("footer", old_footer);
                    key.SetValue("header", old_header);
                }
            }  
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Print();
        }

        private void WebDisplay_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }
    }
}
