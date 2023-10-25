using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Models;
using Proyecto.Services;

namespace Proyecto.Controllers
{
    public class AuthController : Controller
    {

        // Dependencia para comunicarse con la API.
        private readonly IUsuarioService _apiService;

        // Constructor que inyecta el servicio de la API.
        public AuthController(IUsuarioService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var token = await _apiService.Login(model);
                    if (!string.IsNullOrEmpty(token))
                    {
                        // Almacena el token en la sesión o en una cookie
                        HttpContext.Session.SetString("UserToken", token);

                        return RedirectToAction("MiPerfil");
                    }
                    else
                    {
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(model);
                }
            }

            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _apiService.Registro(model);
                    if (result)
                    {
                        return RedirectToAction("Login", "Auth"); // Redirige al inicio de sesión después de un registro exitoso.
                    }
                    else
                    {
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(model);
                }
            }
            return View(model); // Si el modelo no es válido, simplemente retorna a la vista con los datos del formulario.
        }

        public async Task<IActionResult> MiPerfil()
        {
            var token = HttpContext.Session.GetString("UserToken");

            try
            {
                var usuario = await _apiService.GetPerfil(token);
                return View(usuario);
            }
            catch (Exception ex)
            {
                // Puedes manejar el error como prefieras
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            return RedirectToAction("Index", "Home");
        }

    }
}
