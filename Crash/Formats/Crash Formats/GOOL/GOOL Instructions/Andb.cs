namespace Crash.GOOLIns
{
    [GOOLInstruction(7,GameVersion.Crash1)]
    [GOOLInstruction(7,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(7,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(7,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(7,GameVersion.Crash2)]
    [GOOLInstruction(7,GameVersion.Crash3)]
    public sealed class Andb : GOOLInstruction
    {
        public Andb(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "ANDB";
        public override string Format => DefaultFormatLR;
        public override string Comment => $"{GetArg('L')} & {GetArg('R')}";
    }
}
