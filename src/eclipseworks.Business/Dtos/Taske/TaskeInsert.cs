using System.ComponentModel.DataAnnotations;
using static eclipseworks.Domain.Entities.Taske;

namespace eclipseworks.Business.Dtos.Taske
{
    public class TaskeInsert : IValidatableObject
    {
        public string Id { get; private set; } = "";

        [MinLength(TaskeRule.TitleMinimalLenth, ErrorMessage = TaskeMsgDialog.InvalidTitle)]
        [Required(ErrorMessage = TaskeMsgDialog.RequiredTitle)]
        [MaxLength(TaskeRule.TitleMaxLenth, ErrorMessage = TaskeMsgDialog.InvalidTitle)]
        public string Title { get; set; }

        [MinLength(TaskeRule.DescriptionMinimalLenth, ErrorMessage = TaskeMsgDialog.InvalidDescription)]
        [Required(ErrorMessage = TaskeMsgDialog.RequiredDescription)]
        [MaxLength(TaskeRule.DescriptionMaxLenth, ErrorMessage = TaskeMsgDialog.InvalidDescription)]
        public string Description { get; set; }

        [Required(ErrorMessage = TaskeMsgDialog.RequiredExpires)]
        public DateTime Expires { get; set; }

        [Required(ErrorMessage = TaskeMsgDialog.RequiredStatus)]
        public eStatus Status { get; set; }

        [Required(ErrorMessage = TaskeMsgDialog.RequiredPriority)]
        public ePriority Priority { get; set; }

        [Required(ErrorMessage = TaskeMsgDialog.RequiredProjectId)]
        public string ProjectId { get; set; }

        public string UserOwner { get; private set; } = "";

        public TaskeInsert SetUserOwner(string userEvent)
        {
            UserOwner = userEvent;
            return this;
        }

        public void SetId(string id) => Id = id;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(ProjectId) || !long.TryParse(ProjectId, out var idOut) || idOut <= 0)
                yield return new ValidationResult(TaskeMsgDialog.InvalidProjectId, ["ProjectId"]);
        }
    }
}
