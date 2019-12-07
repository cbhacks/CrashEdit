namespace Crash.GOOLIns
{
    [GOOLInstruction(24,GameVersion.Crash1)]
    [GOOLInstruction(24,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(24,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(24,GameVersion.Crash1BetaMAY11)]
    public sealed class Movc_1 : GOOLInstruction
    {
        public Movc_1(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "MOVC";
        public override string Format => "IIIIIIIIIIIIII (RRRRRR) 0000";
        public override string Comment => $"{GetArg('R')} = ins[{GetArg('I')}]";
    }

    [GOOLInstruction(24,GameVersion.Crash2)]
    [GOOLInstruction(24,GameVersion.Crash3)]
    public sealed class Movc_2 : GOOLInstruction
    {
        public Movc_2(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "MOVC";
        public override string Format => "IIIIIIIIIIIIII E 000 (RRRRRR)";
        public override string Comment => $"{GetArg('R')} = {(Args['E'].Value == 0 ? "ins" : "ext")}[{GetArg('I')}]";
    }
}
