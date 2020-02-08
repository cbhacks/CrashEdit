namespace Crash.GOOLIns
{
    [GOOLInstruction(16,GameVersion.Crash1)]
    [GOOLInstruction(16,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(16,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(16,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(16,GameVersion.Crash2)]
    [GOOLInstruction(16,GameVersion.Crash3)]
    public sealed class Rnd : GOOLInstruction
    {
        public Rnd(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "RND";
        public override string Format => "[BBBBBBBBBBBB] [AAAAAAAAAAAA]";
        public override string Comment => $"rand({GetArg('B')}, {GetArg('A')})";
    }
}
