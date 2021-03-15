﻿namespace CrashEdit.Crash.GOOLIns
{
    [GOOLInstruction(12,GameVersion.Crash1)]
    [GOOLInstruction(12,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(12,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(12,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(12,GameVersion.Crash2)]
    [GOOLInstruction(12,GameVersion.Crash3)]
    public sealed class Sge : GOOLInstruction
    {
        public Sge(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "SGE";
        public override string Format => DefaultFormatLR;
        public override string Comment => $"{GetArg('L')} >= {GetArg('R')}";
    }
}
