namespace Crash.GOOLIns
{
    [GOOLInstruction(17,GameVersion.Crash1)]
    [GOOLInstruction(17,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(17,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(17,GameVersion.Crash1BetaMAY11)]
    public sealed class Mov : GOOLInstruction
    {
        public Mov(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "MOV";
        public override string Format => DefaultFormatDS;
        public override string Comment => $"{GetArg('D')} = {GetArg('S')}";
    }
    
    [GOOLInstruction(17,GameVersion.Crash2)]
    [GOOLInstruction(17,GameVersion.Crash3)]
    public sealed class Mov2 : GOOLInstruction
    {
        public Mov2(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "MOV";
        public override string Format => DefaultFormatDS2;
        public override string Comment => $"{GetArg('D')} = {GetArg('S')}";
    }
}
