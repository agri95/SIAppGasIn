using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SiappGasIn.Controllers
{
    [Authorize]
    public class ConverterEnergyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
