namespace Proyecto.Models
{
    public class ReservaViewModel
    {

        public int LocalID { get; set; }
        public int UsuarioID { get; set; }
        public int HorarioID { get; set; } // Relación con Horario
        public DateTime Fecha { get; set; }

        //Relacion
        public LocalViewModel? Local { get; set; }
        public HorarioViewModel? Horario { get; set; }

    }
}
