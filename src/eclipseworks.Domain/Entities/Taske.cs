using System.Threading.Tasks;

namespace eclipseworks.Domain.Entities
{
    public partial class Taske : EntityBase
    {
        public Taske() { }

        public Taske(
            string title, 
            string description, 
            DateTime? expires, 
            eStatus? status,
            ePriority? priority,
            long projectId) 
        {
            this
            .SetTitle(title)
            .SetDescription(description)
            .SetExpires(expires)
            .SetStatus(status)
            .SetPriority(priority);            

            ProjectId = projectId;            
        }

        public long ProjectId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }        
        public DateTime Expires { get; private set; }        
        public eStatus Status { get; private set; }
        public ePriority Priority { get; private set; }
        public Project Project { get; private set; }

        public Taske SetTitle(string title) 
        {
            if (string.IsNullOrWhiteSpace(title)) throw new InvalidDataException(TaskeMsgDialog.RequiredTitle);
            Title = title;
            ToUpdate();
            return this;
        }
        public Taske SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description)) throw new InvalidDataException(TaskeMsgDialog.RequiredDescription);
            Description = description;
            ToUpdate();
            return this;
        }
        public Taske SetExpires(DateTime? expires)
        {
            if (expires is null) throw new InvalidDataException(TaskeMsgDialog.RequiredExpires);
            Expires = expires.Value;
            ToUpdate();
            return this;
        }
        public Taske SetStatus(eStatus? status)
        {
            if (status is null || status == eStatus.Nulo) throw new InvalidDataException(TaskeMsgDialog.RequiredStatus);
            Status = status.Value;
            ToUpdate();
            return this;
        }

        public Taske SetPriority(ePriority? priority)
        {
            if(Priority != ePriority.Nulo) throw new InvalidDataException(TaskeMsgDialog.NotAllowedChangePriority);
            if (priority is null || priority == ePriority.Nulo) throw new InvalidDataException(TaskeMsgDialog.RequiredPriority);
            Priority = priority.Value;
            ToUpdate();
            return this;
        }
    }
}
