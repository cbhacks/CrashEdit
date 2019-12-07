namespace Crash.GOOLIns
{
    [GOOLInstruction(0,GameVersion.Crash1)]
    [GOOLInstruction(0,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(0,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(0,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(0,GameVersion.Crash2)]
    [GOOLInstruction(0,GameVersion.Crash3)]
    public sealed class Add : GOOLInstruction
    {
        public Add(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "ADD";
        public override string Format => DefaultFormatLR;
        public override string Comment => $"{GetArg('L')} + {GetArg('R')}";
    }
}
