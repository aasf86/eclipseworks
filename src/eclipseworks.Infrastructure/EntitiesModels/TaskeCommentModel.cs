using eclipseworks.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eclipseworks.Infrastructure.EntitiesModels
{
    [Table(TableName)]
    public class TaskeCommentModel : TaskeComment
    {
        public const string TableName = "taske_comment";

        public TaskeCommentModel() { }

        public TaskeCommentModel(string comment, string userOwner) : base(comment, userOwner)
        {
            SetLastEventByUser(userOwner);
        }

        [Key]
        public new long Id { get; set; }
    }
}
