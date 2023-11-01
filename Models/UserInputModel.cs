using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class UserInputModel
    {

        [Required(ErrorMessage = "El campo nombre es obligatorio.")]
        public string Nombre { get; set; }


        [Required(ErrorMessage = "El campo email es obligatorio.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Ingrese una dirección de correo válida.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "El campo username es obligatorio.")]
        public string Username { get; set; }


        [Required(ErrorMessage = "El campo password es obligatorio.")]
        [MinLength(4, ErrorMessage = "La password debe tener al menos 4 caracteres.")]
        public string Password { get; set; }

    }
}
