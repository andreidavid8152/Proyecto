using Proyecto.Models;

namespace Proyecto.Services
{
    public interface IUsuarioService
    {

        Task<bool> Registro(RegisterViewModel usuario);
        Task<String> Login(LoginViewModel usuario);
        Task<RegisterViewModel> GetPerfil(string token);

    }
}
