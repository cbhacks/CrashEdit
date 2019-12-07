namespace Crash.GOOLIns
{
    [GOOLInstruction(8,GameVersion.Crash1)]
    [GOOLInstruction(8,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(8,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(8,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(8,GameVersion.Crash2)]
    [GOOLInstruction(8,GameVersion.Crash3)]
    public sealed class Orb : GOOLInstruction
    {
        public Orb(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "ORB";
        public override string Format => DefaultFormatLR;
        public override string Comment => $"{GetArg('L')} | {GetArg('R')}";
    }
}
