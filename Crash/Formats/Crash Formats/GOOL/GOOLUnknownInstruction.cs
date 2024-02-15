namespace Crash
{
    public class GOOLUnknownInstruction : GOOLInstruction
    {
        public GOOLUnknownInstruction(int value, GOOLEntry gool) : base(value, gool, null) { }

        public override string GetName() => $"INS{Opcode}";
        public override string GetFormat() => "IIIIIIIIIIIIIIIIIIIIIIII";
        public override string GetComment() => $"invalid opcode {Opcode}";
    }
}
