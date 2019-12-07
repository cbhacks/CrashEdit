namespace Crash.GOOLIns
{
    [GOOLInstruction(51,GameVersion.Crash2)]
    [GOOLInstruction(51,GameVersion.Crash3)]
    public sealed class Bnez : GOOLInstruction
    {
        public Bnez(int value,GOOLEntry gool) : base(value,gool)
        {
            Args['I'] = new GOOLArgument(BitConv.SignExtendInt32(Args['I'].Value,10)); // sign-extension
        }

        public override string Name => "BNEZ";
        public override string Format => "IIIIIIIIII VVVV (RRRRRR) 10 00";
        public override string Comment
        {
            get
            {
                int v = Args['V'].Value;
                int i = Args['I'].Value;
                string str = $"if {(ObjectFields)Args['R'].Value} is true, ";
                if (v != 0 && i != 0)
                {
                    return str + $"move {i} instructions and pop {v} values";
                }
                else if (v == 0)
                {
                    return str + $"move {i} instructions";
                }
                else
                {
                    return str + $"pop {v} values";
                }
            }
        }
    }
}
