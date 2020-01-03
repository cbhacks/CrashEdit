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
            this.fraLang.SuspendLayout();
            this.SuspendLayout();
            // 
            // dpdLang
            // 
            this.dpdLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dpdLang.FormattingEnabled = true;
            this.dpdLang.Location = new System.Drawing.Point(6, 19);
            this.dpdLang.Name = "dpdLang";
            this.dpdLang.Size = new System.Drawing.Size(121, 21);
            this.dpdLang.TabIndex = 0;
            this.dpdLang.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // fraLang
            // 
            this.fraLang.AutoSize = true;
            this.fraLang.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fraLang.Controls.Add(this.dpdLang);
            this.fraLang.Location = new System.Drawing.Point(3, 3);
            this.fraLang.Name = "fraLang";
            this.fraLang.Size = new System.Drawing.Size(133, 59);
            this.fraLang.TabIndex = 1;
            this.fraLang.TabStop = false;
            this.fraLang.Text = "Language";
            // 
            // ConfigEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
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
    }
}
