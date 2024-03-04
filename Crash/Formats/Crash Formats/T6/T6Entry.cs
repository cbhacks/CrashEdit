namespace CrashEdit.Crash
{
    public sealed class T6Entry : MysteryUniItemEntry
    {
        public T6Entry(byte[] data, int eid) : base(data, eid)
        {
        }

        public override string Title => $"T6 ({EName})";
        public override string ImageKey => "ThingOrange";

        public override int Type => 6;
    }
}
