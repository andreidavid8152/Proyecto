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

        public async Task<bool> Reservar(ReservaViewModel reserva, string token)
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

        public async Task<List<ReservaViewModel>> ObtenerReservasCliente(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{_baseUrl}Reservas/Cliente");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var reserva = JsonConvert.DeserializeObject<List<ReservaViewModel>>(content);
                return reserva;
            }

            throw new Exception("No se pudo obtener las reservas desde la API.");

        }

    }
}
