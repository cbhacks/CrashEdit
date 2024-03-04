namespace CrashEdit.Crash
{
    public sealed class OldSLSTEntry : Entry
    {
        public OldSLSTEntry(OldSLSTSource start, OldSLSTSource end, IEnumerable<OldSLSTDelta> deltas, int eid) : base(eid)
        {
            ArgumentNullException.ThrowIfNull(deltas);
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

        public List<OldSLSTPolygonID> DecodeAt(int index)
        {
            List<OldSLSTPolygonID> polys1 = new List<OldSLSTPolygonID>(Start.Polygons);
            List<OldSLSTPolygonID> polys2 = new List<OldSLSTPolygonID>();
            List<OldSLSTPolygonID> src = polys1;
            List<OldSLSTPolygonID> dst = polys2;
            for (int d = 0; d + 1 <= index; ++d)
            {
                dst.Clear();
                var delta = Deltas[d];
                int srci = 0;

                int remi = 0;
                int addi = 0;
                int rem = 0;
                int add = 0;
                int rem_left = 0;
                int add_left = 0;
                int rem_at = 0;
                int add_at = 0;
                int rem_done = 0;
                int add_done = 0;

                void read_rem_node()
                {
                    if (rem_left > 0)
                    {
                        rem = delta.RemoveNodes[remi++];
                        rem_at += (rem >> 15) & 0x1;
                    }
                    else if (remi < delta.RemoveNodes.Count)
                    {
                        rem = delta.RemoveNodes[remi++];
                        if (remi < delta.RemoveNodes.Count && rem != -1)
                        {
                            rem_at = rem & 0xFFF;
                            rem_left = 1 + ((rem >> 12) & 0xF);
                        }
                        if (rem_left > 0)
                            rem = delta.RemoveNodes[remi++];
                    }
                }

                void read_add_node()
                {
                    if (add_left > 0)
                    {
                        add = delta.AddNodes[addi++];
                        add_at += (add >> 15) & 0x1;
                    }
                    else if (addi < delta.AddNodes.Count)
                    {
                        add = delta.AddNodes[addi++];
                        if (addi < delta.AddNodes.Count && add != -1)
                        {
                            add_at = add & 0xFFF;
                            add_left = 1 + ((add >> 12) & 0xF);
                        }
                        if (add_left > 0)
                            add = delta.AddNodes[addi++];
                    }
                }

                read_rem_node();
                read_add_node();
                while (srci < src.Count || rem_left > 0 || add_left > 0)
                {
                    if (srci < src.Count && rem_left > 0 && (rem_at - 1) == srci)
                    {
                        // iterator advances but no polygon is added - 'removed'
                        ++srci;
                        --rem_left;
                        ++rem_done;
                        read_rem_node();
                    }
                    else if (add_left > 0 && add_at + rem_done == srci)
                    {
                        dst.Add(new OldSLSTPolygonID((short)(add & 0x7FFF)));
                        --add_left;
                        ++add_done;
                        read_add_node();
                    }
                    else if (srci < src.Count)
                    {
                        int copy_amount = Math.Max(1, Math.Min(src.Count - srci - 1, Math.Min(add_at + rem_done - srci, rem_at - srci - 1)));
                        for (int i = 0; i < copy_amount; ++i)
                        {
                            dst.Add(src[srci++]);
                        }
                    }
                }

                int swap_a = 0, swap_b;
                for (int i = 0; i < delta.SwapNodes.Count; ++i)
                {
                    int swap = delta.SwapNodes[i];
                    if (swap == -1)
                        break;
                    if ((swap & 0x8000) != 0)
                    {
                        swap_a = swap & 0x7FF;
                        swap_b = (swap >> 11) & 0xF;
                    }
                    else if ((swap & 0xC000) == 0x4000)
                    {
                        swap_a += (swap >> 5) & 0x1FF;
                        swap_b = (swap & 0x1F) + 16;
                    }
                    else
                    {
                        swap_a = swap & 0xFFF;
                        swap = delta.SwapNodes[++i];
                        swap_b = swap & 0xFFF;
                    }
                    swap_b += swap_a + 1;
                    (dst[swap_b], dst[swap_a]) = (dst[swap_a], dst[swap_b]);
                }

                (dst, src) = (src, dst);
            }

            return src;
        }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte[Deltas.Count + 2][];
            items[0] = Start.Save();
            for (int i = 0; i < Deltas.Count; ++i)
            {
                items[1 + i] = Deltas[i].Save();
            }
            items[1 + Deltas.Count] = End.Save();
            return new UnprocessedEntry(items, EID, Type);
        }
    }
}
