namespace CrashEdit.Crash
{
    internal static class GOOLEntryLoader
    {
        internal static GOOLEntry LoadGOOL(GOOLVersion goolver, byte[][] items, int eid)
        {
            ArgumentNullException.ThrowIfNull(items);
            if (items.Length != 3 && items.Length != 5 && items.Length != 6)
            {
                ErrorManager.SignalError("GOOLEntry: Wrong number of items");
            }
            if (items[0].Length != 24)
            {
                ErrorManager.SignalError("GOOLEntry: Header length is wrong");
            }
            int[] ins = new int[items[2].Length / 4];
            for (int i = 0; i < ins.Length; ++i)
            {
                ins[i] = BitConv.FromInt32(items[2], i * 4);
            }
            short[] statemap = null;
            GOOLStateDescriptor[] statedesc = null;
            List<GOOLFrameGroupBase> fgroups = [];
            if (items.Length > 3)
            {
                statemap = new short[items[3].Length / 2];
                for (int i = 0; i < statemap.Length; ++i)
                {
                    statemap[i] = BitConv.FromInt16(items[3], i * 2);
                }
                statedesc = new GOOLStateDescriptor[items[4].Length / 0x10];
                for (int i = 0; i < statedesc.Length; ++i)
                {
                    statedesc[i] = new GOOLStateDescriptor(
                        BitConv.FromInt32(items[4], i * 0x10 + 0),
                        BitConv.FromInt32(items[4], i * 0x10 + 4),
                        BitConv.FromInt16(items[4], i * 0x10 + 8),
                        BitConv.FromInt16(items[4], i * 0x10 + 10),
                        BitConv.FromInt16(items[4], i * 0x10 + 12),
                        BitConv.FromInt16(items[4], i * 0x10 + 14)
                        );
                }
                if (items.Length > 5)
                {
                    if (goolver == GOOLVersion.Version0)
                    {
                        for (int i = 0; i < items[5].Length / 8; ++i)
                        {
                            fgroups.Add(new ProtoSpriteTexture(BitConv.FromInt32(items[5], i * 8), BitConv.FromInt32(items[5], i * 8 + 4)));
                        }
                    }
                    else
                    {
                        int i = 0;
                        while (i + 2 < items[5].Length)
                        {
                            int type = BitConv.FromInt16(items[5], i);
                            switch (type)
                            {
                                case 1:
                                    if (goolver == GOOLVersion.Version1)
                                        fgroups.Add(VertexGroup.Load(items[5], ref i));
                                    else if (goolver == GOOLVersion.Version2)
                                        fgroups.Add(VertexGroup2.Load(items[5], ref i));
                                    else if (goolver == GOOLVersion.Version3)
                                        fgroups.Add(VertexGroup3.Load(items[5], ref i));
                                    break;
                                case 2:
                                    if (goolver == GOOLVersion.Version1)
                                        fgroups.Add(SpriteGroup.Load(items[5], ref i));
                                    else if (goolver == GOOLVersion.Version2 || goolver == GOOLVersion.Version3)
                                        fgroups.Add(SpriteGroup2.Load(items[5], ref i));
                                    break;
                                case 3:
                                    if (goolver == GOOLVersion.Version1)
                                        fgroups.Add(FontGroup.Load(items[5], ref i));
                                    else if (goolver == GOOLVersion.Version2 || goolver == GOOLVersion.Version3)
                                        fgroups.Add(FontGroup2.Load(items[5], ref i));
                                    break;
                                case 4:
                                    fgroups.Add(TextGroup.Load(items[5], ref i));
                                    break;
                                case 5:
                                    if (goolver == GOOLVersion.Version1)
                                        fgroups.Add(ImageGroup.Load(items[5], ref i));
                                    else if (goolver == GOOLVersion.Version2 || goolver == GOOLVersion.Version3)
                                        fgroups.Add(ImageGroup2.Load(items[5], ref i));
                                    break;
                            }
                        }
                    }
                }
            }
            return new GOOLEntry(goolver, items[0], items[1], ins, statemap, statedesc, fgroups, eid);
        }
    }

    [EntryType(11, GameVersion.Crash1Beta1995)]
    [EntryType(11, GameVersion.Crash1BetaMAR08)]
    public sealed class GOOLv0EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items, int eid)
        {
            return GOOLEntryLoader.LoadGOOL(GOOLVersion.Version0, items, eid);
        }
    }

    [EntryType(11, GameVersion.Crash1BetaMAY11)]
    [EntryType(11, GameVersion.Crash1)]
    public sealed class GOOLv1EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items, int eid)
        {
            return GOOLEntryLoader.LoadGOOL(GOOLVersion.Version1, items, eid);
        }
    }

    [EntryType(11, GameVersion.Crash2)]
    public sealed class GOOLv2EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items, int eid)
        {
            return GOOLEntryLoader.LoadGOOL(GOOLVersion.Version2, items, eid);
        }
    }

    [EntryType(11, GameVersion.Crash3)]
    public sealed class GOOLv3EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items, int eid)
        {
            return GOOLEntryLoader.LoadGOOL(GOOLVersion.Version3, items, eid);
        }
    }
}
