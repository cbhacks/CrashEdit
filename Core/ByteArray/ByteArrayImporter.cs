namespace CrashEdit
{

    public sealed class ByteArrayImporter : Importer<byte[]>
    {

        public override bool Import(IUserInterface ui, ReadOnlySpan<byte> buf, out byte[] res)
        {
            if (ui == null)
                throw new ArgumentNullException();

            res = buf.ToArray();
            return true;
        }

    }

}
