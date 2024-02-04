using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ControlsKeyboardInfo
    {
        public Keys Key { get; }
        public string Instruction { get; }

        public ControlsKeyboardInfo(Keys key, string instruction)
        {
            Key = key;
            Instruction = instruction;
        }

        public string Print(params object[] args) => string.Format(Key.ToString() + ": " + Instruction + "\n", args);
    }
}
