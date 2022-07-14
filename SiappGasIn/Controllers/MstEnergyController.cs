using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiappGasIn.Data;
using SiappGasIn.Models;

namespace SiappGasIn.Controllers
{
    public class MstEnergyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View("~/Views/Master/MstEnergy/List.cshtml");
        }
    }
}

namespace SiappGasIn.Controllers.Api
{

    [Authorize]
    public class MstEnergyController : Controller
    {
        private readonly GasDbContext _dbContext;

        public MstEnergyController(GasDbContext dbContext)
        {
            _dbContext = dbContext;

        }       

        [HttpPost]
        public IActionResult Retrieve()
        {
            var customerData = (from tempcustomer in _dbContext.MstEnergy
                                select tempcustomer);

            var data = customerData.ToList();

            return Ok
                    (
                        new { data = data }
                    );

        }

        [HttpPost]
        public IActionResult CreateData([FromBody] MstEnergy energy)
        {
            try
            {
                if (energy != null)
                {
                    if (energy.Energy != null && energy.Energy != "")
                    {
                        _dbContext.MstEnergy.Add(new MstEnergy()
                        {
                            Energy = energy.Energy,
                            NilaiKalori = energy.NilaiKalori,
                            Satuan = energy.Satuan,
                            CreatedBy = this.User.Identity.Name,
                            CreatedDate = DateTimeOffset.Now
                        });

                        _dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(data: false);
            }

            return Json(data: true);
        }

    }
}