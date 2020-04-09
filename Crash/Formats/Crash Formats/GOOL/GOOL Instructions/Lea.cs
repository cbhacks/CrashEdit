namespace Crash.GOOLIns
{
    [GOOLInstruction(20,GameVersion.Crash1)]
    [GOOLInstruction(20,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(20,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(20,GameVersion.Crash1BetaMAY11)]
    public sealed class Lea : GOOLInstruction
    {
        public Lea(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "LEA";
        public override string Format => DefaultFormatDS;
        public override string Comment => $"{GetArg('D')} = &{GetArg('S')}";
    }
    
    [GOOLInstruction(20,GameVersion.Crash2)]
    [GOOLInstruction(20,GameVersion.Crash3)]
    public sealed class Lea2 : GOOLInstruction
    {
        public Lea2(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "LEA";
        public override string Format => DefaultFormatDS2;
        public override string Comment => $"{GetArg('D')} = &{GetArg('S')}";
    }
}
