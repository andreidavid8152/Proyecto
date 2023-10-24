using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Proyecto.Configurations;
using Proyecto.Models;
using System.Net.Http;
using System.Text;


namespace Proyecto.Services
{
    public class UsuarioService : IUsuarioService
    {

        // URL base de la API con la que se interactúa.
        private readonly string _baseUrl;

        // Cliente HTTP utilizado para hacer las peticiones a la API.
        private readonly HttpClient _httpClient;

        // Constructor: inicializa el URL base y el cliente HTTP.
        public UsuarioService(IOptions<ApiSettings> apiSettings, HttpClient httpClient)
        {
            _baseUrl = apiSettings.Value.BaseUrl;
            _httpClient = httpClient;
        }

        public async Task<bool> registro(RegisterViewModel usuario)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Usuarios", usuario);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(errorMessage);
            }
        }

        public async Task<bool> login(LoginViewModel usuario)
        {

            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Usuarios/login", usuario);
            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                if (message == "Inicio de sesión exitoso")
                    return true;
                else
                    throw new Exception(message);
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(errorMessage);
            }

        }

    }
}
