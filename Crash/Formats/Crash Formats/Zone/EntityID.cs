namespace Crash
{
    public struct EntityID
    {
        public EntityID(int id)
        {
            ID = id;
            AlternateID = null;
        }

        public EntityID(int id,int? alternateid)
        {
            ID = id;
            AlternateID = alternateid;
        }

        public int ID { get; }
        public int? AlternateID { get; }
    }
}
