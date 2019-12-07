namespace Crash.GOOLIns
{
    [GOOLInstruction(52,GameVersion.Crash2)]
    [GOOLInstruction(52,GameVersion.Crash3)]
    public sealed class Beqz : GOOLInstruction
    {
        public Beqz(int value,GOOLEntry gool) : base(value,gool)
        {
            Args['I'] = new GOOLArgument(BitConv.SignExtendInt32(Args['I'].Value,10)); // sign-extension
        }

        public override string Name => "BEQZ";
        public override string Format => "IIIIIIIIII VVVV (RRRRRR) 01 00";
        public override string Comment
        {
            get
            {
                int v = Args['V'].Value;
                int i = Args['I'].Value;
                string str = $"if {(ObjectFields)Args['R'].Value} is false, ";
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
