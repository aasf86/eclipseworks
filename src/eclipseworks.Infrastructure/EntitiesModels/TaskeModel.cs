using eclipseworks.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eclipseworks.Infrastructure.EntitiesModels
{
    [Table(TableName)]
    public class TaskeModel : Taske
    {
        public const string TableName = "taske";
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
                projectId,
                userEvent) 
        {
            SetLastEventByUser(userEvent);            
        }

        [Key]
        public new long Id { get; set; }

        [NotMapped]
        public new eStatus Status { get { return base.Status; } set { SetStatus(value); } }

        [NotMapped]
        public new ePriority Priority { get { return base.Priority; } set { SetPriority(value); } }

        [NotMapped]
        public new Project Project { get { return base.Project; } }

        public int StatusId { get { return (int)Status; } }

        public int PriorityId { get { return (int)Priority; } }
    }
}
