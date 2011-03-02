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
        private readonly string pathRoot;        

        VelocityEngine velocity;
        public WebDisplay()
        {                        
            InitializeComponent();

#if DEBUG
            string temp = Application.ExecutablePath;
            string[] split = Application.ExecutablePath.Split('\\');
            pathRoot = string.Join("\\", split, 0, split.Length - 3) + "\\";
#else 
            pathRoot = Path.GetDirectoryName(Application.ExecutablePath);
#endif            
            velocity = new VelocityEngine();
            var p = new Commons.Collections.ExtendedProperties();            
            p.AddProperty("file.resource.loader.path", new System.Collections.ArrayList(new string[] { pathRoot }));
            velocity.Init(p);            
        }
        

//        <style type=""text/css"">
//{0}
//</style>
        private const string HEAD_TEMPLATE = @"<head><link rel=""stylesheet"" type=""text/css"" href=""{0}"" media=""print,screen""/></head>
";
        private string HTML_BASE = string.Empty;

        private const string TABLE = @"<table id=""the_table"" class=""{0}"">
{1}
</table>";
        private const string TABLE_TITLE = @"<div><p class=""table_title"">{0}</p></div>
";
        private const string TABLE_BODY_START = @"<tbody>";
        private const string TABLE_BODY_END = @"</tbody>";
        
        private const string ROUND_HEADER = @"<thead><th class=""round_number"">Round</th><th class=""round_red"">Red</th><th class=""round_green"">Green</th><th class=""round_blue"">Blue</th><th class=""round_yellow"">Yellow</th></thead>
";
        private const string ROUND_ROW_COLOR = @"<tr><td class=""round_number"">{0}</td><td class=""round_red"">{1}</td><td class=""round_green"">{2}</td><td class=""round_blue"">{3}</td><td class=""round_yellow"">{4}</td></tr>
";
        private const string ROUND_ROW_NO_COLOR = @"<tr><td class=""round_number"">{0}</td><td class=""round_plain"">{1}</td><td class=""round_plain"">{2}</td><td class=""round_plain"">{3}</td><td class=""round_plain"">{4}</td></tr>
";        

        public void RoundDisplay(string title, List<Round> rounds, bool withColor)
        {
            Template t = velocity.GetTemplate(@"templates\round_display.vm");
            VelocityContext c = new VelocityContext();
            c.Put("head", HEAD_TEMPLATE);
            c.Put("rounds",rounds);
            StringWriter sw = new StringWriter();
            t.Merge(c, sw);
            webBrowser.DocumentText = sw.GetStringBuilder().ToString();


            //StringBuilder sb = new StringBuilder();
            //sb.AppendFormat(TABLE_TITLE, title);
            //sb.Append(ROUND_HEADER);
            //sb.Append(TABLE_BODY_START);

            //string template = withColor ? ROUND_ROW_COLOR : ROUND_ROW_NO_COLOR;
            //foreach (var r in rounds)
            //{
            //    //sb.AppendFormat(ROUND_ROW, r.Number, r.Red, r.Green, r.Blue, r.Yellow);
            //    sb.AppendFormat(template, r.Number, r.Red.Name, r.Green.Name, r.Blue.Name, r.Yellow.Name);
            //}
            //sb.Append(TABLE_BODY_END);

            //string table = string.Format(TABLE, "round_table", sb.ToString());
            //string document = string.Format(HTML_BASE, table);
            //webBrowser.DocumentText = document;
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

        private const string CURRENT_ROUND_HEADER = @"<thead><td>Team</td><td>Markers</td><td>Good Cars</td><td>Bad Cars</td><td>Good Logs</td><td>Bad Logs</td><td>Good Coal</td><td>Bad Coal</td><td>Mulitplier</td><td>Score</td><td>Total</td></thead>";        
        private const string CURRENT_ROUND_ROW = @"<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td><td>{10}</td></tr>";
        
        public void UpdateCurrentRound(Round current, Round next1, Round next2)
        {
            //HtmlBuilder html = new HtmlBuilder();
            //html.LinkCSS(cssPath);
            //using (var body = html.WriteTag("body",null))
            //{
            //    using (var table = html.WriteTag("table", null))
            //    {
            //        //using var 
            //    }
            //}
            //StringBuilder sb = new StringBuilder();
            //sb.AppendFormat(TABLE_TITLE, "Round #" + current.Number.ToString());
            //sb.Append(TABLE_BODY_START);

            //for (int i = 0; i < 4; ++i)
            //{
            //    Score s = current.GetScore(i);
            //    sb.AppendFormat(CURRENT_ROUND_ROW, current.Teams[i].Name, s.Markers, s.CarsGood, s.CarsBad, s.LogsGood, s.LogsBad, s.CoalGood, s.CoalBad, s.Multiplier, s.GetScore(), sum);
            //}
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
    }

    public class HtmlBuilder : IDisposable
    {
        private StringBuilder html;

        public HtmlBuilder()
        {
            html = new StringBuilder();
            html.AppendFormat("<html>");
        }
        
        public void LinkCSS(string path)
        {
            string head = @"<head><link rel=""stylesheet"" type=""text/css"" href=""{0}"" media=""print,screen""/></head>";
            html.AppendFormat(head, path);
        }

        //public HtmlElement CreateBody(Dictionary<string, string> options)
        //{
        //    return WriteTag("body", options);
        //}

        //public HtmlElement CreateTable(Dictionary<string,string> options)
        //{
        //    return WriteTag("table",options);            
        //}

        //public HtmlElement CreateTable(Dictionary<string, string> options)
        //{
        //    return WriteTag("table", options);
        //}

        public HtmlElement WriteTag(string tag, Dictionary<string, string> options)
        {
            html.AppendFormat("<{0}",tag);
            if (options != null)
            {
                foreach (var pair in options)
                {
                    html.AppendFormat("{0}=\"{1}\"", pair.Key, pair.Value);
                }
            }
            html.Append(">");
            return new HtmlElement(this, tag);
        }

        #region Write Functions
        public void Append(string text)
        {            
            html.Append(text);
        }

        public void AppendLine()
        {
            html.AppendLine();
        }

        public void AppendFormat(string text, params object[] args)
        {
            html.AppendFormat(text, args);
        }

        public void Clear()
        {
            html.Length = 0;
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            html.AppendFormat("</html>");
        }

        #endregion

        public class HtmlElement : IDisposable
        {
            private string tag;
            private HtmlBuilder builder;

            public HtmlElement(HtmlBuilder parent, string tag)
            {
                builder = parent;
                this.tag = tag;   
            }

            #region IDisposable Members

            public void Dispose()
            {
                builder.AppendFormat("</{0}>",tag);
            }

            #endregion
        }
    }    
}
