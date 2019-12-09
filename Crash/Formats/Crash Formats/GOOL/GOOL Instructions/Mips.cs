namespace Crash.GOOLIns
{
    [GOOLInstruction(73,GameVersion.Crash2)]
    [GOOLInstruction(73,GameVersion.Crash3)]
    public sealed class Mips : GOOLInstruction
    {
        public Mips(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "MIPS";
        public override string Format => "101111100000101111100000".Reverse();
        public override string Comment => "begin PSX native bytecode";
    }
}
