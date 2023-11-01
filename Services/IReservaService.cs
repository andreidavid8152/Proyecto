using Proyecto.Models;

namespace Proyecto.Services
{
    public interface IReservaService
    {

        
        Task<bool> Reservar(ReservaViewModel reserva, string token);
        Task<List<ReservaViewModel>> ObtenerReservasCliente(string token);

    }
}
