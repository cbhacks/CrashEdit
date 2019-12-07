namespace Crash.GOOLIns
{
    [GOOLInstruction(5,GameVersion.Crash1)]
    [GOOLInstruction(5,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(5,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(5,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(5,GameVersion.Crash2)]
    [GOOLInstruction(5,GameVersion.Crash3)]
    public sealed class Andl : GOOLInstruction
    {
        public Andl(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "ANDL";
        public override string Format => DefaultFormatLR;
        public override string Comment => $"{GetArg('L')} && {GetArg('R')}";
    }
}
