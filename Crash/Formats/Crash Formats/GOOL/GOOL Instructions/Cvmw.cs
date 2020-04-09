namespace Crash.GOOLIns
{
    [GOOLInstruction(36,GameVersion.Crash1)]
    [GOOLInstruction(36,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(36,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(36,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(36,GameVersion.Crash2)]
    [GOOLInstruction(36,GameVersion.Crash3)]
    public sealed class Cvmw : GOOLInstruction
    {
        public Cvmw(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "CVMW";
        public override string Format => "[CCCCCCCCCCCC] LLL IIIIII 000";
        public override string Comment => $"{(ObjectFields)Args['L'].Value}->{GOOLInterpreter.GetColor(GOOL.Version,Args['I'].Value)} = {GetArg('C')}";
    }
}
