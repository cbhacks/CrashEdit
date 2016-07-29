using Crash;
using System;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class SoundBox : UserControl
    {
        private SampleSet samples;

        private SoundPlayer spPlayer;

        private ToolStrip tsToolbar;
        private ToolStripButton tbbExport;
        private TableLayoutPanel pnOptions;
        private Button cmdPlay;
        private Button cmdExport;
        private NumericUpDown numSampleRate;

        public SoundBox(SampleSet samples)
        {
            this.samples = samples;

            spPlayer = new SoundPlayer();

            tbbExport = new ToolStripButton();
            tbbExport.Text = "Export";
            tbbExport.Click += new EventHandler(tbbExport_Click);

            tsToolbar = new ToolStrip();
            tsToolbar.Dock = DockStyle.Top;
            tsToolbar.Items.Add(tbbExport);

            cmdPlay = new Button();
            cmdPlay.Dock = DockStyle.Fill;
            cmdPlay.Text = "Play";
            cmdPlay.Click += new EventHandler(cmdPlay_Click);

            cmdExport = new Button();
            cmdPlay.Dock = DockStyle.Fill;
            cmdExport.Text = "Export";
            cmdExport.Click += new EventHandler(cmdExport_Click);

            numSampleRate = new NumericUpDown();
            numSampleRate.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            numSampleRate.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            numSampleRate.Name = "numSampleRate";
            numSampleRate.Value = 11025;

            pnOptions = new TableLayoutPanel();
            pnOptions.Dock = DockStyle.Fill;
            pnOptions.ColumnCount = 2;
            pnOptions.RowCount = 1;
            pnOptions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));
            pnOptions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));
            pnOptions.RowStyles.Add(new RowStyle(SizeType.Percent,50));
            pnOptions.RowStyles.Add(new RowStyle(SizeType.Percent,50));
            pnOptions.Controls.Add(cmdPlay,0,0);
            pnOptions.Controls.Add(cmdExport,1,0);
            pnOptions.Controls.Add(numSampleRate,0,1);

            Controls.Add(pnOptions);
            Controls.Add(tsToolbar);
        }

        void cmdPlay_Click(object sender, EventArgs e)
        {
            Play((int)numSampleRate.Value);
        }

        void cmdExport_Click(object sender, EventArgs e)
        {
            ExportWave((int)numSampleRate.Value);
        }

        public SoundBox(SoundEntry entry)
            : this(entry.Samples)
        {
        }

        public SoundBox(SpeechEntry entry) : this(entry.Samples)
        {
        }

        void tbbExport_Click(object sender,EventArgs e)
        {
            FileUtil.SaveFile(samples.Save(),FileFilters.Any);
        }

        private void Play(int samplerate)
        {
            byte[] wave = WaveConv.ToWave(samples.ToPCM(),samplerate).Save();
            spPlayer.Stop();
            spPlayer.Stream = new MemoryStream(wave);
            spPlayer.Play();
        }

        private void ExportWave(int samplerate)
        {
            byte[] wave = WaveConv.ToWave(samples.ToPCM(),samplerate).Save();
            FileUtil.SaveFile(wave,FileFilters.Wave,FileFilters.Any);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                spPlayer.Stop();
                spPlayer.Dispose();
            }
        }
    }
}
