namespace CrashEdit
{

    public sealed class LegacyVerb : Verb
    {

        public LegacyVerb(string text, Action proc)
        {
            if (text == null)
                throw new ArgumentNullException();
            if (proc == null)
                throw new ArgumentNullException();

            _text = text;
            Proc = proc;
        }

        public string _text;

        public override string Text => _text;

        private Action Proc { get; }

        public override void Execute(IUserInterface ui)
        {
            if (ui == null)
                throw new ArgumentNullException();

            Proc();
        }

    }

}
