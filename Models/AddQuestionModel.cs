using System.ComponentModel.DataAnnotations;

namespace QuizeManagement_Project.Models
{
    public class AddQuestionModel
    {
        public int QuestionID { get; set; }

        [Required(ErrorMessage = "Question text is required")]
        public string QuestionText { get; set; }

        [Required(ErrorMessage = "Option A is required")]
        public string OptionA { get; set; }

        [Required(ErrorMessage = "Option B is required")]
        public string OptionB { get; set; }

        [Required(ErrorMessage = "Option C is required")]
        public string OptionC { get; set; }

        [Required(ErrorMessage = "Option D is required")]
        public string OptionD { get; set; }

        [Required(ErrorMessage = "Correct option is required")]
        public string CorrectOption { get; set; }

        [Required(ErrorMessage = "Question Marks is required")]
        public int? QuestionMarks { get; set; }

        public bool IsActive { get; set; }

        public int? UserID { get; set; }

        public int QuestionLevelID { get; set; }
    }
}