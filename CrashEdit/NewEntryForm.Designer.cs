namespace CrashEdit
{
    public partial class NewEntryForm
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
            this.fraType = new System.Windows.Forms.GroupBox();
            this.numType = new System.Windows.Forms.NumericUpDown();
            this.dpdType = new System.Windows.Forms.ComboBox();
            this.fraName = new System.Windows.Forms.GroupBox();
            this.lblEIDErr = new System.Windows.Forms.Label();
            this.txtEID = new System.Windows.Forms.TextBox();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.fraType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numType)).BeginInit();
            this.fraName.SuspendLayout();
            this.SuspendLayout();
            // 
            // fraType
            // 
            this.fraType.AutoSize = true;
            this.fraType.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fraType.Controls.Add(this.numType);
            this.fraType.Controls.Add(this.dpdType);
            this.fraType.Location = new System.Drawing.Point(181, 12);
            this.fraType.Name = "fraType";
            this.fraType.Size = new System.Drawing.Size(197, 59);
            this.fraType.TabIndex = 1;
            this.fraType.TabStop = false;
            this.fraType.Text = "Entry Type";
            // 
            // numType
            // 
            this.numType.Enabled = false;
            this.numType.Location = new System.Drawing.Point(132, 20);
            this.numType.Maximum = new decimal(new int[] {
            22,
            0,
            0,
            0});
            this.numType.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numType.Name = "numType";
            this.numType.Size = new System.Drawing.Size(59, 20);
            this.numType.TabIndex = 1;
            this.numType.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // dpdType
            // 
            this.dpdType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dpdType.FormattingEnabled = true;
            this.dpdType.Location = new System.Drawing.Point(6, 19);
            this.dpdType.Name = "dpdType";
            this.dpdType.Size = new System.Drawing.Size(120, 21);
            this.dpdType.TabIndex = 0;
            this.dpdType.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // fraName
            // 
            this.fraName.Controls.Add(this.lblEIDErr);
            this.fraName.Controls.Add(this.txtEID);
            this.fraName.Location = new System.Drawing.Point(12, 12);
            this.fraName.Name = "fraName";
            this.fraName.Size = new System.Drawing.Size(163, 71);
            this.fraName.TabIndex = 2;
            this.fraName.TabStop = false;
            this.fraName.Text = "Entry Name";
            // 
            // lblEIDErr
            // 
            this.lblEIDErr.AutoSize = true;
            this.lblEIDErr.ForeColor = System.Drawing.Color.Red;
            this.lblEIDErr.Location = new System.Drawing.Point(6, 46);
            this.lblEIDErr.Name = "lblEIDErr";
            this.lblEIDErr.Size = new System.Drawing.Size(160, 13);
            this.lblEIDErr.TabIndex = 6;
            this.lblEIDErr.Text = "VERY LONG EID ERROR OMG";
            this.lblEIDErr.Visible = false;
            // 
            // txtEID
            // 
            this.txtEID.Location = new System.Drawing.Point(7, 20);
            this.txtEID.MaxLength = 5;
            this.txtEID.Name = "txtEID";
            this.txtEID.Size = new System.Drawing.Size(50, 20);
            this.txtEID.TabIndex = 0;
            this.txtEID.Text = "NONE!";
            this.txtEID.TextChanged += new System.EventHandler(this.txtEID_TextChanged);
            // 
            // cmdOK
            // 
            this.cmdOK.Enabled = false;
            this.cmdOK.Location = new System.Drawing.Point(223, 77);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 4;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(304, 77);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 3;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // NewEntryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(391, 110);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.fraName);
            this.Controls.Add(this.fraType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "NewEntryForm";
            this.Text = "New Entry";
            this.fraType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numType)).EndInit();
            this.fraName.ResumeLayout(false);
            this.fraName.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox fraType;
        private System.Windows.Forms.NumericUpDown numType;
        private System.Windows.Forms.GroupBox fraName;
        private System.Windows.Forms.TextBox txtEID;
        private System.Windows.Forms.Label lblEIDErr;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.ComboBox dpdType;
    }
}