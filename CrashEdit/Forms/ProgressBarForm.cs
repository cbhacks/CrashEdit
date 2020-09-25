using System.Windows.Forms;

namespace CrashEdit.Forms
{
    public partial class ProgressBarForm : Form
    {
        public ProgressBarForm()
        {
            InitializeComponent();
        }

        public ProgressBar ProgressBar => uxProgress;
    }
}
