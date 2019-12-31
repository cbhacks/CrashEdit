namespace Crash
{
    public interface IEntry
    {
        int EID { get; }
        string EName { get; }
        int HashKey { get; }
    }
}
