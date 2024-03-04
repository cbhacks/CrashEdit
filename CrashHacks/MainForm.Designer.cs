namespace CrashHacks
{
    partial class MainForm
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
            ListViewGroup listViewGroup1 = new ListViewGroup("Zone", HorizontalAlignment.Left);
            ListViewGroup listViewGroup2 = new ListViewGroup("Scenery", HorizontalAlignment.Left);
            ListViewGroup listViewGroup3 = new ListViewGroup("Other", HorizontalAlignment.Left);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            tbMain = new ToolStrip();
            tbiMakeISO = new ToolStripButton();
            tbiChooseGameVersion = new ToolStripButton();
            lblGameVersion = new ToolStripLabel();
            lsvScripts = new ListView();
            lscScriptName = new ColumnHeader();
            lscScriptStatus = new ColumnHeader();
            lblInfo = new Label();
            dlgOpenDirectory = new FolderBrowserDialog();
            dlgSaveISO = new SaveFileDialog();
            bgwMakeISO = new System.ComponentModel.BackgroundWorker();
            lblMessage = new Label();
            uxProgress = new ProgressBar();
            bgwMakeBIN = new System.ComponentModel.BackgroundWorker();
            tbMain.SuspendLayout();
            SuspendLayout();
            // 
            // tbMain
            // 
            tbMain.ImageScalingSize = new Size(24, 24);
            tbMain.Items.AddRange(new ToolStripItem[] { tbiMakeISO, tbiChooseGameVersion, lblGameVersion });
            tbMain.Location = new Point(0, 0);
            tbMain.Name = "tbMain";
            tbMain.Size = new Size(644, 39);
            tbMain.TabIndex = 0;
            tbMain.Text = "toolStrip1";
            // 
            // tbiMakeISO
            // 
            tbiMakeISO.Enabled = false;
            tbiMakeISO.Image = Properties.Resources.media_optical_5;
            tbiMakeISO.ImageScaling = ToolStripItemImageScaling.None;
            tbiMakeISO.ImageTransparentColor = Color.Magenta;
            tbiMakeISO.Name = "tbiMakeISO";
            tbiMakeISO.Size = new Size(93, 36);
            tbiMakeISO.Text = "Make ISO";
            tbiMakeISO.Click += tbiMakeISO_Click;
            // 
            // tbiChooseGameVersion
            // 
            tbiChooseGameVersion.Image = Properties.Resources.input_gaming_3;
            tbiChooseGameVersion.ImageScaling = ToolStripItemImageScaling.None;
            tbiChooseGameVersion.ImageTransparentColor = Color.Magenta;
            tbiChooseGameVersion.Name = "tbiChooseGameVersion";
            tbiChooseGameVersion.Size = new Size(108, 36);
            tbiChooseGameVersion.Text = "Select Game";
            tbiChooseGameVersion.Click += tbiChooseGameVersion_Click;
            // 
            // lblGameVersion
            // 
            lblGameVersion.Alignment = ToolStripItemAlignment.Right;
            lblGameVersion.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblGameVersion.Name = "lblGameVersion";
            lblGameVersion.Size = new Size(172, 36);
            lblGameVersion.Text = "<GAME VERSION>";
            // 
            // lsvScripts
            // 
            lsvScripts.CheckBoxes = true;
            lsvScripts.Columns.AddRange(new ColumnHeader[] { lscScriptName, lscScriptStatus });
            lsvScripts.Dock = DockStyle.Fill;
            listViewGroup1.Header = "Zone";
            listViewGroup1.Name = "zone";
            listViewGroup2.Header = "Scenery";
            listViewGroup2.Name = "scenery";
            listViewGroup3.Header = "Other";
            listViewGroup3.Name = "other";
            lsvScripts.Groups.AddRange(new ListViewGroup[] { listViewGroup1, listViewGroup2, listViewGroup3 });
            lsvScripts.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lsvScripts.Location = new Point(0, 39);
            lsvScripts.Margin = new Padding(4, 3, 4, 3);
            lsvScripts.MultiSelect = false;
            lsvScripts.Name = "lsvScripts";
            lsvScripts.Size = new Size(644, 328);
            lsvScripts.TabIndex = 1;
            lsvScripts.UseCompatibleStateImageBehavior = false;
            lsvScripts.View = View.Details;
            lsvScripts.Visible = false;
            lsvScripts.ItemSelectionChanged += lsvScripts_ItemSelectionChanged;
            // 
            // lscScriptName
            // 
            lscScriptName.Text = "Name";
            lscScriptName.Width = 421;
            // 
            // lscScriptStatus
            // 
            lscScriptStatus.Text = "Status";
            lscScriptStatus.TextAlign = HorizontalAlignment.Center;
            lscScriptStatus.Width = 100;
            // 
            // lblInfo
            // 
            lblInfo.BackColor = SystemColors.Control;
            lblInfo.BorderStyle = BorderStyle.Fixed3D;
            lblInfo.Dock = DockStyle.Bottom;
            lblInfo.Location = new Point(0, 367);
            lblInfo.Margin = new Padding(4, 0, 4, 0);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(644, 175);
            lblInfo.TabIndex = 2;
            lblInfo.Text = resources.GetString("lblInfo.Text");
            lblInfo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dlgSaveISO
            // 
            dlgSaveISO.Filter = "Playstation Disc Images (*.bin)|*.bin";
            dlgSaveISO.Title = "Save ISO";
            // 
            // bgwMakeISO
            // 
            bgwMakeISO.DoWork += bgwMakeISO_DoWork;
            bgwMakeISO.RunWorkerCompleted += bgwMakeISO_RunWorkerCompleted;
            // 
            // lblMessage
            // 
            lblMessage.BackColor = SystemColors.Control;
            lblMessage.Dock = DockStyle.Fill;
            lblMessage.Font = new Font("Microsoft Sans Serif", 36F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblMessage.Location = new Point(0, 39);
            lblMessage.Margin = new Padding(4, 0, 4, 0);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(644, 328);
            lblMessage.TabIndex = 3;
            lblMessage.Text = "Read the instructions below";
            lblMessage.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // uxProgress
            // 
            uxProgress.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            uxProgress.BackColor = SystemColors.Control;
            uxProgress.Location = new Point(14, 508);
            uxProgress.Margin = new Padding(4, 3, 4, 3);
            uxProgress.MarqueeAnimationSpeed = 25;
            uxProgress.Name = "uxProgress";
            uxProgress.Size = new Size(616, 21);
            uxProgress.TabIndex = 4;
            uxProgress.Visible = false;
            // 
            // bgwMakeBIN
            // 
            bgwMakeBIN.WorkerReportsProgress = true;
            bgwMakeBIN.DoWork += bgwMakeBIN_DoWork;
            bgwMakeBIN.ProgressChanged += bgwMakeBIN_ProgressChanged;
            bgwMakeBIN.RunWorkerCompleted += bgwMakeBIN_RunWorkerCompleted;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.AppWorkspace;
            ClientSize = new Size(644, 542);
            Controls.Add(uxProgress);
            Controls.Add(lsvScripts);
            Controls.Add(lblMessage);
            Controls.Add(lblInfo);
            Controls.Add(tbMain);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "MainForm";
            Text = "CrashHacks";
            tbMain.ResumeLayout(false);
            tbMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip tbMain;
        private System.Windows.Forms.ToolStripButton tbiMakeISO;
        private System.Windows.Forms.ToolStripButton tbiChooseGameVersion;
        private System.Windows.Forms.ListView lsvScripts;
        private System.Windows.Forms.ColumnHeader lscScriptName;
        private System.Windows.Forms.ColumnHeader lscScriptStatus;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.ToolStripLabel lblGameVersion;
        private System.Windows.Forms.FolderBrowserDialog dlgOpenDirectory;
        private System.Windows.Forms.SaveFileDialog dlgSaveISO;
        private System.ComponentModel.BackgroundWorker bgwMakeISO;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.ProgressBar uxProgress;
        private System.ComponentModel.BackgroundWorker bgwMakeBIN;
    }
}