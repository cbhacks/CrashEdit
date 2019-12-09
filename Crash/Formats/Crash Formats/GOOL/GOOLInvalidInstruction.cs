namespace Crash
{
    public class GOOLInvalidInstruction : GOOLInstruction
    {
        public GOOLInvalidInstruction(int value,GOOLEntry gool) : base(value,gool) {}
        
        public override string Name => $"invalid opcode {Opcode}";
        public override string Format => "IIIIIIIIIIIIIIIIIIIIIIII";
        public override string Comment => string.Empty;
    }
}
