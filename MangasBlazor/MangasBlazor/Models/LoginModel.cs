namespace MangasBlazor.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LoginModel
    {
        [Required(ErrorMessage ="Insert the email")]
        public string? Email { get; set; }

        [Required(ErrorMessage ="Insert the password")]
        public string? Password { get; set; }
    }
}
