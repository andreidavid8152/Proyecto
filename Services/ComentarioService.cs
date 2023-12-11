using Newtonsoft.Json;
using Proyecto.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.Extensions.Options;
using Proyecto.Configurations;

namespace Proyecto.Services
{
    public class ComentarioService : IComentarioService
    {
        // URL base de la API con la que se interactúa.
        private readonly string _baseUrl;

        // Cliente HTTP utilizado para hacer las peticiones a la API.
        private readonly HttpClient _httpClient;

        // Constructor: inicializa el URL base y el cliente HTTP.
        public ComentarioService(IOptions<ApiSettings> apiSettings, HttpClient httpClient)
        {
            _baseUrl = apiSettings.Value.BaseUrl;
            _httpClient = httpClient;
        }

        public async Task<List<Comentario>> ObtenerComentariosPorUsuario(int usuarioId, string token)
        {
            // Añade el token como header de autorización
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Realiza la petición HTTP GET
            var response = await _httpClient.GetAsync($"{_baseUrl}Comentarios/{usuarioId}");

            // Verifica si la petición fue exitosa
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var comentarios = JsonConvert.DeserializeObject<List<Comentario>>(content);
                return comentarios;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(errorMessage);
            }
        }

        public async Task<bool> EliminarComentario(int comentarioId, string token)
        {
            // Añade el token como header de autorización
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Realiza la petición HTTP DELETE
            var response = await _httpClient.DeleteAsync($"{_baseUrl}Comentarios/{comentarioId}");

            // Verifica si la petición fue exitosa
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

    }
}
