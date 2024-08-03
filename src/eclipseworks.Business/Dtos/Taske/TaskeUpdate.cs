using System.ComponentModel.DataAnnotations;
using static eclipseworks.Domain.Entities.Taske;

namespace eclipseworks.Business.Dtos.Taske
{
    public class TaskeUpdate : IValidatableObject
    {
        [MinLength(TaskeRule.IdValueMinimalLenth, ErrorMessage = TaskeMsgDialog.InvalidId)]
        [Required(ErrorMessage = TaskeMsgDialog.InvalidId)]
        public string Id { get; set; }

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

        [Required(ErrorMessage = TaskeMsgDialog.RequiredProjectId)]
        public string ProjectId { get; set; }

        public string UserEvent { get; private set; } = "";

        public TaskeUpdate SetUserEvent(string userEvent)
        {
            UserEvent = userEvent;
            return this;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            long idOut = 0;

            if (string.IsNullOrWhiteSpace(Id) || !long.TryParse(Id, out idOut) || idOut <= 0)
                yield return new ValidationResult(TaskeMsgDialog.InvalidId, ["Id"]);

            if (string.IsNullOrWhiteSpace(ProjectId) || !long.TryParse(ProjectId, out idOut) || idOut <= 0)
                yield return new ValidationResult(TaskeMsgDialog.InvalidProjectId, ["ProjectId"]);
        }
    }
}
