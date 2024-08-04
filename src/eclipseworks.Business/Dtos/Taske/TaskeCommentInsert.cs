using System.ComponentModel.DataAnnotations;
using static eclipseworks.Domain.Entities.TaskeComment;

namespace eclipseworks.Business.Dtos.Taske
{
    public class TaskeCommentInsert
    {
        public string Id { get; private set; } = "";

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
    }
}
