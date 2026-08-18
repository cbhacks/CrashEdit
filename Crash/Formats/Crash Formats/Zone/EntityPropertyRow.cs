namespace CrashEdit.Crash
{
    public sealed class EntityPropertyRow<T>
    {
        private readonly List<T> values;

        public EntityPropertyRow()
        {
            Keyframe = null;
            values = new();
        }

        public short? Keyframe { get; set; }
        public IList<T> Values => values;
    }
}
