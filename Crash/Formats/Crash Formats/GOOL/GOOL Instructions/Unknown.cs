namespace Crash.GOOLIns
{
    [GOOLInstruction(42,GameVersion.Crash2)]
    [GOOLInstruction(42,GameVersion.Crash3)]
    [GOOLInstruction(43,GameVersion.Crash2)]
    [GOOLInstruction(43,GameVersion.Crash3)]
    [GOOLInstruction(44,GameVersion.Crash2)]
    [GOOLInstruction(44,GameVersion.Crash3)]
    [GOOLInstruction(45,GameVersion.Crash2)]
    [GOOLInstruction(45,GameVersion.Crash3)]
    [GOOLInstruction(46,GameVersion.Crash2)]
    [GOOLInstruction(46,GameVersion.Crash3)]
    [GOOLInstruction(72,GameVersion.Crash2)]
    [GOOLInstruction(72,GameVersion.Crash3)]
    [GOOLInstruction(76,GameVersion.Crash2)]
    [GOOLInstruction(76,GameVersion.Crash3)]
    [GOOLInstruction(77,GameVersion.Crash2)]
    [GOOLInstruction(77,GameVersion.Crash3)]
    [GOOLInstruction(78,GameVersion.Crash2)]
    [GOOLInstruction(78,GameVersion.Crash3)]
    public sealed class Unknown : GOOLInstruction
    {
        public Unknown(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => $"UNK{Opcode.ToString()}";
        public override string Format => DefaultFormat;
        public override string Comment => string.Empty;
    }
}
