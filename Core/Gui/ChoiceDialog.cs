using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{

    public sealed class ChoiceDialog : Form
    {

        public ChoiceDialog()
        {
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            MinimumSize = new Size(300, 1);
            FormBorderStyle = FormBorderStyle.FixedDialog;

            OverallTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 1,
                Padding = new Padding(12),
            };
            Controls.Add(OverallTable);

            MessageLabel = new Label
            {
                AutoSize = true,
                Text = "Make a selection.",
            };
            OverallTable.Controls.Add(MessageLabel);

            ChoiceButtonTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 1,
            };
            OverallTable.Controls.Add(ChoiceButtonTable);

            CancelButton = new Button
            {
                Text = "Cancel",
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
            };
            CancelButton.Click += (sender, e) =>
            {
                SelectedChoice = null;
                DialogResult = DialogResult.Cancel;
            };
            OverallTable.Controls.Add(CancelButton);
            base.CancelButton = CancelButton;
        }

        public string MessageText
        {
            get => MessageLabel.Text;
            set => MessageLabel.Text = value;
        }

        public UserChoice? SelectedChoice { get; private set; }

        private TableLayoutPanel OverallTable { get; }

        private Label MessageLabel { get; }

        private TableLayoutPanel ChoiceButtonTable { get; }

        private new Button CancelButton { get; }

        private static Font _choiceFont = new Font(Button.DefaultFont.FontFamily, 16, GraphicsUnit.Pixel);

        public void AddChoice(UserChoice choice)
        {
            if (choice == null)
                throw new ArgumentNullException();

            var button = new Button
            {
                AutoSize = true,
                Margin = new Padding(0, 3, 0, 3),
                Padding = new Padding(12),
                FlatStyle = FlatStyle.Popup,
                Font = _choiceFont,
                Text = "    " + choice.Text,
                TextAlign = ContentAlignment.MiddleCenter,
                ImageKey = choice.ImageKey,
                ImageList = Embeds.ImageList,
                ImageAlign = ContentAlignment.MiddleLeft,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Anchor =
                    AnchorStyles.Top |
                    AnchorStyles.Left |
                    AnchorStyles.Right,
            };
            button.Click += (sender, e) =>
            {
                SelectedChoice = choice;
                DialogResult = DialogResult.OK;
            };
            ChoiceButtonTable.Controls.Add(button);
        }

        public void AddChoices(IEnumerable<UserChoice> choices)
        {
            if (choices == null)
                throw new ArgumentNullException();

            foreach (var c in choices)
            {
                AddChoice(c);
            }
        }

    }

}
