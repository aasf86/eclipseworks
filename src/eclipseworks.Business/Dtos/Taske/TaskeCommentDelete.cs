using System.ComponentModel.DataAnnotations;
using static eclipseworks.Domain.Entities.Rules.GeneralRules;
using static eclipseworks.Domain.Entities.TaskeComment;

namespace eclipseworks.Business.Dtos.Taske
{
    public class TaskeCommentDelete : IValidatableObject
    {         
        [MinLength(TaskeCommentRule.IdValueMinimalLenth, ErrorMessage = TaskeCommentMsgDialog.InvalidId)]
        [Required(ErrorMessage = TaskeCommentMsgDialog.InvalidId)]
        public string Id { get; set; }

        [Required(ErrorMessage = MsgDialog.RequiredUserEvent)]
        public string UserEvent { get; private set; } = "";

        public TaskeCommentDelete SetUserEvent(string userEvent)
        {
            UserEvent = userEvent;
            return this;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Id) || !long.TryParse(Id, out var idOut) || idOut <= 0)
                yield return new ValidationResult(TaskeCommentMsgDialog.InvalidId, ["Id"]);
        }                 
    }
}
