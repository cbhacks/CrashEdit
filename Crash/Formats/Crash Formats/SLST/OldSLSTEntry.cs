using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class OldSLSTEntry : Entry
    {
        private List<OldSLSTDelta> deltas;

        public OldSLSTEntry(OldSLSTSource start, OldSLSTSource end, IEnumerable<OldSLSTDelta> deltas,int eid) : base(eid)
        {
            if (deltas == null)
                throw new ArgumentNullException("deltas");
            this.deltas = new List<OldSLSTDelta>(deltas);
            Start = start;
            End = end;
        }

        public override int Type => 4;
        public IList<OldSLSTDelta> Deltas => deltas;
        public OldSLSTSource Start { get; }
        public OldSLSTSource End { get; }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [deltas.Count + 2][];
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
