using System.Collections.Generic;

namespace Crash
{
    public sealed class UnprocessedEntry : MysteryMultiItemEntry
    {
        private int type;

        public UnprocessedEntry(IEnumerable<byte[]> items,int eid,int type) : base(items,eid)
        {
            this.type = type;
        }

        public override int Type => type;
        public int HeaderLength => 20 + Items.Count * 4;

        public Entry Process(GameVersion gameversion)
        {
            Dictionary<int,EntryLoader> loaders = GetLoaders(gameversion);
            if (loaders.ContainsKey(type))
            {
                var result = loaders[type].Load(((List<byte[]>)Items).ToArray(),EID);

                if (result.IgnoreResaveErrors)
                    return result;

                var resultOut = result.Unprocess();
                if (Items.Count != resultOut.Items.Count) {
                    ErrorManager.SignalIgnorableError("Entry: Processed entry deprocesses to different item count");
                    return result;
                }

                for (int i = 0; i < Items.Count; i++) {
                    byte[] shorterData;
                    byte[] longerData;
                    if (Items[i].Length < resultOut.Items[i].Length) {
                        shorterData = Items[i];
                        longerData = resultOut.Items[i];
                    } else {
                        shorterData = resultOut.Items[i];
                        longerData = Items[i];
                    }

                    // Compare the data held in both the original item data
                    // and the output item data. Warn on mismatch.
                    for (int j = 0; j < shorterData.Length; j++) {
                        if (shorterData[j] == longerData[j])
                            continue;

                        ErrorManager.SignalIgnorableError("Entry: Processed entry deprocesses to different item data");
                        return result;
                    }

                    // Compare excess bytes in whichever item's data was
                    // longer. Warn on non-zero.
                    //
                    // This check is done instead of a simple SequenceEqual
                    // via LINQ because the input may be padded differently.
                    for (int j = shorterData.Length; j < longerData.Length; j++) {
                        if (longerData[j] == 0)
                            continue;

                        ErrorManager.SignalIgnorableError("Entry: Processed entry deprocesses to different item data");
                        return result;
                    }
                }

                return result;
            }
            else
            {
                ErrorManager.SignalError("UnprocessedEntry: Unknown entry type");
                return null;
            }
        }

        public override UnprocessedEntry Unprocess()
        {
            return this;
        }

        public override byte[] Save()
        {
            int length = 20 + Items.Count * 4;
            Aligner.Align(ref length,4);
            foreach (byte[] item in Items)
            {
                length += item.Length;
                Aligner.Align(ref length,4);
            }
            byte[] data = new byte [length];
            BitConv.ToInt32(data,0,Magic);
            BitConv.ToInt32(data,4,EID);
            BitConv.ToInt32(data,8,Type);
            BitConv.ToInt32(data,12,Items.Count);
            int offset = 20 + Items.Count * 4;
            //Aligner.Align(ref offset,4);
            BitConv.ToInt32(data,16,offset);
            for (int i = 0;i < Items.Count;i++)
            {
                Items[i].CopyTo(data,offset);
                offset += Items[i].Length;
                Aligner.Align(ref offset,4);
                BitConv.ToInt32(data,20 + i * 4,offset);
            }
            return data;
        }
    }
}
