using Microsoft.AspNetCore.Mvc;

namespace Proyecto.Controllers
{
    [Route("MisReservas")]
    public class ReservasController : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
