namespace CrashEdit
{
    partial class SceneryColorListBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.fraColor = new System.Windows.Forms.GroupBox();
            this.lblColorIndex = new System.Windows.Forms.Label();
            this.cmdNextColor = new System.Windows.Forms.Button();
            this.cmdPreviousColor = new System.Windows.Forms.Button();
            this.cmdInsertColor = new System.Windows.Forms.Button();
            this.lblBlue = new System.Windows.Forms.Label();
            this.cmdRemoveColor = new System.Windows.Forms.Button();
            this.lblGreen = new System.Windows.Forms.Label();
            this.cmdAppendColor = new System.Windows.Forms.Button();
            this.lblRed = new System.Windows.Forms.Label();
            this.numBlue = new System.Windows.Forms.NumericUpDown();
            this.numGreen = new System.Windows.Forms.NumericUpDown();
            this.numRed = new System.Windows.Forms.NumericUpDown();
            this.tbcTabs = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.fraColor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBlue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRed)).BeginInit();
            this.tbcTabs.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.SuspendLayout();
            // 
            // fraColor
            // 
            this.fraColor.Controls.Add(this.lblColorIndex);
            this.fraColor.Controls.Add(this.cmdNextColor);
            this.fraColor.Controls.Add(this.cmdPreviousColor);
            this.fraColor.Controls.Add(this.cmdInsertColor);
            this.fraColor.Controls.Add(this.lblBlue);
            this.fraColor.Controls.Add(this.cmdRemoveColor);
            this.fraColor.Controls.Add(this.lblGreen);
            this.fraColor.Controls.Add(this.cmdAppendColor);
            this.fraColor.Controls.Add(this.lblRed);
            this.fraColor.Controls.Add(this.numBlue);
            this.fraColor.Controls.Add(this.numGreen);
            this.fraColor.Controls.Add(this.numRed);
            this.fraColor.Location = new System.Drawing.Point(3, 3);
            this.fraColor.Name = "fraColor";
            this.fraColor.Size = new System.Drawing.Size(384, 135);
            this.fraColor.TabIndex = 1;
            this.fraColor.TabStop = false;
            this.fraColor.Text = "Color(s)";
            // 
            // lblColorIndex
            // 
            this.lblColorIndex.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColorIndex.Location = new System.Drawing.Point(6, 19);
            this.lblColorIndex.Name = "lblColorIndex";
            this.lblColorIndex.Size = new System.Drawing.Size(60, 23);
            this.lblColorIndex.TabIndex = 5;
            this.lblColorIndex.Text = "?? / ??";
            this.lblColorIndex.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmdNextColor
            // 
            this.cmdNextColor.Location = new System.Drawing.Point(136, 19);
            this.cmdNextColor.Name = "cmdNextColor";
            this.cmdNextColor.Size = new System.Drawing.Size(58, 23);
            this.cmdNextColor.TabIndex = 1;
            this.cmdNextColor.Text = "Next";
            this.cmdNextColor.UseVisualStyleBackColor = true;
            this.cmdNextColor.Click += new System.EventHandler(this.cmdNextColor_Click);
            // 
            // cmdPreviousColor
            // 
            this.cmdPreviousColor.Location = new System.Drawing.Point(72, 19);
            this.cmdPreviousColor.Name = "cmdPreviousColor";
            this.cmdPreviousColor.Size = new System.Drawing.Size(58, 23);
            this.cmdPreviousColor.TabIndex = 0;
            this.cmdPreviousColor.Text = "Previous";
            this.cmdPreviousColor.UseVisualStyleBackColor = true;
            this.cmdPreviousColor.Click += new System.EventHandler(this.cmdPreviousColor_Click);
            // 
            // cmdInsertColor
            // 
            this.cmdInsertColor.Location = new System.Drawing.Point(137, 75);
            this.cmdInsertColor.Name = "cmdInsertColor";
            this.cmdInsertColor.Size = new System.Drawing.Size(75, 23);
            this.cmdInsertColor.TabIndex = 6;
            this.cmdInsertColor.Text = "Insert";
            this.cmdInsertColor.UseVisualStyleBackColor = true;
            this.cmdInsertColor.Click += new System.EventHandler(this.cmdInsertColor_Click);
            // 
            // lblBlue
            // 
            this.lblBlue.AutoSize = true;
            this.lblBlue.Location = new System.Drawing.Point(6, 106);
            this.lblBlue.Name = "lblBlue";
            this.lblBlue.Size = new System.Drawing.Size(28, 13);
            this.lblBlue.TabIndex = 5;
            this.lblBlue.Text = "Blue";
            // 
            // cmdRemoveColor
            // 
            this.cmdRemoveColor.Location = new System.Drawing.Point(137, 101);
            this.cmdRemoveColor.Name = "cmdRemoveColor";
            this.cmdRemoveColor.Size = new System.Drawing.Size(75, 23);
            this.cmdRemoveColor.TabIndex = 7;
            this.cmdRemoveColor.Text = "Remove";
            this.cmdRemoveColor.UseVisualStyleBackColor = true;
            this.cmdRemoveColor.Click += new System.EventHandler(this.cmdRemoveColor_Click);
            // 
            // lblGreen
            // 
            this.lblGreen.AutoSize = true;
            this.lblGreen.Location = new System.Drawing.Point(6, 80);
            this.lblGreen.Name = "lblGreen";
            this.lblGreen.Size = new System.Drawing.Size(36, 13);
            this.lblGreen.TabIndex = 4;
            this.lblGreen.Text = "Green";
            // 
            // cmdAppendColor
            // 
            this.cmdAppendColor.Location = new System.Drawing.Point(137, 49);
            this.cmdAppendColor.Name = "cmdAppendColor";
            this.cmdAppendColor.Size = new System.Drawing.Size(75, 23);
            this.cmdAppendColor.TabIndex = 5;
            this.cmdAppendColor.Text = "Append";
            this.cmdAppendColor.UseVisualStyleBackColor = true;
            this.cmdAppendColor.Click += new System.EventHandler(this.cmdAppendColor_Click);
            // 
            // lblRed
            // 
            this.lblRed.AutoSize = true;
            this.lblRed.Location = new System.Drawing.Point(6, 54);
            this.lblRed.Name = "lblRed";
            this.lblRed.Size = new System.Drawing.Size(27, 13);
            this.lblRed.TabIndex = 3;
            this.lblRed.Text = "Red";
            // 
            // numBlue
            // 
            this.numBlue.Location = new System.Drawing.Point(44, 104);
            this.numBlue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numBlue.Name = "numBlue";
            this.numBlue.Size = new System.Drawing.Size(86, 20);
            this.numBlue.TabIndex = 4;
            this.numBlue.ValueChanged += new System.EventHandler(this.numBlue_ValueChanged);
            // 
            // numGreen
            // 
            this.numGreen.Location = new System.Drawing.Point(44, 78);
            this.numGreen.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numGreen.Name = "numGreen";
            this.numGreen.Size = new System.Drawing.Size(86, 20);
            this.numGreen.TabIndex = 3;
            this.numGreen.ValueChanged += new System.EventHandler(this.numGreen_ValueChanged);
            // 
            // numRed
            // 
            this.numRed.Location = new System.Drawing.Point(44, 52);
            this.numRed.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numRed.Name = "numRed";
            this.numRed.Size = new System.Drawing.Size(86, 20);
            this.numRed.TabIndex = 2;
            this.numRed.ValueChanged += new System.EventHandler(this.numRed_ValueChanged);
            // 
            // tbcTabs
            // 
            this.tbcTabs.Controls.Add(this.tabGeneral);
            this.tbcTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcTabs.Location = new System.Drawing.Point(0, 0);
            this.tbcTabs.Name = "tbcTabs";
            this.tbcTabs.SelectedIndex = 0;
            this.tbcTabs.Size = new System.Drawing.Size(398, 454);
            this.tbcTabs.TabIndex = 7;
            // 
            // tabGeneral
            // 
            this.tabGeneral.AutoScroll = true;
            this.tabGeneral.Controls.Add(this.fraColor);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Size = new System.Drawing.Size(390, 428);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // SceneryColorListBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbcTabs);
            this.Name = "SceneryColorListBox";
            this.Size = new System.Drawing.Size(398, 454);
            this.fraColor.ResumeLayout(false);
            this.fraColor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBlue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRed)).EndInit();
            this.tbcTabs.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox fraColor;
        private System.Windows.Forms.Label lblBlue;
        private System.Windows.Forms.Label lblGreen;
        private System.Windows.Forms.Label lblRed;
        private System.Windows.Forms.NumericUpDown numBlue;
        private System.Windows.Forms.NumericUpDown numGreen;
        private System.Windows.Forms.NumericUpDown numRed;
        private System.Windows.Forms.Button cmdInsertColor;
        private System.Windows.Forms.Button cmdRemoveColor;
        private System.Windows.Forms.Button cmdAppendColor;
        private System.Windows.Forms.Button cmdNextColor;
        private System.Windows.Forms.Button cmdPreviousColor;
        private System.Windows.Forms.Label lblColorIndex;
        private System.Windows.Forms.TabControl tbcTabs;
        private System.Windows.Forms.TabPage tabGeneral;
    }
}
