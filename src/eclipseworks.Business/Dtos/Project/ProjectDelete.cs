using System.ComponentModel.DataAnnotations;
using static eclipseworks.Domain.Entities.Project;
using static eclipseworks.Domain.Entities.Rules.GeneralRules;

namespace eclipseworks.Business.Dtos.Project
{
    public class ProjectDelete : IValidatableObject
    {
        [MinLength(ProjectRule.IdValueMinimalLenth, ErrorMessage = ProjectMsgDialog.InvalidId)]
        [Required(ErrorMessage = ProjectMsgDialog.InvalidId)]
        public string Id { get; set; }

        [Required(ErrorMessage = MsgDialog.RequiredUserEvent)]
        public string UserEvent { get; private set; } = "";

        public ProjectDelete SetUserEvent(string userEvent)
        {
            UserEvent = userEvent;
            return this;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Id) || !long.TryParse(Id, out var idOut) || idOut <= 0)
                yield return new ValidationResult(ProjectMsgDialog.InvalidId, ["Id"]);
        }
    }
}
