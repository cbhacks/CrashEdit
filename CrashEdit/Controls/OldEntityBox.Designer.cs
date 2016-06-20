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
            this.numType = new System.Windows.Forms.NumericUpDown();
            this.fraType = new System.Windows.Forms.GroupBox();
            this.lblCodeName = new System.Windows.Forms.Label();
            this.fraSubtype = new System.Windows.Forms.GroupBox();
            this.numSubtype = new System.Windows.Forms.NumericUpDown();
            this.fraPosition = new System.Windows.Forms.GroupBox();
            this.cmdNextAndRemove = new System.Windows.Forms.Button();
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
            this.lblD = new System.Windows.Forms.Label();
            this.numD = new System.Windows.Forms.NumericUpDown();
            this.lblC = new System.Windows.Forms.Label();
            this.lblB = new System.Windows.Forms.Label();
            this.lblA = new System.Windows.Forms.Label();
            this.numC = new System.Windows.Forms.NumericUpDown();
            this.numB = new System.Windows.Forms.NumericUpDown();
            this.numA = new System.Windows.Forms.NumericUpDown();
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
            ((System.ComponentModel.ISupportInitialize)(this.numD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numA)).BeginInit();
            this.SuspendLayout();
            // 
            // numType
            // 
            this.numType.Location = new System.Drawing.Point(6,22);
            this.numType.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numType.Name = "numType";
            this.numType.Size = new System.Drawing.Size(120,20);
            this.numType.TabIndex = 1;
            this.numType.ValueChanged += new System.EventHandler(this.numType_ValueChanged);
            // 
            // fraType
            // 
            this.fraType.Controls.Add(this.lblCodeName);
            this.fraType.Controls.Add(this.numType);
            this.fraType.Location = new System.Drawing.Point(209,3);
            this.fraType.Name = "fraType";
            this.fraType.Size = new System.Drawing.Size(132,67);
            this.fraType.TabIndex = 4;
            this.fraType.TabStop = false;
            this.fraType.Text = "Type";
            // 
            // lblCodeName
            // 
            this.lblCodeName.Font = new System.Drawing.Font("Microsoft Sans Serif",8.25F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
            this.lblCodeName.Location = new System.Drawing.Point(2,45);
            this.lblCodeName.Name = "lblCodeName";
            this.lblCodeName.Size = new System.Drawing.Size(120,19);
            this.lblCodeName.TabIndex = 9;
            this.lblCodeName.Text = "CodeC";
            this.lblCodeName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // fraSubtype
            // 
            this.fraSubtype.Controls.Add(this.numSubtype);
            this.fraSubtype.Location = new System.Drawing.Point(209,76);
            this.fraSubtype.Name = "fraSubtype";
            this.fraSubtype.Size = new System.Drawing.Size(132,46);
            this.fraSubtype.TabIndex = 5;
            this.fraSubtype.TabStop = false;
            this.fraSubtype.Text = "Subtype";
            // 
            // numSubtype
            // 
            this.numSubtype.Location = new System.Drawing.Point(6,20);
            this.numSubtype.Name = "numSubtype";
            this.numSubtype.Size = new System.Drawing.Size(120,20);
            this.numSubtype.TabIndex = 1;
            this.numSubtype.ValueChanged += new System.EventHandler(this.numSubtype_ValueChanged);
            // 
            // fraPosition
            // 
            this.fraPosition.Controls.Add(this.cmdNextAndRemove);
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
            this.fraPosition.Location = new System.Drawing.Point(3,3);
            this.fraPosition.Name = "fraPosition";
            this.fraPosition.Size = new System.Drawing.Size(200,166);
            this.fraPosition.TabIndex = 1;
            this.fraPosition.TabStop = false;
            this.fraPosition.Text = "Position(s)";
            // 
            // cmdNextAndRemove
            // 
            this.cmdNextAndRemove.Location = new System.Drawing.Point(88,130);
            this.cmdNextAndRemove.Name = "cmdNextAndRemove";
            this.cmdNextAndRemove.Size = new System.Drawing.Size(106,23);
            this.cmdNextAndRemove.TabIndex = 8;
            this.cmdNextAndRemove.Text = "Next and Remove";
            this.cmdNextAndRemove.UseVisualStyleBackColor = true;
            this.cmdNextAndRemove.Click += new System.EventHandler(this.cmdNextAndRemovePosition_Click);
            // 
            // lblPositionIndex
            // 
            this.lblPositionIndex.Font = new System.Drawing.Font("Microsoft Sans Serif",8.25F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
            this.lblPositionIndex.Location = new System.Drawing.Point(6,19);
            this.lblPositionIndex.Name = "lblPositionIndex";
            this.lblPositionIndex.Size = new System.Drawing.Size(60,23);
            this.lblPositionIndex.TabIndex = 5;
            this.lblPositionIndex.Text = "?? / ??";
            this.lblPositionIndex.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmdNextPosition
            // 
            this.cmdNextPosition.Location = new System.Drawing.Point(136,19);
            this.cmdNextPosition.Name = "cmdNextPosition";
            this.cmdNextPosition.Size = new System.Drawing.Size(58,23);
            this.cmdNextPosition.TabIndex = 1;
            this.cmdNextPosition.Text = "Next";
            this.cmdNextPosition.UseVisualStyleBackColor = true;
            this.cmdNextPosition.Click += new System.EventHandler(this.cmdNextPosition_Click);
            // 
            // cmdPreviousPosition
            // 
            this.cmdPreviousPosition.Location = new System.Drawing.Point(72,19);
            this.cmdPreviousPosition.Name = "cmdPreviousPosition";
            this.cmdPreviousPosition.Size = new System.Drawing.Size(58,23);
            this.cmdPreviousPosition.TabIndex = 0;
            this.cmdPreviousPosition.Text = "Previous";
            this.cmdPreviousPosition.UseVisualStyleBackColor = true;
            this.cmdPreviousPosition.Click += new System.EventHandler(this.cmdPreviousPosition_Click);
            // 
            // cmdInsertPosition
            // 
            this.cmdInsertPosition.Location = new System.Drawing.Point(119,75);
            this.cmdInsertPosition.Name = "cmdInsertPosition";
            this.cmdInsertPosition.Size = new System.Drawing.Size(75,23);
            this.cmdInsertPosition.TabIndex = 6;
            this.cmdInsertPosition.Text = "Insert";
            this.cmdInsertPosition.UseVisualStyleBackColor = true;
            this.cmdInsertPosition.Click += new System.EventHandler(this.cmdInsertPosition_Click);
            // 
            // lblZ
            // 
            this.lblZ.AutoSize = true;
            this.lblZ.Location = new System.Drawing.Point(6,106);
            this.lblZ.Name = "lblZ";
            this.lblZ.Size = new System.Drawing.Size(14,13);
            this.lblZ.TabIndex = 5;
            this.lblZ.Text = "Z";
            // 
            // cmdRemovePosition
            // 
            this.cmdRemovePosition.Location = new System.Drawing.Point(119,101);
            this.cmdRemovePosition.Name = "cmdRemovePosition";
            this.cmdRemovePosition.Size = new System.Drawing.Size(75,23);
            this.cmdRemovePosition.TabIndex = 7;
            this.cmdRemovePosition.Text = "Remove";
            this.cmdRemovePosition.UseVisualStyleBackColor = true;
            this.cmdRemovePosition.Click += new System.EventHandler(this.cmdRemovePosition_Click);
            // 
            // lblY
            // 
            this.lblY.AutoSize = true;
            this.lblY.Location = new System.Drawing.Point(6,80);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(14,13);
            this.lblY.TabIndex = 4;
            this.lblY.Text = "Y";
            // 
            // cmdAppendPosition
            // 
            this.cmdAppendPosition.Location = new System.Drawing.Point(119,49);
            this.cmdAppendPosition.Name = "cmdAppendPosition";
            this.cmdAppendPosition.Size = new System.Drawing.Size(75,23);
            this.cmdAppendPosition.TabIndex = 5;
            this.cmdAppendPosition.Text = "Append";
            this.cmdAppendPosition.UseVisualStyleBackColor = true;
            this.cmdAppendPosition.Click += new System.EventHandler(this.cmdAppendPosition_Click);
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.Location = new System.Drawing.Point(6,54);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(14,13);
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
            this.numZ.Location = new System.Drawing.Point(26,104);
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
            this.numZ.Size = new System.Drawing.Size(86,20);
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
            this.numY.Location = new System.Drawing.Point(26,78);
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
            this.numY.Size = new System.Drawing.Size(86,20);
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
            this.numX.Location = new System.Drawing.Point(26,52);
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
            this.numX.Size = new System.Drawing.Size(86,20);
            this.numX.TabIndex = 2;
            this.numX.ValueChanged += new System.EventHandler(this.numX_ValueChanged);
            // 
            // fraID
            // 
            this.fraID.Controls.Add(this.numID);
            this.fraID.Location = new System.Drawing.Point(209,128);
            this.fraID.Name = "fraID";
            this.fraID.Size = new System.Drawing.Size(132,49);
            this.fraID.TabIndex = 3;
            this.fraID.TabStop = false;
            this.fraID.Text = "ID";
            // 
            // numID
            // 
            this.numID.Location = new System.Drawing.Point(6,19);
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
            this.numID.Size = new System.Drawing.Size(120,20);
            this.numID.TabIndex = 1;
            this.numID.ValueChanged += new System.EventHandler(this.numID_ValueChanged);
            // 
            // tbcTabs
            // 
            this.tbcTabs.Controls.Add(this.tabGeneral);
            this.tbcTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcTabs.Location = new System.Drawing.Point(0,0);
            this.tbcTabs.Name = "tbcTabs";
            this.tbcTabs.SelectedIndex = 0;
            this.tbcTabs.Size = new System.Drawing.Size(398,454);
            this.tbcTabs.TabIndex = 7;
            // 
            // tabGeneral
            // 
            this.tabGeneral.AutoScroll = true;
            this.tabGeneral.Controls.Add(this.fraSettings);
            this.tabGeneral.Controls.Add(this.fraType);
            this.tabGeneral.Controls.Add(this.fraSubtype);
            this.tabGeneral.Controls.Add(this.fraPosition);
            this.tabGeneral.Controls.Add(this.fraID);
            this.tabGeneral.Location = new System.Drawing.Point(4,22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Size = new System.Drawing.Size(390,428);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // fraSettings
            // 
            this.fraSettings.Controls.Add(this.lblD);
            this.fraSettings.Controls.Add(this.numD);
            this.fraSettings.Controls.Add(this.lblC);
            this.fraSettings.Controls.Add(this.lblB);
            this.fraSettings.Controls.Add(this.lblA);
            this.fraSettings.Controls.Add(this.numC);
            this.fraSettings.Controls.Add(this.numB);
            this.fraSettings.Controls.Add(this.numA);
            this.fraSettings.Location = new System.Drawing.Point(3,175);
            this.fraSettings.Name = "fraSettings";
            this.fraSettings.Size = new System.Drawing.Size(200,133);
            this.fraSettings.TabIndex = 8;
            this.fraSettings.TabStop = false;
            this.fraSettings.Text = "Special Settings";
            // 
            // lblD
            // 
            this.lblD.AutoSize = true;
            this.lblD.Location = new System.Drawing.Point(6,99);
            this.lblD.Name = "lblD";
            this.lblD.Size = new System.Drawing.Size(41,13);
            this.lblD.TabIndex = 7;
            this.lblD.Text = "Setting D";
            // 
            // numD
            // 
            this.numD.Location = new System.Drawing.Point(62,97);
            this.numD.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.numD.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.numD.Name = "numD";
            this.numD.Size = new System.Drawing.Size(86,20);
            this.numD.TabIndex = 6;
            this.numD.ValueChanged += new System.EventHandler(this.numD_ValueChanged);
            // 
            // lblC
            // 
            this.lblC.AutoSize = true;
            this.lblC.Location = new System.Drawing.Point(6,73);
            this.lblC.Name = "lblC";
            this.lblC.Size = new System.Drawing.Size(50,13);
            this.lblC.TabIndex = 5;
            this.lblC.Text = "Setting C";
            // 
            // lblB
            // 
            this.lblB.AutoSize = true;
            this.lblB.Location = new System.Drawing.Point(6,47);
            this.lblB.Name = "lblB";
            this.lblB.Size = new System.Drawing.Size(50,13);
            this.lblB.TabIndex = 4;
            this.lblB.Text = "Setting B";
            // 
            // lblA
            // 
            this.lblA.AutoSize = true;
            this.lblA.Location = new System.Drawing.Point(6,21);
            this.lblA.Name = "lblA";
            this.lblA.Size = new System.Drawing.Size(50,13);
            this.lblA.TabIndex = 3;
            this.lblA.Text = "Setting A";
            // 
            // numC
            // 
            this.numC.Location = new System.Drawing.Point(62,71);
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
            this.numC.Size = new System.Drawing.Size(86,20);
            this.numC.TabIndex = 4;
            this.numC.ValueChanged += new System.EventHandler(this.numC_ValueChanged);
            // 
            // numB
            // 
            this.numB.Location = new System.Drawing.Point(62,45);
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
            this.numB.Size = new System.Drawing.Size(86,20);
            this.numB.TabIndex = 3;
            this.numB.ValueChanged += new System.EventHandler(this.numB_ValueChanged);
            // 
            // numA
            // 
            this.numA.Location = new System.Drawing.Point(62,19);
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
            this.numA.Size = new System.Drawing.Size(86,20);
            this.numA.TabIndex = 2;
            this.numA.ValueChanged += new System.EventHandler(this.numA_ValueChanged);
            // 
            // OldEntityBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbcTabs);
            this.Name = "OldEntityBox";
            this.Size = new System.Drawing.Size(398,454);
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
            this.fraSettings.ResumeLayout(false);
            this.fraSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numA)).EndInit();
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
        private System.Windows.Forms.Label lblD;
        private System.Windows.Forms.NumericUpDown numD;
        private System.Windows.Forms.Label lblC;
        private System.Windows.Forms.Label lblB;
        private System.Windows.Forms.Label lblA;
        private System.Windows.Forms.NumericUpDown numC;
        private System.Windows.Forms.NumericUpDown numB;
        private System.Windows.Forms.NumericUpDown numA;
        private System.Windows.Forms.Button cmdNextAndRemove;
        private System.Windows.Forms.Label lblCodeName;
    }
}
