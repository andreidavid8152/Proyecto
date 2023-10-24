using Proyecto.Models;

namespace Proyecto.Services
{
    public interface IUsuarioService
    {

        Task<bool> registro(RegisterViewModel usuario);
        Task<bool> login(LoginViewModel usuario);

    }
}
