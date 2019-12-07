namespace Crash.GOOLIns
{
    [GOOLInstruction(3,GameVersion.Crash1)]
    [GOOLInstruction(3,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(3,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(3,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(3,GameVersion.Crash2)]
    [GOOLInstruction(3,GameVersion.Crash3)]
    public sealed class Div : GOOLInstruction
    {
        public Div(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "DIV";
        public override string Format => DefaultFormatLR;
        public override string Comment => $"{GetArg('L')} / {GetArg('R')}";
    }
}
