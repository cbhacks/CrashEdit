namespace CrashEdit
{
    public sealed class MysteryBox : UserControl
    {
        private readonly byte[] data;
        private bool saving;

        private readonly ToolStrip tsToolbar;
        private readonly ToolStripButton tbbExport;
        private readonly HexBox hbData;

        public MysteryBox(byte[] data)
        {
            this.data = data;
            saving = false;

            tbbExport = new ToolStripButton();
            tbbExport.Text = "Export";
            tbbExport.Click += new EventHandler(tbbExport_Click);

            tsToolbar = new ToolStrip();
            tsToolbar.Dock = DockStyle.Top;
            tsToolbar.Items.Add(tbbExport);

            hbData = new HexBox();
            hbData.Dock = DockStyle.Fill;
            hbData.Data = data;

            Controls.Add(hbData);
            Controls.Add(tsToolbar);
        }

        void tbbExport_Click(object sender, EventArgs e)
        {
            if (!saving)
            {
                saving = true;
                FileUtil.SaveFile(data, FileFilters.Any);
                saving = false;
            }
        }
    }
}
