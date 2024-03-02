using System.Windows.Forms;

namespace CrashEdit
{

    public class MainControl : UserControl, IWorkspaceHost, IVerbExecutor
    {

        public MainControl(IUserInterface ui, Controller rootController)
        {
            ArgumentNullException.ThrowIfNull(ui);
            ArgumentNullException.ThrowIfNull(rootController);

            Ui = ui;
            RootController = rootController;

            Split = new SplitContainer
            {
                Dock = DockStyle.Fill
            };
            Controls.Add(Split);

            ResourceTree = new ResourceTreeView(this)
            {
                Dock = DockStyle.Fill,
                RootController = RootController,
                HideSelection = false
            };
            ResourceTree.SelectedControllerChanged += (sender, e) =>
            {
                _activeController = ResourceTree.SelectedController;
                ResourceBox.ActiveController = _activeController;
                OnActiveControllerChanged(EventArgs.Empty);
            };
            Split.Panel1.Controls.Add(ResourceTree);

            ResourceBox = new ResourceBox
            {
                Dock = DockStyle.Fill
            };
            Split.Panel2.Controls.Add(ResourceBox);
        }

        public IUserInterface Ui { get; }

        public Controller RootController { get; }

        private Controller? _activeController;

        public Controller? ActiveController
        {
            get { return _activeController; }
            set
            {
                if (_activeController == value)
                    return;

                // This raises an event which sets the field.
                ResourceTree.SelectedController = value;
            }
        }

        public event EventHandler? ActiveControllerChanged;

        private string _searchQuery = "";

        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                if (_searchQuery == value)
                    return;

                _searchQuery = value;

                var queryLowerCase = value.ToLower();
                if (value == "")
                {
                    SearchPredicate = null;
                }
                else
                {
                    SearchPredicate = (x =>
                        x.Text.ToLower().Contains(queryLowerCase));
                }
            }
        }

        public Predicate<Controller>? SearchPredicate { get; private set; }

        public SplitContainer Split { get; }

        public ResourceTreeView ResourceTree { get; }

        public ResourceBox ResourceBox { get; }

        public void Sync()
        {
            ResourceTree.Sync();
            ResourceBox.Sync();
        }

        public void ExecuteVerb(Verb verb)
        {
            ArgumentNullException.ThrowIfNull(verb);

            verb.Execute(Ui);

            RootController.Sync();
            Sync();
        }

        public void ExecuteVerbChoice(List<Verb> verbs)
        {
            ArgumentNullException.ThrowIfNull(verbs);

            // Don't bother if there are no choices.
            if (verbs.Count == 0)
                return;

            // TODO let the user choose one
            ExecuteVerb(verbs[0]);
        }

        protected virtual void OnActiveControllerChanged(EventArgs e)
        {
            ActiveControllerChanged?.Invoke(this, e);
        }

    }

}
