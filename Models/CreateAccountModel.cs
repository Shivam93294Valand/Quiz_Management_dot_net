using System.ComponentModel.DataAnnotations;

namespace QuizeManagement_Project.Models
{
    public class CreateAccountModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string userName { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public string mobile { get; set; }

        [Required]
        public string email { get; set; }
        
    }
}
