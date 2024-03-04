using System.Windows.Forms;

namespace CrashEdit
{

    public abstract class Editor : IDisposable
    {

        public bool IsInitialized { get; private set; }

        public void Initialize(Controller subj)
        {
            ArgumentNullException.ThrowIfNull(subj);
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (IsInitialized)
                throw new InvalidOperationException();
            if (!ApplicableForSubject(subj))
                throw new ArgumentException();

            IsInitialized = true;
            _subject = subj;
            _control = MakeControl();
            if (_control == null)
            {
                throw new Exception();
            }
        }

        private Controller? _subject;

        public Controller Subject
        {
            get
            {
                if (IsDisposed)
                    throw new ObjectDisposedException(GetType().FullName);
                if (_subject == null)
                    throw new InvalidOperationException();

                return _subject!;
            }
        }

        private Control? _control;

        public Control Control
        {
            get
            {
                if (IsDisposed)
                    throw new ObjectDisposedException(GetType().FullName);
                if (_control == null)
                    throw new InvalidOperationException();

                return _control!;
            }
        }

        public abstract string Text { get; }

        public abstract bool ApplicableForSubject(Controller subj);

        protected abstract Control MakeControl();

        public abstract void Sync();

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                if (_control != null)
                {
                    _control.Dispose();
                }
            }
            IsDisposed = true;
        }

        public static List<Editor> AllEditors { get; } =
            new List<Editor>();

        [Registrar.TypeProcessor]
        private static void ProcessEditorType(Type type)
        {
            if (!typeof(Editor).IsAssignableFrom(type))
                return;
            if (type.IsAbstract)
                return;

            AllEditors.Add((Editor)Activator.CreateInstance(type));
        }


    }

}
