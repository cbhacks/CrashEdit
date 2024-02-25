namespace CrashEdit.Crash
{
    public sealed class OldSLSTEntry : Entry
    {
        public OldSLSTEntry(OldSLSTSource start, OldSLSTSource end, IEnumerable<OldSLSTDelta> deltas, int eid) : base(eid)
        {
            if (deltas == null)
                throw new ArgumentNullException(nameof(deltas));
            Deltas.AddRange(deltas);
            Start = start;
            End = end;
        }

        public override string Title => $"Old Sort List ({EName})";
        public override string ImageKey => "ThingGray";

        public override int Type => 4;

        [SubresourceSlot]
        public OldSLSTSource Start { get; }

        [SubresourceList]
        public List<OldSLSTDelta> Deltas { get; } = new List<OldSLSTDelta>();

        [SubresourceSlot]
        public OldSLSTSource End { get; }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte[Deltas.Count + 2][];
            items[0] = Start.Save();
            for (int i = 0; i < Deltas.Count; ++i)
            {
                items[1+i] = Deltas[i].Save();
            }
            items[1 + Deltas.Count] = End.Save();
            return new UnprocessedEntry(items, EID, Type);
        }
    }
}
