namespace Crash.GOOLIns
{
    [GOOLInstruction(145,GameVersion.Crash1)]
    [GOOLInstruction(145,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(70,GameVersion.Crash2)]
    [GOOLInstruction(70,GameVersion.Crash3)]
    public sealed class Chlf : GOOLInstruction
    {
        public Chlf(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "CHLF";
        public override string Format => "CCCCCC SSSSSS TTTTTTTT AAAA";
        public override string Comment => $"spawnf {(Args['C'].Value != 0 ? GetArg('C') : "[sp]")}x object {GetArg('T')} subtype {GetArg('S')}" + (Args['A'].Value > 0 ? $" with {GetArg('A')} arguments" : "");
    }
}
