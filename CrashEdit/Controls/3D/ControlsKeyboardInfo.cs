namespace CrashEdit.CE
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

        public string PrintKeys()
        {
            if ((Key & Keys.Modifiers) == 0)
                return Key.ToString();

            string modifiers = (Key & Keys.Control) != 0 ? "Ctrl" : string.Empty;
            if ((Key & Keys.Shift) != 0)
                modifiers += string.IsNullOrEmpty(modifiers) ? "Shift" : " + Shift";
            if ((Key & Keys.Alt) != 0)
                modifiers += string.IsNullOrEmpty(modifiers) ? "Alt" : " + Alt";
            return (Key & Keys.KeyCode) == 0 ? modifiers : $"{modifiers} + {Key & Keys.KeyCode}";
        }

        public string Print(params object[] args) => string.Format(PrintKeys() + ": " + Instruction + "\n", args);
    }
}
