namespace CrashEdit.Crash
{
    public sealed class SLSTEntry : Entry
    {
        public SLSTEntry(SLSTSource start, SLSTSource end, IEnumerable<SLSTDelta> deltas, int eid) : base(eid)
        {
            if (deltas == null)
                throw new ArgumentNullException("deltas");
            Deltas.AddRange(deltas);
            Start = start;
            End = end;
        }

        public override string Title => $"Sort List ({EName})";
        public override string ImageKey => "ThingGray";

        public override int Type => 4;

        [SubresourceSlot]
        public SLSTSource Start { get; }

        [SubresourceList]
        public List<SLSTDelta> Deltas { get; } = new List<SLSTDelta>();

        [SubresourceSlot]
        public SLSTSource End { get; }

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
