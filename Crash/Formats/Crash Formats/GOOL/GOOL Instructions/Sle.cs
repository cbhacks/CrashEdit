namespace Crash.GOOLIns
{
    [GOOLInstruction(10,GameVersion.Crash1)]
    [GOOLInstruction(10,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(10,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(10,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(10,GameVersion.Crash2)]
    [GOOLInstruction(10,GameVersion.Crash3)]
    public sealed class Sle : GOOLInstruction
    {
        public Sle(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "SLE";
        public override string Format => DefaultFormatLR;
        public override string Comment => $"{GetArg('L')} <= {GetArg('R')}";
    }
}
