using Dapper.Contrib.Extensions;
using eclipseworks.Domain.Entities;

namespace eclipseworks.Infrastructure.EntitiesModels
{
    [Table("\"Project\"")]
    public class ProjectModel : Project
    {
        public ProjectModel() { }
        public ProjectModel(string name, string userOwner) : base(name, userOwner) 
        { 
            LastEventByUser = userOwner;
        }

        [Key]
        public new long Id { get; set; }
    }
}
