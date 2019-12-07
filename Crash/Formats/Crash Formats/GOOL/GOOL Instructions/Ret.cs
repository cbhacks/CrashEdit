namespace Crash.GOOLIns
{
    [GOOLInstruction(49,GameVersion.Crash2)]
    [GOOLInstruction(49,GameVersion.Crash3)]
    public sealed class Ret : GOOLInstruction
    {
        public Ret(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "RET";
        public override string Format => "00000000000000 101001 00 01";
        public override string Comment => $"return";
    }
}
