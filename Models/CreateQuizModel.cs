using System.ComponentModel.DataAnnotations;

namespace QuizeManagement_Project.Models
{
    public class CreateQuizModel
    {
        public int? QuizID { get; set; }

        [Required(ErrorMessage = "Please Enter Quiz Name")]
        public string QuizName { get; set; }

        [Required]
        public int? TotalQuestions { get; set; } // Make TotalQuestions nullable (int?)

        [Required]
        [Display(Name = "Quiz Date")]
        public DateTime? QuizDate { get; set; } // Make QuizDate nullable (DateTime?)

        //Temparary data
        public int UserID { get; set; }
    }
}