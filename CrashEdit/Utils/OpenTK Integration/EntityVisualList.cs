namespace CrashEdit.CE
{
    internal class EntityVisualList : Dictionary<int, EntityVisual>
    {
        public void AddVisual(int type, int subtype, EntityVisual visual)
        {
            int key = type * 100000 + subtype;
            if (ContainsKey(key))
                this[key] = visual;
            else
                Add(key, visual);
        }

        public bool TryGetVisual(int type, int subtype, out EntityVisual visual)
        {
            return TryGetValue(type * 100000 + subtype, out visual);
        }
    }
}
