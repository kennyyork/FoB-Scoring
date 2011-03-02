﻿using System;
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

        private readonly string HEAD_TEMPLATE;

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
            HEAD_TEMPLATE = string.Format(@"<head><link rel=""stylesheet"" type=""text/css"" href=""{0}"" media=""print,screen""/></head>", pathRoot + @"best.css");
            velocity = new VelocityEngine();
            var p = new Commons.Collections.ExtendedProperties();            
            p.AddProperty("file.resource.loader.path", new System.Collections.ArrayList(new string[] { pathRoot }));
            velocity.Init(p);            
        }
        
        public void RoundDisplay(string title, List<Round> rounds, bool withColor)
        {
            Template t = velocity.GetTemplate(@"templates\round_display.vm");
            VelocityContext c = new VelocityContext();
            c.Put("head", HEAD_TEMPLATE);
            c.Put("rounds",rounds);
            StringWriter sw = new StringWriter();
            t.Merge(c, sw);
            webBrowser.DocumentText = sw.GetStringBuilder().ToString();
        }

        public void TeamRoundDisplay(Team team)
        {
            Template t = velocity.GetTemplate(@"templates\team_round_display.vm");
            VelocityContext c = new VelocityContext();
            c.Put("head", HEAD_TEMPLATE);
            c.Put("team", team);

            var x = from r in team.Rounds orderby r.Number select new { Color = r.TeamColor(team), r.Number };
            c.Put("rounds", x);

            StringWriter sw = new StringWriter();
            t.Merge(c, sw);
            webBrowser.DocumentText = sw.GetStringBuilder().ToString();
        }

        public void TeamScoreDisplay(Team team)
        {
            Template t = velocity.GetTemplate(@"templates\team_score_display.vm");
            VelocityContext c = new VelocityContext();
            c.Put("head", HEAD_TEMPLATE);
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

        public void UpdateCurrentRound(Round current, Round next1, Round next2)
        {
            throw new NotImplementedException();
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
}
