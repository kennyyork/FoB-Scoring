using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Threading;
using NVelocity;
using NVelocity.App;
using NVelocity.Context;

namespace Scoring
{
    public partial class WebDisplay : Form
    {             
        VelocityEngine velocity;
        VelocityContext baseContext; 

        private readonly string cssFile;
        private readonly string scriptFile;

        public WebDisplay()
        {                        
            InitializeComponent();
            webBrowser.ScriptErrorsSuppressed = true;
            velocity = new VelocityEngine();
            var p = new Commons.Collections.ExtendedProperties();           

#if DEBUG
            string temp = Application.ExecutablePath;
            string[] split = Application.ExecutablePath.Split('\\');
            string pathRoot = string.Join("\\", split, 0, split.Length - 3);
            cssFile = string.Format(@"file:///{0}\html\best.css", pathRoot);
            scriptFile = string.Format(@"file:///{0}\html\scripts.js", pathRoot);\
            p.AddProperty("file.resource.loader.path", new System.Collections.ArrayList(new string[] { pathRoot }));
#else
            cssFile = "best.css";
            scriptFile = "scripts.js";            
#endif
            velocity.Init(p);
            baseContext = new VelocityContext();
            baseContext.Put("script", scriptFile);
            baseContext.Put("css", cssFile);
        }
        
        public void RoundDisplay(string title, IEnumerable<Round> rounds)
        {
            Template t = velocity.GetTemplate(@"templates\round_display.vm");
            VelocityContext c = new VelocityContext(baseContext);            
            c.Put("rounds",rounds);
            c.Put("title", title);
            StringWriter sw = new StringWriter();
            t.Merge(c, sw);            
            webBrowser.DocumentText = sw.GetStringBuilder().ToString();
        }

        public void TeamRoundDisplay(Team team)
        {
            Template t = velocity.GetTemplate(@"templates\team_rounds_display.vm");
            VelocityContext c = new VelocityContext(baseContext);
            c.Put("team", team);

            var x = from r in team.Rounds orderby r.Number select new { Color = r.TeamColor(team), r.Number };
            c.Put("rounds", x);

            StringWriter sw = new StringWriter();
            t.Merge(c, sw);
            webBrowser.DocumentText = sw.GetStringBuilder().ToString();
        }

        public void TeamScoreDisplay(Team team)
        {
            Template t = velocity.GetTemplate(@"templates\team_scores_display.vm");
            VelocityContext c = new VelocityContext(baseContext);            
            c.Put("team", team);

            var scores = from s in team.Scores
                    orderby s.Round.Number
                    group s by s.Round.Type into types
                    select new
                    {
                        Type = types.Key.ToString(),
                        Scores = types
                    };
           c.Put("scores", scores);            

            StringWriter sw = new StringWriter();
            t.Merge(c, sw);            
            webBrowser.DocumentText = sw.GetStringBuilder().ToString();
        }

        public void LastRoundDisplay(Round current, Round next1, Round next2)
        {
            Template template = velocity.GetTemplate(@"templates\last_round_scores.vm");
            VelocityContext c = new VelocityContext(baseContext);            

            if (current != null)
            {
                var scores = from t in current.Teams select new { Name = t.Name, Score = t.GetScore(current), TotalScore = t.TotalScore(current.Type) };
                c.Put("current", current);
                c.Put("scores", scores);
            }
            else
            {
                List<object> scores = new List<object>();
                scores.Add(new object());
                scores.Add(scores[0]);
                scores.Add(scores[0]);
                scores.Add(scores[0]);
                c.Put("scores", scores);
            }

            System.Collections.ArrayList list = new System.Collections.ArrayList();
            if (next1 != null)
            {
                list.Add(new { Number = next1.Number, Red = next1.Red.Name, Green = next1.Green.Name, Blue = next1.Blue.Name, Yellow = next1.Yellow.Name });
            }
            else
            {
                list.Add(new object());
            }

            if (next2 != null)
            {
                list.Add(new { Number = next2.Number, Red = next2.Red.Name, Green = next2.Green.Name, Blue = next2.Blue.Name, Yellow = next2.Yellow.Name });
            }
            else
            {
                list.Add(new object());
            }

            c.Put("next", list);
            
            StringWriter sw = new StringWriter();
            template.Merge(c, sw);            
            webBrowser.DocumentText = sw.GetStringBuilder().ToString();
        }

        public void OverallScoresDisplay(IEnumerable<Team> teams, Round.Types type, bool split)
        {
            Template template = velocity.GetTemplate(@"templates\overall_scores.vm");
            
            //filter teams by round
            var teamSet = teams.Where(t => t.Rounds.Any(r => r.Type == type));

            var scores = (from t in teamSet orderby t.TotalScore(type) descending select new TempScore { Name = t.Name, Score = t.TotalScore(type) }).ToList();
            
            int place = 1;
            int count = 1;
            double last = double.MaxValue;

            foreach (var t in scores)
            {
                if (t.Score == last)
                {
                    t.Place = place;
                }
                else
                {
                    t.Place = count;
                    place = count;
                    last = t.Score;
                }

                ++count;
            }

            if (split)
            {
                int total = scores.Count;
                for (int i = 0; i < total; i += 16)
                {
                    VelocityContext c = new VelocityContext(baseContext);
                    var section = scores.Skip(i).Take(16).ToList();
                    c.Put("teams", section);
                    StringWriter sw = new StringWriter();
                    template.Merge(c, sw);

                    File.WriteAllText(string.Format(@"html\overall_scores_{0}.html", i / 16), sw.ToString());
                }
            }
            else
            {
                VelocityContext c = new VelocityContext(baseContext);
                c.Put("teams", scores);
                StringWriter sw = new StringWriter();
                template.Merge(c, sw);
                webBrowser.DocumentText = sw.ToString();
            }
        }

        private class TempScore
        {
            public string Name { get; set; }
            public int Place { get; set; }
            public double Score { get; set; }
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

        public void Print(bool preview)
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
                    if (preview)
                    {
                        webBrowser.ShowPrintPreviewDialog();
                    }
                    else
                    {
                        webBrowser.Print();      
                    }
                    key.SetValue("footer", old_footer);
                    key.SetValue("header", old_header);
                }
            }  
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Print(false);
        }

        private void WebDisplay_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            webBrowser.Refresh(WebBrowserRefreshOption.Completely);         
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Print(true);   
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "HTML files (*.html)|*.html";
            if (DialogResult.OK == sfd.ShowDialog())
            {                
                File.WriteAllText(sfd.FileName, webBrowser.DocumentText);
            }
        }
    }
}
