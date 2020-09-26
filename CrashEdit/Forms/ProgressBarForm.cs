using System.Windows.Forms;

namespace CrashEdit.Forms
{
    public partial class ProgressBarForm : Form
    {
        public ProgressBarForm()
        {
            InitializeComponent();
            uxProgress.Invalidated += uxProgress_Invalidated;
        }

        private void uxProgress_Invalidated(object sender, InvalidateEventArgs e)
        {
            if (uxProgress.Value == 100)
            {
                Close();
            }
        }

        public ProgressBar ProgressBar => uxProgress;
    }
}
