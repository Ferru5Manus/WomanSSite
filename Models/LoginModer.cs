using System.ComponentModel.DataAnnotations;

namespace WomanSite.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не указан Email")]
        public string name { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        public string key { get; set; }
    }
}
