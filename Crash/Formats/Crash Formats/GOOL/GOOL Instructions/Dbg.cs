namespace Crash.GOOLIns
{
    [GOOLInstruction(128,GameVersion.Crash1)]
    [GOOLInstruction(128,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(128,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(128,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(48,GameVersion.Crash2)]
    [GOOLInstruction(48,GameVersion.Crash3)]
    public sealed class Dbg : GOOLInstruction
    {
        public Dbg(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "DBG";
        public override string Format => DefaultFormat;
        public override string Comment => $"print {GetArg('A')} and {GetArg('B')}";
    }
}
