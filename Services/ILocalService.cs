using Proyecto.Models;

namespace Proyecto.Services
{
    public interface ILocalService
    {
        Task<List<LocalViewModel>> ObtenerTodosLocales(string token);
        Task<LocalViewModel> ObtenerLocal(int id, string token);
        Task<LocalViewModel> CrearLocal(LocalViewModel local, string token);
        Task<bool> EditarLocal(int id, LocalViewModel local, string token);
        Task<List<LocalViewModel>> ObtenerLocalesArrendador(string token);
        Task<bool> AddHorarios(string token, int localId, List<HorarioViewModel> horarios);
        Task<bool> AddImagenes(string token, int localId, List<ImagenLocalViewModel> imagenes);
        Task<bool> EditarImagenesLocal(int localId, List<ImagenLocalViewModel> imagenesNuevas, string token);
        Task<bool> EliminarLocal(int id, string token);
    }
}
