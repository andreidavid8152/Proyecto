using Proyecto.Models;

namespace Proyecto.Services
{
    public interface ILocalService
    {
        Task<List<Local>> ObtenerTodosLocales(string token);
        Task<Local> ObtenerLocal(int id, string token);
        Task<Local> CrearLocal(Local local, string token);
        Task<bool> EditarLocal(int id, Local local, string token);
        Task<List<Local>> ObtenerLocalesArrendador(string token);
        Task<bool> AddHorarios(string token, int localId, List<Horario> horarios);
        Task<bool> AddImagenes(string token, int localId, List<ImagenLocal> imagenes);
        Task<bool> EditarImagenesLocal(int localId, List<ImagenLocal> imagenesNuevas, string token);
        Task<bool> EliminarLocal(int id, string token);
        Task<List<ImagenLocal>> ObtenerImagenesLocal(int id, string token);    
        Task<List<Horario>> ObtenerHorariosLocal(int id, string token);
        Task<bool> EditarHorariosLocal(int id, List<Horario> horario, string token);
    }
}
