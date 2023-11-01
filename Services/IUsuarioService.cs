using Proyecto.Models;

namespace Proyecto.Services
{
    public interface IUsuarioService
    {

        Task<bool> Registro(UserInputModel usuario);
        Task<String> Login(LoginViewModel usuario);
        Task<UserInputModel> GetPerfil(string token);
        Task<bool> EditarPerfil(UserInputModel usuario, string token);
    }
}
