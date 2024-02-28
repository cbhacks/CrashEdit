namespace CrashEdit
{

    public sealed class LegacyVerb : Verb
    {

        public LegacyVerb(string text, Action proc)
        {
            ArgumentNullException.ThrowIfNull(text);
            ArgumentNullException.ThrowIfNull(proc);

            _text = text;
            Proc = proc;
        }

        public string _text;

        public override string Text => _text;

        private Action Proc { get; }

        public override void Execute(IUserInterface ui)
        {
            ArgumentNullException.ThrowIfNull(ui);

            Proc();
        }

    }

}
