using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
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
            var token = HttpContext.Session.GetString("UserToken"); // Obtiene el token de la sesión.

            try
            {
                var locales = await _localService.ObtenerLocalesArrendador(token);
                return View(locales); // Pasa los locales a la vista.
            }
            catch (Exception ex)
            {
                // Puedes manejar el error como prefieras.
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
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
                    var localCreado = await _localService.CrearLocal(local, token);
                    if (localCreado != null)
                    {
                        return RedirectToAction("CreateHorarios", new { id = localCreado.Id });
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

        
        [HttpGet("Horarios/{id}")]
        public async Task<IActionResult> CreateHorarios(int id)
        {
            return View();
        }

        [HttpPost("Horarios/{id}")]
        public async Task<IActionResult> CreateHorarios(int id, List<HorarioViewModel> horarios)
        {

            // Iterar a través de la lista de horarios
            for (int i = 0; i < horarios.Count; i++)
            {
                // Cambiar la propiedad LocalID aquí, por ejemplo:
                horarios[i].LocalID = id;
            }

            if (ModelState.IsValid)
            {
                var token = HttpContext.Session.GetString("UserToken"); // Obtiene el token de la sesión.

                try
                {
                    var result = await _localService.AddHorarios(token, id, horarios);
                    if (result)
                    {
                        // Procesar los datos y redirigir
                        return RedirectToAction("CreateImagenes", new { id = id });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "No se pudieron añadir los horarios.");
                        return View(horarios);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(horarios);
                }
            }
            else
            {
                // Retornar la misma vista con los errores
                return View(horarios);
            }
        }

        [HttpGet("Imagenes/{id}")]
        public async Task<IActionResult> CreateImagenes(int id)
        {
            return View();
        }

        [HttpPost("Imagenes/{id}")]
        public async Task<IActionResult> CreateImagenes(int id, List<ImagenLocalViewModel> imagenes)
        {
            var token = HttpContext.Session.GetString("UserToken"); // Obtiene el token de la sesión.
            try
            {
                var result = await _localService.AddImagenes(token, id, imagenes);
                if (result)
                {
                    // Procesar los datos y redirigir
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No se pudieron añadir las imagenes.");
                    return View(imagenes);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(imagenes);
            }
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
