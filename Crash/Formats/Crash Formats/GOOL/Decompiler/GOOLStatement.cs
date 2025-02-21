namespace CrashEdit.Crash
{
    public class GOOLStatement
    {
        public List<GOOLInstruction> Instructions { get; } = [];
        public GObj? LispOut { get; private set; } = null;

        public GObj DecompileSingleInsToLisp(ref int i)
        {
            return Instructions[i].DecompileToLisp(this, ref i);
        }

        public void DecompileToLispFull()
        {
            int i = 0;
            LispOut = DecompileSingleInsToLisp(ref i);
        }
    }
}
