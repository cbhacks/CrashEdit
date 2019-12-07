namespace Crash.GOOLIns
{
    [GOOLInstruction(4,GameVersion.Crash1)]
    [GOOLInstruction(4,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(4,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(4,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(4,GameVersion.Crash2)]
    [GOOLInstruction(4,GameVersion.Crash3)]
    public sealed class Ceq : GOOLInstruction
    {
        public Ceq(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "CEQ";
        public override string Format => DefaultFormatLR;
        public override string Comment => $"{GetArg('L')} == {GetArg('R')}";
    }
}
