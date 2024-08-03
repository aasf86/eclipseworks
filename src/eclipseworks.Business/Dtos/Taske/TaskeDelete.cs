using System.ComponentModel.DataAnnotations;
using static eclipseworks.Domain.Entities.Rules.GeneralRules;
using static eclipseworks.Domain.Entities.Taske;

namespace eclipseworks.Business.Dtos.Taske
{
    public class TaskeDelete : IValidatableObject
    {
        [MinLength(TaskeRule.IdValueMinimalLenth, ErrorMessage = TaskeMsgDialog.InvalidId)]
        [Required(ErrorMessage = TaskeMsgDialog.InvalidId)]
        public string Id { get; set; }

        [Required(ErrorMessage = MsgDialog.RequiredUserEvent)]
        public string UserEvent { get; private set; } = "";

        public TaskeDelete SetUserEvent(string userEvent)
        {
            UserEvent = userEvent;
            return this;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Id) || !long.TryParse(Id, out var idOut) || idOut <= 0)
                yield return new ValidationResult(TaskeMsgDialog.InvalidId, ["Id"]);
        }
    }
}
