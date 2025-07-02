using System.ComponentModel.DataAnnotations;

namespace SmartTrackKargo.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı boş olamaz.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre boş olamaz.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
