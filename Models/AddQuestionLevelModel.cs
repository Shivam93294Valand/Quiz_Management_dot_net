using System.ComponentModel.DataAnnotations;

namespace QuizeManagement_Project.Models
{
    public class AddQuestionLevelModel
    {
        public int QuestionLevelID { get; set; }
        public int? UserID { get; set; }

        [Required(ErrorMessage = "Please Enter Level Name")]
        public string QuestionLevel { get; set; }
    }
}