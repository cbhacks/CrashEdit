using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class SLSTEntry : Entry
    {
        private List<SLSTDelta> deltas;

        public SLSTEntry(SLSTSource start, SLSTSource end, IEnumerable<SLSTDelta> deltas, int eid) : base(eid)
        {
            if (deltas == null)
                throw new ArgumentNullException("deltas");
            this.deltas = new List<SLSTDelta>(deltas);
            Start = start;
            End = end;
        }

        public override int Type => 4;
        public IList<SLSTDelta> Deltas => deltas;
        public SLSTSource Start { get; }
        public SLSTSource End { get; }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte[deltas.Count + 2][];
            items[0] = Start.Save();
            for (int i = 0;i < deltas.Count;++i)
            {
                items[1+i] = deltas[i].Save();
            }
            items[1 + deltas.Count] = End.Save();
            return new UnprocessedEntry(items,EID,Type);
        }
    }
}
