namespace Crash.GOOLIns
{
    [GOOLInstruction(25,GameVersion.Crash1)]
    [GOOLInstruction(25,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(25,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(25,GameVersion.Crash1BetaMAY11)]
    public sealed class Abs : GOOLInstruction
    {
        public Abs(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "ABS";
        public override string Format => DefaultFormatDS;
        public override string Comment => $"{GetArg('D')} = abs({GetArg('S')})";
    }
    
    [GOOLInstruction(25,GameVersion.Crash2)]
    [GOOLInstruction(25,GameVersion.Crash3)]
    public sealed class Abs2 : GOOLInstruction
    {
        public Abs2(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "ABS";
        public override string Format => DefaultFormatDS2;
        public override string Comment => $"{GetArg('D')} = abs({GetArg('S')})";
    }
}
