using System.ComponentModel.DataAnnotations;
using static eclipseworks.Domain.Entities.TaskeComment;

namespace eclipseworks.Business.Dtos.Taske
{
    public class TaskeCommentInsert : IValidatableObject
    {
        public string Id { get; private set; } = "";

        [Required(ErrorMessage = TaskeCommentMsgDialog.RequiredTaskeId)]
        public string TaskeId { get; set; }

        [MinLength(TaskeCommentRule.CommentMinimalLenth, ErrorMessage = TaskeCommentMsgDialog.InvalidComment)]
        [Required(ErrorMessage = TaskeCommentMsgDialog.RequiredComment)]
        [MaxLength(TaskeCommentRule.CommentMaxLenth, ErrorMessage = TaskeCommentMsgDialog.InvalidComment)]
        public string Comment { get; set; } = "";

        public string UserOwner { get; private set; } = "";

        public TaskeCommentInsert SetUserOwner(string userOwner)
        {
            UserOwner = userOwner;
            return this;
        }
        public void SetId(string id) => Id = id;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(TaskeId) || !long.TryParse(TaskeId, out var idOut) || idOut <= 0)
                yield return new ValidationResult(TaskeCommentMsgDialog.RequiredTaskeId, ["TaskeId"]);
        }
    }
}
