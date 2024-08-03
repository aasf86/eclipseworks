using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entity = eclipseworks.Domain.Entities;

namespace eclipseworks.Infrastructure.EntitiesModels
{
    [Table("task")]
    public class TaskeModel : Entity.Taske
    {
        public TaskeModel() { }
        public TaskeModel(
            string userOwner,
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
            SetLastEventByUser(userOwner);            
        }

        [Key]
        public new long Id { get; set; }
    }
}
