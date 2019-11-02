namespace Crash
{
    public struct EntityVictim
    {
        public EntityVictim(short victimid)
        {
            VictimID = victimid;
        }

        public short VictimID { get; }
    }
}
