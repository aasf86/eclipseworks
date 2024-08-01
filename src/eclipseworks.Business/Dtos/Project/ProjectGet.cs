using System.ComponentModel.DataAnnotations;
using static eclipseworks.Domain.Entities.Project;

namespace eclipseworks.Business.Dtos.Project
{
    public class ProjectGet : IValidatableObject
    {
        [MinLength(ProjectRule.IdValueMinimalLenth, ErrorMessage = ProjectMsgDialog.InvalidId)]
        [Required(ErrorMessage = ProjectMsgDialog.InvalidId)]
        public string Id { get; set; }

        public string Name { get; set; } = "";

        public string UserOwner { get; set; } = "";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Id) || !long.TryParse(Id, out var idOut) || idOut <= 0)
                yield return new ValidationResult(ProjectMsgDialog.InvalidId, ["Id"]);
        }
    }
}
