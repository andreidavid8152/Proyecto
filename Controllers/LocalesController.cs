using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using Proyecto.Models;
using Proyecto.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;

namespace Proyecto.Controllers
{
    // Ruta sobreescrita
    [Route("MisLocales")]
    public class LocalesController : Controller
    {

        private readonly ILocalService _localService;

        public LocalesController(ILocalService localService)
        {
            _localService = localService;
        }

        // Ruta que muestra los locales creados del cliente
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

        // Ruta va a la vista para crear un local
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // Ruta que crea un local
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
        
        // Ruta que va a la vista para crear horarios
        [HttpGet("Horarios/{id}")]
        public async Task<IActionResult> CreateHorarios(int id)
        {
            return View();
        }

        // Ruta que crea horarios
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

        // Ruta que va a la vista para crear imagenes
        [HttpGet("Imagenes/{id}")]
        public async Task<IActionResult> CreateImagenes(int id)
        {
            return View();
        }

        // Ruta que crea imagenes
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

        // Ruta que muestra el detalles de un local publicado por otro
        [HttpGet("DetallesCliente")]
        public async Task<IActionResult> VerDetallesCliente(int id)
        {
            var token = HttpContext.Session.GetString("UserToken"); // Obtiene el token de la sesión.

            try
            {
                var local = await _localService.ObtenerLocal(id, token);
                return View(local); // Pasa los locales a la vista.
            }
            catch (Exception ex)
            {
                // Puedes manejar el error como prefieras.
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        // Ruta que muestra el detalle de un local creado por el cliente
        [HttpGet("DetallesArrendador")]
        public async Task<IActionResult> VerDetallesArrendador(int id)
        {
            var token = HttpContext.Session.GetString("UserToken"); // Obtiene el token de la sesión.

            try
            {
                var local = await _localService.ObtenerLocal(id, token);
                return View(local); // Pasa los locales a la vista.
            }
            catch (Exception ex)
            {
                // Puedes manejar el error como prefieras.
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        // Ruta que va a la vista para editar un local
        [HttpGet("Editar")]
        public async Task<IActionResult> EditarLocal(int id)
        {
            var token = HttpContext.Session.GetString("UserToken"); // Obtiene el token de la sesión.

            try
            {
                var local = await _localService.ObtenerLocal(id, token);
                return View(local); // Pasa los locales a la vista.
            }
            catch (Exception ex)
            {
                // Puedes manejar el error como prefieras.
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        // Ruta que edita un local
        [HttpPost("Editar")]
        public async Task<IActionResult> EditarLocal(int id, LocalViewModel local)
        {

            if (ModelState.IsValid)
            {
                var token = HttpContext.Session.GetString("UserToken"); // Obtiene el token de la sesión.

                try
                {
                    var resultado = await _localService.EditarLocal(id, local, token);

                    if (resultado)
                    {
                        return RedirectToAction("EditarImagenes", new { id = id }); // Si todo sale bien, redirige a otra vista, como el índice.
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "No se pudo editar el local.";
                        return View(local); // Devuelve la misma vista con un mensaje de error.
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                    return View(local); // Devuelve la misma vista con el mensaje de error.
                }
            }
            else
            {
                return View(local);
            }
        }

        // Ruta que va a la vista para editar imagenes
        [HttpGet("Editar/Imagenes")]
        public async Task<IActionResult> EditarImagenes(int id)
        {
            TempData["LocalId"] = id;  // En el método GET
            return View();
        }

        // Ruta que edita imagenes
        [HttpPost("Editar/Imagenes")]
        public async Task<IActionResult> EditarImagenes(List<ImagenLocalViewModel> imagenes)
        {
            var id = (int)TempData["LocalId"];  // En el método POST
            var token = HttpContext.Session.GetString("UserToken"); // Obtiene el token de la sesión.

            try
            {
                var local = await _localService.EditarImagenesLocal(id, imagenes, token);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Puedes manejar el error como prefieras.
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        // Ruta que elimina un local
        [HttpGet("Eliminar/{id}")]
        public async Task<IActionResult> EliminarLocal(int id)
        {
            var token = HttpContext.Session.GetString("UserToken"); // Obtiene el token de la sesión.

            try
            {
                var resultado = await _localService.EliminarLocal(id, token);

                if (resultado)
                {
                    TempData["MensajeError"] = "Local eliminado correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["MensajeError"] = "No se pudo eliminar el local porque tiene reservaciones";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
