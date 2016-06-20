namespace Crash
{
    public struct EntityID
    {
        private int id;
        private int? alternateid;

        public EntityID(int id)
        {
            this.id = id;
            alternateid = null;
        }

        public EntityID(int id,int? alternateid)
        {
            this.id = id;
            this.alternateid = alternateid;
        }

        public int ID
        {
            get { return id; }
        }

        public int? AlternateID
        {
            get { return alternateid; }
        }
    }
}
