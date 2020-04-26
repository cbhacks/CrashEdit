namespace CrashEdit
{
    partial class ConfigEditor
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
            this.dpdLang = new System.Windows.Forms.ComboBox();
            this.fraLang = new System.Windows.Forms.GroupBox();
            this.cmdReset = new System.Windows.Forms.Button();
            this.fraSize = new System.Windows.Forms.GroupBox();
            this.lblH = new System.Windows.Forms.Label();
            this.lblW = new System.Windows.Forms.Label();
            this.numH = new System.Windows.Forms.NumericUpDown();
            this.numW = new System.Windows.Forms.NumericUpDown();
            this.chkNormalDisplay = new System.Windows.Forms.CheckBox();
            this.chkCollisionDisplay = new System.Windows.Forms.CheckBox();
            this.chkUseAnimLinks = new System.Windows.Forms.CheckBox();
            this.cdlClearCol = new System.Windows.Forms.ColorDialog();
            this.fraClearCol = new System.Windows.Forms.GroupBox();
            this.picClearCol = new System.Windows.Forms.PictureBox();
            this.fraLang.SuspendLayout();
            this.fraSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numW)).BeginInit();
            this.fraClearCol.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picClearCol)).BeginInit();
            this.SuspendLayout();
            // 
            // dpdLang
            // 
            this.dpdLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dpdLang.FormattingEnabled = true;
            this.dpdLang.Location = new System.Drawing.Point(6, 19);
            this.dpdLang.Name = "dpdLang";
            this.dpdLang.Size = new System.Drawing.Size(175, 21);
            this.dpdLang.TabIndex = 0;
            // 
            // fraLang
            // 
            this.fraLang.Controls.Add(this.dpdLang);
            this.fraLang.Location = new System.Drawing.Point(3, 3);
            this.fraLang.Name = "fraLang";
            this.fraLang.Size = new System.Drawing.Size(187, 49);
            this.fraLang.TabIndex = 1;
            this.fraLang.TabStop = false;
            this.fraLang.Text = "Language (requires restart)";
            // 
            // cmdReset
            // 
            this.cmdReset.Location = new System.Drawing.Point(3, 224);
            this.cmdReset.Name = "cmdReset";
            this.cmdReset.Size = new System.Drawing.Size(100, 23);
            this.cmdReset.TabIndex = 1;
            this.cmdReset.Text = "Reset Settings";
            this.cmdReset.UseVisualStyleBackColor = true;
            this.cmdReset.Click += new System.EventHandler(this.cmdReset_Click);
            // 
            // fraSize
            // 
            this.fraSize.Controls.Add(this.lblH);
            this.fraSize.Controls.Add(this.lblW);
            this.fraSize.Controls.Add(this.numH);
            this.fraSize.Controls.Add(this.numW);
            this.fraSize.Location = new System.Drawing.Point(3, 58);
            this.fraSize.Name = "fraSize";
            this.fraSize.Size = new System.Drawing.Size(131, 74);
            this.fraSize.TabIndex = 1;
            this.fraSize.TabStop = false;
            this.fraSize.Text = "Default Window Size";
            // 
            // lblH
            // 
            this.lblH.AutoSize = true;
            this.lblH.Location = new System.Drawing.Point(6, 47);
            this.lblH.Name = "lblH";
            this.lblH.Size = new System.Drawing.Size(38, 13);
            this.lblH.TabIndex = 3;
            this.lblH.Text = "Height";
            // 
            // lblW
            // 
            this.lblW.AutoSize = true;
            this.lblW.Location = new System.Drawing.Point(6, 21);
            this.lblW.Name = "lblW";
            this.lblW.Size = new System.Drawing.Size(35, 13);
            this.lblW.TabIndex = 2;
            this.lblW.Text = "Width";
            // 
            // numH
            // 
            this.numH.Location = new System.Drawing.Point(50, 45);
            this.numH.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.numH.Minimum = new decimal(new int[] {
            480,
            0,
            0,
            0});
            this.numH.Name = "numH";
            this.numH.Size = new System.Drawing.Size(75, 20);
            this.numH.TabIndex = 1;
            this.numH.Value = new decimal(new int[] {
            480,
            0,
            0,
            0});
            this.numH.ValueChanged += new System.EventHandler(this.numH_ValueChanged);
            // 
            // numW
            // 
            this.numW.Location = new System.Drawing.Point(50, 19);
            this.numW.Maximum = new decimal(new int[] {
            8192,
            0,
            0,
            0});
            this.numW.Minimum = new decimal(new int[] {
            640,
            0,
            0,
            0});
            this.numW.Name = "numW";
            this.numW.Size = new System.Drawing.Size(75, 20);
            this.numW.TabIndex = 0;
            this.numW.Value = new decimal(new int[] {
            640,
            0,
            0,
            0});
            this.numW.ValueChanged += new System.EventHandler(this.numW_ValueChanged);
            // 
            // chkNormalDisplay
            // 
            this.chkNormalDisplay.AutoSize = true;
            this.chkNormalDisplay.Checked = true;
            this.chkNormalDisplay.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNormalDisplay.Location = new System.Drawing.Point(3, 138);
            this.chkNormalDisplay.Name = "chkNormalDisplay";
            this.chkNormalDisplay.Size = new System.Drawing.Size(99, 17);
            this.chkNormalDisplay.TabIndex = 0;
            this.chkNormalDisplay.Text = "Display normals";
            this.chkNormalDisplay.UseVisualStyleBackColor = true;
            this.chkNormalDisplay.CheckedChanged += new System.EventHandler(this.chkNormalDisplay_CheckedChanged);
            // 
            // chkCollisionDisplay
            // 
            this.chkCollisionDisplay.AutoSize = true;
            this.chkCollisionDisplay.Checked = true;
            this.chkCollisionDisplay.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCollisionDisplay.Location = new System.Drawing.Point(3, 161);
            this.chkCollisionDisplay.Name = "chkCollisionDisplay";
            this.chkCollisionDisplay.Size = new System.Drawing.Size(178, 17);
            this.chkCollisionDisplay.TabIndex = 2;
            this.chkCollisionDisplay.Text = "Display frame collision by default";
            this.chkCollisionDisplay.UseVisualStyleBackColor = true;
            this.chkCollisionDisplay.CheckedChanged += new System.EventHandler(this.chkCollisionDisplay_CheckedChanged);
            // 
            // chkUseAnimLinks
            // 
            this.chkUseAnimLinks.AutoSize = true;
            this.chkUseAnimLinks.Checked = true;
            this.chkUseAnimLinks.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseAnimLinks.Location = new System.Drawing.Point(3, 184);
            this.chkUseAnimLinks.Name = "chkUseAnimLinks";
            this.chkUseAnimLinks.Size = new System.Drawing.Size(231, 17);
            this.chkUseAnimLinks.TabIndex = 3;
            this.chkUseAnimLinks.Text = "(Crash 3) Used saved animation-model links";
            this.chkUseAnimLinks.UseVisualStyleBackColor = true;
            this.chkUseAnimLinks.CheckedChanged += new System.EventHandler(this.chkUseAnimLinks_CheckedChanged);
            // 
            // cdlClearCol
            // 
            this.cdlClearCol.AnyColor = true;
            this.cdlClearCol.FullOpen = true;
            this.cdlClearCol.SolidColorOnly = true;
            // 
            // fraClearCol
            // 
            this.fraClearCol.Controls.Add(this.picClearCol);
            this.fraClearCol.Location = new System.Drawing.Point(140, 58);
            this.fraClearCol.Name = "fraClearCol";
            this.fraClearCol.Size = new System.Drawing.Size(72, 74);
            this.fraClearCol.TabIndex = 4;
            this.fraClearCol.TabStop = false;
            this.fraClearCol.Text = "Clear Color";
            // 
            // picClearCol
            // 
            this.picClearCol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picClearCol.Location = new System.Drawing.Point(6, 19);
            this.picClearCol.Name = "picClearCol";
            this.picClearCol.Size = new System.Drawing.Size(60, 49);
            this.picClearCol.TabIndex = 0;
            this.picClearCol.TabStop = false;
            this.picClearCol.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // ConfigEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.fraClearCol);
            this.Controls.Add(this.chkUseAnimLinks);
            this.Controls.Add(this.chkCollisionDisplay);
            this.Controls.Add(this.chkNormalDisplay);
            this.Controls.Add(this.fraSize);
            this.Controls.Add(this.cmdReset);
            this.Controls.Add(this.fraLang);
            this.Name = "ConfigEditor";
            this.Size = new System.Drawing.Size(433, 250);
            this.fraLang.ResumeLayout(false);
            this.fraSize.ResumeLayout(false);
            this.fraSize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numW)).EndInit();
            this.fraClearCol.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picClearCol)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox dpdLang;
        private System.Windows.Forms.GroupBox fraLang;
        private System.Windows.Forms.Button cmdReset;
        private System.Windows.Forms.GroupBox fraSize;
        private System.Windows.Forms.Label lblH;
        private System.Windows.Forms.Label lblW;
        private System.Windows.Forms.NumericUpDown numH;
        private System.Windows.Forms.NumericUpDown numW;
        private System.Windows.Forms.CheckBox chkNormalDisplay;
        private System.Windows.Forms.CheckBox chkCollisionDisplay;
        private System.Windows.Forms.CheckBox chkUseAnimLinks;
        private System.Windows.Forms.ColorDialog cdlClearCol;
        private System.Windows.Forms.GroupBox fraClearCol;
        private System.Windows.Forms.PictureBox picClearCol;
    }
}
