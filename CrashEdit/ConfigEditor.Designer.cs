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
            TableLayoutPanel tableLayoutPanel3;
            TableLayoutPanel tableLayoutPanel4;
            TableLayoutPanel tableLayoutPanel1;
            Panel panel2;
            Panel panel1;
            fraAnimGrid = new GroupBox();
            numAnimGrid = new NumericUpDown();
            lblAnimGrid = new Label();
            chkAnimGrid = new CheckBox();
            fraClearCol = new GroupBox();
            picClearCol = new PictureBox();
            fraFont = new GroupBox();
            lblFontName = new Label();
            lblFontSize = new Label();
            dpdFont = new ComboBox();
            numFontSize = new NumericUpDown();
            chkViewerShowHelp = new CheckBox();
            chkFont2DEnable = new CheckBox();
            chkFont3DAutoscale = new CheckBox();
            chkFont3DEnable = new CheckBox();
            chkCollisionDisplay = new CheckBox();
            chkNormalDisplay = new CheckBox();
            chkPatchNSDSavesNSF = new CheckBox();
            chkDeleteInvalidEntries = new CheckBox();
            chkUseAnimLinks = new CheckBox();
            dpdLang = new ComboBox();
            lblLang = new Label();
            numH = new NumericUpDown();
            lblWH = new Label();
            numW = new NumericUpDown();
            fraMisc = new GroupBox();
            cmdReset = new Button();
            fraSize = new GroupBox();
            cdlClearCol = new ColorDialog();
            fraNodeShadeAmt = new GroupBox();
            sldNodeShadeAmt = new TrackBar();
            tableLayoutPanel5 = new TableLayoutPanel();
            panel3 = new Panel();
            panel4 = new Panel();
            tableLayoutPanel3 = new TableLayoutPanel();
            tableLayoutPanel4 = new TableLayoutPanel();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel2 = new Panel();
            panel1 = new Panel();
            tableLayoutPanel3.SuspendLayout();
            fraAnimGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numAnimGrid).BeginInit();
            fraClearCol.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picClearCol).BeginInit();
            fraFont.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numFontSize).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numH).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numW).BeginInit();
            fraMisc.SuspendLayout();
            fraSize.SuspendLayout();
            fraNodeShadeAmt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)sldNodeShadeAmt).BeginInit();
            tableLayoutPanel5.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.AutoSize = true;
            tableLayoutPanel3.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel3.ColumnCount = 4;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 168F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 93F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 280F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel3.Controls.Add(fraAnimGrid, 0, 0);
            tableLayoutPanel3.Controls.Add(fraClearCol, 1, 0);
            tableLayoutPanel3.Controls.Add(fraFont, 2, 0);
            tableLayoutPanel3.Dock = DockStyle.Top;
            tableLayoutPanel3.Location = new Point(4, 3);
            tableLayoutPanel3.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tableLayoutPanel3.Size = new Size(545, 100);
            tableLayoutPanel3.TabIndex = 3;
            // 
            // fraAnimGrid
            // 
            fraAnimGrid.AutoSize = true;
            fraAnimGrid.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            fraAnimGrid.Controls.Add(numAnimGrid);
            fraAnimGrid.Controls.Add(lblAnimGrid);
            fraAnimGrid.Controls.Add(chkAnimGrid);
            fraAnimGrid.Dock = DockStyle.Fill;
            fraAnimGrid.Location = new Point(4, 3);
            fraAnimGrid.Margin = new Padding(4, 3, 4, 3);
            fraAnimGrid.Name = "fraAnimGrid";
            fraAnimGrid.Padding = new Padding(4, 3, 4, 3);
            fraAnimGrid.Size = new Size(160, 94);
            fraAnimGrid.TabIndex = 6;
            fraAnimGrid.TabStop = false;
            fraAnimGrid.Text = "3D Viewer World Grid";
            // 
            // numAnimGrid
            // 
            numAnimGrid.Location = new Point(66, 48);
            numAnimGrid.Margin = new Padding(4, 3, 4, 3);
            numAnimGrid.Maximum = new decimal(new int[] { 32767, 0, 0, 0 });
            numAnimGrid.Name = "numAnimGrid";
            numAnimGrid.Size = new Size(86, 23);
            numAnimGrid.TabIndex = 2;
            numAnimGrid.Value = new decimal(new int[] { 4, 0, 0, 0 });
            numAnimGrid.ValueChanged += numAnimGrid_ValueChanged;
            // 
            // lblAnimGrid
            // 
            lblAnimGrid.AutoSize = true;
            lblAnimGrid.Location = new Point(8, 50);
            lblAnimGrid.Margin = new Padding(4, 0, 4, 0);
            lblAnimGrid.Name = "lblAnimGrid";
            lblAnimGrid.Size = new Size(51, 15);
            lblAnimGrid.TabIndex = 1;
            lblAnimGrid.Text = "Amount";
            // 
            // chkAnimGrid
            // 
            chkAnimGrid.AutoSize = true;
            chkAnimGrid.Location = new Point(8, 22);
            chkAnimGrid.Margin = new Padding(4, 3, 4, 3);
            chkAnimGrid.Name = "chkAnimGrid";
            chkAnimGrid.Size = new Size(68, 19);
            chkAnimGrid.TabIndex = 0;
            chkAnimGrid.Text = "Enabled";
            chkAnimGrid.UseVisualStyleBackColor = true;
            chkAnimGrid.CheckedChanged += chkAnimGrid_CheckedChanged;
            // 
            // fraClearCol
            // 
            fraClearCol.AutoSize = true;
            fraClearCol.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            fraClearCol.Controls.Add(picClearCol);
            fraClearCol.Dock = DockStyle.Fill;
            fraClearCol.Location = new Point(172, 3);
            fraClearCol.Margin = new Padding(4, 3, 4, 3);
            fraClearCol.Name = "fraClearCol";
            fraClearCol.Padding = new Padding(4, 3, 4, 3);
            fraClearCol.Size = new Size(85, 94);
            fraClearCol.TabIndex = 4;
            fraClearCol.TabStop = false;
            fraClearCol.Text = "Clear Color";
            // 
            // picClearCol
            // 
            picClearCol.BorderStyle = BorderStyle.FixedSingle;
            picClearCol.Location = new Point(7, 22);
            picClearCol.Margin = new Padding(4, 3, 4, 3);
            picClearCol.Name = "picClearCol";
            picClearCol.Size = new Size(70, 54);
            picClearCol.TabIndex = 0;
            picClearCol.TabStop = false;
            picClearCol.Click += pictureBox1_Click;
            // 
            // fraFont
            // 
            fraFont.AutoSize = true;
            fraFont.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            fraFont.Controls.Add(tableLayoutPanel4);
            fraFont.Dock = DockStyle.Fill;
            fraFont.Location = new Point(265, 3);
            fraFont.Margin = new Padding(4, 3, 4, 3);
            fraFont.Name = "fraFont";
            fraFont.Padding = new Padding(4, 3, 4, 3);
            fraFont.Size = new Size(272, 94);
            fraFont.TabIndex = 8;
            fraFont.TabStop = false;
            fraFont.Text = "Font Renderer";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            tableLayoutPanel4.AutoSize = true;
            tableLayoutPanel4.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 62F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Controls.Add(lblFontName, 0, 0);
            tableLayoutPanel4.Controls.Add(lblFontSize, 0, 1);
            tableLayoutPanel4.Controls.Add(dpdFont, 1, 0);
            tableLayoutPanel4.Controls.Add(numFontSize, 1, 1);
            tableLayoutPanel4.Location = new Point(7, 18);
            tableLayoutPanel4.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle());
            tableLayoutPanel4.RowStyles.Add(new RowStyle());
            tableLayoutPanel4.Size = new Size(261, 58);
            tableLayoutPanel4.TabIndex = 5;
            // 
            // lblFontName
            // 
            lblFontName.AutoSize = true;
            lblFontName.Dock = DockStyle.Fill;
            lblFontName.Location = new Point(4, 0);
            lblFontName.Margin = new Padding(4, 0, 4, 0);
            lblFontName.Name = "lblFontName";
            lblFontName.Size = new Size(54, 29);
            lblFontName.TabIndex = 3;
            lblFontName.Text = "Font";
            lblFontName.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblFontSize
            // 
            lblFontSize.AutoSize = true;
            lblFontSize.Dock = DockStyle.Fill;
            lblFontSize.Location = new Point(4, 29);
            lblFontSize.Margin = new Padding(4, 0, 4, 0);
            lblFontSize.Name = "lblFontSize";
            lblFontSize.Size = new Size(54, 29);
            lblFontSize.TabIndex = 4;
            lblFontSize.Text = "Font Size";
            lblFontSize.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dpdFont
            // 
            dpdFont.DropDownStyle = ComboBoxStyle.DropDownList;
            dpdFont.FormattingEnabled = true;
            dpdFont.Location = new Point(66, 3);
            dpdFont.Margin = new Padding(4, 3, 4, 3);
            dpdFont.Name = "dpdFont";
            dpdFont.Size = new Size(191, 23);
            dpdFont.TabIndex = 1;
            // 
            // numFontSize
            // 
            numFontSize.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            numFontSize.Location = new Point(66, 32);
            numFontSize.Margin = new Padding(4, 3, 4, 3);
            numFontSize.Maximum = new decimal(new int[] { 99, 0, 0, 0 });
            numFontSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numFontSize.Name = "numFontSize";
            numFontSize.Size = new Size(63, 23);
            numFontSize.TabIndex = 3;
            numFontSize.Value = new decimal(new int[] { 20, 0, 0, 0 });
            numFontSize.ValueChanged += numFontSize_ValueChanged;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(panel2, 0, 1);
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(4, 19);
            tableLayoutPanel1.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(526, 239);
            tableLayoutPanel1.TabIndex = 9;
            // 
            // panel2
            // 
            panel2.AutoSize = true;
            panel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel2.Controls.Add(chkViewerShowHelp);
            panel2.Controls.Add(chkFont2DEnable);
            panel2.Controls.Add(chkFont3DAutoscale);
            panel2.Controls.Add(chkFont3DEnable);
            panel2.Controls.Add(chkCollisionDisplay);
            panel2.Controls.Add(chkNormalDisplay);
            panel2.Controls.Add(chkPatchNSDSavesNSF);
            panel2.Controls.Add(chkDeleteInvalidEntries);
            panel2.Controls.Add(chkUseAnimLinks);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(4, 53);
            panel2.Margin = new Padding(4, 3, 4, 3);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(12, 6, 12, 6);
            panel2.Size = new Size(518, 183);
            panel2.TabIndex = 11;
            // 
            // chkViewerShowHelp
            // 
            chkViewerShowHelp.AutoSize = true;
            chkViewerShowHelp.Checked = true;
            chkViewerShowHelp.CheckState = CheckState.Checked;
            chkViewerShowHelp.Dock = DockStyle.Top;
            chkViewerShowHelp.Location = new Point(12, 158);
            chkViewerShowHelp.Margin = new Padding(4, 3, 4, 3);
            chkViewerShowHelp.Name = "chkViewerShowHelp";
            chkViewerShowHelp.Size = new Size(494, 19);
            chkViewerShowHelp.TabIndex = 7;
            chkViewerShowHelp.Text = "Display help text by default";
            chkViewerShowHelp.UseVisualStyleBackColor = true;
            chkViewerShowHelp.CheckedChanged += chkViewerShowHelp_CheckedChanged;
            // 
            // chkFont2DEnable
            // 
            chkFont2DEnable.AutoSize = true;
            chkFont2DEnable.Checked = true;
            chkFont2DEnable.CheckState = CheckState.Checked;
            chkFont2DEnable.Dock = DockStyle.Top;
            chkFont2DEnable.Location = new Point(12, 139);
            chkFont2DEnable.Margin = new Padding(4, 3, 4, 3);
            chkFont2DEnable.Name = "chkFont2DEnable";
            chkFont2DEnable.Size = new Size(494, 19);
            chkFont2DEnable.TabIndex = 6;
            chkFont2DEnable.Text = "Display debug text";
            chkFont2DEnable.UseVisualStyleBackColor = true;
            chkFont2DEnable.CheckedChanged += chkFont2DEnable_CheckedChanged;
            // 
            // chkFont3DAutoscale
            // 
            chkFont3DAutoscale.AutoSize = true;
            chkFont3DAutoscale.Checked = true;
            chkFont3DAutoscale.CheckState = CheckState.Checked;
            chkFont3DAutoscale.Dock = DockStyle.Top;
            chkFont3DAutoscale.Location = new Point(12, 120);
            chkFont3DAutoscale.Margin = new Padding(4, 3, 4, 3);
            chkFont3DAutoscale.Name = "chkFont3DAutoscale";
            chkFont3DAutoscale.Size = new Size(494, 19);
            chkFont3DAutoscale.TabIndex = 7;
            chkFont3DAutoscale.Text = "Scale entity text";
            chkFont3DAutoscale.UseVisualStyleBackColor = true;
            chkFont3DAutoscale.CheckedChanged += chkFont3DAutoscale_CheckedChanged;
            // 
            // chkFont3DEnable
            // 
            chkFont3DEnable.AutoSize = true;
            chkFont3DEnable.Checked = true;
            chkFont3DEnable.CheckState = CheckState.Checked;
            chkFont3DEnable.Dock = DockStyle.Top;
            chkFont3DEnable.Location = new Point(12, 101);
            chkFont3DEnable.Margin = new Padding(4, 3, 4, 3);
            chkFont3DEnable.Name = "chkFont3DEnable";
            chkFont3DEnable.Size = new Size(494, 19);
            chkFont3DEnable.TabIndex = 5;
            chkFont3DEnable.Text = "Display entity text";
            chkFont3DEnable.UseVisualStyleBackColor = true;
            chkFont3DEnable.CheckedChanged += chkFont3DEnable_CheckedChanged;
            // 
            // chkCollisionDisplay
            // 
            chkCollisionDisplay.AutoSize = true;
            chkCollisionDisplay.Checked = true;
            chkCollisionDisplay.CheckState = CheckState.Checked;
            chkCollisionDisplay.Dock = DockStyle.Top;
            chkCollisionDisplay.Location = new Point(12, 82);
            chkCollisionDisplay.Margin = new Padding(4, 3, 4, 3);
            chkCollisionDisplay.Name = "chkCollisionDisplay";
            chkCollisionDisplay.Size = new Size(494, 19);
            chkCollisionDisplay.TabIndex = 2;
            chkCollisionDisplay.Text = "Display frame collision by default";
            chkCollisionDisplay.UseVisualStyleBackColor = true;
            chkCollisionDisplay.CheckedChanged += chkCollisionDisplay_CheckedChanged;
            // 
            // chkNormalDisplay
            // 
            chkNormalDisplay.AutoSize = true;
            chkNormalDisplay.Checked = true;
            chkNormalDisplay.CheckState = CheckState.Checked;
            chkNormalDisplay.Dock = DockStyle.Top;
            chkNormalDisplay.Location = new Point(12, 63);
            chkNormalDisplay.Margin = new Padding(4, 3, 4, 3);
            chkNormalDisplay.Name = "chkNormalDisplay";
            chkNormalDisplay.Size = new Size(494, 19);
            chkNormalDisplay.TabIndex = 0;
            chkNormalDisplay.Text = "Display normals";
            chkNormalDisplay.UseVisualStyleBackColor = true;
            chkNormalDisplay.CheckedChanged += chkNormalDisplay_CheckedChanged;
            // 
            // chkPatchNSDSavesNSF
            // 
            chkPatchNSDSavesNSF.AutoSize = true;
            chkPatchNSDSavesNSF.Checked = true;
            chkPatchNSDSavesNSF.CheckState = CheckState.Checked;
            chkPatchNSDSavesNSF.Dock = DockStyle.Top;
            chkPatchNSDSavesNSF.Location = new Point(12, 44);
            chkPatchNSDSavesNSF.Margin = new Padding(4, 3, 4, 3);
            chkPatchNSDSavesNSF.Name = "chkPatchNSDSavesNSF";
            chkPatchNSDSavesNSF.Size = new Size(494, 19);
            chkPatchNSDSavesNSF.TabIndex = 7;
            chkPatchNSDSavesNSF.Text = "(Patch NSD) Always save NSF after NSD patching";
            chkPatchNSDSavesNSF.UseVisualStyleBackColor = true;
            chkPatchNSDSavesNSF.CheckedChanged += chkPatchNSDSavesNSF_CheckedChanged;
            // 
            // chkDeleteInvalidEntries
            // 
            chkDeleteInvalidEntries.AutoSize = true;
            chkDeleteInvalidEntries.Checked = true;
            chkDeleteInvalidEntries.CheckState = CheckState.Checked;
            chkDeleteInvalidEntries.Dock = DockStyle.Top;
            chkDeleteInvalidEntries.Location = new Point(12, 25);
            chkDeleteInvalidEntries.Margin = new Padding(4, 3, 4, 3);
            chkDeleteInvalidEntries.Name = "chkDeleteInvalidEntries";
            chkDeleteInvalidEntries.Size = new Size(494, 19);
            chkDeleteInvalidEntries.TabIndex = 5;
            chkDeleteInvalidEntries.Text = "(Patch NSD) Delete non-existent entries from load lists";
            chkDeleteInvalidEntries.UseVisualStyleBackColor = true;
            chkDeleteInvalidEntries.CheckedChanged += chkDeleteInvalidEntries_CheckedChanged;
            // 
            // chkUseAnimLinks
            // 
            chkUseAnimLinks.AutoSize = true;
            chkUseAnimLinks.Checked = true;
            chkUseAnimLinks.CheckState = CheckState.Checked;
            chkUseAnimLinks.Dock = DockStyle.Top;
            chkUseAnimLinks.Location = new Point(12, 6);
            chkUseAnimLinks.Margin = new Padding(4, 3, 4, 3);
            chkUseAnimLinks.Name = "chkUseAnimLinks";
            chkUseAnimLinks.Size = new Size(494, 19);
            chkUseAnimLinks.TabIndex = 3;
            chkUseAnimLinks.Text = "(Crash 3) Used saved animation-model links";
            chkUseAnimLinks.UseVisualStyleBackColor = true;
            chkUseAnimLinks.CheckedChanged += chkUseAnimLinks_CheckedChanged;
            // 
            // panel1
            // 
            panel1.AutoSize = true;
            panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel1.Controls.Add(dpdLang);
            panel1.Controls.Add(lblLang);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(4, 3);
            panel1.Margin = new Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(12, 0, 12, 0);
            panel1.Size = new Size(518, 44);
            panel1.TabIndex = 10;
            // 
            // dpdLang
            // 
            dpdLang.Dock = DockStyle.Top;
            dpdLang.DropDownStyle = ComboBoxStyle.DropDownList;
            dpdLang.FormattingEnabled = true;
            dpdLang.Location = new Point(12, 21);
            dpdLang.Margin = new Padding(4, 3, 4, 3);
            dpdLang.MaximumSize = new Size(154, 0);
            dpdLang.Name = "dpdLang";
            dpdLang.Size = new Size(154, 23);
            dpdLang.TabIndex = 0;
            // 
            // lblLang
            // 
            lblLang.AutoSize = true;
            lblLang.Dock = DockStyle.Top;
            lblLang.Location = new Point(12, 0);
            lblLang.Margin = new Padding(4, 0, 4, 0);
            lblLang.Name = "lblLang";
            lblLang.Padding = new Padding(0, 0, 0, 6);
            lblLang.Size = new Size(148, 21);
            lblLang.TabIndex = 8;
            lblLang.Text = "Language (requires restart)";
            // 
            // numH
            // 
            numH.Location = new Point(88, 22);
            numH.Margin = new Padding(4, 3, 4, 3);
            numH.Maximum = new decimal(new int[] { 4096, 0, 0, 0 });
            numH.MaximumSize = new Size(88, 0);
            numH.Minimum = new decimal(new int[] { 480, 0, 0, 0 });
            numH.Name = "numH";
            numH.Size = new Size(51, 23);
            numH.TabIndex = 1;
            numH.Value = new decimal(new int[] { 480, 0, 0, 0 });
            numH.ValueChanged += numH_ValueChanged;
            // 
            // lblWH
            // 
            lblWH.AutoSize = true;
            lblWH.Location = new Point(67, 24);
            lblWH.Margin = new Padding(4, 0, 4, 0);
            lblWH.Name = "lblWH";
            lblWH.Size = new Size(13, 15);
            lblWH.TabIndex = 2;
            lblWH.Text = "x";
            lblWH.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numW
            // 
            numW.Location = new Point(8, 22);
            numW.Margin = new Padding(4, 3, 4, 3);
            numW.Maximum = new decimal(new int[] { 8192, 0, 0, 0 });
            numW.MaximumSize = new Size(88, 0);
            numW.Minimum = new decimal(new int[] { 640, 0, 0, 0 });
            numW.Name = "numW";
            numW.Size = new Size(51, 23);
            numW.TabIndex = 0;
            numW.Value = new decimal(new int[] { 640, 0, 0, 0 });
            numW.ValueChanged += numW_ValueChanged;
            // 
            // fraMisc
            // 
            fraMisc.AutoSize = true;
            fraMisc.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            fraMisc.Controls.Add(tableLayoutPanel1);
            fraMisc.Dock = DockStyle.Fill;
            fraMisc.Location = new Point(4, 3);
            fraMisc.Margin = new Padding(7);
            fraMisc.MaximumSize = new Size(534, 0);
            fraMisc.Name = "fraMisc";
            fraMisc.Padding = new Padding(4, 3, 4, 3);
            fraMisc.Size = new Size(534, 261);
            fraMisc.TabIndex = 1;
            fraMisc.TabStop = false;
            fraMisc.Text = "Miscellaneous";
            // 
            // cmdReset
            // 
            cmdReset.AutoSize = true;
            cmdReset.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            cmdReset.Location = new Point(10, 10);
            cmdReset.Margin = new Padding(4, 3, 4, 3);
            cmdReset.Name = "cmdReset";
            cmdReset.Size = new Size(90, 25);
            cmdReset.TabIndex = 1;
            cmdReset.Text = "Reset Settings";
            cmdReset.UseVisualStyleBackColor = true;
            cmdReset.Click += cmdReset_Click;
            // 
            // fraSize
            // 
            fraSize.AutoSize = true;
            fraSize.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            fraSize.Controls.Add(numW);
            fraSize.Controls.Add(numH);
            fraSize.Controls.Add(lblWH);
            fraSize.Dock = DockStyle.Fill;
            fraSize.Location = new Point(4, 3);
            fraSize.Margin = new Padding(4, 3, 4, 3);
            fraSize.Name = "fraSize";
            fraSize.Padding = new Padding(4, 3, 4, 3);
            fraSize.Size = new Size(160, 67);
            fraSize.TabIndex = 1;
            fraSize.TabStop = false;
            fraSize.Text = "Default Window Size";
            // 
            // cdlClearCol
            // 
            cdlClearCol.AnyColor = true;
            cdlClearCol.FullOpen = true;
            cdlClearCol.SolidColorOnly = true;
            // 
            // fraNodeShadeAmt
            // 
            fraNodeShadeAmt.AutoSize = true;
            fraNodeShadeAmt.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            fraNodeShadeAmt.Controls.Add(sldNodeShadeAmt);
            fraNodeShadeAmt.Dock = DockStyle.Fill;
            fraNodeShadeAmt.Location = new Point(172, 3);
            fraNodeShadeAmt.Margin = new Padding(4, 3, 4, 3);
            fraNodeShadeAmt.Name = "fraNodeShadeAmt";
            fraNodeShadeAmt.Padding = new Padding(4, 3, 4, 3);
            fraNodeShadeAmt.Size = new Size(365, 67);
            fraNodeShadeAmt.TabIndex = 10;
            fraNodeShadeAmt.TabStop = false;
            fraNodeShadeAmt.Text = "Collision Node Shade Amount";
            // 
            // sldNodeShadeAmt
            // 
            sldNodeShadeAmt.Dock = DockStyle.Fill;
            sldNodeShadeAmt.LargeChange = 10;
            sldNodeShadeAmt.Location = new Point(4, 19);
            sldNodeShadeAmt.Margin = new Padding(4, 3, 4, 3);
            sldNodeShadeAmt.Maximum = 100;
            sldNodeShadeAmt.Name = "sldNodeShadeAmt";
            sldNodeShadeAmt.Size = new Size(357, 45);
            sldNodeShadeAmt.TabIndex = 0;
            sldNodeShadeAmt.TickFrequency = 5;
            sldNodeShadeAmt.Value = 20;
            sldNodeShadeAmt.Scroll += sldNodeShadeAmt_Scroll;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.AutoSize = true;
            tableLayoutPanel5.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel5.ColumnCount = 3;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 168F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 373F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel5.Controls.Add(fraNodeShadeAmt, 1, 0);
            tableLayoutPanel5.Controls.Add(fraSize, 0, 0);
            tableLayoutPanel5.Dock = DockStyle.Top;
            tableLayoutPanel5.Location = new Point(4, 103);
            tableLayoutPanel5.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new RowStyle());
            tableLayoutPanel5.Size = new Size(545, 73);
            tableLayoutPanel5.TabIndex = 11;
            // 
            // panel3
            // 
            panel3.AutoSize = true;
            panel3.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel3.Controls.Add(cmdReset);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(4, 443);
            panel3.Margin = new Padding(4, 3, 4, 3);
            panel3.Name = "panel3";
            panel3.Padding = new Padding(4, 3, 4, 3);
            panel3.Size = new Size(545, 41);
            panel3.TabIndex = 12;
            // 
            // panel4
            // 
            panel4.AutoSize = true;
            panel4.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel4.Controls.Add(fraMisc);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(4, 176);
            panel4.Margin = new Padding(4, 3, 4, 3);
            panel4.Name = "panel4";
            panel4.Padding = new Padding(4, 3, 4, 3);
            panel4.Size = new Size(545, 267);
            panel4.TabIndex = 13;
            // 
            // ConfigEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = SystemColors.ButtonHighlight;
            Controls.Add(panel3);
            Controls.Add(panel4);
            Controls.Add(tableLayoutPanel5);
            Controls.Add(tableLayoutPanel3);
            Margin = new Padding(4, 3, 4, 3);
            Name = "ConfigEditor";
            Padding = new Padding(4, 3, 4, 3);
            Size = new Size(553, 551);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            fraAnimGrid.ResumeLayout(false);
            fraAnimGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numAnimGrid).EndInit();
            fraClearCol.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picClearCol).EndInit();
            fraFont.ResumeLayout(false);
            fraFont.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numFontSize).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numH).EndInit();
            ((System.ComponentModel.ISupportInitialize)numW).EndInit();
            fraMisc.ResumeLayout(false);
            fraMisc.PerformLayout();
            fraSize.ResumeLayout(false);
            fraSize.PerformLayout();
            fraNodeShadeAmt.ResumeLayout(false);
            fraNodeShadeAmt.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)sldNodeShadeAmt).EndInit();
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button cmdReset;
        private System.Windows.Forms.GroupBox fraSize;
        private System.Windows.Forms.Label lblWH;
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
    }
}
