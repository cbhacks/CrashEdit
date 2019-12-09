using System;

namespace Crash
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class GOOLInstructionAttribute : Attribute
    {
        public GOOLInstructionAttribute(int opcode, GameVersion gameversion)
        {
            Opcode = opcode;
            Version = GOOLInterpreter.GetVersion(gameversion);
        }

        public GOOLInstructionAttribute(int opcode, GOOLVersion goolversion)
        {
            Opcode = opcode;
            Version = goolversion;
        }

        public int Opcode { get; }
        public GOOLVersion Version { get; }
    }
}
