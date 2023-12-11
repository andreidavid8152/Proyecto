using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Proyecto.Configurations;
using Proyecto.Models;
using System.Net.Http.Headers;
using System.Text;

namespace Proyecto.Services
{
    public class ReservaService : IReservaService
    {

        // URL base de la API con la que se interactúa.
        private readonly string _baseUrl;

        // Cliente HTTP utilizado para hacer las peticiones a la API.
        private readonly HttpClient _httpClient;

        // Constructor: inicializa el URL base y el cliente HTTP.
        public ReservaService(IOptions<ApiSettings> apiSettings, HttpClient httpClient)
        {
            _baseUrl = apiSettings.Value.BaseUrl;
            _httpClient = httpClient;
        }

        public async Task<bool> Reservar(Reserva reserva, string token)
        {
            // Añade el token como header de autorización
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Reservas", reserva);

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

        public async Task<List<Reserva>> ObtenerReservasCliente(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{_baseUrl}Reservas/Cliente");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var reserva = JsonConvert.DeserializeObject<List<Reserva>>(content);
                return reserva;
            }

            throw new Exception("No se pudo obtener las reservas desde la API.");

        }

        public async Task<bool> ComentarReserva(Comentario comentario, string token)
        {
            // Añade el token como header de autorización
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Realiza la petición HTTP POST
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Reservas/comentar", comentario);

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

        public async Task<bool> EliminarReserva(int id, string token)
        {
            // Añade el token como header de autorización
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Realiza la petición HTTP DELETE
            var response = await _httpClient.DeleteAsync($"{_baseUrl}Reservas/{id}");

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
