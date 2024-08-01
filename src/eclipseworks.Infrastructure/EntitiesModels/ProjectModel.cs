using eclipseworks.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eclipseworks.Infrastructure.EntitiesModels
{
    [Table("project")]
    public class ProjectModel : Project
    {
        public ProjectModel() { }
        public ProjectModel(string name, string userOwner) : base(name, userOwner) 
        {
            SetLastEventByUser(userOwner);            
        }

        [Key]
        public new long Id { get; set; }
    }
}
