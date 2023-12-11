using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class UsuarioViewModel
    {

        public int Id { get; set; } 

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string nombre { get; set; }


        [Required(ErrorMessage = "El email es obligatorio.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Ingrese una dirección de correo válida.")]
        public string email { get; set; }


        [Required(ErrorMessage = "El username es obligatorio.")]
        public string username { get; set; }


        [Required(ErrorMessage = "El password es obligatorio.")]
        [MinLength(4, ErrorMessage = "La password debe tener al menos 4 caracteres.")]
        public string password { get; set; }

        public List<Local> locales { get; set; }

        public List<Reserva> reservas { get; set; }

        public List<Comentario> comentarios { get; set; }
    }
}
