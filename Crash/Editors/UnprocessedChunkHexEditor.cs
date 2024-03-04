using CrashEdit.Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class UnprocessedChunkHexEditor : Editor
    {
        public override string Text => "Hex";

        public override bool ApplicableForSubject(Controller subj)
        {
            ArgumentNullException.ThrowIfNull(subj);

            return subj.Resource is UnprocessedChunk;
        }

        protected override Control MakeControl()
        {
            return new HexView
            {
                Data = ((UnprocessedChunk)Subject.Resource).Data,
                DataChangeHandler = HexView_DataChangeHandler,
            };
        }

        private bool HexView_DataChangeHandler(int destOffset, int destLength, byte[] source)
        {
            var data = ((UnprocessedChunk)Subject.Resource).Data;

            if (destLength != source.Length)
                throw new ArgumentException();
            if (destOffset < 0 || destOffset >= data.Length)
                throw new ArgumentException();

            Array.Copy(source, 0, data, destOffset, destLength);
            return true;
        }

        public override void Sync()
        {
            ((HexView)Control).Invalidate();
        }
    }
}
