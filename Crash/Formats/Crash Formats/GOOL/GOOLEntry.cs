using System.Reflection;

namespace CrashEdit.Crash
{
    public sealed class GOOLEntry : Entry
    {
        internal static Dictionary<GOOLVersion, Dictionary<int, Type>> opsets;

        static GOOLEntry()
        {
            var assembly_types = Assembly.GetExecutingAssembly().GetTypes();
            opsets = new();
            foreach (Type type in assembly_types)
            {
                foreach (GOOLInstructionAttribute attribute in type.GetCustomAttributes(typeof(GOOLInstructionAttribute), false))
                {
                    if (!opsets.TryGetValue(attribute.Version, out var value))
                    {
                        value = new();
                        opsets.Add(attribute.Version, value);
                    }
                    Dictionary<int, Type> opset = value;
                    opset.TryAdd(attribute.Opcode, type);
                }
            }
        }

        internal GOOLInstruction LoadInstruction(int ins, bool mips)
        {
            if (!mips)
            {
                if (opsets.TryGetValue(Version, out var value))
                {
                    Dictionary<int, Type> opset = value;
                    int opcode = ins >> 24 & 0xFF;
                    if (opset.ContainsKey(opcode))
                    {
                        return new GOOLInstruction(ins, this, opset[opcode]);
                    }
                }
                return new GOOLUnknownInstruction(ins, this);
            }
            else
            {
                return new MIPSInstruction(ins, this);
            }
        }

        private readonly List<GOOLInstruction> instructions;
        private readonly List<GOOLStateDescriptor> statedescriptors;
        private readonly List<GOOLFrameGroupBase> framegroups;
        private readonly List<int> externals;

        public GOOLEntry(GOOLVersion version, byte[] header, byte[] instructions, int[] data, short[] statemap, IEnumerable<GOOLStateDescriptor> statedescriptors, IEnumerable<GOOLFrameGroupBase> fgroups, int eid) : base(eid)
        {
            Version = version;
            Header = header;
            this.instructions = new();
            bool mips = false;
            for (int i = 0; i < instructions.Length / 4; ++i)
            {
                int encins = BitConv.FromInt32(instructions, i * 4);
                GOOLInstruction ins = LoadInstruction(encins, mips);
                if (version == GOOLVersion.Version3 && (ins.Opcode == 142 || ins.Opcode == 174 || encins == 0x26D6FFE4 || encins == 0x00002821 || encins == 0x34050001 || (uint)encins == 0x8C670094U))
                {
                    mips = true;
                    ins = LoadInstruction(BitConv.FromInt32(instructions, i * 4), mips);
                }
                this.instructions.Add(ins);
                if (mips)
                {
                    MIPSInstruction prev = null;
                    if (this.instructions[this.instructions.Count - 2] is MIPSInstruction mips_ins)
                        prev = mips_ins;
                    if (prev != null && (prev.Value == 0x03E0A809 || prev.Value == 0x03E00008)) // native mips returns or ends here
                        mips = false;
                }
                else
                    mips = GOOLInterpreter.IsMIPSInstruction(ins);
            }
            Data = data;
            StateMap = statemap;
            externals = new();
            if (statedescriptors == null)
                this.statedescriptors = null;
            else
            {
                this.statedescriptors = new List<GOOLStateDescriptor>(statedescriptors);
                foreach (var state in this.statedescriptors)
                {
                    int ext_eid = Data[state.GOOLIndex];
                    if (ext_eid != eid && !externals.Contains(ext_eid))
                    {
                        externals.Add(ext_eid);
                    }
                }
            }
            framegroups = fgroups == null && version == GOOLVersion.Version0 ? null : new(fgroups);
        }

        public override string Title => Version switch
        {
            GOOLVersion.Version0 => $"Prototype GOOL ({EName})",
            GOOLVersion.Version1 => $"GOOL ({EName})",
            GOOLVersion.Version2 => $"GOOLv2 ({EName})",
            GOOLVersion.Version3 => $"GOOLv3 ({EName})",
            _ => $"GOOL ({EName})",
        };

        public override string ImageKey => "ThingCode";

        public override int Type => 11;

        public GOOLVersion Version { get; }

        public byte[] Header { get; }
        public int[] Data { get; }
        public short[] StateMap { get; }
        public IList<GOOLStateDescriptor> StateDescriptors => statedescriptors;
        public IList<GOOLFrameGroupBase> FrameGroups => framegroups;

        public int ID => BitConv.FromInt32(Header, 0);
        public int Class => BitConv.FromInt32(Header, 4);
        public int Format => BitConv.FromInt32(Header, 8);
        public int HeapBase => BitConv.FromInt32(Header, 12);
        public int EventCount => BitConv.FromInt32(Header, 16);
        public int EntryCount => BitConv.FromInt32(Header, 20);

        public IList<GOOLInstruction> Instructions => instructions;

        public IList<int> Externals => externals;
        public GOOLEntry ParentGOOL { get; set; }

        public override UnprocessedEntry Unprocess()
        {
            int itemcount = Format == 1 ? (FrameGroups == null ? 5 : 6) : 3;

            byte[][] items = new byte[itemcount][];
            items[0] = Header;
            items[1] = new byte[instructions.Count * 4];
            for (int i = 0; i < instructions.Count; ++i)
            {
                BitConv.ToInt32(items[1], i * 4, instructions[i].Save());
            }
            items[2] = new byte[Data.Length * 4];
            for (int i = 0; i < Data.Length; ++i)
            {
                BitConv.ToInt32(items[2], i * 4, Data[i]);
            }
            if (Format == 1)
            {
                items[3] = new byte[StateMap.Length * 2];
                for (int i = 0; i < StateMap.Length; ++i)
                {
                    BitConv.ToInt16(items[3], i * 2, StateMap[i]);
                }
                items[4] = new byte[statedescriptors.Count * 0x10];
                for (int i = 0; i < statedescriptors.Count; ++i)
                {
                    statedescriptors[i].Save().CopyTo(items[4], i * 0x10);
                }
                if (FrameGroups != null)
                {
                    items[5] = new byte[0];
                    foreach (var group in FrameGroups)
                    {
                        var data = group.Save();
                        Array.Resize(ref items[5], data.Length + items[5].Length);
                        Array.Copy(data, 0, items[5], items[5].Length - data.Length, data.Length);
                    }
                }
            }
            return new UnprocessedEntry(items, EID, Type);
        }
    }
}
