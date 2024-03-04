namespace CrashEdit
{

    public sealed class ByteArrayExporter : Exporter<byte[]>
    {

        public override string Text => "Raw data";

        public override bool Export(IUserInterface ui, out ReadOnlySpan<byte> buf, byte[] res)
        {
            ArgumentNullException.ThrowIfNull(ui);
            ArgumentNullException.ThrowIfNull(res);

            buf = new ReadOnlySpan<byte>(res);
            return true;
        }

    }

}
