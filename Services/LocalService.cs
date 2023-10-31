using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NuGet.Common;
using Proyecto.Configurations;
using Proyecto.Models;
using System.Net.Http.Headers;
using System.Text;


namespace Proyecto.Services
{
    public class LocalService : ILocalService
    {

        // URL base de la API con la que se interactúa.
        private readonly string _baseUrl;

        // Cliente HTTP utilizado para hacer las peticiones a la API.
        private readonly HttpClient _httpClient;

        // Constructor: inicializa el URL base y el cliente HTTP.
        public LocalService(IOptions<ApiSettings> apiSettings, HttpClient httpClient)
        {
            _baseUrl = apiSettings.Value.BaseUrl;
            _httpClient = httpClient;
        }

        public async Task<List<LocalViewModel>> ObtenerTodosLocales(string token)
        {
            // Añade el token como header de autorización
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{_baseUrl}Locales");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var locales = JsonConvert.DeserializeObject<List<LocalViewModel>>(content);
                return locales;
            }
                
            throw new Exception("No se pudo obtener los locales desde la API.");
        }

        public async Task<LocalViewModel> ObtenerLocalCliente(int id, string token)
        {
            // Añade el token como header de autorización
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{_baseUrl}Locales/Cliente/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var local = JsonConvert.DeserializeObject<LocalViewModel>(content);
                return local;
            }

            throw new Exception("No se pudo obtener los locales desde la API.");
        }

        public async Task<LocalViewModel> CrearLocal(LocalViewModel local, string token)
        {
            // Añade el token como header de autorización
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Locales", local);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var localCreado = JsonConvert.DeserializeObject<LocalViewModel>(content);
                return localCreado;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(errorMessage);
            }
        }

        public async Task<List<LocalViewModel>> ObtenerLocalesArrendador(string token)
        {
            // Añade el token como header de autorización
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{_baseUrl}Locales/Arrendador");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var localesArrendador = JsonConvert.DeserializeObject<List<LocalViewModel>>(content);
                return localesArrendador;
            }

            throw new Exception("No se pudo obtener los locales del arrendador desde la API.");
        }

        public async Task<bool> AddHorarios(string token, int localId, List<HorarioViewModel> horarios)
        {
            // Añade el token como header de autorización
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Realiza la petición HTTP PATCH
            var response = await _httpClient.PatchAsync($"{_baseUrl}Locales/AddHorarios/{localId}",
                new StringContent(JsonConvert.SerializeObject(horarios), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                return true; // Retornamos true para indicar éxito en la operación
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(errorMessage);
            }
        }

        public async Task<bool> AddImagenes(string token, int localId, List<ImagenLocalViewModel> imagenes)
        {
            // Añade el token como header de autorización
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Realiza la petición HTTP PATCH
            var response = await _httpClient.PatchAsync($"{_baseUrl}Locales/AddImagenes/{localId}",
                new StringContent(JsonConvert.SerializeObject(imagenes), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                return true; // Retornamos true para indicar éxito en la operación
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(errorMessage);
            }
        }
    }
}
