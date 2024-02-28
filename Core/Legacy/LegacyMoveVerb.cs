namespace CrashEdit
{

    public sealed class LegacyMoveVerb : TransitiveVerb
    {

        public override string Text => "Move here";

        public override bool ApplicableForSource(Controller src)
        {
            ArgumentNullException.ThrowIfNull(src);

            return (src.Legacy != null);
        }

        public override bool ApplicableForTransit(Controller src, Controller dest)
        {
            ArgumentNullException.ThrowIfNull(src);
            ArgumentNullException.ThrowIfNull(dest);

            if (src.Legacy == null)
                return false;
            if (dest.Legacy == null)
                return false;

            return src.Legacy.CanMoveTo(dest.Legacy);
        }

        public override void Execute(IUserInterface ui)
        {
            ArgumentNullException.ThrowIfNull(ui);
            if (Source == null)
                throw new InvalidOperationException();
            if (Destination == null)
                throw new InvalidOperationException();
            if (Source.Legacy == null)
                throw new InvalidOperationException();
            if (Destination.Legacy == null)
                throw new InvalidOperationException();

            Source.Legacy.MoveTo(Destination.Legacy);
        }

    }

}
