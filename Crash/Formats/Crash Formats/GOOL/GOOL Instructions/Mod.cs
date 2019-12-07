namespace Crash.GOOLIns
{
    [GOOLInstruction(13,GameVersion.Crash1)]
    [GOOLInstruction(13,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(13,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(13,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(13,GameVersion.Crash2)]
    [GOOLInstruction(13,GameVersion.Crash3)]
    public sealed class Mod : GOOLInstruction
    {
        public Mod(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "MOD";
        public override string Format => DefaultFormatLR;
        public override string Comment => $"{GetArg('L')} % {GetArg('R')}";
    }
}
