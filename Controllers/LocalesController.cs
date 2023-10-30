using Microsoft.AspNetCore.Mvc;
using Proyecto.Models;
using Proyecto.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;

namespace Proyecto.Controllers
{

    [Route("MisLocales")]
    public class LocalesController : Controller
    {

        private readonly ILocalService _localService;

        public LocalesController(ILocalService localService)
        {
            _localService = localService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(LocalViewModel local)
        {
            if (ModelState.IsValid)
            {
                var token = HttpContext.Session.GetString("UserToken"); // Obtiene el token de la sesión.

                // Decodifica el token y obtiene el claim
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(token) as JwtSecurityToken;
                var claimUserId = tokenS.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;

                try
                {
                    local.PropietarioID = int.Parse(claimUserId);
                    var result = await _localService.CrearLocal(local, token);
                    if (result)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(local);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(local);
                }
            }
            return View(local);
        }


        [HttpGet("DetallesCliente")]
        public async Task<IActionResult> VerDetallesCliente(int id)
        {
            var token = HttpContext.Session.GetString("UserToken"); // Obtiene el token de la sesión.

            try
            {
                var local = await _localService.ObtenerLocalCliente(id, token);
                return View(local); // Pasa los locales a la vista.
            }
            catch (Exception ex)
            {
                // Puedes manejar el error como prefieras.
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }
    }
}
