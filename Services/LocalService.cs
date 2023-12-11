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

        public async Task<List<Local>> ObtenerTodosLocales(string token)
        {
            // Añade el token como header de autorización
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{_baseUrl}Locales");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var locales = JsonConvert.DeserializeObject<List<Local>>(content);
                return locales;
            }
                
            throw new Exception("No se pudo obtener los locales desde la API.");
        }

        public async Task<Local> ObtenerLocal(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{_baseUrl}Locales/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var local = JsonConvert.DeserializeObject<Local>(content);
                return local;
            }

            throw new Exception("No se pudo obtener los locales desde la API.");
        }

        public async Task<Local> CrearLocal(Local local, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Locales", local);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var localCreado = JsonConvert.DeserializeObject<Local>(content);
                return localCreado;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(errorMessage);
            }
        }

        public async Task<List<Local>> ObtenerLocalesArrendador(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{_baseUrl}Locales/Arrendador");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var localesArrendador = JsonConvert.DeserializeObject<List<Local>>(content);
                return localesArrendador;
            }

            throw new Exception("No se pudo obtener los locales del arrendador desde la API.");
        }

        public async Task<bool> AddHorarios(string token, int localId, List<Horario> horarios)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsync($"{_baseUrl}Locales/AddHorarios/{localId}",
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

        public async Task<bool> AddImagenes(string token, int localId, List<ImagenLocal> imagenes)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsync($"{_baseUrl}Locales/AddImagenes/{localId}",
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

        public async Task<bool> EditarLocal(int id, Local local, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Realiza la petición HTTP PUT
            var response = await _httpClient.PutAsync($"{_baseUrl}Locales/{id}",
                new StringContent(JsonConvert.SerializeObject(local), Encoding.UTF8, "application/json"));

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

        public async Task<bool> EditarImagenesLocal(int localId, List<ImagenLocal> imagenesNuevas, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Realiza la petición HTTP PUT
            var response = await _httpClient.PutAsync($"{_baseUrl}Locales/Imagenes/Edit/{localId}",
                new StringContent(JsonConvert.SerializeObject(imagenesNuevas), Encoding.UTF8, "application/json"));

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

        public async Task<bool> EliminarLocal(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"{_baseUrl}Locales/{id}");

            if (response.IsSuccessStatusCode)
            {
                return true; // Retornamos true para indicar éxito en la operación
            }
            else
            {
                return false;
            }
        }

        public async Task<List<ImagenLocal>> ObtenerImagenesLocal(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{_baseUrl}Locales/imageneslocal/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var imagenesLocal = JsonConvert.DeserializeObject<List<ImagenLocal>>(content);
                return imagenesLocal;
            }

            throw new Exception("No se pudo obtener las imágenes del local desde la API.");
        }

        public async Task<List<Horario>> ObtenerHorariosLocal(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{_baseUrl}Locales/horarioslocal/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var horariosLocal = JsonConvert.DeserializeObject<List<Horario>>(content);
                return horariosLocal;
            }

            throw new Exception("No se pudo obtener los horarios del local desde la API.");
        }

        public async Task<bool> EditarHorariosLocal(int id, List<Horario> horario, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Realiza la petición HTTP PUT
            var response = await _httpClient.PutAsync($"{_baseUrl}Locales/HorariosLocal/Edit/{id}",
                new StringContent(JsonConvert.SerializeObject(horario), Encoding.UTF8, "application/json"));

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
