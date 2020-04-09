namespace Crash.GOOLIns
{
    [GOOLInstruction(18,GameVersion.Crash1)]
    [GOOLInstruction(18,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(18,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(18,GameVersion.Crash1BetaMAY11)]
    public sealed class Notl : GOOLInstruction
    {
        public Notl(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "NOTL";
        public override string Format => DefaultFormatDS;
        public override string Comment => $"{GetArg('D')} = !{GetArg('S')}";
    }
    
    [GOOLInstruction(18,GameVersion.Crash2)]
    [GOOLInstruction(18,GameVersion.Crash3)]
    public sealed class Notl2 : GOOLInstruction
    {
        public Notl2(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "NOTL";
        public override string Format => DefaultFormatDS2;
        public override string Comment => $"{GetArg('D')} = !{GetArg('S')}";
    }
}
