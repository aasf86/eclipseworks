using eclipseworks.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entity = eclipseworks.Domain.Entities;

namespace eclipseworks.Infrastructure.EntitiesModels
{
    [Table("taske")]
    public class TaskeModel : Entity.Taske
    {
        public TaskeModel() { }
        public TaskeModel(
            string userEvent,
            string title,
            string description,
            DateTime? expires,
            eStatus? status,
            ePriority? priority,
            long projectId) : base(
                title,
                description,
                expires,
                status,
                priority,
                projectId) 
        {
            SetLastEventByUser(userEvent);            
        }

        [Key]
        public new long Id { get; set; }

        [NotMapped]
        public new eStatus Status { get { return base.Status; } }

        [NotMapped]
        public new ePriority Priority { get { return base.Priority; } }

        [NotMapped]
        public new Project Project { get { return base.Project; } }

        public int StatusId { get { return (int)base.Status; } }

        public int PriorityId { get { return (int)base.Priority; } }


    }
}
