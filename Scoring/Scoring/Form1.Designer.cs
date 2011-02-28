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
            this.scoringInput1 = new Scoring.ScoringInput();
            this.nudRound = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRound)).BeginInit();
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
            // 
            // btnWildcard
            // 
            this.btnWildcard.Location = new System.Drawing.Point(6, 59);
            this.btnWildcard.Name = "btnWildcard";
            this.btnWildcard.Size = new System.Drawing.Size(103, 34);
            this.btnWildcard.TabIndex = 7;
            this.btnWildcard.Text = "Generate Wildcard";
            this.btnWildcard.UseVisualStyleBackColor = true;
            // 
            // btnSemi
            // 
            this.btnSemi.Location = new System.Drawing.Point(6, 99);
            this.btnSemi.Name = "btnSemi";
            this.btnSemi.Size = new System.Drawing.Size(103, 34);
            this.btnSemi.TabIndex = 8;
            this.btnSemi.Text = "Generate Semi-Finals";
            this.btnSemi.UseVisualStyleBackColor = true;
            // 
            // btnFinals
            // 
            this.btnFinals.Location = new System.Drawing.Point(6, 139);
            this.btnFinals.Name = "btnFinals";
            this.btnFinals.Size = new System.Drawing.Size(103, 34);
            this.btnFinals.TabIndex = 9;
            this.btnFinals.Text = "Generate Finals";
            this.btnFinals.UseVisualStyleBackColor = true;
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
            // 
            // scoringInput1
            // 
            this.scoringInput1.Location = new System.Drawing.Point(12, 12);
            this.scoringInput1.Name = "scoringInput1";
            this.scoringInput1.Size = new System.Drawing.Size(650, 180);
            this.scoringInput1.TabIndex = 5;
            this.scoringInput1.Visible = false;
            // 
            // nudRound
            // 
            this.nudRound.Location = new System.Drawing.Point(304, 206);
            this.nudRound.Name = "nudRound";
            this.nudRound.Size = new System.Drawing.Size(62, 20);
            this.nudRound.TabIndex = 13;
            this.nudRound.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 553);
            this.Controls.Add(this.nudRound);
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
    }
}

