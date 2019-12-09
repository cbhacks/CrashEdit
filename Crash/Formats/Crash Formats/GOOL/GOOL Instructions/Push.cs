namespace Crash.GOOLIns
{
    [GOOLInstruction(22,GameVersion.Crash1)]
    [GOOLInstruction(22,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(22,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(22,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(22,GameVersion.Crash2)]
    [GOOLInstruction(22,GameVersion.Crash3)]
    public sealed class Push : GOOLInstruction
    {
        public Push(int value, GOOLEntry gool) : base(value,gool) {}

        public override string Name => "PUSH";
        public override string Format => DefaultFormat;
        public override string Comment
        {
            get
            {
                if (Args['B'].Value != NullRef)
                    return $"push {GetArg('A')} and {GetArg('B')}";
                else
                    return $"push {GetArg('A')}";
            }
        }
    }
}
