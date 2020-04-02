namespace Crash.GOOLIns
{
    [GOOLInstruction(78,GameVersion.Crash2)]
    [GOOLInstruction(78,GameVersion.Crash3)]
    public sealed class Arrs : GOOLInstruction
    {
        public Arrs(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "ARRS";
        public override string Format => DefaultFormatLR;
        public override string Comment => $"{GetArg('L')}[{GetArg('R')}] = [sp]";
    }
}
