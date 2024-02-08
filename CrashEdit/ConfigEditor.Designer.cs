﻿namespace CrashEdit
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
            System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
            System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
            System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
            System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
            System.Windows.Forms.Panel panel2;
            System.Windows.Forms.Panel panel1;
            this.lblH = new System.Windows.Forms.Label();
            this.numH = new System.Windows.Forms.NumericUpDown();
            this.lblW = new System.Windows.Forms.Label();
            this.numW = new System.Windows.Forms.NumericUpDown();
            this.fraAnimGrid = new System.Windows.Forms.GroupBox();
            this.numAnimGrid = new System.Windows.Forms.NumericUpDown();
            this.lblAnimGrid = new System.Windows.Forms.Label();
            this.chkAnimGrid = new System.Windows.Forms.CheckBox();
            this.fraClearCol = new System.Windows.Forms.GroupBox();
            this.picClearCol = new System.Windows.Forms.PictureBox();
            this.fraFont = new System.Windows.Forms.GroupBox();
            this.lblFontName = new System.Windows.Forms.Label();
            this.lblFontSize = new System.Windows.Forms.Label();
            this.dpdFont = new System.Windows.Forms.ComboBox();
            this.numFontSize = new System.Windows.Forms.NumericUpDown();
            this.chkViewerShowHelp = new System.Windows.Forms.CheckBox();
            this.chkFont2DEnable = new System.Windows.Forms.CheckBox();
            this.chkFont3DAutoscale = new System.Windows.Forms.CheckBox();
            this.chkFont3DEnable = new System.Windows.Forms.CheckBox();
            this.chkCollisionDisplay = new System.Windows.Forms.CheckBox();
            this.chkNormalDisplay = new System.Windows.Forms.CheckBox();
            this.chkPatchNSDSavesNSF = new System.Windows.Forms.CheckBox();
            this.chkDeleteInvalidEntries = new System.Windows.Forms.CheckBox();
            this.chkUseAnimLinks = new System.Windows.Forms.CheckBox();
            this.dpdLang = new System.Windows.Forms.ComboBox();
            this.lblLang = new System.Windows.Forms.Label();
            this.fraMisc = new System.Windows.Forms.GroupBox();
            this.cmdReset = new System.Windows.Forms.Button();
            this.fraSize = new System.Windows.Forms.GroupBox();
            this.cdlClearCol = new System.Windows.Forms.ColorDialog();
            this.fraNodeShadeAmt = new System.Windows.Forms.GroupBox();
            this.sldNodeShadeAmt = new System.Windows.Forms.TrackBar();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.chkCacheShaderUniformLoc = new System.Windows.Forms.CheckBox();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            panel2 = new System.Windows.Forms.Panel();
            panel1 = new System.Windows.Forms.Panel();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numW)).BeginInit();
            tableLayoutPanel3.SuspendLayout();
            this.fraAnimGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAnimGrid)).BeginInit();
            this.fraClearCol.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picClearCol)).BeginInit();
            this.fraFont.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFontSize)).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            this.fraMisc.SuspendLayout();
            this.fraSize.SuspendLayout();
            this.fraNodeShadeAmt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sldNodeShadeAmt)).BeginInit();
            this.tableLayoutPanel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.AutoSize = true;
            tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel2.Controls.Add(this.lblH, 0, 1);
            tableLayoutPanel2.Controls.Add(this.numH, 1, 1);
            tableLayoutPanel2.Controls.Add(this.lblW, 0, 0);
            tableLayoutPanel2.Controls.Add(this.numW, 1, 0);
            tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel2.Size = new System.Drawing.Size(132, 52);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // lblH
            // 
            this.lblH.AutoSize = true;
            this.lblH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblH.Location = new System.Drawing.Point(3, 26);
            this.lblH.Name = "lblH";
            this.lblH.Size = new System.Drawing.Size(38, 26);
            this.lblH.TabIndex = 3;
            this.lblH.Text = "Height";
            this.lblH.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numH
            // 
            this.numH.Location = new System.Drawing.Point(47, 29);
            this.numH.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.numH.MaximumSize = new System.Drawing.Size(75, 0);
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
            // lblW
            // 
            this.lblW.AutoSize = true;
            this.lblW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblW.Location = new System.Drawing.Point(3, 0);
            this.lblW.Name = "lblW";
            this.lblW.Size = new System.Drawing.Size(38, 26);
            this.lblW.TabIndex = 2;
            this.lblW.Text = "Width";
            this.lblW.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numW
            // 
            this.numW.Location = new System.Drawing.Point(47, 3);
            this.numW.Maximum = new decimal(new int[] {
            8192,
            0,
            0,
            0});
            this.numW.MaximumSize = new System.Drawing.Size(75, 0);
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
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.AutoSize = true;
            tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tableLayoutPanel3.ColumnCount = 4;
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 144F));
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 240F));
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel3.Controls.Add(this.fraAnimGrid, 0, 0);
            tableLayoutPanel3.Controls.Add(this.fraClearCol, 1, 0);
            tableLayoutPanel3.Controls.Add(this.fraFont, 2, 0);
            tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel3.Size = new System.Drawing.Size(468, 90);
            tableLayoutPanel3.TabIndex = 3;
            // 
            // fraAnimGrid
            // 
            this.fraAnimGrid.Controls.Add(this.numAnimGrid);
            this.fraAnimGrid.Controls.Add(this.lblAnimGrid);
            this.fraAnimGrid.Controls.Add(this.chkAnimGrid);
            this.fraAnimGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fraAnimGrid.Location = new System.Drawing.Point(3, 3);
            this.fraAnimGrid.Name = "fraAnimGrid";
            this.fraAnimGrid.Size = new System.Drawing.Size(138, 84);
            this.fraAnimGrid.TabIndex = 6;
            this.fraAnimGrid.TabStop = false;
            this.fraAnimGrid.Text = "3D Viewer World Grid";
            // 
            // numAnimGrid
            // 
            this.numAnimGrid.Location = new System.Drawing.Point(56, 45);
            this.numAnimGrid.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.numAnimGrid.Name = "numAnimGrid";
            this.numAnimGrid.Size = new System.Drawing.Size(74, 20);
            this.numAnimGrid.TabIndex = 2;
            this.numAnimGrid.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numAnimGrid.ValueChanged += new System.EventHandler(this.numAnimGrid_ValueChanged);
            // 
            // lblAnimGrid
            // 
            this.lblAnimGrid.AutoSize = true;
            this.lblAnimGrid.Location = new System.Drawing.Point(6, 47);
            this.lblAnimGrid.Name = "lblAnimGrid";
            this.lblAnimGrid.Size = new System.Drawing.Size(43, 13);
            this.lblAnimGrid.TabIndex = 1;
            this.lblAnimGrid.Text = "Amount";
            // 
            // chkAnimGrid
            // 
            this.chkAnimGrid.AutoSize = true;
            this.chkAnimGrid.Location = new System.Drawing.Point(9, 19);
            this.chkAnimGrid.Name = "chkAnimGrid";
            this.chkAnimGrid.Size = new System.Drawing.Size(65, 17);
            this.chkAnimGrid.TabIndex = 0;
            this.chkAnimGrid.Text = "Enabled";
            this.chkAnimGrid.UseVisualStyleBackColor = true;
            this.chkAnimGrid.CheckedChanged += new System.EventHandler(this.chkAnimGrid_CheckedChanged);
            // 
            // fraClearCol
            // 
            this.fraClearCol.Controls.Add(this.picClearCol);
            this.fraClearCol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fraClearCol.Location = new System.Drawing.Point(147, 3);
            this.fraClearCol.Name = "fraClearCol";
            this.fraClearCol.Size = new System.Drawing.Size(74, 84);
            this.fraClearCol.TabIndex = 4;
            this.fraClearCol.TabStop = false;
            this.fraClearCol.Text = "Clear Color";
            // 
            // picClearCol
            // 
            this.picClearCol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picClearCol.Location = new System.Drawing.Point(6, 19);
            this.picClearCol.Name = "picClearCol";
            this.picClearCol.Size = new System.Drawing.Size(60, 46);
            this.picClearCol.TabIndex = 0;
            this.picClearCol.TabStop = false;
            this.picClearCol.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // fraFont
            // 
            this.fraFont.AutoSize = true;
            this.fraFont.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fraFont.Controls.Add(tableLayoutPanel4);
            this.fraFont.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fraFont.Location = new System.Drawing.Point(227, 3);
            this.fraFont.Name = "fraFont";
            this.fraFont.Size = new System.Drawing.Size(234, 84);
            this.fraFont.TabIndex = 8;
            this.fraFont.TabStop = false;
            this.fraFont.Text = "Font Renderer";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            tableLayoutPanel4.AutoSize = true;
            tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel4.Controls.Add(this.lblFontName, 0, 0);
            tableLayoutPanel4.Controls.Add(this.lblFontSize, 0, 1);
            tableLayoutPanel4.Controls.Add(this.dpdFont, 1, 0);
            tableLayoutPanel4.Controls.Add(this.numFontSize, 1, 1);
            tableLayoutPanel4.Location = new System.Drawing.Point(6, 16);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel4.Size = new System.Drawing.Size(189, 53);
            tableLayoutPanel4.TabIndex = 5;
            // 
            // lblFontName
            // 
            this.lblFontName.AutoSize = true;
            this.lblFontName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFontName.Location = new System.Drawing.Point(3, 0);
            this.lblFontName.Name = "lblFontName";
            this.lblFontName.Size = new System.Drawing.Size(28, 27);
            this.lblFontName.TabIndex = 3;
            this.lblFontName.Text = "Font";
            this.lblFontName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFontSize
            // 
            this.lblFontSize.AutoSize = true;
            this.lblFontSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFontSize.Location = new System.Drawing.Point(3, 27);
            this.lblFontSize.Name = "lblFontSize";
            this.lblFontSize.Size = new System.Drawing.Size(28, 26);
            this.lblFontSize.TabIndex = 4;
            this.lblFontSize.Text = "Size";
            this.lblFontSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dpdFont
            // 
            this.dpdFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dpdFont.FormattingEnabled = true;
            this.dpdFont.Location = new System.Drawing.Point(37, 3);
            this.dpdFont.Name = "dpdFont";
            this.dpdFont.Size = new System.Drawing.Size(149, 21);
            this.dpdFont.TabIndex = 1;
            // 
            // numFontSize
            // 
            this.numFontSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.numFontSize.DecimalPlaces = 2;
            this.numFontSize.Location = new System.Drawing.Point(37, 30);
            this.numFontSize.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numFontSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFontSize.Name = "numFontSize";
            this.numFontSize.Size = new System.Drawing.Size(60, 20);
            this.numFontSize.TabIndex = 3;
            this.numFontSize.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numFontSize.ValueChanged += new System.EventHandler(this.numFontSize_ValueChanged);
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel1.Controls.Add(panel2, 0, 1);
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.Size = new System.Drawing.Size(452, 231);
            tableLayoutPanel1.TabIndex = 9;
            // 
            // panel2
            // 
            panel2.AutoSize = true;
            panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            panel2.Controls.Add(this.chkCacheShaderUniformLoc);
            panel2.Controls.Add(this.chkViewerShowHelp);
            panel2.Controls.Add(this.chkFont2DEnable);
            panel2.Controls.Add(this.chkFont3DAutoscale);
            panel2.Controls.Add(this.chkFont3DEnable);
            panel2.Controls.Add(this.chkCollisionDisplay);
            panel2.Controls.Add(this.chkNormalDisplay);
            panel2.Controls.Add(this.chkPatchNSDSavesNSF);
            panel2.Controls.Add(this.chkDeleteInvalidEntries);
            panel2.Controls.Add(this.chkUseAnimLinks);
            panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            panel2.Location = new System.Drawing.Point(3, 48);
            panel2.Name = "panel2";
            panel2.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            panel2.Size = new System.Drawing.Size(446, 180);
            panel2.TabIndex = 11;
            // 
            // chkViewerShowHelp
            // 
            this.chkViewerShowHelp.AutoSize = true;
            this.chkViewerShowHelp.Checked = true;
            this.chkViewerShowHelp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkViewerShowHelp.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkViewerShowHelp.Location = new System.Drawing.Point(10, 141);
            this.chkViewerShowHelp.Name = "chkViewerShowHelp";
            this.chkViewerShowHelp.Size = new System.Drawing.Size(426, 17);
            this.chkViewerShowHelp.TabIndex = 7;
            this.chkViewerShowHelp.Text = "Display help text by default";
            this.chkViewerShowHelp.UseVisualStyleBackColor = true;
            this.chkViewerShowHelp.CheckedChanged += new System.EventHandler(this.chkViewerShowHelp_CheckedChanged);
            // 
            // chkFont2DEnable
            // 
            this.chkFont2DEnable.AutoSize = true;
            this.chkFont2DEnable.Checked = true;
            this.chkFont2DEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFont2DEnable.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkFont2DEnable.Location = new System.Drawing.Point(10, 124);
            this.chkFont2DEnable.Name = "chkFont2DEnable";
            this.chkFont2DEnable.Size = new System.Drawing.Size(426, 17);
            this.chkFont2DEnable.TabIndex = 6;
            this.chkFont2DEnable.Text = "Display debug text";
            this.chkFont2DEnable.UseVisualStyleBackColor = true;
            this.chkFont2DEnable.CheckedChanged += new System.EventHandler(this.chkFont2DEnable_CheckedChanged);
            // 
            // chkFont3DAutoscale
            // 
            this.chkFont3DAutoscale.AutoSize = true;
            this.chkFont3DAutoscale.Checked = true;
            this.chkFont3DAutoscale.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFont3DAutoscale.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkFont3DAutoscale.Location = new System.Drawing.Point(10, 107);
            this.chkFont3DAutoscale.Name = "chkFont3DAutoscale";
            this.chkFont3DAutoscale.Size = new System.Drawing.Size(426, 17);
            this.chkFont3DAutoscale.TabIndex = 7;
            this.chkFont3DAutoscale.Text = "Scale entity text";
            this.chkFont3DAutoscale.UseVisualStyleBackColor = true;
            this.chkFont3DAutoscale.CheckedChanged += new System.EventHandler(this.chkFont3DAutoscale_CheckedChanged);
            // 
            // chkFont3DEnable
            // 
            this.chkFont3DEnable.AutoSize = true;
            this.chkFont3DEnable.Checked = true;
            this.chkFont3DEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFont3DEnable.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkFont3DEnable.Location = new System.Drawing.Point(10, 90);
            this.chkFont3DEnable.Name = "chkFont3DEnable";
            this.chkFont3DEnable.Size = new System.Drawing.Size(426, 17);
            this.chkFont3DEnable.TabIndex = 5;
            this.chkFont3DEnable.Text = "Display entity text";
            this.chkFont3DEnable.UseVisualStyleBackColor = true;
            this.chkFont3DEnable.CheckedChanged += new System.EventHandler(this.chkFont3DEnable_CheckedChanged);
            // 
            // chkCollisionDisplay
            // 
            this.chkCollisionDisplay.AutoSize = true;
            this.chkCollisionDisplay.Checked = true;
            this.chkCollisionDisplay.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCollisionDisplay.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkCollisionDisplay.Location = new System.Drawing.Point(10, 73);
            this.chkCollisionDisplay.Name = "chkCollisionDisplay";
            this.chkCollisionDisplay.Size = new System.Drawing.Size(426, 17);
            this.chkCollisionDisplay.TabIndex = 2;
            this.chkCollisionDisplay.Text = "Display frame collision by default";
            this.chkCollisionDisplay.UseVisualStyleBackColor = true;
            this.chkCollisionDisplay.CheckedChanged += new System.EventHandler(this.chkCollisionDisplay_CheckedChanged);
            // 
            // chkNormalDisplay
            // 
            this.chkNormalDisplay.AutoSize = true;
            this.chkNormalDisplay.Checked = true;
            this.chkNormalDisplay.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNormalDisplay.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkNormalDisplay.Location = new System.Drawing.Point(10, 56);
            this.chkNormalDisplay.Name = "chkNormalDisplay";
            this.chkNormalDisplay.Size = new System.Drawing.Size(426, 17);
            this.chkNormalDisplay.TabIndex = 0;
            this.chkNormalDisplay.Text = "Display normals";
            this.chkNormalDisplay.UseVisualStyleBackColor = true;
            this.chkNormalDisplay.CheckedChanged += new System.EventHandler(this.chkNormalDisplay_CheckedChanged);
            // 
            // chkPatchNSDSavesNSF
            // 
            this.chkPatchNSDSavesNSF.AutoSize = true;
            this.chkPatchNSDSavesNSF.Checked = true;
            this.chkPatchNSDSavesNSF.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPatchNSDSavesNSF.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkPatchNSDSavesNSF.Location = new System.Drawing.Point(10, 39);
            this.chkPatchNSDSavesNSF.Name = "chkPatchNSDSavesNSF";
            this.chkPatchNSDSavesNSF.Size = new System.Drawing.Size(426, 17);
            this.chkPatchNSDSavesNSF.TabIndex = 7;
            this.chkPatchNSDSavesNSF.Text = "(Patch NSD) Always save NSF after NSD patching";
            this.chkPatchNSDSavesNSF.UseVisualStyleBackColor = true;
            this.chkPatchNSDSavesNSF.CheckedChanged += new System.EventHandler(this.chkPatchNSDSavesNSF_CheckedChanged);
            // 
            // chkDeleteInvalidEntries
            // 
            this.chkDeleteInvalidEntries.AutoSize = true;
            this.chkDeleteInvalidEntries.Checked = true;
            this.chkDeleteInvalidEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDeleteInvalidEntries.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkDeleteInvalidEntries.Location = new System.Drawing.Point(10, 22);
            this.chkDeleteInvalidEntries.Name = "chkDeleteInvalidEntries";
            this.chkDeleteInvalidEntries.Size = new System.Drawing.Size(426, 17);
            this.chkDeleteInvalidEntries.TabIndex = 5;
            this.chkDeleteInvalidEntries.Text = "(Patch NSD) Delete non-existent entries from load lists";
            this.chkDeleteInvalidEntries.UseVisualStyleBackColor = true;
            this.chkDeleteInvalidEntries.CheckedChanged += new System.EventHandler(this.chkDeleteInvalidEntries_CheckedChanged);
            // 
            // chkUseAnimLinks
            // 
            this.chkUseAnimLinks.AutoSize = true;
            this.chkUseAnimLinks.Checked = true;
            this.chkUseAnimLinks.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseAnimLinks.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkUseAnimLinks.Location = new System.Drawing.Point(10, 5);
            this.chkUseAnimLinks.Name = "chkUseAnimLinks";
            this.chkUseAnimLinks.Size = new System.Drawing.Size(426, 17);
            this.chkUseAnimLinks.TabIndex = 3;
            this.chkUseAnimLinks.Text = "(Crash 3) Used saved animation-model links";
            this.chkUseAnimLinks.UseVisualStyleBackColor = true;
            this.chkUseAnimLinks.CheckedChanged += new System.EventHandler(this.chkUseAnimLinks_CheckedChanged);
            // 
            // panel1
            // 
            panel1.AutoSize = true;
            panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            panel1.Controls.Add(this.dpdLang);
            panel1.Controls.Add(this.lblLang);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(3, 3);
            panel1.Name = "panel1";
            panel1.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            panel1.Size = new System.Drawing.Size(446, 39);
            panel1.TabIndex = 10;
            // 
            // dpdLang
            // 
            this.dpdLang.Dock = System.Windows.Forms.DockStyle.Top;
            this.dpdLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dpdLang.FormattingEnabled = true;
            this.dpdLang.Location = new System.Drawing.Point(10, 18);
            this.dpdLang.MaximumSize = new System.Drawing.Size(133, 0);
            this.dpdLang.Name = "dpdLang";
            this.dpdLang.Size = new System.Drawing.Size(133, 21);
            this.dpdLang.TabIndex = 0;
            // 
            // lblLang
            // 
            this.lblLang.AutoSize = true;
            this.lblLang.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLang.Location = new System.Drawing.Point(10, 0);
            this.lblLang.Name = "lblLang";
            this.lblLang.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblLang.Size = new System.Drawing.Size(133, 18);
            this.lblLang.TabIndex = 8;
            this.lblLang.Text = "Language (requires restart)";
            // 
            // fraMisc
            // 
            this.fraMisc.AutoSize = true;
            this.fraMisc.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fraMisc.Controls.Add(tableLayoutPanel1);
            this.fraMisc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fraMisc.Location = new System.Drawing.Point(3, 3);
            this.fraMisc.Margin = new System.Windows.Forms.Padding(6);
            this.fraMisc.MaximumSize = new System.Drawing.Size(458, 0);
            this.fraMisc.Name = "fraMisc";
            this.fraMisc.Size = new System.Drawing.Size(458, 250);
            this.fraMisc.TabIndex = 1;
            this.fraMisc.TabStop = false;
            this.fraMisc.Text = "Miscellaneous";
            // 
            // cmdReset
            // 
            this.cmdReset.AutoSize = true;
            this.cmdReset.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cmdReset.Location = new System.Drawing.Point(9, 9);
            this.cmdReset.Name = "cmdReset";
            this.cmdReset.Size = new System.Drawing.Size(86, 23);
            this.cmdReset.TabIndex = 1;
            this.cmdReset.Text = "Reset Settings";
            this.cmdReset.UseVisualStyleBackColor = true;
            this.cmdReset.Click += new System.EventHandler(this.cmdReset_Click);
            // 
            // fraSize
            // 
            this.fraSize.AutoSize = true;
            this.fraSize.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fraSize.Controls.Add(tableLayoutPanel2);
            this.fraSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fraSize.Location = new System.Drawing.Point(3, 3);
            this.fraSize.Name = "fraSize";
            this.fraSize.Size = new System.Drawing.Size(138, 71);
            this.fraSize.TabIndex = 1;
            this.fraSize.TabStop = false;
            this.fraSize.Text = "Default Window Size";
            // 
            // cdlClearCol
            // 
            this.cdlClearCol.AnyColor = true;
            this.cdlClearCol.FullOpen = true;
            this.cdlClearCol.SolidColorOnly = true;
            // 
            // fraNodeShadeAmt
            // 
            this.fraNodeShadeAmt.AutoSize = true;
            this.fraNodeShadeAmt.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fraNodeShadeAmt.Controls.Add(this.sldNodeShadeAmt);
            this.fraNodeShadeAmt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fraNodeShadeAmt.Location = new System.Drawing.Point(147, 3);
            this.fraNodeShadeAmt.Name = "fraNodeShadeAmt";
            this.fraNodeShadeAmt.Size = new System.Drawing.Size(314, 71);
            this.fraNodeShadeAmt.TabIndex = 10;
            this.fraNodeShadeAmt.TabStop = false;
            this.fraNodeShadeAmt.Text = "Collision Node Shade Amount";
            // 
            // sldNodeShadeAmt
            // 
            this.sldNodeShadeAmt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sldNodeShadeAmt.LargeChange = 10;
            this.sldNodeShadeAmt.Location = new System.Drawing.Point(3, 16);
            this.sldNodeShadeAmt.Maximum = 100;
            this.sldNodeShadeAmt.Name = "sldNodeShadeAmt";
            this.sldNodeShadeAmt.Size = new System.Drawing.Size(308, 52);
            this.sldNodeShadeAmt.TabIndex = 0;
            this.sldNodeShadeAmt.TickFrequency = 5;
            this.sldNodeShadeAmt.Value = 20;
            this.sldNodeShadeAmt.Scroll += new System.EventHandler(this.sldNodeShadeAmt_Scroll);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.AutoSize = true;
            this.tableLayoutPanel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 144F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 320F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.Controls.Add(this.fraNodeShadeAmt, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.fraSize, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 93);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(468, 77);
            this.tableLayoutPanel5.TabIndex = 11;
            // 
            // panel3
            // 
            this.panel3.AutoSize = true;
            this.panel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel3.Controls.Add(this.cmdReset);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(3, 426);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(3);
            this.panel3.Size = new System.Drawing.Size(468, 38);
            this.panel3.TabIndex = 12;
            // 
            // panel4
            // 
            this.panel4.AutoSize = true;
            this.panel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel4.Controls.Add(this.fraMisc);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(3, 170);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(3);
            this.panel4.Size = new System.Drawing.Size(468, 256);
            this.panel4.TabIndex = 13;
            // 
            // chkCacheShaderUniformLoc
            // 
            this.chkCacheShaderUniformLoc.AutoSize = true;
            this.chkCacheShaderUniformLoc.Checked = true;
            this.chkCacheShaderUniformLoc.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCacheShaderUniformLoc.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkCacheShaderUniformLoc.Location = new System.Drawing.Point(10, 158);
            this.chkCacheShaderUniformLoc.Name = "chkCacheShaderUniformLoc";
            this.chkCacheShaderUniformLoc.Size = new System.Drawing.Size(426, 17);
            this.chkCacheShaderUniformLoc.TabIndex = 8;
            this.chkCacheShaderUniformLoc.Text = "Cache shader uniform locations";
            this.chkCacheShaderUniformLoc.UseVisualStyleBackColor = true;
            this.chkCacheShaderUniformLoc.CheckedChanged += new System.EventHandler(this.chkCacheShaderUniformLoc_CheckedChanged);
            // 
            // ConfigEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.tableLayoutPanel5);
            this.Controls.Add(tableLayoutPanel3);
            this.Name = "ConfigEditor";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(474, 488);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numW)).EndInit();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            this.fraAnimGrid.ResumeLayout(false);
            this.fraAnimGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAnimGrid)).EndInit();
            this.fraClearCol.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picClearCol)).EndInit();
            this.fraFont.ResumeLayout(false);
            this.fraFont.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFontSize)).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            this.fraMisc.ResumeLayout(false);
            this.fraMisc.PerformLayout();
            this.fraSize.ResumeLayout(false);
            this.fraSize.PerformLayout();
            this.fraNodeShadeAmt.ResumeLayout(false);
            this.fraNodeShadeAmt.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sldNodeShadeAmt)).EndInit();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button cmdReset;
        private System.Windows.Forms.GroupBox fraSize;
        private System.Windows.Forms.Label lblH;
        private System.Windows.Forms.Label lblW;
        private System.Windows.Forms.NumericUpDown numH;
        private System.Windows.Forms.NumericUpDown numW;
        private System.Windows.Forms.ColorDialog cdlClearCol;
        private System.Windows.Forms.GroupBox fraClearCol;
        private System.Windows.Forms.PictureBox picClearCol;
        private System.Windows.Forms.GroupBox fraAnimGrid;
        private System.Windows.Forms.NumericUpDown numAnimGrid;
        private System.Windows.Forms.Label lblAnimGrid;
        private System.Windows.Forms.CheckBox chkAnimGrid;
        private System.Windows.Forms.ComboBox dpdFont;
        private System.Windows.Forms.GroupBox fraFont;
        private System.Windows.Forms.NumericUpDown numFontSize;
        private System.Windows.Forms.Label lblFontSize;
        private System.Windows.Forms.Label lblFontName;
        private System.Windows.Forms.GroupBox fraMisc;
        private System.Windows.Forms.GroupBox fraNodeShadeAmt;
        private System.Windows.Forms.TrackBar sldNodeShadeAmt;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.CheckBox chkViewerShowHelp;
        private System.Windows.Forms.CheckBox chkFont2DEnable;
        private System.Windows.Forms.CheckBox chkFont3DAutoscale;
        private System.Windows.Forms.CheckBox chkFont3DEnable;
        private System.Windows.Forms.CheckBox chkCollisionDisplay;
        private System.Windows.Forms.CheckBox chkNormalDisplay;
        private System.Windows.Forms.CheckBox chkPatchNSDSavesNSF;
        private System.Windows.Forms.CheckBox chkDeleteInvalidEntries;
        private System.Windows.Forms.CheckBox chkUseAnimLinks;
        private System.Windows.Forms.ComboBox dpdLang;
        private System.Windows.Forms.Label lblLang;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.CheckBox chkCacheShaderUniformLoc;
    }
}
