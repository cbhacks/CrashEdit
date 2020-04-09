namespace Crash.GOOLIns
{
    [GOOLInstruction(37,GameVersion.Crash1)]
    [GOOLInstruction(37,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(37,GameVersion.Crash2)]
    [GOOLInstruction(37,GameVersion.Crash3)]
    public sealed class Dsek : GOOLInstruction
    {
        public Dsek(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "DSEK";
        public override string Format => DefaultFormatLR;
        public override string Comment => $"degseek({GetArg('L')}, " + (Args['R'].Value == DoubleStackRef ? "[sp-1], [sp])" : $"{GetArg('R')}, 0x100)");
    }
}
