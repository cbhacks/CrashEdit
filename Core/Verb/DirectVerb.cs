namespace CrashEdit
{

    public abstract class DirectVerb : Verb
    {

        public Controller? Subject { get; set; }

        public abstract bool ApplicableForSubject(Controller subj);

    }

}
