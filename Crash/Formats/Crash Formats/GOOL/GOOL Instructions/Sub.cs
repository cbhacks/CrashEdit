namespace Crash.GOOLIns
{
    [GOOLInstruction(1,GameVersion.Crash1)]
    [GOOLInstruction(1,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(1,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(1,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(1,GameVersion.Crash2)]
    [GOOLInstruction(1,GameVersion.Crash3)]
    public sealed class Sub : GOOLInstruction
    {
        public Sub(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "SUB";
        public override string Format => DefaultFormatLR;
        public override string Comment => $"{GetArg('L')} - {GetArg('R')}";
    }
}
