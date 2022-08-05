using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SiappGasIn.Controllers
{
    [Authorize(Roles = "Super Admin, Admin, User")]
    public class ConverterEnergyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
