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
        public WebDisplay()
        {                        
            InitializeComponent();
            webBrowser.ScriptErrorsSuppressed = true;
            
        }

        public void DisplayPage(string url)
        {            
            webBrowser.Navigate(@"http://localhost/" + url);
            txtUrl.Text = url;
        }

        private void WaitForComplete()
        {
            while (webBrowser.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
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
                    Dictionary<string, object> backup = key.GetValueNames().ToDictionary(s => s, s => key.GetValue(s));

                    key.SetValue("footer", "");
                    key.SetValue("header", "");
                    key.SetValue("margin_left", "0.25");
                    key.SetValue("margin_right", "0.25");
                    key.SetValue("margin_bottom", "0.25");
                    key.SetValue("header_top", "0.25");

                    if (preview)
                    {
                        webBrowser.ShowPrintPreviewDialog();
                    }
                    else
                    {
                        webBrowser.Print();                              
                        //webBrowser.ShowPrintDialog();
                    }

                    foreach (var p in backup)
                    {
                        key.SetValue(p.Key, p.Value);
                    }
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

        private void txtUrl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                webBrowser.Navigate(@"http://localhost/" + txtUrl.Text);
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            webBrowser.Navigate(@"http://localhost/index.html");
        }
    }
}
