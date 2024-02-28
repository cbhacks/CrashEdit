namespace CrashEdit.Crash
{

    public sealed class SeqExporter : Exporter<SEQ>
    {

        public override string Text => "PlayStation SEQ";

        public override string[] FileFilters => new string[] {
            "PlayStation SEQ files (*.seq)|*.seq;*.SEQ"
        };

        public override bool Export(IUserInterface ui, out ReadOnlySpan<byte> buf, SEQ res)
        {
            ArgumentNullException.ThrowIfNull(ui);
            ArgumentNullException.ThrowIfNull(res);

            buf = new ReadOnlySpan<byte>(res.Save());
            return true;
        }

    }

}
