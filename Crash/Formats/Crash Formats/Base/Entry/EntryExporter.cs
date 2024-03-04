namespace CrashEdit.Crash
{

    public sealed class EntryExporter : Exporter<Entry>
    {

        public override string Text => "Crash Bandicoot NSF entry";

        public override string[] FileFilters => new string[] {
            "nsentry files (*.nsentry)|*.nsentry;*.NSENTRY"
        };

        public override bool Export(IUserInterface ui, out ReadOnlySpan<byte> buf, Entry res)
        {
            ArgumentNullException.ThrowIfNull(ui);
            ArgumentNullException.ThrowIfNull(res);

            buf = new ReadOnlySpan<byte>(res.Save());
            return true;
        }

    }

}
