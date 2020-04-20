namespace Crash.GOOLIns
{
    [GOOLInstruction(55,GameVersion.Crash2)]
    [GOOLInstruction(55,GameVersion.Crash3)]
    public sealed class Ceqz : GOOLInstruction
    {
        public Ceqz(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "CEQZ";
        public override string Format => "SSSSSSSSSS VVVV (RRRRRR) 01 10";
        public override string Comment => $"if {(ObjectFields)Args['R'].Value} is false, change to state {GetArg('S')}" + (Args['V'].Value > 0 ? $" with {GetArg('V')} argument(s)" : string.Empty);
    }
}
