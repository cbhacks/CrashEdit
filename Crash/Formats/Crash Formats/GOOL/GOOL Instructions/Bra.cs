namespace Crash.GOOLIns
{
    [GOOLInstruction(50,GameVersion.Crash2)]
    [GOOLInstruction(50,GameVersion.Crash3)]
    public sealed class Bra : GOOLInstruction
    {
        public Bra(int value, GOOLEntry gool) : base(value,gool)
        {
            Args['I'] = new GOOLArgument(BitConv.SignExtendInt32(Args['I'].Value,10)); // sign-extension
        }

        public override string Name => "BRA";
        public override string Format => "IIIIIIIIII VVVV (RRRRRR) 00 00";
        public override string Comment
        {
            get
            {
                int v = Args['V'].Value;
                int i = Args['I'].Value;
                if (v != 0 && i != 0)
                {
                    return $"move {i} instructions and pop {v} values";
                }
                else if (v == 0)
                {
                    return $"move {i} instructions";
                }
                else
                {
                    return $"pop {v} values";
                }
            }
        }
    }
}
