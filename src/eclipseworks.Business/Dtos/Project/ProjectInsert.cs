using System.ComponentModel.DataAnnotations;
using static eclipseworks.Domain.Entities.Project;

namespace eclipseworks.Business.Dtos.Project
{
    public class ProjectInsert
    {
        public string Id { get; private set; } = "";

        [MinLength(ProjectRule.NameMinimalLenth, ErrorMessage = ProjectMsgDialog.InvalidName)]
        [Required(ErrorMessage = ProjectMsgDialog.RequiredName)]
        [MaxLength(ProjectRule.NameMaxLenth, ErrorMessage = ProjectMsgDialog.InvalidName)]
        public string Name { get; set; } = "";

        public string UserOwner { get; private set; } = "";

        public void SetUserOwner(string userOwner) => UserOwner = userOwner;
        public void SetId(string id) => Id = id;
    }
}
