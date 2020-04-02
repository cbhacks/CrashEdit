namespace Crash.GOOLIns
{
    [GOOLInstruction(134,GameVersion.Crash1)]
    [GOOLInstruction(134,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(134,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(134,GameVersion.Crash1BetaMAY11)]
    public sealed class Jal_1 : GOOLInstruction
    {
        public Jal_1(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "JAL";
        public override string Format => "IIIIIIIIIIIIII (RRRRRR) VVVV";
        public override string Comment => $"jump and link to {Args['I'].Value}" + (Args['V'].Value > 0 ? $" with {GetArg('V')} argument(s)" : string.Empty);
    }

    [GOOLInstruction(59,GameVersion.Crash2)]
    [GOOLInstruction(59,GameVersion.Crash3)]
    public sealed class Jal_2 : GOOLInstruction
    {
        public Jal_2(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "JAL";
        public override string Format => "IIIIIIIIIIIIII E 00000 VVVV";
        public override string Comment => $"jump and link to {Args['I'].Value}" + (Args['E'].Value == 1 ? " (external)" : string.Empty) + (Args['V'].Value > 0 ? $" with {GetArg('V')} argument(s)" : string.Empty);
    }
}
