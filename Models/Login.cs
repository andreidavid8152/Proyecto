using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class Login
    {

        [Required(ErrorMessage = "El username es obligatorio.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "El password es obligatorio.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
