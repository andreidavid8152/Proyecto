﻿using Microsoft.AspNetCore.Mvc;
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


    }
}
