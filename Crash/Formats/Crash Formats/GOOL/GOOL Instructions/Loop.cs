namespace Crash.GOOLIns
{
    [GOOLInstruction(19,GameVersion.Crash1)]
    [GOOLInstruction(19,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(19,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(19,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(19,GameVersion.Crash2)]
    [GOOLInstruction(19,GameVersion.Crash3)]
    public sealed class Loop : GOOLInstruction
    {
        public Loop(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "LOOP";
        public override string Format => DefaultFormatLR;
        public override string Comment => $"loop({GetArg('L')}, " + (Args['R'].Value == DoubleStackRef ? "[sp-1], [sp])" : $"{GetArg('R')}, 0x100)");
    }
}
