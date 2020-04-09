namespace Crash.GOOLIns
{
    [GOOLInstruction(23,GameVersion.Crash1)]
    [GOOLInstruction(23,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(23,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(23,GameVersion.Crash1BetaMAY11)]
    public sealed class Notb : GOOLInstruction
    {
        public Notb(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "NOTB";
        public override string Format => DefaultFormatDS;
        public override string Comment => $"{GetArg('D')} = ~{GetArg('S')}";
    }
    
    [GOOLInstruction(23,GameVersion.Crash2)]
    [GOOLInstruction(23,GameVersion.Crash3)]
    public sealed class Notb2 : GOOLInstruction
    {
        public Notb2(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "NOTB";
        public override string Format => DefaultFormatDS2;
        public override string Comment => $"{GetArg('D')} = ~{GetArg('S')}";
    }
}
