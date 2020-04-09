namespace Crash.GOOLIns
{
    [GOOLInstruction(42,GameVersion.Crash2)]
    [GOOLInstruction(42,GameVersion.Crash3)]
    public sealed class Arrl : GOOLInstruction
    {
        public Arrl(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "ARRL";
        public override string Format => DefaultFormatLR;
        public override string Comment => $"{GetArg('L')}[{GetArg('R')}]";
    }
}
