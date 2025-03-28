using System;
using System.ComponentModel.DataAnnotations;

namespace QuizeManagement_Project.Models
{
    public class QuizDropDownModel
    {
        public int QuizID { get; set; }
        public string QuizName { get; set; }
    }

    public class QuestionDropDownModel
    {
        public int QuestionID { get; set; }
        public string QuestionText { get; set; }
    }

    public class QuestionLevelDropDownModel
    {
        public int QuestionLevelID { get; set; }
        public string QuestionLevel { get; set; }
        public string ColorClass { get; set; }
    }
}