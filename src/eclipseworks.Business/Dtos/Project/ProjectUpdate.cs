using System.ComponentModel.DataAnnotations;
using static eclipseworks.Domain.Entities.Project;

namespace eclipseworks.Business.Dtos.Project
{
    public class ProjectUpdate : IValidatableObject
    {
        [MinLength(ProjectRule.IdValueMinimalLenth, ErrorMessage = ProjectMsgDialog.InvalidId)]
        [Required(ErrorMessage = ProjectMsgDialog.InvalidId)]
        public string Id { get; set; }

        [MinLength(ProjectRule.NameMinimalLenth, ErrorMessage = ProjectMsgDialog.InvalidName)]
        [Required(ErrorMessage = ProjectMsgDialog.RequiredName)]
        [MaxLength(ProjectRule.NameMaxLenth, ErrorMessage = ProjectMsgDialog.InvalidName)]
        public string Name { get; set; } = "";

        public string User { get; private set; } = "";

        public void SetUser(string user) => User = user;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Id) || !long.TryParse(Id, out var idOut) || idOut <= 0)
                yield return new ValidationResult(ProjectMsgDialog.InvalidId, ["Id"]);
        }
    }
}
