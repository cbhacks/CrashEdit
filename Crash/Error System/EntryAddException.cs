using System.Runtime.Serialization;

namespace CrashEdit.Crash
{
    [Serializable]
    public class EntryAddException : Exception
    {
        public EntryAddException(int eid) : base(string.Format("An entry with the name {0} already exists.", Entry.EIDToEName(eid)))
        {
            EID = eid;
        }

        public EntryAddException(int eid, string message) : base(message)
        {
            EID = eid;
        }

        public EntryAddException(int eid, string message, Exception inner) : base(message, inner)
        {
            EID = eid;
        }

        public int EID { get; }
    }
}
