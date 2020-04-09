namespace Crash.GOOLIns
{
    [GOOLInstruction(137,GameVersion.Crash1)]
    [GOOLInstruction(137,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(137,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(137,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(62,GameVersion.Crash2)]
    [GOOLInstruction(62,GameVersion.Crash3)]
    public sealed class Evha : GOOLInstruction
    {
        public Evha(int value, GOOLEntry gool) : base(value,gool) {}

        public override string Name => "EVHA";
        public override string Format => "IIIIIIIIII VVVV (RRRRRR) CC TT";
        public override string Comment
        {
            get
            {
                string str = string.Empty;
                if (Args['C'].Value != 0)
                {
                    if (Args['C'].Value == 1)
                        str += $"if {GetArg('R')}, ";
                    else if (Args['C'].Value == 2)
                        str += $"if not {GetArg('R')}, ";
                }
                str += "accept event";

                if (Args['T'].Value == 0)
                {
                    str += $" or move {GetArg('I')} instructions";
                }
                else if (Args['T'].Value == 1)
                {
                    str += $" and change state to {GetArg('I')}";
                }
                else if (Args['T'].Value == 2)
                {
                    str += $" and return";
                }

                return str;
            }
        }
    }
}
