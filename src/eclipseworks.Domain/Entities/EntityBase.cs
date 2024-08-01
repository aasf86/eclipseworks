namespace eclipseworks.Domain.Entities
{
    public abstract class EntityBase
    {
        public EntityBase() { }
        public virtual long Id { get; set; }
        public virtual Guid Guid { get; set; } = Guid.NewGuid();
        public virtual DateTime Inserted { get; set; } = DateTime.Now;
        public virtual DateTime Updated { get; set; } = DateTime.Now;
        public virtual string LastEventByUser { get; set; } = "";
    }
}
