namespace Proyecto.Models
{
    public class ImagenLocalViewModel
    {

        public string Url { get; set; } // Ruta o URL de la imagen.
        public int LocalID { get; set; } // El ID del local al que pertenece la imagen.
        public LocalViewModel? Local { get; set; }


    }
}
