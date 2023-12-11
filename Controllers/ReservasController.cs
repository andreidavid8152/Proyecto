using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto.Models;
using Proyecto.Services;
using System.IdentityModel.Tokens.Jwt;

namespace Proyecto.Controllers
{
    public class ReservasController : Controller
    {

        private readonly IReservaService _reservaService;
        private readonly IUsuarioService _usuarioService;

        public ReservasController(IReservaService reservaService, IUsuarioService usuarioService)
        {
            _reservaService = reservaService;
            _usuarioService = usuarioService;
        }

        // Ruta que muestra las reservas del usuario.
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("UserToken"); // Obtiene el token de la sesión.
            // Intenta recuperar la información del usuario de la sesión.
            var usuarioViewModelString = HttpContext.Session.GetString("InformacionUsuario");
            UsuarioViewModel usuarioViewModel = null;

            if (!string.IsNullOrEmpty(usuarioViewModelString))
            {
                usuarioViewModel = JsonConvert.DeserializeObject<UsuarioViewModel>(usuarioViewModelString);
                usuarioViewModel = await _usuarioService.GetInformacionUsuario(usuarioViewModel.Id, token);

                //Actualizar viewmodel
                HttpContext.Session.SetString("InformacionUsuario", JsonConvert.SerializeObject(usuarioViewModel));
            }
            else
            {
                // Manejar el caso en que no hay información del usuario en la sesión - se redirige al dashboard, por ejemplo.
                return RedirectToAction("Dashboard");
            }

            try
            {
                // Aquí asumimos que 'usuarioViewModel' contiene una propiedad 'reservas' que queremos mostrar.
                var reservas = usuarioViewModel.reservas;

                return View(reservas); // Pasa los reservas a la vista.
            }
            catch (Exception ex)
            {
                // Manejo de errores
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        // Ruta que crea una reserva.
        [HttpPost]
        public async Task<IActionResult> CrearReserva(Reserva reserva)
        {
            var token = HttpContext.Session.GetString("UserToken"); // Obtiene el token de la sesión.

            // Decodifica el token y obtiene el claim
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;
            var claimUserId = tokenS.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;

            reserva.UsuarioID = int.Parse(claimUserId);

            try
            {
                var result = await _reservaService.Reservar(reserva, token);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Puedes manejar el error como prefieras.
                ViewBag.ErrorMessage = ex.Message;
                return View("Index");
            }
        }

        // Ruta que comenta una reserva.
        [HttpPost("Comentario")]
        public async Task<IActionResult> ComentarReserva(Comentario comentario)
        {
            // Obtiene el token de la sesión.
            var token = HttpContext.Session.GetString("UserToken");

            // Decodifica el token y obtiene el claim
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;
            var claimUserId = tokenS.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;

            // Agrega el UsuarioID al comentario antes de enviarlo al servicio
            comentario.UsuarioID = int.Parse(claimUserId);

            // Envía el comentario al servicio y guarda el resultado
            try
            {
                Console.WriteLine(comentario);

                var result = await _reservaService.ComentarReserva(comentario, token);

                if (result)
                {
                    return RedirectToAction("Index"); // Redirige al index si todo sale bien
                }
                else
                {
                    // En caso de que el servicio devuelva false, puedes decidir qué hacer aquí.
                    ViewBag.ErrorMessage = "No se pudo agregar el comentario.";
                    return View("Index");
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                ViewBag.ErrorMessage = ex.Message;
                return View("Index");
            }
        }

        // Ruta para eliminar una reserva.
        [HttpGet("EliminarReserva/{id}")]
        public async Task<IActionResult> EliminarReserva(int id)
        {
            try
            {
                // Obtiene el token de la sesión.
                var token = HttpContext.Session.GetString("UserToken");

                // Verifica si el token existe
                if (string.IsNullOrEmpty(token))
                {
                    // Manejar el caso en que no hay token - se podría redirigir al login, por ejemplo.
                    return RedirectToAction("Login");
                }

                // Llama al servicio para eliminar la reserva.
                var resultado = await _reservaService.EliminarReserva(id, token);

                if (resultado)
                {
                    // Si se eliminó la reserva con éxito, redirige a la página que muestra las reservas.
                    return RedirectToAction("Index");
                }
                else
                {
                    // Manejar el caso en que la reserva no se pudo eliminar.
                    ViewBag.ErrorMessage = "No se pudo eliminar la reserva.";
                    return View("Index");
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                ViewBag.ErrorMessage = ex.Message;
                return View("Index");
            }
        }


    }
}
