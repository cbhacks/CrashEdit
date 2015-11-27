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
        private int? cleanoffset;

        public event EventHandler CommandExecuted;

        public CommandManager()
        {
            this.undochain = new Stack<Command>();
            this.redochain = new Stack<Command>();
            this.undostrchain = new Stack<string>();
            this.redostrchain = new Stack<string>();
            this.cleanoffset = 0;
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

        public bool Dirty
        {
            get { return cleanoffset != 0; }
        }

        public void MarkClean()
        {
            cleanoffset = 0;
        }

        public void Undo()
        {
            if (undochain.Count == 0)
                throw new InvalidOperationException();
            if (cleanoffset.HasValue)
            {
                cleanoffset--;
            }
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
            if (cleanoffset.HasValue)
            {
                cleanoffset++;
            }
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
            if (cleanoffset.HasValue && cleanoffset.Value < 0)
            {
                cleanoffset = null;
            }
            redochain.Clear();
            redochain.Push(command);
            redostrchain.Clear();
            redostrchain.Push(str);
            Redo();
        }
    }
}
