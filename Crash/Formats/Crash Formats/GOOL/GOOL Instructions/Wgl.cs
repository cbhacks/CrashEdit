namespace Crash.GOOLIns
{
    [GOOLInstruction(32,GameVersion.Crash1)]
    [GOOLInstruction(32,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(32,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(32,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(32,GameVersion.Crash2)]
    [GOOLInstruction(32,GameVersion.Crash3)]
    public sealed class Wgl : GOOLInstruction
    {
        public Wgl(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "WGL";
        public override string Format => "[IIIIIIIIIIII] [SSSSSSSSSSSS]";
        public override string Comment => $"global[{GetArg('I')}] = {GetArg('S')}";
    }
}
