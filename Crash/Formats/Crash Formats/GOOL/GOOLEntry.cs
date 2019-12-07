using System;
using System.Collections.Generic;
using System.Reflection;

namespace Crash
{
    public sealed class GOOLEntry : Entry
    {
        internal static Dictionary<GOOLVersion,Dictionary<int,Type>> opsets;

        static GOOLEntry()
        {
            var assembly_types = Assembly.GetExecutingAssembly().GetTypes();
            opsets = new Dictionary<GOOLVersion,Dictionary<int,Type>>();
            foreach (Type type in assembly_types)
            {
                foreach (GOOLInstructionAttribute attribute in type.GetCustomAttributes(typeof(GOOLInstructionAttribute), false))
                {
                    if (!opsets.ContainsKey(attribute.Version))
                    {
                        opsets.Add(attribute.Version,new Dictionary<int,Type>());
                    }
                    Dictionary<int,Type> opset = opsets[attribute.Version];
                    if (!opset.ContainsKey(attribute.Opcode))
                    {
                        opset.Add(attribute.Opcode,type);
                    }
                }
            }
        }

        internal GOOLInstruction LoadInstruction(int ins, ref bool mips)
        {
            if (!mips)
            {
                if (opsets.ContainsKey(Version))
                {
                    Dictionary<int, Type> opset = opsets[Version];
                    int opcode = ins >> 24 & 0xFF;
                }
            }
            else
            {
            }
        }
        private List<GOOLInstruction> instructions;

        public GOOLEntry(GOOLVersion version,byte[] header,byte[] instructions,byte[] data,byte[] statemap,byte[] statedescriptors,byte[] anims,int eid,int size) : base(eid,size)
        {
            Version = version;
            Header = header;
            this.instructions = new List<GOOLInstruction>();
            bool mips = false;
            for (int i = 0; i < instructions.Length / 4; ++i)
            {
                int ins = BitConv.FromInt32(instructions,i*4);
                if (!mips)
                    this.instructions.Add(LoadInstruction(ins,ref mips));
                else
                {
                    this.instructions.Add(LoadInstruction(ins,ref mips));
                }
            }
            Data = data;
            StateMap = statemap;
            StateDescriptors = statedescriptors;
            Anims = anims;
        }

        public override int Type => 11;

        public GOOLVersion Version { get; }

        public byte[] Header { get; }
        public byte[] Data { get; }
        public byte[] StateMap { get; }
        public byte[] StateDescriptors { get; }
        public byte[] Anims { get; }

        public int Format => BitConv.FromInt32(Header,8);

        public IList<GOOLInstruction> Instructions => instructions;
        
        public int GetConst(int i)
        {
            return BitConv.FromInt32(Data,i*4);
        }

        public override UnprocessedEntry Unprocess()
        {
            int itemcount =
                Anims != null ? 6 : (
                StateDescriptors != null ? 5 : (
                StateMap != null ? 4 : 3
                )
                );
                
            byte[][] items = new byte [itemcount][];
            items[0] = Header;
            items[1] = new byte [instructions.Count * 4];
            for (int i = 0;i < instructions.Count;++i)
            {
                BitConv.ToInt32(items[1],i*4,instructions[i].Save());
            }
            items[2] = Data;
            if (itemcount >= 4)
            {
                items[3] = StateMap;
                if (itemcount >= 5)
                {
                    items[4] = StateDescriptors;
                    if (itemcount >= 6)
                    {
                        items[5] = Anims;
                    }
                }
            }
            return new UnprocessedEntry(items,EID,Type,Size);
        }
    }
}
