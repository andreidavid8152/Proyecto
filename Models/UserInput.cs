using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class UserInput
    {

        public int Id { get; set; } 

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }


        [Required(ErrorMessage = "El email es obligatorio.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Ingrese una dirección de correo válida.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "El username es obligatorio.")]
        public string Username { get; set; }


        [Required(ErrorMessage = "El password es obligatorio.")]
        [MinLength(4, ErrorMessage = "La password debe tener al menos 4 caracteres.")]
        public string Password { get; set; }

    }
}
