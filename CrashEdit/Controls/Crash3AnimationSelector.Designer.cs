namespace CrashEdit
{
    partial class Crash3AnimationSelector
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
            this.lblDesc = new System.Windows.Forms.Label();
            this.txtEName = new System.Windows.Forms.TextBox();
            this.lblEIDErr = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblDesc
            // 
            this.lblDesc.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDesc.Location = new System.Drawing.Point(0, 0);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(350, 123);
            this.lblDesc.TabIndex = 0;
            this.lblDesc.Text = "Type in a Model (or Compressed Model) name and press Enter.";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // txtEName
            // 
            this.txtEName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtEName.Location = new System.Drawing.Point(145, 126);
            this.txtEName.MaxLength = 5;
            this.txtEName.Name = "txtEName";
            this.txtEName.Size = new System.Drawing.Size(60, 20);
            this.txtEName.TabIndex = 1;
            this.txtEName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtEName.TextChanged += new System.EventHandler(this.txtEName_TextChanged);
            // 
            // lblEIDErr
            // 
            this.lblEIDErr.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblEIDErr.ForeColor = System.Drawing.Color.Red;
            this.lblEIDErr.Location = new System.Drawing.Point(15, 149);
            this.lblEIDErr.Name = "lblEIDErr";
            this.lblEIDErr.Size = new System.Drawing.Size(320, 13);
            this.lblEIDErr.TabIndex = 7;
            this.lblEIDErr.Text = "VERY STUPIDLY INCREDIBLY LONG EID ERROR OH MY GOD";
            this.lblEIDErr.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblEIDErr.Visible = false;
            // 
            // Crash3AnimationSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblEIDErr);
            this.Controls.Add(this.txtEName);
            this.Controls.Add(this.lblDesc);
            this.Name = "Crash3AnimationSelector";
            this.Size = new System.Drawing.Size(350, 280);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.TextBox txtEName;
        private System.Windows.Forms.Label lblEIDErr;
    }
}
