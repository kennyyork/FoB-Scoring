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
            this.gbRounds = new System.Windows.Forms.GroupBox();
            this.btnNotebook = new System.Windows.Forms.Button();
            this.nudRound = new System.Windows.Forms.NumericUpDown();
            this.pnlScoreIn = new System.Windows.Forms.Panel();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.gbWeb = new System.Windows.Forms.GroupBox();
            this.btnWebAllScores = new System.Windows.Forms.Button();
            this.btnWebLastRound = new System.Windows.Forms.Button();
            this.btnWebSchedDisplay = new System.Windows.Forms.Button();
            this.btnWebScore = new System.Windows.Forms.Button();
            this.btnWebRef = new System.Windows.Forms.Button();
            this.btnWebSched = new System.Windows.Forms.Button();
            this.gbPrint = new System.Windows.Forms.GroupBox();
            this.btnScoreSheets = new System.Windows.Forms.Button();
            this.btnPrintRefSheet = new System.Windows.Forms.Button();
            this.btnPrintBlank = new System.Windows.Forms.Button();
            this.btnPrintFinal = new System.Windows.Forms.Button();
            this.btnPrintSchedule = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.scoringInput1 = new Scoring.ScoringInput();
            this.gbRounds.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRound)).BeginInit();
            this.pnlScoreIn.SuspendLayout();
            this.gbWeb.SuspendLayout();
            this.gbPrint.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPrelim
            // 
            this.btnPrelim.Location = new System.Drawing.Point(6, 20);
            this.btnPrelim.Name = "btnPrelim";
            this.btnPrelim.Size = new System.Drawing.Size(103, 34);
            this.btnPrelim.TabIndex = 6;
            this.btnPrelim.Text = "Generate Prelim";
            this.btnPrelim.UseVisualStyleBackColor = true;
            this.btnPrelim.Click += new System.EventHandler(this.btnPrelim_Click);
            // 
            // btnWildcard
            // 
            this.btnWildcard.Location = new System.Drawing.Point(6, 102);
            this.btnWildcard.Name = "btnWildcard";
            this.btnWildcard.Size = new System.Drawing.Size(103, 34);
            this.btnWildcard.TabIndex = 7;
            this.btnWildcard.Text = "Generate Wildcard";
            this.btnWildcard.UseVisualStyleBackColor = true;
            this.btnWildcard.Click += new System.EventHandler(this.btnWildcard_Click);
            // 
            // btnSemi
            // 
            this.btnSemi.Location = new System.Drawing.Point(6, 143);
            this.btnSemi.Name = "btnSemi";
            this.btnSemi.Size = new System.Drawing.Size(103, 34);
            this.btnSemi.TabIndex = 8;
            this.btnSemi.Text = "Generate Semi-Finals";
            this.btnSemi.UseVisualStyleBackColor = true;
            this.btnSemi.Click += new System.EventHandler(this.btnSemi_Click);
            // 
            // btnFinals
            // 
            this.btnFinals.Location = new System.Drawing.Point(6, 184);
            this.btnFinals.Name = "btnFinals";
            this.btnFinals.Size = new System.Drawing.Size(103, 34);
            this.btnFinals.TabIndex = 9;
            this.btnFinals.Text = "Generate Finals";
            this.btnFinals.UseVisualStyleBackColor = true;
            this.btnFinals.Click += new System.EventHandler(this.btnFinals_Click);
            // 
            // gbRounds
            // 
            this.gbRounds.Controls.Add(this.btnPrelim);
            this.gbRounds.Controls.Add(this.btnFinals);
            this.gbRounds.Controls.Add(this.btnWildcard);
            this.gbRounds.Controls.Add(this.btnSemi);
            this.gbRounds.Controls.Add(this.btnNotebook);
            this.gbRounds.Location = new System.Drawing.Point(159, 253);
            this.gbRounds.Name = "gbRounds";
            this.gbRounds.Size = new System.Drawing.Size(115, 267);
            this.gbRounds.TabIndex = 10;
            this.gbRounds.TabStop = false;
            this.gbRounds.Text = "Round Progression";
            // 
            // btnNotebook
            // 
            this.btnNotebook.Location = new System.Drawing.Point(6, 61);
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
            // gbWeb
            // 
            this.gbWeb.Controls.Add(this.btnWebAllScores);
            this.gbWeb.Controls.Add(this.btnWebLastRound);
            this.gbWeb.Controls.Add(this.btnWebSchedDisplay);
            this.gbWeb.Controls.Add(this.btnWebScore);
            this.gbWeb.Controls.Add(this.btnWebRef);
            this.gbWeb.Controls.Add(this.btnWebSched);
            this.gbWeb.Location = new System.Drawing.Point(280, 253);
            this.gbWeb.Name = "gbWeb";
            this.gbWeb.Size = new System.Drawing.Size(115, 267);
            this.gbWeb.TabIndex = 17;
            this.gbWeb.TabStop = false;
            this.gbWeb.Text = "Webpage";
            // 
            // btnWebAllScores
            // 
            this.btnWebAllScores.Location = new System.Drawing.Point(6, 224);
            this.btnWebAllScores.Name = "btnWebAllScores";
            this.btnWebAllScores.Size = new System.Drawing.Size(103, 35);
            this.btnWebAllScores.TabIndex = 10;
            this.btnWebAllScores.Text = "All Scores Display";
            this.btnWebAllScores.UseVisualStyleBackColor = true;
            this.btnWebAllScores.Click += new System.EventHandler(this.btnWebAllScores_Click);
            // 
            // btnWebLastRound
            // 
            this.btnWebLastRound.Location = new System.Drawing.Point(6, 183);
            this.btnWebLastRound.Name = "btnWebLastRound";
            this.btnWebLastRound.Size = new System.Drawing.Size(103, 35);
            this.btnWebLastRound.TabIndex = 9;
            this.btnWebLastRound.Text = "Last Round Display";
            this.btnWebLastRound.UseVisualStyleBackColor = true;
            this.btnWebLastRound.Click += new System.EventHandler(this.btnWebLastRound_Click);
            // 
            // btnWebSchedDisplay
            // 
            this.btnWebSchedDisplay.Location = new System.Drawing.Point(6, 142);
            this.btnWebSchedDisplay.Name = "btnWebSchedDisplay";
            this.btnWebSchedDisplay.Size = new System.Drawing.Size(103, 35);
            this.btnWebSchedDisplay.TabIndex = 8;
            this.btnWebSchedDisplay.Text = "Sched Display";
            this.btnWebSchedDisplay.UseVisualStyleBackColor = true;
            this.btnWebSchedDisplay.Click += new System.EventHandler(this.btnWebSchedDisplay_Click);
            // 
            // btnWebScore
            // 
            this.btnWebScore.Location = new System.Drawing.Point(6, 101);
            this.btnWebScore.Name = "btnWebScore";
            this.btnWebScore.Size = new System.Drawing.Size(103, 35);
            this.btnWebScore.TabIndex = 7;
            this.btnWebScore.Text = "Score Sheets";
            this.btnWebScore.UseVisualStyleBackColor = true;
            this.btnWebScore.Click += new System.EventHandler(this.btnWebScore_Click);
            // 
            // btnWebRef
            // 
            this.btnWebRef.Location = new System.Drawing.Point(6, 60);
            this.btnWebRef.Name = "btnWebRef";
            this.btnWebRef.Size = new System.Drawing.Size(103, 35);
            this.btnWebRef.TabIndex = 7;
            this.btnWebRef.Text = "Ref Sheets";
            this.btnWebRef.UseVisualStyleBackColor = true;
            this.btnWebRef.Click += new System.EventHandler(this.btnWebRef_Click);
            // 
            // btnWebSched
            // 
            this.btnWebSched.Location = new System.Drawing.Point(6, 19);
            this.btnWebSched.Name = "btnWebSched";
            this.btnWebSched.Size = new System.Drawing.Size(103, 35);
            this.btnWebSched.TabIndex = 7;
            this.btnWebSched.Text = "Schedule";
            this.btnWebSched.UseVisualStyleBackColor = true;
            this.btnWebSched.Click += new System.EventHandler(this.btnWebSched_Click);
            // 
            // gbPrint
            // 
            this.gbPrint.Controls.Add(this.btnScoreSheets);
            this.gbPrint.Controls.Add(this.btnPrintRefSheet);
            this.gbPrint.Controls.Add(this.btnPrintBlank);
            this.gbPrint.Controls.Add(this.btnPrintFinal);
            this.gbPrint.Controls.Add(this.btnPrintSchedule);
            this.gbPrint.Location = new System.Drawing.Point(401, 253);
            this.gbPrint.Name = "gbPrint";
            this.gbPrint.Size = new System.Drawing.Size(115, 267);
            this.gbPrint.TabIndex = 18;
            this.gbPrint.TabStop = false;
            this.gbPrint.Text = "Printing";
            // 
            // btnScoreSheets
            // 
            this.btnScoreSheets.Location = new System.Drawing.Point(6, 101);
            this.btnScoreSheets.Name = "btnScoreSheets";
            this.btnScoreSheets.Size = new System.Drawing.Size(103, 35);
            this.btnScoreSheets.TabIndex = 6;
            this.btnScoreSheets.Text = "Score Sheets";
            this.btnScoreSheets.UseVisualStyleBackColor = true;
            this.btnScoreSheets.Click += new System.EventHandler(this.btnScoreSheets_Click);
            // 
            // btnPrintRefSheet
            // 
            this.btnPrintRefSheet.Location = new System.Drawing.Point(6, 60);
            this.btnPrintRefSheet.Name = "btnPrintRefSheet";
            this.btnPrintRefSheet.Size = new System.Drawing.Size(103, 35);
            this.btnPrintRefSheet.TabIndex = 5;
            this.btnPrintRefSheet.Text = "Ref Sheets";
            this.btnPrintRefSheet.UseVisualStyleBackColor = true;
            this.btnPrintRefSheet.Click += new System.EventHandler(this.btnPrintRefSheet_Click);
            // 
            // btnPrintBlank
            // 
            this.btnPrintBlank.Location = new System.Drawing.Point(6, 142);
            this.btnPrintBlank.Name = "btnPrintBlank";
            this.btnPrintBlank.Size = new System.Drawing.Size(103, 35);
            this.btnPrintBlank.TabIndex = 4;
            this.btnPrintBlank.Text = "Blank Master";
            this.btnPrintBlank.UseVisualStyleBackColor = true;
            this.btnPrintBlank.Click += new System.EventHandler(this.btnPrintBlank_Click);
            // 
            // btnPrintFinal
            // 
            this.btnPrintFinal.Location = new System.Drawing.Point(6, 183);
            this.btnPrintFinal.Name = "btnPrintFinal";
            this.btnPrintFinal.Size = new System.Drawing.Size(103, 35);
            this.btnPrintFinal.TabIndex = 3;
            this.btnPrintFinal.Text = "Final Scores";
            this.btnPrintFinal.UseVisualStyleBackColor = true;
            this.btnPrintFinal.Click += new System.EventHandler(this.btnPrintFinal_Click);
            // 
            // btnPrintSchedule
            // 
            this.btnPrintSchedule.Location = new System.Drawing.Point(6, 19);
            this.btnPrintSchedule.Name = "btnPrintSchedule";
            this.btnPrintSchedule.Size = new System.Drawing.Size(103, 35);
            this.btnPrintSchedule.TabIndex = 2;
            this.btnPrintSchedule.Text = "Schedule";
            this.btnPrintSchedule.UseVisualStyleBackColor = true;
            this.btnPrintSchedule.Click += new System.EventHandler(this.btnPrintSchedule_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(12, 234);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(650, 10);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            // 
            // scoringInput1
            // 
            this.scoringInput1.Location = new System.Drawing.Point(12, 12);
            this.scoringInput1.Name = "scoringInput1";
            this.scoringInput1.Size = new System.Drawing.Size(650, 180);
            this.scoringInput1.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 528);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gbPrint);
            this.Controls.Add(this.gbWeb);
            this.Controls.Add(this.pnlScoreIn);
            this.Controls.Add(this.gbRounds);
            this.Controls.Add(this.scoringInput1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gbRounds.ResumeLayout(false);
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
        private System.Windows.Forms.GroupBox gbRounds;
        private System.Windows.Forms.Button btnNotebook;
        private System.Windows.Forms.NumericUpDown nudRound;
        private System.Windows.Forms.Panel pnlScoreIn;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.GroupBox gbWeb;
        private System.Windows.Forms.GroupBox gbPrint;
        private System.Windows.Forms.Button btnPrintSchedule;
        private System.Windows.Forms.Button btnPrintFinal;
        private System.Windows.Forms.Button btnPrintBlank;
        private System.Windows.Forms.Button btnPrintRefSheet;
        private System.Windows.Forms.Button btnScoreSheets;
        private System.Windows.Forms.Button btnWebScore;
        private System.Windows.Forms.Button btnWebRef;
        private System.Windows.Forms.Button btnWebSched;
        private System.Windows.Forms.Button btnWebAllScores;
        private System.Windows.Forms.Button btnWebLastRound;
        private System.Windows.Forms.Button btnWebSchedDisplay;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}

