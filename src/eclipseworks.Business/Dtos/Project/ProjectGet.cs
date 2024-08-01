using System.ComponentModel.DataAnnotations;
using static eclipseworks.Domain.Entities.Project;

namespace eclipseworks.Business.Dtos.Project
{
    public class ProjectGet
    {
        [MinLength(ProjectRule.IdValueMinimalLenth, ErrorMessage = ProjectMsgDialog.InvalidId)]
        [Required(ErrorMessage = ProjectMsgDialog.InvalidId)]
        public string Id { get; set; }

        public string Name { get; set; } = "";

        public string UserOwner { get; set; } = "";
    }
}
