using Microsoft.AspNetCore.Mvc;
using Proyecto.Models;
using Proyecto.Services;
using System.IdentityModel.Tokens.Jwt;

namespace Proyecto.Controllers
{
    [Route("MisReservas")]
    public class ReservasController : Controller
    {

        private readonly IReservaService _reservaService;

        public ReservasController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        // Ruta que muestra las reservas del usuario.
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("UserToken"); // Obtiene el token de la sesión.

            try
            {
                var reservas = await _reservaService.ObtenerReservasCliente(token);
                return View(reservas); // Pasa los locales a la vista.
            }
            catch (Exception ex)
            {
                // Puedes manejar el error como prefieras.
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        // Ruta que crea una reserva.
        [HttpPost]
        public async Task<IActionResult> CrearReserva(ReservaViewModel reserva)
        {
            var token = HttpContext.Session.GetString("UserToken"); // Obtiene el token de la sesión.

            // Decodifica el token y obtiene el claim
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;
            var claimUserId = tokenS.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;

            reserva.UsuarioID = int.Parse(claimUserId);

            Console.WriteLine(reserva);

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
        public async Task<IActionResult> ComentarReserva(ComentarioViewModel comentario)
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

    }
}
