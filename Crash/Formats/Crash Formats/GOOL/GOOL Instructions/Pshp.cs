namespace Crash.GOOLIns
{
    [GOOLInstruction(38,GameVersion.Crash1)]
    [GOOLInstruction(38,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(38,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(38,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(38,GameVersion.Crash2)]
    [GOOLInstruction(38,GameVersion.Crash3)]
    public sealed class Pshp : GOOLInstruction
    {
        public Pshp(int value, GOOLEntry gool) : base(value,gool) {}

        public override string Name => "PSHP";
        public override string Format => DefaultFormat;
        public override string Comment
        {
            get
            {
                if (Args['B'].Value != NullRef)
                    return $"push &{GetArg('A')} and &{GetArg('B')}";
                else
                    return $"push &{GetArg('A')}";
            }
        }
    }
}
