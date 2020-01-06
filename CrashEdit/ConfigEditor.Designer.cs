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
            this.dpdLang = new System.Windows.Forms.ComboBox();
            this.fraLang = new System.Windows.Forms.GroupBox();
            this.cmdReset = new System.Windows.Forms.Button();
            this.fraLang.SuspendLayout();
            this.SuspendLayout();
            // 
            // dpdLang
            // 
            this.dpdLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dpdLang.FormattingEnabled = true;
            this.dpdLang.Location = new System.Drawing.Point(6, 19);
            this.dpdLang.Name = "dpdLang";
            this.dpdLang.Size = new System.Drawing.Size(150, 21);
            this.dpdLang.TabIndex = 0;
            // 
            // fraLang
            // 
            this.fraLang.AutoSize = true;
            this.fraLang.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fraLang.Controls.Add(this.dpdLang);
            this.fraLang.Location = new System.Drawing.Point(3, 3);
            this.fraLang.Name = "fraLang";
            this.fraLang.Size = new System.Drawing.Size(162, 59);
            this.fraLang.TabIndex = 1;
            this.fraLang.TabStop = false;
            this.fraLang.Text = "Language (requires restart)";
            // 
            // cmdReset
            // 
            this.cmdReset.Location = new System.Drawing.Point(3, 68);
            this.cmdReset.Name = "cmdReset";
            this.cmdReset.Size = new System.Drawing.Size(100, 23);
            this.cmdReset.TabIndex = 1;
            this.cmdReset.Text = "Reset Settings";
            this.cmdReset.UseVisualStyleBackColor = true;
            this.cmdReset.Click += new System.EventHandler(this.cmdReset_Click);
            // 
            // ConfigEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.cmdReset);
            this.Controls.Add(this.fraLang);
            this.Name = "ConfigEditor";
            this.Size = new System.Drawing.Size(408, 372);
            this.fraLang.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox dpdLang;
        private System.Windows.Forms.GroupBox fraLang;
        private System.Windows.Forms.Button cmdReset;
    }
}
