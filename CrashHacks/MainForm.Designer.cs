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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Zone",System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Scenery",System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Other",System.Windows.Forms.HorizontalAlignment.Left);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tbMain = new System.Windows.Forms.ToolStrip();
            this.tbiMakeISO = new System.Windows.Forms.ToolStripButton();
            this.tbiChooseGameVersion = new System.Windows.Forms.ToolStripButton();
            this.lblGameVersion = new System.Windows.Forms.ToolStripLabel();
            this.lsvScripts = new System.Windows.Forms.ListView();
            this.lscScriptName = new System.Windows.Forms.ColumnHeader();
            this.lscScriptStatus = new System.Windows.Forms.ColumnHeader();
            this.lblInfo = new System.Windows.Forms.Label();
            this.dlgOpenDirectory = new System.Windows.Forms.FolderBrowserDialog();
            this.dlgSaveISO = new System.Windows.Forms.SaveFileDialog();
            this.bgwMakeISO = new System.ComponentModel.BackgroundWorker();
            this.lblMessage = new System.Windows.Forms.Label();
            this.uxProgress = new System.Windows.Forms.ProgressBar();
            this.bgwMakeBIN = new System.ComponentModel.BackgroundWorker();
            this.tbMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbMain
            // 
            this.tbMain.ImageScalingSize = new System.Drawing.Size(24,24);
            this.tbMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbiMakeISO,
            this.tbiChooseGameVersion,
            this.lblGameVersion});
            this.tbMain.Location = new System.Drawing.Point(0,0);
            this.tbMain.Name = "tbMain";
            this.tbMain.Size = new System.Drawing.Size(552,39);
            this.tbMain.TabIndex = 0;
            this.tbMain.Text = "toolStrip1";
            // 
            // tbiMakeISO
            // 
            this.tbiMakeISO.Enabled = false;
            this.tbiMakeISO.Image = global::CrashHacks.Properties.Resources.media_optical_5;
            this.tbiMakeISO.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tbiMakeISO.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbiMakeISO.Name = "tbiMakeISO";
            this.tbiMakeISO.Size = new System.Drawing.Size(89,36);
            this.tbiMakeISO.Text = "Make ISO";
            this.tbiMakeISO.Click += new System.EventHandler(this.tbiMakeISO_Click);
            // 
            // tbiChooseGameVersion
            // 
            this.tbiChooseGameVersion.Image = global::CrashHacks.Properties.Resources.input_gaming_3;
            this.tbiChooseGameVersion.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tbiChooseGameVersion.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbiChooseGameVersion.Name = "tbiChooseGameVersion";
            this.tbiChooseGameVersion.Size = new System.Drawing.Size(102,36);
            this.tbiChooseGameVersion.Text = "Select Game";
            this.tbiChooseGameVersion.Click += new System.EventHandler(this.tbiChooseGameVersion_Click);
            // 
            // lblGameVersion
            // 
            this.lblGameVersion.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblGameVersion.Font = new System.Drawing.Font("Tahoma",14.25F,System.Drawing.FontStyle.Regular,System.Drawing.GraphicsUnit.Point,((byte)(0)));
            this.lblGameVersion.Name = "lblGameVersion";
            this.lblGameVersion.Size = new System.Drawing.Size(172,36);
            this.lblGameVersion.Text = "<GAME VERSION>";
            // 
            // lsvScripts
            // 
            this.lsvScripts.CheckBoxes = true;
            this.lsvScripts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lscScriptName,
            this.lscScriptStatus});
            this.lsvScripts.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewGroup1.Header = "Zone";
            listViewGroup1.Name = "zone";
            listViewGroup2.Header = "Scenery";
            listViewGroup2.Name = "scenery";
            listViewGroup3.Header = "Other";
            listViewGroup3.Name = "other";
            this.lsvScripts.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            this.lsvScripts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lsvScripts.Location = new System.Drawing.Point(0,39);
            this.lsvScripts.MultiSelect = false;
            this.lsvScripts.Name = "lsvScripts";
            this.lsvScripts.Size = new System.Drawing.Size(552,279);
            this.lsvScripts.TabIndex = 1;
            this.lsvScripts.UseCompatibleStateImageBehavior = false;
            this.lsvScripts.View = System.Windows.Forms.View.Details;
            this.lsvScripts.Visible = false;
            this.lsvScripts.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lsvScripts_ItemSelectionChanged);
            // 
            // lscScriptName
            // 
            this.lscScriptName.Text = "Name";
            this.lscScriptName.Width = 421;
            // 
            // lscScriptStatus
            // 
            this.lscScriptStatus.Text = "Status";
            this.lscScriptStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.lscScriptStatus.Width = 100;
            // 
            // lblInfo
            // 
            this.lblInfo.BackColor = System.Drawing.SystemColors.Control;
            this.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblInfo.Location = new System.Drawing.Point(0,318);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(552,152);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Text = resources.GetString("lblInfo.Text");
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dlgSaveISO
            // 
            this.dlgSaveISO.Filter = "Playstation Disc Images (*.bin)|*.bin";
            this.dlgSaveISO.Title = "Save ISO";
            // 
            // bgwMakeISO
            // 
            this.bgwMakeISO.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwMakeISO_DoWork);
            this.bgwMakeISO.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwMakeISO_RunWorkerCompleted);
            // 
            // lblMessage
            // 
            this.lblMessage.BackColor = System.Drawing.SystemColors.Control;
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif",36F,System.Drawing.FontStyle.Regular,System.Drawing.GraphicsUnit.Point,((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(0,39);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(552,279);
            this.lblMessage.TabIndex = 3;
            this.lblMessage.Text = "Read the instructions below";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uxProgress
            // 
            this.uxProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.uxProgress.BackColor = System.Drawing.SystemColors.Control;
            this.uxProgress.Location = new System.Drawing.Point(12,440);
            this.uxProgress.MarqueeAnimationSpeed = 25;
            this.uxProgress.Name = "uxProgress";
            this.uxProgress.Size = new System.Drawing.Size(528,18);
            this.uxProgress.TabIndex = 4;
            this.uxProgress.Visible = false;
            // 
            // bgwMakeBIN
            // 
            this.bgwMakeBIN.WorkerReportsProgress = true;
            this.bgwMakeBIN.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwMakeBIN_DoWork);
            this.bgwMakeBIN.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwMakeBIN_RunWorkerCompleted);
            this.bgwMakeBIN.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwMakeBIN_ProgressChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(552,470);
            this.Controls.Add(this.uxProgress);
            this.Controls.Add(this.lsvScripts);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.tbMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "CrashHacks";
            this.tbMain.ResumeLayout(false);
            this.tbMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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