namespace Scoring
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnPrelim = new System.Windows.Forms.Button();
            this.btnWildcard = new System.Windows.Forms.Button();
            this.btnSemi = new System.Windows.Forms.Button();
            this.btnFinals = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnNotebook = new System.Windows.Forms.Button();
            this.nudRound = new System.Windows.Forms.NumericUpDown();
            this.pnlScoreIn = new System.Windows.Forms.Panel();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnCurrentSched = new System.Windows.Forms.Button();
            this.btnPrintScore = new System.Windows.Forms.Button();
            this.gbWeb = new System.Windows.Forms.GroupBox();
            this.btnUpdateWeb = new System.Windows.Forms.Button();
            this.gbPrint = new System.Windows.Forms.GroupBox();
            this.btnScoreSheets = new System.Windows.Forms.Button();
            this.btnPrintRefSheet = new System.Windows.Forms.Button();
            this.btnPrintBlank = new System.Windows.Forms.Button();
            this.btnPrintFinal = new System.Windows.Forms.Button();
            this.btnPrintSchedule = new System.Windows.Forms.Button();
            this.btnPrintAll = new System.Windows.Forms.Button();
            this.scoringInput1 = new Scoring.ScoringInput();
            this.btnWebSched = new System.Windows.Forms.Button();
            this.btnWebRef = new System.Windows.Forms.Button();
            this.btnWebScore = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRound)).BeginInit();
            this.pnlScoreIn.SuspendLayout();
            this.gbWeb.SuspendLayout();
            this.gbPrint.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPrelim
            // 
            this.btnPrelim.Location = new System.Drawing.Point(6, 19);
            this.btnPrelim.Name = "btnPrelim";
            this.btnPrelim.Size = new System.Drawing.Size(103, 34);
            this.btnPrelim.TabIndex = 6;
            this.btnPrelim.Text = "Generate Prelim";
            this.btnPrelim.UseVisualStyleBackColor = true;
            this.btnPrelim.Click += new System.EventHandler(this.btnPrelim_Click);
            // 
            // btnWildcard
            // 
            this.btnWildcard.Location = new System.Drawing.Point(6, 59);
            this.btnWildcard.Name = "btnWildcard";
            this.btnWildcard.Size = new System.Drawing.Size(103, 34);
            this.btnWildcard.TabIndex = 7;
            this.btnWildcard.Text = "Generate Wildcard";
            this.btnWildcard.UseVisualStyleBackColor = true;
            this.btnWildcard.Click += new System.EventHandler(this.btnWildcard_Click);
            // 
            // btnSemi
            // 
            this.btnSemi.Location = new System.Drawing.Point(6, 99);
            this.btnSemi.Name = "btnSemi";
            this.btnSemi.Size = new System.Drawing.Size(103, 34);
            this.btnSemi.TabIndex = 8;
            this.btnSemi.Text = "Generate Semi-Finals";
            this.btnSemi.UseVisualStyleBackColor = true;
            this.btnSemi.Click += new System.EventHandler(this.btnSemi_Click);
            // 
            // btnFinals
            // 
            this.btnFinals.Location = new System.Drawing.Point(6, 139);
            this.btnFinals.Name = "btnFinals";
            this.btnFinals.Size = new System.Drawing.Size(103, 34);
            this.btnFinals.TabIndex = 9;
            this.btnFinals.Text = "Generate Finals";
            this.btnFinals.UseVisualStyleBackColor = true;
            this.btnFinals.Click += new System.EventHandler(this.btnFinals_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnPrelim);
            this.groupBox1.Controls.Add(this.btnFinals);
            this.groupBox1.Controls.Add(this.btnWildcard);
            this.groupBox1.Controls.Add(this.btnSemi);
            this.groupBox1.Location = new System.Drawing.Point(668, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(115, 179);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Round Generation";
            // 
            // btnNotebook
            // 
            this.btnNotebook.Location = new System.Drawing.Point(674, 197);
            this.btnNotebook.Name = "btnNotebook";
            this.btnNotebook.Size = new System.Drawing.Size(103, 34);
            this.btnNotebook.TabIndex = 11;
            this.btnNotebook.Text = "Enter Notebooks";
            this.btnNotebook.UseVisualStyleBackColor = true;
            this.btnNotebook.Click += new System.EventHandler(this.btnNotebook_Click);
            // 
            // nudRound
            // 
            this.nudRound.Location = new System.Drawing.Point(3, 3);
            this.nudRound.Name = "nudRound";
            this.nudRound.Size = new System.Drawing.Size(62, 20);
            this.nudRound.TabIndex = 13;
            this.nudRound.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudRound.ValueChanged += new System.EventHandler(this.nudRound_ValueChanged);
            // 
            // pnlScoreIn
            // 
            this.pnlScoreIn.Controls.Add(this.btnSubmit);
            this.pnlScoreIn.Controls.Add(this.nudRound);
            this.pnlScoreIn.Location = new System.Drawing.Point(271, 205);
            this.pnlScoreIn.Name = "pnlScoreIn";
            this.pnlScoreIn.Size = new System.Drawing.Size(132, 26);
            this.pnlScoreIn.TabIndex = 14;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(71, 3);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(58, 20);
            this.btnSubmit.TabIndex = 14;
            this.btnSubmit.Text = "Enter";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnCurrentSched
            // 
            this.btnCurrentSched.Location = new System.Drawing.Point(674, 237);
            this.btnCurrentSched.Name = "btnCurrentSched";
            this.btnCurrentSched.Size = new System.Drawing.Size(103, 34);
            this.btnCurrentSched.TabIndex = 15;
            this.btnCurrentSched.Text = "View Current Schedule";
            this.btnCurrentSched.UseVisualStyleBackColor = true;
            this.btnCurrentSched.Click += new System.EventHandler(this.btnCurrentSched_Click);
            // 
            // btnPrintScore
            // 
            this.btnPrintScore.Location = new System.Drawing.Point(674, 277);
            this.btnPrintScore.Name = "btnPrintScore";
            this.btnPrintScore.Size = new System.Drawing.Size(103, 34);
            this.btnPrintScore.TabIndex = 16;
            this.btnPrintScore.Text = "Print Score Sheets";
            this.btnPrintScore.UseVisualStyleBackColor = true;
            this.btnPrintScore.Click += new System.EventHandler(this.btnPrintScore_Click);
            // 
            // gbWeb
            // 
            this.gbWeb.Controls.Add(this.btnWebScore);
            this.gbWeb.Controls.Add(this.btnWebRef);
            this.gbWeb.Controls.Add(this.btnWebSched);
            this.gbWeb.Controls.Add(this.btnUpdateWeb);
            this.gbWeb.Location = new System.Drawing.Point(133, 205);
            this.gbWeb.Name = "gbWeb";
            this.gbWeb.Size = new System.Drawing.Size(115, 267);
            this.gbWeb.TabIndex = 17;
            this.gbWeb.TabStop = false;
            this.gbWeb.Text = "Webpage";
            // 
            // btnUpdateWeb
            // 
            this.btnUpdateWeb.Location = new System.Drawing.Point(6, 19);
            this.btnUpdateWeb.Name = "btnUpdateWeb";
            this.btnUpdateWeb.Size = new System.Drawing.Size(103, 35);
            this.btnUpdateWeb.TabIndex = 0;
            this.btnUpdateWeb.Text = "Update Current";
            this.btnUpdateWeb.UseVisualStyleBackColor = true;
            // 
            // gbPrint
            // 
            this.gbPrint.Controls.Add(this.btnScoreSheets);
            this.gbPrint.Controls.Add(this.btnPrintRefSheet);
            this.gbPrint.Controls.Add(this.btnPrintBlank);
            this.gbPrint.Controls.Add(this.btnPrintFinal);
            this.gbPrint.Controls.Add(this.btnPrintSchedule);
            this.gbPrint.Controls.Add(this.btnPrintAll);
            this.gbPrint.Location = new System.Drawing.Point(12, 205);
            this.gbPrint.Name = "gbPrint";
            this.gbPrint.Size = new System.Drawing.Size(115, 270);
            this.gbPrint.TabIndex = 18;
            this.gbPrint.TabStop = false;
            this.gbPrint.Text = "Printing";
            // 
            // btnScoreSheets
            // 
            this.btnScoreSheets.Location = new System.Drawing.Point(6, 142);
            this.btnScoreSheets.Name = "btnScoreSheets";
            this.btnScoreSheets.Size = new System.Drawing.Size(103, 35);
            this.btnScoreSheets.TabIndex = 6;
            this.btnScoreSheets.Text = "Score Sheets";
            this.btnScoreSheets.UseVisualStyleBackColor = true;
            this.btnScoreSheets.Click += new System.EventHandler(this.btnScoreSheets_Click);
            // 
            // btnPrintRefSheet
            // 
            this.btnPrintRefSheet.Location = new System.Drawing.Point(6, 101);
            this.btnPrintRefSheet.Name = "btnPrintRefSheet";
            this.btnPrintRefSheet.Size = new System.Drawing.Size(103, 35);
            this.btnPrintRefSheet.TabIndex = 5;
            this.btnPrintRefSheet.Text = "Ref Sheets";
            this.btnPrintRefSheet.UseVisualStyleBackColor = true;
            this.btnPrintRefSheet.Click += new System.EventHandler(this.btnPrintRefSheet_Click);
            // 
            // btnPrintBlank
            // 
            this.btnPrintBlank.Location = new System.Drawing.Point(6, 183);
            this.btnPrintBlank.Name = "btnPrintBlank";
            this.btnPrintBlank.Size = new System.Drawing.Size(103, 35);
            this.btnPrintBlank.TabIndex = 4;
            this.btnPrintBlank.Text = "Blank Master";
            this.btnPrintBlank.UseVisualStyleBackColor = true;
            this.btnPrintBlank.Click += new System.EventHandler(this.btnPrintBlank_Click);
            // 
            // btnPrintFinal
            // 
            this.btnPrintFinal.Location = new System.Drawing.Point(6, 224);
            this.btnPrintFinal.Name = "btnPrintFinal";
            this.btnPrintFinal.Size = new System.Drawing.Size(103, 35);
            this.btnPrintFinal.TabIndex = 3;
            this.btnPrintFinal.Text = "Final Scores";
            this.btnPrintFinal.UseVisualStyleBackColor = true;
            this.btnPrintFinal.Click += new System.EventHandler(this.btnPrintFinal_Click);
            // 
            // btnPrintSchedule
            // 
            this.btnPrintSchedule.Location = new System.Drawing.Point(6, 60);
            this.btnPrintSchedule.Name = "btnPrintSchedule";
            this.btnPrintSchedule.Size = new System.Drawing.Size(103, 35);
            this.btnPrintSchedule.TabIndex = 2;
            this.btnPrintSchedule.Text = "Schedule";
            this.btnPrintSchedule.UseVisualStyleBackColor = true;
            this.btnPrintSchedule.Click += new System.EventHandler(this.btnPrintSchedule_Click);
            // 
            // btnPrintAll
            // 
            this.btnPrintAll.Location = new System.Drawing.Point(6, 19);
            this.btnPrintAll.Name = "btnPrintAll";
            this.btnPrintAll.Size = new System.Drawing.Size(103, 35);
            this.btnPrintAll.TabIndex = 1;
            this.btnPrintAll.Text = "All for Current";
            this.btnPrintAll.UseVisualStyleBackColor = true;
            this.btnPrintAll.Click += new System.EventHandler(this.btnPrintAll_Click);
            // 
            // scoringInput1
            // 
            this.scoringInput1.Location = new System.Drawing.Point(12, 12);
            this.scoringInput1.Name = "scoringInput1";
            this.scoringInput1.Size = new System.Drawing.Size(650, 180);
            this.scoringInput1.TabIndex = 5;
            // 
            // btnWebSched
            // 
            this.btnWebSched.Location = new System.Drawing.Point(6, 60);
            this.btnWebSched.Name = "btnWebSched";
            this.btnWebSched.Size = new System.Drawing.Size(103, 35);
            this.btnWebSched.TabIndex = 7;
            this.btnWebSched.Text = "Schedule";
            this.btnWebSched.UseVisualStyleBackColor = true;
            // 
            // btnWebRef
            // 
            this.btnWebRef.Location = new System.Drawing.Point(6, 101);
            this.btnWebRef.Name = "btnWebRef";
            this.btnWebRef.Size = new System.Drawing.Size(103, 35);
            this.btnWebRef.TabIndex = 7;
            this.btnWebRef.Text = "Ref Sheets";
            this.btnWebRef.UseVisualStyleBackColor = true;
            // 
            // btnWebScore
            // 
            this.btnWebScore.Location = new System.Drawing.Point(6, 142);
            this.btnWebScore.Name = "btnWebScore";
            this.btnWebScore.Size = new System.Drawing.Size(103, 35);
            this.btnWebScore.TabIndex = 7;
            this.btnWebScore.Text = "Score Sheets";
            this.btnWebScore.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 553);
            this.Controls.Add(this.gbPrint);
            this.Controls.Add(this.gbWeb);
            this.Controls.Add(this.btnPrintScore);
            this.Controls.Add(this.btnCurrentSched);
            this.Controls.Add(this.pnlScoreIn);
            this.Controls.Add(this.btnNotebook);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.scoringInput1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudRound)).EndInit();
            this.pnlScoreIn.ResumeLayout(false);
            this.gbWeb.ResumeLayout(false);
            this.gbPrint.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ScoringInput scoringInput1;
        private System.Windows.Forms.Button btnPrelim;
        private System.Windows.Forms.Button btnWildcard;
        private System.Windows.Forms.Button btnSemi;
        private System.Windows.Forms.Button btnFinals;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnNotebook;
        private System.Windows.Forms.NumericUpDown nudRound;
        private System.Windows.Forms.Panel pnlScoreIn;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnCurrentSched;
        private System.Windows.Forms.Button btnPrintScore;
        private System.Windows.Forms.GroupBox gbWeb;
        private System.Windows.Forms.Button btnUpdateWeb;
        private System.Windows.Forms.GroupBox gbPrint;
        private System.Windows.Forms.Button btnPrintSchedule;
        private System.Windows.Forms.Button btnPrintAll;
        private System.Windows.Forms.Button btnPrintFinal;
        private System.Windows.Forms.Button btnPrintBlank;
        private System.Windows.Forms.Button btnPrintRefSheet;
        private System.Windows.Forms.Button btnScoreSheets;
        private System.Windows.Forms.Button btnWebScore;
        private System.Windows.Forms.Button btnWebRef;
        private System.Windows.Forms.Button btnWebSched;
    }
}

