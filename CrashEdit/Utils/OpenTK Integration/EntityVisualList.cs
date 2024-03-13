namespace CrashEdit.CE
{
    internal class EntityVisualList : Dictionary<int, EntityVisual>
    {
        public void AddVisual(int type, int subtype, EntityVisual visual)
        {
            Add(type * 10000 + subtype, visual);
        }

        public bool TryGetVisual(int type, int subtype, out EntityVisual visual)
        {
            return TryGetValue(type * 10000 + subtype, out visual);
        }
    }
}
