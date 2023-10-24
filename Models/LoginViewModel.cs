using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "El username es requerido.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "El password es requerido.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
