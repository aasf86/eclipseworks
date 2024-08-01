using System.ComponentModel.DataAnnotations;
using static eclipseworks.Domain.Entities.Project;

namespace eclipseworks.Business.Dtos.Project
{
    public class ProjectUpdate
    {
        /*
        [Range(ProjectRule.YearMinimalValue, int.MaxValue, ErrorMessage = ProjectMsgDialog.InvalidYear)]
        public int Year { get; set; }

        [MinLength(ProjectRule.ModelMinimalLenth, ErrorMessage = ProjectMsgDialog.RequiredModel)]
        [Required(ErrorMessage = ProjectMsgDialog.RequiredModel)]
        [MaxLength(100, ErrorMessage = ProjectMsgDialog.InvalidModel )]
        public string Model { get; set; } = "";

        [MinLength(ProjectRule.PlateMinimalLenth, ErrorMessage = ProjectMsgDialog.InvalidPlate)]
        [Required(ErrorMessage = ProjectMsgDialog.RequiredPlate)]
        [MaxLength(ProjectRule.PlateMaxLenth, ErrorMessage = ProjectMsgDialog.InvalidPlate)]
        public string Plate { get; set; } = "";
        */
    }
}
