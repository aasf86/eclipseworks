using System.ComponentModel.DataAnnotations;
using static eclipseworks.Domain.Entities.TaskeComment;

namespace eclipseworks.Business.Dtos.Taske
{
    public class TaskeCommentUpdate : IValidatableObject
    {        
        [MinLength(TaskeCommentRule.IdValueMinimalLenth, ErrorMessage = TaskeCommentMsgDialog.InvalidId)]
        [Required(ErrorMessage = TaskeCommentMsgDialog.InvalidId)]
        public string Id { get; set; }

        [MinLength(TaskeCommentRule.CommentMinimalLenth, ErrorMessage = TaskeCommentMsgDialog.InvalidComment)]
        [Required(ErrorMessage = TaskeCommentMsgDialog.RequiredComment)]
        [MaxLength(TaskeCommentRule.CommentMaxLenth, ErrorMessage = TaskeCommentMsgDialog.InvalidComment)]
        public string Comment { get; set; } = "";

        public string UserEvent { get; private set; } = "";

        public TaskeCommentUpdate SetUserEvent(string userEvent)
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
