using Crash;

namespace CrashEdit
{
    partial class ErrorReporter
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
            ErrorManager.Signal -= ErrorManager_Signal;
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.pnOptions = new System.Windows.Forms.Panel();
            this.optIgnore = new System.Windows.Forms.RadioButton();
            this.optSkip = new System.Windows.Forms.RadioButton();
            this.optAbort = new System.Windows.Forms.RadioButton();
            this.optBreak = new System.Windows.Forms.RadioButton();
            this.cmdOK = new System.Windows.Forms.Button();
            this.pnOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif",15.75F,System.Drawing.FontStyle.Regular,System.Drawing.GraphicsUnit.Point,((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(12,9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(390,32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "An error occurred.";
            // 
            // lblMessage
            // 
            this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMessage.Location = new System.Drawing.Point(12,41);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(390,40);
            this.lblMessage.TabIndex = 1;
            this.lblMessage.Text = "<MESSAGE>";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnOptions
            // 
            this.pnOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnOptions.Controls.Add(this.optIgnore);
            this.pnOptions.Controls.Add(this.optSkip);
            this.pnOptions.Controls.Add(this.optAbort);
            this.pnOptions.Controls.Add(this.optBreak);
            this.pnOptions.Location = new System.Drawing.Point(12,84);
            this.pnOptions.Name = "pnOptions";
            this.pnOptions.Size = new System.Drawing.Size(390,99);
            this.pnOptions.TabIndex = 2;
            // 
            // optIgnore
            // 
            this.optIgnore.AutoSize = true;
            this.optIgnore.Location = new System.Drawing.Point(5,49);
            this.optIgnore.Name = "optIgnore";
            this.optIgnore.Size = new System.Drawing.Size(204,17);
            this.optIgnore.TabIndex = 3;
            this.optIgnore.Text = "Ignore the error and continue anyway.";
            this.optIgnore.UseVisualStyleBackColor = true;
            // 
            // optSkip
            // 
            this.optSkip.AutoSize = true;
            this.optSkip.Location = new System.Drawing.Point(5,26);
            this.optSkip.Name = "optSkip";
            this.optSkip.Size = new System.Drawing.Size(212,17);
            this.optSkip.TabIndex = 2;
            this.optSkip.Text = "Skip this object, leaving it unprocessed.";
            this.optSkip.UseVisualStyleBackColor = true;
            // 
            // optAbort
            // 
            this.optAbort.AutoSize = true;
            this.optAbort.Checked = true;
            this.optAbort.Location = new System.Drawing.Point(5,3);
            this.optAbort.Name = "optAbort";
            this.optAbort.Size = new System.Drawing.Size(119,17);
            this.optAbort.TabIndex = 1;
            this.optAbort.TabStop = true;
            this.optAbort.Text = "Abort this operation.";
            this.optAbort.UseVisualStyleBackColor = true;
            // 
            // optBreak
            // 
            this.optBreak.AutoSize = true;
            this.optBreak.Location = new System.Drawing.Point(5,72);
            this.optBreak.Name = "optBreak";
            this.optBreak.Size = new System.Drawing.Size(209,17);
            this.optBreak.TabIndex = 0;
            this.optBreak.Text = "Break out to a debugger. (Experts only)";
            this.optBreak.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.Location = new System.Drawing.Point(327,196);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75,23);
            this.cmdOK.TabIndex = 3;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // ErrorReporter
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414,231);
            this.ControlBox = false;
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.pnOptions);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ErrorReporter";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Error Reporter";
            this.pnOptions.ResumeLayout(false);
            this.pnOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Panel pnOptions;
        private System.Windows.Forms.RadioButton optSkip;
        private System.Windows.Forms.RadioButton optAbort;
        private System.Windows.Forms.RadioButton optBreak;
        private System.Windows.Forms.RadioButton optIgnore;
        private System.Windows.Forms.Button cmdOK;
    }
}