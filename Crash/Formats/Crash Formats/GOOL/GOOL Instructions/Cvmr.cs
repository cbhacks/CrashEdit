namespace Crash.GOOLIns
{
    [GOOLInstruction(35,GameVersion.Crash1)]
    [GOOLInstruction(35,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(35,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(35,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(35,GameVersion.Crash2)]
    [GOOLInstruction(35,GameVersion.Crash3)]
    public sealed class Cvmr : GOOLInstruction
    {
        public Cvmr(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "CVMR";
        public override string Format => "000001111101 LLL IIIIII 000";
        public override string Comment => $"{(ObjectFields)Args['L'].Value}->{GOOLInterpreter.GetColor(GOOL.Version,Args['I'].Value)}";
    }
}
