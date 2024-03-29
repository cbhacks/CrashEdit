namespace CrashEdit.Crash
{

    public sealed class ChunkExporter : Exporter<Chunk>
    {

        public override string Text => "Crash Bandicoot NSF chunk (page)";

        public override string[] FileFilters => new string[] {
            "nschunk files (*.nschunk)|*.nschunk;*.NSCHUNK"
        };

        public override bool Export(IUserInterface ui, out ReadOnlySpan<byte> buf, Chunk res)
        {
            ArgumentNullException.ThrowIfNull(ui);
            ArgumentNullException.ThrowIfNull(res);

            buf = new ReadOnlySpan<byte>(res.Save());
            return true;
        }

    }

}
