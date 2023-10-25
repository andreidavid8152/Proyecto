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
                    var isSuccessful = await _apiService.login(model);
                    if (isSuccessful)
                    {
                        // Supongamos que redireccionas a otra vista o dashboard después de un inicio de sesión exitoso
                        return RedirectToAction("Dashboard", "Home");
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
                    var result = await _apiService.registro(model);
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

        public IActionResult MiPerfil()
        {
            // Aquí puedes recuperar la información del perfil del usuario logueado y enviarla a la vista
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            return RedirectToAction("Index", "Home");  // Redirige al usuario a la página principal después de cerrar sesión.
        }



    }
}
