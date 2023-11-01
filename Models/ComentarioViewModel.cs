namespace Proyecto.Models
{
    public class ComentarioViewModel
    {
        public int LocalID { get; set; }
        public int UsuarioID { get; set; }
        public string Texto { get; set; }
        public DateTime Fecha { get; set; }
        public int Calificacion { get; set; }
    }
}
