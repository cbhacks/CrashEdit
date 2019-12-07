namespace Crash.GOOLIns
{
    [GOOLInstruction(11,GameVersion.Crash1)]
    [GOOLInstruction(11,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(11,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(11,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(11,GameVersion.Crash2)]
    [GOOLInstruction(11,GameVersion.Crash3)]
    public sealed class Sgt : GOOLInstruction
    {
        public Sgt(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "SGT";
        public override string Format => DefaultFormatLR;
        public override string Comment => $"{GetArg('L')} > {GetArg('R')}";
    }
}
