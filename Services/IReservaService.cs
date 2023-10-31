using Proyecto.Models;

namespace Proyecto.Services
{
    public interface IReservaService
    {

        Task<bool> Reservar(ReservaViewModel reserva, string token);

    }
}
