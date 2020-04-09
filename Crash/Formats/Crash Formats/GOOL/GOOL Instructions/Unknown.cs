namespace Crash.GOOLIns
{
    [GOOLInstruction(72,GameVersion.Crash2)]
    [GOOLInstruction(72,GameVersion.Crash3)]
    [GOOLInstruction(74,GameVersion.Crash2)]
    [GOOLInstruction(74,GameVersion.Crash3)]
    [GOOLInstruction(75,GameVersion.Crash2)]
    [GOOLInstruction(75,GameVersion.Crash3)]
    [GOOLInstruction(76,GameVersion.Crash2)]
    [GOOLInstruction(76,GameVersion.Crash3)]
    [GOOLInstruction(77,GameVersion.Crash2)]
    [GOOLInstruction(77,GameVersion.Crash3)]
    [GOOLInstruction(79,GameVersion.Crash3)]
    [GOOLInstruction(80,GameVersion.Crash3)]
    [GOOLInstruction(81,GameVersion.Crash3)]
    public sealed class Unknown : GOOLInstruction
    {
        public Unknown(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => $"UNK{Opcode}";
        public override string Format => DefaultFormat;
        public override string Comment => string.Empty;
    }
    
    [GOOLInstruction(46,GameVersion.Crash2)]
    [GOOLInstruction(46,GameVersion.Crash3)]
    public sealed class Unknown2 : GOOLInstruction
    {
        public Unknown2(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => $"UNK{Opcode}";
        public override string Format => DefaultFormatDS2;
        public override string Comment => string.Empty;
    }
}
