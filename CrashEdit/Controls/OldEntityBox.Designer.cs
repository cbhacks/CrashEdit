namespace CrashEdit
{
    partial class OldEntityBox
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
            this.components = new System.ComponentModel.Container();
            this.numType = new System.Windows.Forms.NumericUpDown();
            this.fraType = new System.Windows.Forms.GroupBox();
            this.lblCodeName = new System.Windows.Forms.Label();
            this.fraSubtype = new System.Windows.Forms.GroupBox();
            this.numSubtype = new System.Windows.Forms.NumericUpDown();
            this.fraPosition = new System.Windows.Forms.GroupBox();
            this.cmdInterpolate = new System.Windows.Forms.Button();
            this.lblPositionIndex = new System.Windows.Forms.Label();
            this.cmdNextPosition = new System.Windows.Forms.Button();
            this.cmdPreviousPosition = new System.Windows.Forms.Button();
            this.cmdInsertPosition = new System.Windows.Forms.Button();
            this.lblZ = new System.Windows.Forms.Label();
            this.cmdRemovePosition = new System.Windows.Forms.Button();
            this.lblY = new System.Windows.Forms.Label();
            this.cmdAppendPosition = new System.Windows.Forms.Button();
            this.lblX = new System.Windows.Forms.Label();
            this.numZ = new System.Windows.Forms.NumericUpDown();
            this.numY = new System.Windows.Forms.NumericUpDown();
            this.numX = new System.Windows.Forms.NumericUpDown();
            this.fraID = new System.Windows.Forms.GroupBox();
            this.numID = new System.Windows.Forms.NumericUpDown();
            this.tbcTabs = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.fraSettings = new System.Windows.Forms.GroupBox();
            this.chkHexC = new System.Windows.Forms.CheckBox();
            this.chkHexB = new System.Windows.Forms.CheckBox();
            this.chkHexA = new System.Windows.Forms.CheckBox();
            this.chkHexFlags = new System.Windows.Forms.CheckBox();
            this.lblC = new System.Windows.Forms.Label();
            this.numC = new System.Windows.Forms.NumericUpDown();
            this.lblB = new System.Windows.Forms.Label();
            this.lblA = new System.Windows.Forms.Label();
            this.lblFlags = new System.Windows.Forms.Label();
            this.numB = new System.Windows.Forms.NumericUpDown();
            this.numA = new System.Windows.Forms.NumericUpDown();
            this.numFlags = new System.Windows.Forms.NumericUpDown();
            this.numSpawn = new System.Windows.Forms.NumericUpDown();
            this.tipHover = new System.Windows.Forms.ToolTip(this.components);
            this.fraSpawn = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numType)).BeginInit();
            this.fraType.SuspendLayout();
            this.fraSubtype.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSubtype)).BeginInit();
            this.fraPosition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numX)).BeginInit();
            this.fraID.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numID)).BeginInit();
            this.tbcTabs.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.fraSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFlags)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSpawn)).BeginInit();
            this.fraSpawn.SuspendLayout();
            this.SuspendLayout();
            // 
            // numType
            // 
            this.numType.Location = new System.Drawing.Point(6, 22);
            this.numType.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numType.Name = "numType";
            this.numType.Size = new System.Drawing.Size(77, 20);
            this.numType.TabIndex = 1;
            this.numType.ValueChanged += new System.EventHandler(this.numType_ValueChanged);
            // 
            // fraType
            // 
            this.fraType.Controls.Add(this.lblCodeName);
            this.fraType.Controls.Add(this.numType);
            this.fraType.Location = new System.Drawing.Point(209, 3);
            this.fraType.Name = "fraType";
            this.fraType.Size = new System.Drawing.Size(89, 67);
            this.fraType.TabIndex = 4;
            this.fraType.TabStop = false;
            this.fraType.Text = "Type";
            // 
            // lblCodeName
            // 
            this.lblCodeName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodeName.Location = new System.Drawing.Point(2, 45);
            this.lblCodeName.Name = "lblCodeName";
            this.lblCodeName.Size = new System.Drawing.Size(120, 19);
            this.lblCodeName.TabIndex = 9;
            this.lblCodeName.Text = "(Unknown)";
            this.lblCodeName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // fraSubtype
            // 
            this.fraSubtype.Controls.Add(this.numSubtype);
            this.fraSubtype.Location = new System.Drawing.Point(209, 76);
            this.fraSubtype.Name = "fraSubtype";
            this.fraSubtype.Size = new System.Drawing.Size(89, 46);
            this.fraSubtype.TabIndex = 5;
            this.fraSubtype.TabStop = false;
            this.fraSubtype.Text = "Subtype";
            // 
            // numSubtype
            // 
            this.numSubtype.Location = new System.Drawing.Point(6, 20);
            this.numSubtype.Name = "numSubtype";
            this.numSubtype.Size = new System.Drawing.Size(77, 20);
            this.numSubtype.TabIndex = 1;
            this.numSubtype.ValueChanged += new System.EventHandler(this.numSubtype_ValueChanged);
            // 
            // fraPosition
            // 
            this.fraPosition.AutoSize = true;
            this.fraPosition.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fraPosition.Controls.Add(this.cmdInterpolate);
            this.fraPosition.Controls.Add(this.lblPositionIndex);
            this.fraPosition.Controls.Add(this.cmdNextPosition);
            this.fraPosition.Controls.Add(this.cmdPreviousPosition);
            this.fraPosition.Controls.Add(this.cmdInsertPosition);
            this.fraPosition.Controls.Add(this.lblZ);
            this.fraPosition.Controls.Add(this.cmdRemovePosition);
            this.fraPosition.Controls.Add(this.lblY);
            this.fraPosition.Controls.Add(this.cmdAppendPosition);
            this.fraPosition.Controls.Add(this.lblX);
            this.fraPosition.Controls.Add(this.numZ);
            this.fraPosition.Controls.Add(this.numY);
            this.fraPosition.Controls.Add(this.numX);
            this.fraPosition.Location = new System.Drawing.Point(3, 3);
            this.fraPosition.Name = "fraPosition";
            this.fraPosition.Size = new System.Drawing.Size(200, 172);
            this.fraPosition.TabIndex = 1;
            this.fraPosition.TabStop = false;
            this.fraPosition.Text = "Position(s)";
            // 
            // cmdInterpolate
            // 
            this.cmdInterpolate.Location = new System.Drawing.Point(6, 130);
            this.cmdInterpolate.Name = "cmdInterpolate";
            this.cmdInterpolate.Size = new System.Drawing.Size(75, 23);
            this.cmdInterpolate.TabIndex = 8;
            this.cmdInterpolate.Text = "Interpolate";
            this.cmdInterpolate.UseVisualStyleBackColor = true;
            this.cmdInterpolate.Click += new System.EventHandler(this.cmdInterpolate_Click);
            // 
            // lblPositionIndex
            // 
            this.lblPositionIndex.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPositionIndex.Location = new System.Drawing.Point(6, 19);
            this.lblPositionIndex.Name = "lblPositionIndex";
            this.lblPositionIndex.Size = new System.Drawing.Size(60, 23);
            this.lblPositionIndex.TabIndex = 5;
            this.lblPositionIndex.Text = "?? / ??";
            this.lblPositionIndex.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmdNextPosition
            // 
            this.cmdNextPosition.Location = new System.Drawing.Point(136, 19);
            this.cmdNextPosition.Name = "cmdNextPosition";
            this.cmdNextPosition.Size = new System.Drawing.Size(58, 23);
            this.cmdNextPosition.TabIndex = 1;
            this.cmdNextPosition.Text = "Next";
            this.cmdNextPosition.UseVisualStyleBackColor = true;
            this.cmdNextPosition.Click += new System.EventHandler(this.cmdNextPosition_Click);
            // 
            // cmdPreviousPosition
            // 
            this.cmdPreviousPosition.Location = new System.Drawing.Point(72, 19);
            this.cmdPreviousPosition.Name = "cmdPreviousPosition";
            this.cmdPreviousPosition.Size = new System.Drawing.Size(58, 23);
            this.cmdPreviousPosition.TabIndex = 0;
            this.cmdPreviousPosition.Text = "Previous";
            this.cmdPreviousPosition.UseVisualStyleBackColor = true;
            this.cmdPreviousPosition.Click += new System.EventHandler(this.cmdPreviousPosition_Click);
            // 
            // cmdInsertPosition
            // 
            this.cmdInsertPosition.Location = new System.Drawing.Point(119, 75);
            this.cmdInsertPosition.Name = "cmdInsertPosition";
            this.cmdInsertPosition.Size = new System.Drawing.Size(75, 23);
            this.cmdInsertPosition.TabIndex = 6;
            this.cmdInsertPosition.Text = "Insert";
            this.cmdInsertPosition.UseVisualStyleBackColor = true;
            this.cmdInsertPosition.Click += new System.EventHandler(this.cmdInsertPosition_Click);
            // 
            // lblZ
            // 
            this.lblZ.AutoSize = true;
            this.lblZ.Location = new System.Drawing.Point(6, 106);
            this.lblZ.Name = "lblZ";
            this.lblZ.Size = new System.Drawing.Size(14, 13);
            this.lblZ.TabIndex = 5;
            this.lblZ.Text = "Z";
            // 
            // cmdRemovePosition
            // 
            this.cmdRemovePosition.Location = new System.Drawing.Point(119, 101);
            this.cmdRemovePosition.Name = "cmdRemovePosition";
            this.cmdRemovePosition.Size = new System.Drawing.Size(75, 23);
            this.cmdRemovePosition.TabIndex = 7;
            this.cmdRemovePosition.Text = "Remove";
            this.cmdRemovePosition.UseVisualStyleBackColor = true;
            this.cmdRemovePosition.Click += new System.EventHandler(this.cmdRemovePosition_Click);
            // 
            // lblY
            // 
            this.lblY.AutoSize = true;
            this.lblY.Location = new System.Drawing.Point(6, 80);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(14, 13);
            this.lblY.TabIndex = 4;
            this.lblY.Text = "Y";
            // 
            // cmdAppendPosition
            // 
            this.cmdAppendPosition.Location = new System.Drawing.Point(119, 49);
            this.cmdAppendPosition.Name = "cmdAppendPosition";
            this.cmdAppendPosition.Size = new System.Drawing.Size(75, 23);
            this.cmdAppendPosition.TabIndex = 5;
            this.cmdAppendPosition.Text = "Append";
            this.cmdAppendPosition.UseVisualStyleBackColor = true;
            this.cmdAppendPosition.Click += new System.EventHandler(this.cmdAppendPosition_Click);
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.Location = new System.Drawing.Point(6, 54);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(14, 13);
            this.lblX.TabIndex = 3;
            this.lblX.Text = "X";
            // 
            // numZ
            // 
            this.numZ.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numZ.Location = new System.Drawing.Point(26, 104);
            this.numZ.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.numZ.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.numZ.Name = "numZ";
            this.numZ.Size = new System.Drawing.Size(86, 20);
            this.numZ.TabIndex = 4;
            this.numZ.ValueChanged += new System.EventHandler(this.numZ_ValueChanged);
            // 
            // numY
            // 
            this.numY.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numY.Location = new System.Drawing.Point(26, 78);
            this.numY.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.numY.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.numY.Name = "numY";
            this.numY.Size = new System.Drawing.Size(86, 20);
            this.numY.TabIndex = 3;
            this.numY.ValueChanged += new System.EventHandler(this.numY_ValueChanged);
            // 
            // numX
            // 
            this.numX.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numX.Location = new System.Drawing.Point(26, 52);
            this.numX.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.numX.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.numX.Name = "numX";
            this.numX.Size = new System.Drawing.Size(86, 20);
            this.numX.TabIndex = 2;
            this.numX.ValueChanged += new System.EventHandler(this.numX_ValueChanged);
            // 
            // fraID
            // 
            this.fraID.Controls.Add(this.numID);
            this.fraID.Location = new System.Drawing.Point(209, 128);
            this.fraID.Name = "fraID";
            this.fraID.Size = new System.Drawing.Size(89, 49);
            this.fraID.TabIndex = 3;
            this.fraID.TabStop = false;
            this.fraID.Text = "ID";
            // 
            // numID
            // 
            this.numID.Location = new System.Drawing.Point(6, 19);
            this.numID.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numID.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.numID.Name = "numID";
            this.numID.Size = new System.Drawing.Size(77, 20);
            this.numID.TabIndex = 1;
            this.numID.ValueChanged += new System.EventHandler(this.numID_ValueChanged);
            // 
            // tbcTabs
            // 
            this.tbcTabs.Controls.Add(this.tabGeneral);
            this.tbcTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcTabs.Location = new System.Drawing.Point(0, 0);
            this.tbcTabs.Name = "tbcTabs";
            this.tbcTabs.SelectedIndex = 0;
            this.tbcTabs.Size = new System.Drawing.Size(349, 370);
            this.tbcTabs.TabIndex = 7;
            // 
            // tabGeneral
            // 
            this.tabGeneral.AutoScroll = true;
            this.tabGeneral.Controls.Add(this.fraSpawn);
            this.tabGeneral.Controls.Add(this.fraSettings);
            this.tabGeneral.Controls.Add(this.fraType);
            this.tabGeneral.Controls.Add(this.fraSubtype);
            this.tabGeneral.Controls.Add(this.fraPosition);
            this.tabGeneral.Controls.Add(this.fraID);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Size = new System.Drawing.Size(341, 344);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // fraSettings
            // 
            this.fraSettings.Controls.Add(this.chkHexC);
            this.fraSettings.Controls.Add(this.chkHexB);
            this.fraSettings.Controls.Add(this.chkHexA);
            this.fraSettings.Controls.Add(this.chkHexFlags);
            this.fraSettings.Controls.Add(this.lblC);
            this.fraSettings.Controls.Add(this.numC);
            this.fraSettings.Controls.Add(this.lblB);
            this.fraSettings.Controls.Add(this.lblA);
            this.fraSettings.Controls.Add(this.lblFlags);
            this.fraSettings.Controls.Add(this.numB);
            this.fraSettings.Controls.Add(this.numA);
            this.fraSettings.Controls.Add(this.numFlags);
            this.fraSettings.Location = new System.Drawing.Point(3, 181);
            this.fraSettings.Name = "fraSettings";
            this.fraSettings.Size = new System.Drawing.Size(200, 133);
            this.fraSettings.TabIndex = 8;
            this.fraSettings.TabStop = false;
            this.fraSettings.Text = "Settings";
            // 
            // chkHexC
            // 
            this.chkHexC.AutoSize = true;
            this.chkHexC.Location = new System.Drawing.Point(154, 98);
            this.chkHexC.Name = "chkHexC";
            this.chkHexC.Size = new System.Drawing.Size(45, 17);
            this.chkHexC.TabIndex = 11;
            this.chkHexC.Text = "Hex";
            this.chkHexC.UseVisualStyleBackColor = true;
            this.chkHexC.CheckedChanged += new System.EventHandler(this.chkHexC_CheckedChanged);
            // 
            // chkHexB
            // 
            this.chkHexB.AutoSize = true;
            this.chkHexB.Location = new System.Drawing.Point(154, 72);
            this.chkHexB.Name = "chkHexB";
            this.chkHexB.Size = new System.Drawing.Size(45, 17);
            this.chkHexB.TabIndex = 10;
            this.chkHexB.Text = "Hex";
            this.chkHexB.UseVisualStyleBackColor = true;
            this.chkHexB.CheckedChanged += new System.EventHandler(this.chkHexB_CheckedChanged);
            // 
            // chkHexA
            // 
            this.chkHexA.AutoSize = true;
            this.chkHexA.Location = new System.Drawing.Point(154, 46);
            this.chkHexA.Name = "chkHexA";
            this.chkHexA.Size = new System.Drawing.Size(45, 17);
            this.chkHexA.TabIndex = 9;
            this.chkHexA.Text = "Hex";
            this.chkHexA.UseVisualStyleBackColor = true;
            this.chkHexA.CheckedChanged += new System.EventHandler(this.chkHexA_CheckedChanged);
            // 
            // chkHexFlags
            // 
            this.chkHexFlags.AutoSize = true;
            this.chkHexFlags.Checked = true;
            this.chkHexFlags.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHexFlags.Location = new System.Drawing.Point(154, 20);
            this.chkHexFlags.Name = "chkHexFlags";
            this.chkHexFlags.Size = new System.Drawing.Size(45, 17);
            this.chkHexFlags.TabIndex = 8;
            this.chkHexFlags.Text = "Hex";
            this.chkHexFlags.UseVisualStyleBackColor = true;
            this.chkHexFlags.CheckedChanged += new System.EventHandler(this.chkHexUnknown_CheckedChanged);
            // 
            // lblC
            // 
            this.lblC.AutoSize = true;
            this.lblC.Location = new System.Drawing.Point(6, 99);
            this.lblC.Name = "lblC";
            this.lblC.Size = new System.Drawing.Size(48, 13);
            this.lblC.TabIndex = 7;
            this.lblC.Text = "Vector Z";
            // 
            // numC
            // 
            this.numC.Location = new System.Drawing.Point(62, 97);
            this.numC.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.numC.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.numC.Name = "numC";
            this.numC.Size = new System.Drawing.Size(86, 20);
            this.numC.TabIndex = 6;
            this.numC.ValueChanged += new System.EventHandler(this.numC_ValueChanged);
            // 
            // lblB
            // 
            this.lblB.AutoSize = true;
            this.lblB.Location = new System.Drawing.Point(6, 73);
            this.lblB.Name = "lblB";
            this.lblB.Size = new System.Drawing.Size(48, 13);
            this.lblB.TabIndex = 5;
            this.lblB.Text = "Vector Y";
            // 
            // lblA
            // 
            this.lblA.AutoSize = true;
            this.lblA.Location = new System.Drawing.Point(6, 47);
            this.lblA.Name = "lblA";
            this.lblA.Size = new System.Drawing.Size(48, 13);
            this.lblA.TabIndex = 4;
            this.lblA.Text = "Vector X";
            // 
            // lblFlags
            // 
            this.lblFlags.AutoSize = true;
            this.lblFlags.Location = new System.Drawing.Point(6, 21);
            this.lblFlags.Name = "lblFlags";
            this.lblFlags.Size = new System.Drawing.Size(32, 13);
            this.lblFlags.TabIndex = 3;
            this.lblFlags.Text = "Flags";
            // 
            // numB
            // 
            this.numB.Location = new System.Drawing.Point(62, 71);
            this.numB.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.numB.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.numB.Name = "numB";
            this.numB.Size = new System.Drawing.Size(86, 20);
            this.numB.TabIndex = 4;
            this.numB.ValueChanged += new System.EventHandler(this.numB_ValueChanged);
            // 
            // numA
            // 
            this.numA.Location = new System.Drawing.Point(62, 45);
            this.numA.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.numA.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.numA.Name = "numA";
            this.numA.Size = new System.Drawing.Size(86, 20);
            this.numA.TabIndex = 3;
            this.numA.ValueChanged += new System.EventHandler(this.numA_ValueChanged);
            // 
            // numFlags
            // 
            this.numFlags.Hexadecimal = true;
            this.numFlags.Location = new System.Drawing.Point(62, 19);
            this.numFlags.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numFlags.Name = "numFlags";
            this.numFlags.Size = new System.Drawing.Size(86, 20);
            this.numFlags.TabIndex = 2;
            this.numFlags.ValueChanged += new System.EventHandler(this.numUnknown_ValueChanged);
            // 
            // numSpawn
            // 
            this.numSpawn.Location = new System.Drawing.Point(6, 19);
            this.numSpawn.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numSpawn.Name = "numSpawn";
            this.numSpawn.Size = new System.Drawing.Size(77, 20);
            this.numSpawn.TabIndex = 12;
            this.numSpawn.ValueChanged += new System.EventHandler(this.numSpawn_ValueChanged);
            // 
            // tipHover
            // 
            this.tipHover.AutomaticDelay = 250;
            // 
            // fraSpawn
            // 
            this.fraSpawn.Controls.Add(this.numSpawn);
            this.fraSpawn.Location = new System.Drawing.Point(209, 183);
            this.fraSpawn.Name = "fraSpawn";
            this.fraSpawn.Size = new System.Drawing.Size(89, 49);
            this.fraSpawn.TabIndex = 4;
            this.fraSpawn.TabStop = false;
            this.fraSpawn.Text = "Spawn [?]";
            this.tipHover.SetToolTip(this.fraSpawn, "Must be set to 3, or entity will not spawn!");
            // 
            // OldEntityBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbcTabs);
            this.Name = "OldEntityBox";
            this.Size = new System.Drawing.Size(349, 370);
            ((System.ComponentModel.ISupportInitialize)(this.numType)).EndInit();
            this.fraType.ResumeLayout(false);
            this.fraSubtype.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numSubtype)).EndInit();
            this.fraPosition.ResumeLayout(false);
            this.fraPosition.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numX)).EndInit();
            this.fraID.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numID)).EndInit();
            this.tbcTabs.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.fraSettings.ResumeLayout(false);
            this.fraSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFlags)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSpawn)).EndInit();
            this.fraSpawn.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numType;
        private System.Windows.Forms.GroupBox fraType;
        private System.Windows.Forms.GroupBox fraSubtype;
        private System.Windows.Forms.NumericUpDown numSubtype;
        private System.Windows.Forms.GroupBox fraPosition;
        private System.Windows.Forms.Label lblZ;
        private System.Windows.Forms.Label lblY;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.NumericUpDown numZ;
        private System.Windows.Forms.NumericUpDown numY;
        private System.Windows.Forms.NumericUpDown numX;
        private System.Windows.Forms.Button cmdInsertPosition;
        private System.Windows.Forms.Button cmdRemovePosition;
        private System.Windows.Forms.Button cmdAppendPosition;
        private System.Windows.Forms.Button cmdNextPosition;
        private System.Windows.Forms.Button cmdPreviousPosition;
        private System.Windows.Forms.Label lblPositionIndex;
        private System.Windows.Forms.GroupBox fraID;
        private System.Windows.Forms.NumericUpDown numID;
        private System.Windows.Forms.TabControl tbcTabs;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.GroupBox fraSettings;
        private System.Windows.Forms.Label lblC;
        private System.Windows.Forms.NumericUpDown numC;
        private System.Windows.Forms.Label lblB;
        private System.Windows.Forms.Label lblA;
        private System.Windows.Forms.Label lblFlags;
        private System.Windows.Forms.NumericUpDown numB;
        private System.Windows.Forms.NumericUpDown numA;
        private System.Windows.Forms.NumericUpDown numFlags;
        private System.Windows.Forms.Label lblCodeName;
        private System.Windows.Forms.CheckBox chkHexC;
        private System.Windows.Forms.CheckBox chkHexB;
        private System.Windows.Forms.CheckBox chkHexA;
        private System.Windows.Forms.CheckBox chkHexFlags;
        private System.Windows.Forms.Button cmdInterpolate;
        private System.Windows.Forms.NumericUpDown numSpawn;
        private System.Windows.Forms.ToolTip tipHover;
        private System.Windows.Forms.GroupBox fraSpawn;
    }
}
