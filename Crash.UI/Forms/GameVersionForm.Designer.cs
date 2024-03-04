namespace CrashEdit.CrashUI
{
    partial class GameVersionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise,false.</param>
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
            lblMessage = new System.Windows.Forms.Label();
            fraRelease = new System.Windows.Forms.GroupBox();
            cmdCrash3 = new System.Windows.Forms.Button();
            cmdCrash2 = new System.Windows.Forms.Button();
            cmdCrash1 = new System.Windows.Forms.Button();
            fraPrerelease = new System.Windows.Forms.GroupBox();
            cmdCrash1Beta1995 = new System.Windows.Forms.Button();
            cmdCrash2Beta = new System.Windows.Forms.Button();
            cmdCrash1BetaMAY11 = new System.Windows.Forms.Button();
            cmdCrash1BetaMAR08 = new System.Windows.Forms.Button();
            cmdCancel = new System.Windows.Forms.Button();
            fraRelease.SuspendLayout();
            fraPrerelease.SuspendLayout();
            SuspendLayout();
            // 
            // lblMessage
            // 
            lblMessage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblMessage.Location = new System.Drawing.Point(14, 10);
            lblMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new System.Drawing.Size(438, 75);
            lblMessage.TabIndex = 0;
            lblMessage.Text = "<SELECT GAME MESSAGE>";
            lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fraRelease
            // 
            fraRelease.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            fraRelease.Controls.Add(cmdCrash3);
            fraRelease.Controls.Add(cmdCrash2);
            fraRelease.Controls.Add(cmdCrash1);
            fraRelease.Location = new System.Drawing.Point(14, 89);
            fraRelease.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            fraRelease.Name = "fraRelease";
            fraRelease.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            fraRelease.Size = new System.Drawing.Size(438, 204);
            fraRelease.TabIndex = 1;
            fraRelease.TabStop = false;
            fraRelease.Text = "<RELEASE>";
            // 
            // cmdCrash3
            // 
            cmdCrash3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmdCrash3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            cmdCrash3.Location = new System.Drawing.Point(7, 142);
            cmdCrash3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmdCrash3.Name = "cmdCrash3";
            cmdCrash3.Size = new System.Drawing.Size(424, 53);
            cmdCrash3.TabIndex = 4;
            cmdCrash3.Text = "Crash Bandicoot: Warped\r\nクラッシュバンディクー　3:　ブッとび！　世界一周";
            cmdCrash3.UseVisualStyleBackColor = true;
            cmdCrash3.Click += cmdCrash3_Click;
            // 
            // cmdCrash2
            // 
            cmdCrash2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmdCrash2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            cmdCrash2.Location = new System.Drawing.Point(7, 82);
            cmdCrash2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmdCrash2.Name = "cmdCrash2";
            cmdCrash2.Size = new System.Drawing.Size(424, 53);
            cmdCrash2.TabIndex = 3;
            cmdCrash2.Text = "Crash Bandicoot 2: Cortex Strikes Back\r\nクラッシュバンディクー　2:　コルテックスのぎゃくしゅう！";
            cmdCrash2.UseVisualStyleBackColor = true;
            cmdCrash2.Click += cmdCrash2_Click;
            // 
            // cmdCrash1
            // 
            cmdCrash1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmdCrash1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            cmdCrash1.Location = new System.Drawing.Point(7, 22);
            cmdCrash1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmdCrash1.Name = "cmdCrash1";
            cmdCrash1.Size = new System.Drawing.Size(424, 53);
            cmdCrash1.TabIndex = 2;
            cmdCrash1.Text = "Crash Bandicoot\r\nクラッシュバンディクー";
            cmdCrash1.UseVisualStyleBackColor = true;
            cmdCrash1.Click += cmdCrash1_Click;
            // 
            // fraPrerelease
            // 
            fraPrerelease.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            fraPrerelease.Controls.Add(cmdCrash1Beta1995);
            fraPrerelease.Controls.Add(cmdCrash2Beta);
            fraPrerelease.Controls.Add(cmdCrash1BetaMAY11);
            fraPrerelease.Controls.Add(cmdCrash1BetaMAR08);
            fraPrerelease.Location = new System.Drawing.Point(14, 300);
            fraPrerelease.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            fraPrerelease.Name = "fraPrerelease";
            fraPrerelease.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            fraPrerelease.Size = new System.Drawing.Size(438, 264);
            fraPrerelease.TabIndex = 5;
            fraPrerelease.TabStop = false;
            fraPrerelease.Text = "<PRERELEASE>";
            // 
            // cmdCrash1Beta1995
            // 
            cmdCrash1Beta1995.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmdCrash1Beta1995.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            cmdCrash1Beta1995.Location = new System.Drawing.Point(7, 22);
            cmdCrash1Beta1995.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmdCrash1Beta1995.Name = "cmdCrash1Beta1995";
            cmdCrash1Beta1995.Size = new System.Drawing.Size(424, 53);
            cmdCrash1Beta1995.TabIndex = 9;
            cmdCrash1Beta1995.Text = "Crash Bandicoot\r\n\"Early Prototype\" (1995)";
            cmdCrash1Beta1995.UseVisualStyleBackColor = true;
            cmdCrash1Beta1995.Click += cmdCrash1Beta1995_Click;
            // 
            // cmdCrash2Beta
            // 
            cmdCrash2Beta.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmdCrash2Beta.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            cmdCrash2Beta.Location = new System.Drawing.Point(7, 202);
            cmdCrash2Beta.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmdCrash2Beta.Name = "cmdCrash2Beta";
            cmdCrash2Beta.Size = new System.Drawing.Size(424, 53);
            cmdCrash2Beta.TabIndex = 8;
            cmdCrash2Beta.Text = "Crash Bandicoot 2: Cortex Strikes Back\r\n\"Review Copy\"";
            cmdCrash2Beta.UseVisualStyleBackColor = true;
            cmdCrash2Beta.Click += cmdCrash2Beta_Click;
            // 
            // cmdCrash1BetaMAY11
            // 
            cmdCrash1BetaMAY11.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmdCrash1BetaMAY11.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            cmdCrash1BetaMAY11.Location = new System.Drawing.Point(7, 142);
            cmdCrash1BetaMAY11.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmdCrash1BetaMAY11.Name = "cmdCrash1BetaMAY11";
            cmdCrash1BetaMAY11.Size = new System.Drawing.Size(424, 53);
            cmdCrash1BetaMAY11.TabIndex = 7;
            cmdCrash1BetaMAY11.Text = "Crash Bandicoot\r\n\"E3 Demo\" (May 11,1996)";
            cmdCrash1BetaMAY11.UseVisualStyleBackColor = true;
            cmdCrash1BetaMAY11.Click += cmdCrash1BetaMAY11_Click;
            // 
            // cmdCrash1BetaMAR08
            // 
            cmdCrash1BetaMAR08.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmdCrash1BetaMAR08.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            cmdCrash1BetaMAR08.Location = new System.Drawing.Point(7, 82);
            cmdCrash1BetaMAR08.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmdCrash1BetaMAR08.Name = "cmdCrash1BetaMAR08";
            cmdCrash1BetaMAR08.Size = new System.Drawing.Size(424, 53);
            cmdCrash1BetaMAR08.TabIndex = 6;
            cmdCrash1BetaMAR08.Text = "Crash Bandicoot\r\n\"Prototype\" (April 8,1996)";
            cmdCrash1BetaMAR08.UseVisualStyleBackColor = true;
            cmdCrash1BetaMAR08.Click += cmdCrash1BetaMAR08_Click;
            // 
            // cmdCancel
            // 
            cmdCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cmdCancel.Location = new System.Drawing.Point(364, 576);
            cmdCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmdCancel.Name = "cmdCancel";
            cmdCancel.Size = new System.Drawing.Size(88, 27);
            cmdCancel.TabIndex = 0;
            cmdCancel.Text = "<CANCEL>";
            cmdCancel.UseVisualStyleBackColor = true;
            cmdCancel.Click += cmdCancel_Click;
            // 
            // GameVersionForm
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            CancelButton = cmdCancel;
            ClientSize = new System.Drawing.Size(465, 616);
            Controls.Add(cmdCancel);
            Controls.Add(fraPrerelease);
            Controls.Add(fraRelease);
            Controls.Add(lblMessage);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "GameVersionForm";
            Text = "<GAME VERSION SELECTION>";
            fraRelease.ResumeLayout(false);
            fraPrerelease.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.GroupBox fraRelease;
        private System.Windows.Forms.GroupBox fraPrerelease;
        private System.Windows.Forms.Button cmdCrash1;
        private System.Windows.Forms.Button cmdCrash3;
        private System.Windows.Forms.Button cmdCrash2;
        private System.Windows.Forms.Button cmdCrash1BetaMAY11;
        private System.Windows.Forms.Button cmdCrash1BetaMAR08;
        private System.Windows.Forms.Button cmdCrash2Beta;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdCrash1Beta1995;
    }
}