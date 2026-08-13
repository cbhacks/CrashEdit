namespace CrashEdit.Crash
{
    public class GOOLUnknownInstruction : GOOLInstruction
    {
        public GOOLUnknownInstruction(int value, GOOLEntry gool) : base(value, gool, null) { }

        public override string GetName() => $"INS{ID}";
        public override string GetFormat() => "IIIIIIIIIIIIIIIIIIIIIIII";
        public override string GetComment() => $"invalid opcode {ID}";
    }
}
