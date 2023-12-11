using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto.Models;
using Proyecto.Services;

namespace Proyecto.Controllers
{
    public class ComentariosController : Controller
    {

        private readonly IComentarioService _comentarioService;
        private readonly IUsuarioService _usuarioService;

        public ComentariosController(IComentarioService comentarioService, IUsuarioService usuarioService)
        {
            _comentarioService = comentarioService;
            _usuarioService = usuarioService;
        }

        [HttpGet]
        // GET: ComentarioController
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
                // Aquí asumimos que 'usuarioViewModel' contiene una propiedad 'comentarios' que queremos mostrar.
                var comentarios = usuarioViewModel.comentarios;

                return View(comentarios); // Pasa los comentarios a la vista.
            }
            catch (Exception ex)
            {
                // Manejo de errores
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        [HttpGet("EliminarComentario/{comentarioId}")]
        public async Task<IActionResult> EliminarComentario(int comentarioId)
        {
            var token = HttpContext.Session.GetString("UserToken"); // Obtiene el token de la sesión.

            try
            {
                var resultado = await _comentarioService.EliminarComentario(comentarioId, token);

                if (resultado)
                {
                    // Aquí puedes redirigir a la vista que prefieras o mostrar un mensaje de éxito.
                    return RedirectToAction("index");
                }
                else
                {
                    // Manejar el caso en que no se pudo eliminar el comentario
                    ViewBag.ErrorMessage = "No se pudo eliminar el comentario.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

    }
}
