namespace CrashEdit
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
            this.fraVersions = new System.Windows.Forms.GroupBox();
            this.optNone = new System.Windows.Forms.RadioButton();
            this.lblOther = new System.Windows.Forms.Label();
            this.optCrash2Beta = new System.Windows.Forms.RadioButton();
            this.optCrash1MAY11 = new System.Windows.Forms.RadioButton();
            this.optCrash1MAR08 = new System.Windows.Forms.RadioButton();
            this.lblBeta = new System.Windows.Forms.Label();
            this.optCrash3 = new System.Windows.Forms.RadioButton();
            this.optCrash2 = new System.Windows.Forms.RadioButton();
            this.lblRetail = new System.Windows.Forms.Label();
            this.optCrash1 = new System.Windows.Forms.RadioButton();
            this.cmdOK = new System.Windows.Forms.Button();
            this.fraVersions.SuspendLayout();
            this.SuspendLayout();
            // 
            // fraVersions
            // 
            this.fraVersions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fraVersions.Controls.Add(this.optNone);
            this.fraVersions.Controls.Add(this.lblOther);
            this.fraVersions.Controls.Add(this.optCrash2Beta);
            this.fraVersions.Controls.Add(this.optCrash1MAY11);
            this.fraVersions.Controls.Add(this.optCrash1MAR08);
            this.fraVersions.Controls.Add(this.lblBeta);
            this.fraVersions.Controls.Add(this.optCrash3);
            this.fraVersions.Controls.Add(this.optCrash2);
            this.fraVersions.Controls.Add(this.lblRetail);
            this.fraVersions.Controls.Add(this.optCrash1);
            this.fraVersions.Location = new System.Drawing.Point(12,12);
            this.fraVersions.Name = "fraVersions";
            this.fraVersions.Size = new System.Drawing.Size(248,244);
            this.fraVersions.TabIndex = 0;
            this.fraVersions.TabStop = false;
            this.fraVersions.Text = "Versions";
            // 
            // optNone
            // 
            this.optNone.AutoSize = true;
            this.optNone.Location = new System.Drawing.Point(6,217);
            this.optNone.Name = "optNone";
            this.optNone.Size = new System.Drawing.Size(51,17);
            this.optNone.TabIndex = 6;
            this.optNone.TabStop = true;
            this.optNone.Text = "None";
            this.optNone.UseVisualStyleBackColor = true;
            // 
            // lblOther
            // 
            this.lblOther.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblOther.Font = new System.Drawing.Font("Microsoft Sans Serif",8.25F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
            this.lblOther.Location = new System.Drawing.Point(6,194);
            this.lblOther.Name = "lblOther";
            this.lblOther.Size = new System.Drawing.Size(236,20);
            this.lblOther.TabIndex = 9;
            this.lblOther.Text = "Other";
            this.lblOther.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // optCrash2Beta
            // 
            this.optCrash2Beta.AutoSize = true;
            this.optCrash2Beta.Location = new System.Drawing.Point(6,174);
            this.optCrash2Beta.Name = "optCrash2Beta";
            this.optCrash2Beta.Size = new System.Drawing.Size(146,17);
            this.optCrash2Beta.TabIndex = 5;
            this.optCrash2Beta.TabStop = true;
            this.optCrash2Beta.Text = "Crash 2 EU Beta (SEP14)";
            this.optCrash2Beta.UseVisualStyleBackColor = true;
            // 
            // optCrash1MAY11
            // 
            this.optCrash1MAY11.AutoSize = true;
            this.optCrash1MAY11.Location = new System.Drawing.Point(6,151);
            this.optCrash1MAY11.Name = "optCrash1MAY11";
            this.optCrash1MAY11.Size = new System.Drawing.Size(226,17);
            this.optCrash1MAY11.TabIndex = 4;
            this.optCrash1MAY11.TabStop = true;
            this.optCrash1MAY11.Text = "Crash 1 US Beta (MAY11) aka \"E3 Demo\"";
            this.optCrash1MAY11.UseVisualStyleBackColor = true;
            // 
            // optCrash1MAR08
            // 
            this.optCrash1MAR08.AutoSize = true;
            this.optCrash1MAR08.Location = new System.Drawing.Point(6,128);
            this.optCrash1MAR08.Name = "optCrash1MAR08";
            this.optCrash1MAR08.Size = new System.Drawing.Size(228,17);
            this.optCrash1MAR08.TabIndex = 3;
            this.optCrash1MAR08.TabStop = true;
            this.optCrash1MAR08.Text = "Crash 1 US Beta (MAR08) aka \"Prototype\"";
            this.optCrash1MAR08.UseVisualStyleBackColor = true;
            // 
            // lblBeta
            // 
            this.lblBeta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBeta.Font = new System.Drawing.Font("Microsoft Sans Serif",8.25F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
            this.lblBeta.Location = new System.Drawing.Point(6,105);
            this.lblBeta.Name = "lblBeta";
            this.lblBeta.Size = new System.Drawing.Size(236,20);
            this.lblBeta.TabIndex = 5;
            this.lblBeta.Text = "Beta Versions";
            this.lblBeta.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // optCrash3
            // 
            this.optCrash3.AutoSize = true;
            this.optCrash3.Location = new System.Drawing.Point(6,85);
            this.optCrash3.Name = "optCrash3";
            this.optCrash3.Size = new System.Drawing.Size(156,17);
            this.optCrash3.TabIndex = 2;
            this.optCrash3.TabStop = true;
            this.optCrash3.Text = "Crash Bandicoot 3: Warped";
            this.optCrash3.UseVisualStyleBackColor = true;
            // 
            // optCrash2
            // 
            this.optCrash2.AutoSize = true;
            this.optCrash2.Location = new System.Drawing.Point(6,62);
            this.optCrash2.Name = "optCrash2";
            this.optCrash2.Size = new System.Drawing.Size(211,17);
            this.optCrash2.TabIndex = 1;
            this.optCrash2.TabStop = true;
            this.optCrash2.Text = "Crash Bandicoot 2: Cortex Strikes Back";
            this.optCrash2.UseVisualStyleBackColor = true;
            // 
            // lblRetail
            // 
            this.lblRetail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRetail.Font = new System.Drawing.Font("Microsoft Sans Serif",8.25F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
            this.lblRetail.Location = new System.Drawing.Point(6,16);
            this.lblRetail.Name = "lblRetail";
            this.lblRetail.Size = new System.Drawing.Size(236,20);
            this.lblRetail.TabIndex = 2;
            this.lblRetail.Text = "Retail Versions";
            this.lblRetail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // optCrash1
            // 
            this.optCrash1.AutoSize = true;
            this.optCrash1.Location = new System.Drawing.Point(6,39);
            this.optCrash1.Name = "optCrash1";
            this.optCrash1.Size = new System.Drawing.Size(103,17);
            this.optCrash1.TabIndex = 0;
            this.optCrash1.TabStop = true;
            this.optCrash1.Text = "Crash Bandicoot";
            this.optCrash1.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.Enabled = false;
            this.cmdOK.Location = new System.Drawing.Point(185,262);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75,23);
            this.cmdOK.TabIndex = 1;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // GameVersionForm
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(272,297);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.fraVersions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameVersionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CrashEdit Game Version Selection";
            this.fraVersions.ResumeLayout(false);
            this.fraVersions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox fraVersions;
        private System.Windows.Forms.RadioButton optCrash1;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.RadioButton optCrash1MAR08;
        private System.Windows.Forms.Label lblBeta;
        private System.Windows.Forms.RadioButton optCrash3;
        private System.Windows.Forms.RadioButton optCrash2;
        private System.Windows.Forms.Label lblRetail;
        private System.Windows.Forms.RadioButton optCrash1MAY11;
        private System.Windows.Forms.RadioButton optCrash2Beta;
        private System.Windows.Forms.Label lblOther;
        private System.Windows.Forms.RadioButton optNone;
    }
}