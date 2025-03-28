using System.ComponentModel.DataAnnotations;

namespace QuizeManagement_Project.Models
{
    public class LoginAccountModel
    {
        public LoginAccountModel()
        {
            Username = string.Empty;
            Password = string.Empty;
        }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
