namespace Crash.GOOLIns
{
    [GOOLInstruction(0x81,GameVersion.Crash1)]
    [GOOLInstruction(0x81,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(0x81,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(0x81,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(47,GameVersion.Crash2)]
    [GOOLInstruction(47,GameVersion.Crash3)]
    public sealed class Nop : GOOLInstruction
    {
        public Nop(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "NOP";
        public override string Format => "101111100000101111100000".Reverse();
        public override string Comment => "no operation";
    }
}
