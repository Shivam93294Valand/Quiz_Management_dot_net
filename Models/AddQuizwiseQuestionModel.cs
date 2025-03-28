using System.ComponentModel.DataAnnotations;

namespace QuizeManagement_Project.Models
{
    public class AddQuizwiseQuestionModel
    {
        public int QuizWiseQuestionsID { get; set; }

        [Required(ErrorMessage = "Please select a quiz")]
        public int QuizID { get; set; }

        [Required(ErrorMessage = "Please select a question")]
        public int QuestionID { get; set; }
        public int? UserID { get; set; }
    }
}