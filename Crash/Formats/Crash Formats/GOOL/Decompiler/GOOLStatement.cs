namespace CrashEdit.Crash
{
    public enum GoolStatementType
    {
        Normal,
        LetBegin,
        LetEnd
    }

    public class GOOLStatement
    {
        public List<GOOLInstruction> Instructions { get; } = [];
        public GObj LispOut { get; private set; } = null;

        public GoolStatementType Type { get; set; } = GoolStatementType.Normal;

        public int GetRealInstructionCount()
        {
            return Instructions.Count - Instructions.Where(ins => ins.DecompFakeInstruction).Count();
        }

        public GObj DecompileSingleInsToLisp(ref int i)
        {
            return Instructions[i].DecompileToLisp(this, ref i);
        }

        public void DecompileToLispFull()
        {
            int i = 0;
            LispOut = DecompileSingleInsToLisp(ref i);
            if (i < Instructions.Count - 1)
            {
                int zz = 00;
            }
        }

        public void PrintLispOutputFull(ref int indent, ref string fout, ref int stackvar)
        {
            var istr = new string(' ', indent);
            if (LispOut is TokenObj tok)
            {
                fout += istr + $"(let ((stack{stackvar++} {tok.Print()}))";
                indent += 2;
            }
            fout += istr + LispOut.Print() + "\n";
        }
    }
}
