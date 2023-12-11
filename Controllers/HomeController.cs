using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto.Models;
using Proyecto.Services;
using System.Diagnostics;

namespace Proyecto.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUsuarioService _usuarioService;

        public HomeController(ILogger<HomeController> logger, IUsuarioService localService)
        {
            _logger = logger;
            _usuarioService = localService;
        }

        // Ruta que muestra la pagina principal
        public IActionResult Index()
        {
            return RedirectToAction("Login", "Auth");
        }

        // Ruta que muestra el dashboard cuando un usuario ha iniciado sesión
        public async Task<IActionResult> Dashboard()
        {
            var token = HttpContext.Session.GetString("UserToken"); // Obtiene el token de la sesión.

            try
            {
                var usuarios = await _usuarioService.GetUsuarios(token);
                return View(usuarios); // Pasa los usuarios a la vista.
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> InformacionUsuario(int idUsuario)
        {
            var token = HttpContext.Session.GetString("UserToken"); // Obtiene el token de la sesión.

            UsuarioViewModel usuario;

            if (idUsuario == -1 || idUsuario == 0)
            {
                var usuarioViewModelString = HttpContext.Session.GetString("InformacionUsuario");
                if (string.IsNullOrEmpty(usuarioViewModelString))
                {
                    // Manejar el caso en que no hay información del usuario en la sesión - se redirige al dashboard
                    return RedirectToAction("Dashboard");
                }

                usuario = JsonConvert.DeserializeObject<UsuarioViewModel>(usuarioViewModelString);

                // Usa el ID del usuario obtenido del ViewModel
                idUsuario = usuario.Id;
            }
            else
            {
                try
                {
                    usuario = await _usuarioService.GetInformacionUsuario(idUsuario, token);
                    usuario.password = new string('*', usuario.password.Length);

                    // Guardando la información del usuario en una variable de sesión
                    HttpContext.Session.SetString("InformacionUsuario", JsonConvert.SerializeObject(usuario));
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                    return View();
                }
            }

            return View(usuario);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}