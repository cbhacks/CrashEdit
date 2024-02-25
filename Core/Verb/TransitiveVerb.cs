namespace CrashEdit
{

    public abstract class TransitiveVerb : Verb
    {

        public Controller? Source { get; set; }

        public Controller? Destination { get; set; }

        public abstract bool ApplicableForSource(Controller src);

        public abstract bool ApplicableForTransit(Controller src, Controller dest);

    }

}
