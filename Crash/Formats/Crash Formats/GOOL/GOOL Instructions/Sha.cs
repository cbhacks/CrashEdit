namespace Crash.GOOLIns
{
    [GOOLInstruction(21,GameVersion.Crash1)]
    [GOOLInstruction(21,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(21,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(21,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(21,GameVersion.Crash2)]
    [GOOLInstruction(21,GameVersion.Crash3)]
    public sealed class Sha : GOOLInstruction
    {
        public Sha(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "SHA";
        public override string Format => DefaultFormatLR;
        public override string Comment => $"{GetArg('L')} << {GetArg('R')}";
    }
}
