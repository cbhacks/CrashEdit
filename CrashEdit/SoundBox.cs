using Crash;
using Crash.Audio;
using System;
using System.IO;
using System.Media;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    [EditorControl(typeof(SoundEntry))]
    [EditorControl(typeof(SpeechEntry))]
    public sealed class SoundBox : UserControl
    {
        private SampleSet samples;

        private SoundPlayer spPlayer;

        private ToolStrip tsToolbar;
        private ToolStripButton tbbExport;
        private TableLayoutPanel pnOptions;
        private Button cmdPlay11025;
        private Button cmdPlay22050;
        private Button cmdExport11025;
        private Button cmdExport22050;

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

            cmdPlay11025 = new Button();
            cmdPlay11025.Dock = DockStyle.Fill;
            cmdPlay11025.Text = "Play (11025 Hz)";
            cmdPlay11025.Click += new EventHandler(cmdPlay11025_Click);

            cmdPlay22050 = new Button();
            cmdPlay22050.Dock = DockStyle.Fill;
            cmdPlay22050.Text = "Play (22050 Hz)";
            cmdPlay22050.Click += new EventHandler(cmdPlay22050_Click);

            cmdExport11025 = new Button();
            cmdExport11025.Dock = DockStyle.Fill;
            cmdExport11025.Text = "Export Wave (11025 Hz)";
            cmdExport11025.Click += new EventHandler(cmdExport11025_Click);

            cmdExport22050 = new Button();
            cmdExport22050.Dock = DockStyle.Fill;
            cmdExport22050.Text = "Export Wave (22050 Hz)";
            cmdExport22050.Click += new EventHandler(cmdExport22050_Click);

            pnOptions = new TableLayoutPanel();
            pnOptions.Dock = DockStyle.Fill;
            pnOptions.ColumnCount = 2;
            pnOptions.RowCount = 3;
            pnOptions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));
            pnOptions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));
            pnOptions.RowStyles.Add(new RowStyle(SizeType.Percent,50));
            pnOptions.RowStyles.Add(new RowStyle(SizeType.Percent,50));
            pnOptions.Controls.Add(cmdPlay11025,0,0);
            pnOptions.Controls.Add(cmdPlay22050,0,1);
            pnOptions.Controls.Add(cmdExport11025,1,0);
            pnOptions.Controls.Add(cmdExport22050,1,1);

            Controls.Add(pnOptions);
            Controls.Add(tsToolbar);
        }

        void cmdPlay11025_Click(object sender,EventArgs e)
        {
            Play(11025);
        }

        void cmdPlay22050_Click(object sender,EventArgs e)
        {
            Play(22050);
        }

        void cmdExport11025_Click(object sender,EventArgs e)
        {
            ExportWave(11025);
        }

        void cmdExport22050_Click(object sender,EventArgs e)
        {
            ExportWave(22050);
        }

        public SoundBox(SoundEntry entry) : this(entry.Samples)
        {
        }

        public SoundBox(SpeechEntry entry) : this(entry.Samples)
        {
        }

        void tbbExport_Click(object sender,EventArgs e)
        {
            FileUtil.SaveFile(samples.Save(),FileUtil.AnyFilter);
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
            FileUtil.SaveFile(wave,FileUtil.WaveFilter + "|" + FileUtil.AnyFilter);
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
