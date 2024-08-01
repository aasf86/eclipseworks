namespace eclipseworks.Domain.Entities
{
    public abstract class EntityBase
    {
        public EntityBase() { }
        public virtual long Id { get; private set; }
        public virtual Guid Guid { get; private set; } = Guid.NewGuid();
        public virtual DateTime Inserted { get; private set; } = DateTime.Now;
        public virtual DateTime Updated { get; private set; } = DateTime.Now;
        public virtual string LastEventByUser { get; private set; } = "";

        public virtual void SetLastEventByUser(string lastEventByUser)
        {
            LastEventByUser = lastEventByUser;
            ToUpdate();
        }

        public void ToUpdate()
        {
            Updated = DateTime.Now;
        }
    }
}
