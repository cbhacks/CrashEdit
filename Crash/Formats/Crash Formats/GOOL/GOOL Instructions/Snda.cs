namespace Crash.GOOLIns
{
    [GOOLInstruction(141,GameVersion.Crash1)]
    [GOOLInstruction(141,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(141,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(141,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(66,GameVersion.Crash2)]
    [GOOLInstruction(66,GameVersion.Crash3)]
    public sealed class Snda : GOOLInstruction
    {
        public Snda(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "SNDA";
        public override string Format => "[SSSSSSSSSSSS] (RRRRRR) FF TTTT";
        public override string Comment
        {
            get
            {
                switch (Args['T'].Value)
                {
                    case 1: return $"set audio pitch to {GetArg('S')}";
                    case 4: return $"set audio count to {GetArg('S')}";
                    case 7: return $"set audio delay to {GetArg('S')}";
                    case 12: return $"set audio decay rate to {GetArg('S')}";
                    default: return string.Empty;
                }
            }
        }
    }
}
