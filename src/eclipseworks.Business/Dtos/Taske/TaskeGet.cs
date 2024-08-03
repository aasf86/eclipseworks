using eclipseworks.Business.Dtos.Project;
using static eclipseworks.Domain.Entities.Taske;

namespace eclipseworks.Business.Dtos.Taske
{
    public class TaskeGet
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime Expires { get; set; }
        public eStatus Status { get; set; }
        public ePriority Priority { get; set; }
        public ProjectGet Project { get; set; } = new ProjectGet();
    }
}
