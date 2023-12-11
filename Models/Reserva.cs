namespace Proyecto.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        public int LocalID { get; set; }

        public int UsuarioID { get; set; }

        public int HorarioID { get; set; }

        public DateTime Fecha { get; set; }


        //Relaciones
        public Local? Local { get; set; }
        public Horario? Horario { get; set; }

    }
}
