namespace CrashEdit
{

    public sealed class ByteArrayImporter : Importer<byte[]>
    {

        public override bool Import(IUserInterface ui, ReadOnlySpan<byte> buf, out byte[] res)
        {
            ArgumentNullException.ThrowIfNull(ui);

            res = buf.ToArray();
            return true;
        }

    }

}
