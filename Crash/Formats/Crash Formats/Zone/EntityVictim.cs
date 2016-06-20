namespace Crash
{
    public struct EntityVictim
    {
        private short victimid;

        public EntityVictim(short victimid)
        {
            this.victimid = victimid;
        }

        public short VictimID
        {
            get { return victimid; }
        }
    }
}
