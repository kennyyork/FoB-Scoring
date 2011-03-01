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
        public WebDisplay()
        {
            InitializeComponent();            
        }

        private string lastOutput;

        private const string HTML_BASE = @"<html><body>{0}</body></html>";
        private const string TABLE = @"<table class=""{0}"">{1}</table>";

        private const string ROUND_HEADER = @"<tr><td class=""round_number"">Round Number</td><td class=""round_red"">Red</td><td class=""round_green"">Green</td><td class=""round_blue"">Blue</td><td class=""round_yellow"">Yellow</td></tr>";
        private const string ROUND_ROW_COLOR = @"<tr><td class=""round_number"">{0}</td><td class=""round_red"">{1}</td><td class=""round_green"">{2}</td><td class=""round_blue"">{3}</td><td class=""round_yellow"">{4}</td></tr>";
        private const string ROUND_ROW_NO_COLOR = @"<tr><td class=""round_number"">{0}</td><td class=""round_plain"">{1}</td><td class=""round_plain"">{2}</td><td class=""round_plain"">{3}</td><td class=""round_plain"">{4}</td></tr>";

        public void RoundDisplay(List<Round> rounds, bool withColor)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ROUND_HEADER);

            string template = withColor ? ROUND_ROW_COLOR : ROUND_ROW_NO_COLOR;
            foreach (var r in rounds)
            {
                //sb.AppendFormat(ROUND_ROW, r.Number, r.Red, r.Green, r.Blue, r.Yellow);
                sb.AppendFormat(template, r.Number, r.Red.Name, r.Green.Name, r.Blue.Name, r.Yellow.Name);
            }

            string table = string.Format(TABLE, "round_table", sb.ToString());
            string document = string.Format(HTML_BASE, table);
            WriteDocument(document);
        }

        private const string TEAM_ROUND_HEADER = @"<tr><td class=""round_number"">Round Number</td><td class=""round_color"">Color</td></tr>";
        private const string TEAM_ROUND_ROW = @"<tr><td class=""round_number"">{0}</td><td class=""round_{1}"">{2}</td></tr>";

        public void TeamRoundDisplay(Team team)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(TEAM_ROUND_HEADER);

            foreach (var r in team.Rounds)
            {
                string color = r.TeamColor(team);
                sb.AppendFormat(TEAM_ROUND_ROW, r.Number, color.ToLower(), color);
            }

            string table = string.Format(TABLE, "tean_round_table", sb.ToString());
            string document = string.Format(HTML_BASE, table);
            WriteDocument(document);
        }

        private void WriteDocument(string text)
        {            
            webBrowser.DocumentText = text;
            lastOutput = text;
        }

        public void WriteHTML(string path)
        {
            DateTime t = DateTime.Now;
            
            try
            {
                File.WriteAllText(path, lastOutput);
            }
            catch
            {
                throw;
            }
        }

        public void QuickPrint()
        {
            while (webBrowser.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }

            //Show();
            //Visible = false;
            Print();
            //Hide();
            //Close();
        }

        public void Print()
        {
            DateTime t = DateTime.Now;

            //reset.WaitOne();

            if (webBrowser.ReadyState != WebBrowserReadyState.Complete)
            {
                return;
            }

            string keyName = @"Software\Microsoft\Internet Explorer\PageSetup";
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName, true))
            {
                if (key != null)
                {
                    string old_footer = (string)key.GetValue("footer");
                    string old_header = (string)key.GetValue("header");
                    key.SetValue("footer", "");
                    key.SetValue("header", "");
                    //webBrowser.ShowPrintPreviewDialog();
                    webBrowser.Print();      
                    key.SetValue("footer", old_footer);
                    key.SetValue("header", old_header);
                }
            }  
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Print();   
        }
    }
}
