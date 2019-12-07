namespace Crash.GOOLIns
{
    [GOOLInstruction(14,GameVersion.Crash1)]
    [GOOLInstruction(14,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(14,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(14,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(14,GameVersion.Crash2)]
    [GOOLInstruction(14,GameVersion.Crash3)]
    public sealed class Xor : GOOLInstruction
    {
        public Xor(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "XOR";
        public override string Format => DefaultFormatLR;
        public override string Comment => $"{GetArg('L')} ^ {GetArg('R')}";
    }
}
