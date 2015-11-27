using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class CommandManager
    {
        private Stack<Command> undochain;
        private Stack<Command> redochain;
        private Stack<string> undostrchain;
        private Stack<string> redostrchain;

        public event EventHandler CommandExecuted;

        public CommandManager()
        {
            this.undochain = new Stack<Command>();
            this.redochain = new Stack<Command>();
            this.undostrchain = new Stack<string>();
            this.redostrchain = new Stack<string>();
        }

        public int UndoDepth
        {
            get { return undochain.Count; }
        }

        public int RedoDepth
        {
            get { return redochain.Count; }
        }

        public IEnumerable<string> UndoChain
        {
            get { return undostrchain; }
        }

        public IEnumerable<string> RedoChain
        {
            get { return redostrchain; }
        }

        public void Undo()
        {
            if (undochain.Count == 0)
                throw new InvalidOperationException();
            redochain.Push(undochain.Pop().Run());
            redostrchain.Push(undostrchain.Pop());
            if (CommandExecuted != null)
            {
                CommandExecuted(this,EventArgs.Empty);
            }
        }

        public void Undo(int times)
        {
            if (times == 0)
                return;
            if (times < 0)
                throw new ArgumentOutOfRangeException("times");
            if (times > undochain.Count)
                throw new InvalidOperationException();
            for (int i = 0;i < times;i++)
            {
                Undo();
            }
        }

        public void Redo()
        {
            if (redochain.Count == 0)
                throw new InvalidOperationException();
            undochain.Push(redochain.Pop().Run());
            undostrchain.Push(redostrchain.Pop());
            if (CommandExecuted != null)
            {
                CommandExecuted(this,EventArgs.Empty);
            }
        }

        public void Redo(int times)
        {
            if (times == 0)
                return;
            if (times < 0)
                throw new ArgumentOutOfRangeException("times");
            if (times > redochain.Count)
                throw new InvalidOperationException();
            for (int i = 0;i < times;i++)
            {
                Redo();
            }
        }

        public void Submit(Command command,string str)
        {
            redochain.Clear();
            redochain.Push(command);
            redostrchain.Clear();
            redostrchain.Push(str);
            Redo();
        }
    }
}
