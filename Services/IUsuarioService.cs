using Proyecto.Models;

namespace Proyecto.Services
{
    public interface IUsuarioService
    {
        Task<bool> Registro(UserInput usuario);
        Task<String> Login(Login usuario);
        Task<UserInput> GetPerfil(string token);
        Task<bool> EditarPerfil(UserInput usuario, string token);
        Task<List<UserInput>> GetUsuarios(string token);
        Task<UsuarioViewModel> GetInformacionUsuario(int idUsuario, string token);
    }
}
