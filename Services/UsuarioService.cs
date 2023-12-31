﻿using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Proyecto.Configurations;
using Proyecto.Models;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public async Task<bool> Registro(UserInput usuario)
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

        public async Task<String> Login(Login usuario)
        {

            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Usuarios/login", usuario);
            if (response.IsSuccessStatusCode)
            {
                // Deserializar el cuerpo de la respuesta para obtener el token
                var responseBody = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseBody);

                return tokenResponse.Token;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(errorMessage);
            }

        }

        public async Task<UserInput> GetPerfil(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"{_baseUrl}Usuarios/perfil");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserInput>(content);
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(errorMessage);
            }
        }

        public async Task<bool> EditarPerfil(UserInput usuario, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(usuario), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_baseUrl}Usuarios/editarperfil", jsonContent);
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

        public async Task<List<UserInput>> GetUsuarios(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"{_baseUrl}Usuarios/usuarios");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<UserInput>>(content);
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(errorMessage);
            }
        }

        public async Task<UsuarioViewModel> GetInformacionUsuario(int idUsuario, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"{_baseUrl}Usuarios/informacionUsuario/{idUsuario}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UsuarioViewModel>(content);
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(errorMessage);
            }
        }
    }
}
