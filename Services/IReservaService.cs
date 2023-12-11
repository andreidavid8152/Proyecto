using Proyecto.Models;

namespace Proyecto.Services
{
    public interface IReservaService
    {
        Task<bool> Reservar(Reserva reserva, string token);
        Task<List<Reserva>> ObtenerReservasCliente(string token);
        Task<bool> ComentarReserva(Comentario comentario, string token);
        Task<bool> EliminarReserva(int id, string token);
    }
}
