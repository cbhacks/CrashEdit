namespace Crash.GOOLIns
{
    [GOOLInstruction(15,GameVersion.Crash1)]
    [GOOLInstruction(15,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(15,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(15,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(15,GameVersion.Crash2)]
    [GOOLInstruction(15,GameVersion.Crash3)]
    public sealed class Tst : GOOLInstruction
    {
        public Tst(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "TST";
        public override string Format => DefaultFormatLR;
        public override string Comment => $"{GetArg('L')} has {GetArg('R')}";
    }
}
