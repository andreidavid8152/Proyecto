using Proyecto.Models;

namespace Proyecto.Services
{
    public interface ILocalService
    {
        Task<List<LocalViewModel>> ObtenerTodosLocales(string token);
        Task<LocalViewModel> ObtenerLocalCliente(int id, string token);
        Task<bool> CrearLocal(LocalViewModel local, string token);
        Task<List<LocalViewModel>> ObtenerLocalesArrendador(string token);
    }
}
