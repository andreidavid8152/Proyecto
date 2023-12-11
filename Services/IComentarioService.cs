using Proyecto.Models;

namespace Proyecto.Services
{
    public interface IComentarioService
    {
        Task<List<Comentario>> ObtenerComentariosPorUsuario(int usuarioId, string token);
        Task<bool> EliminarComentario(int comentarioId, string token);
    }
}
