using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiappGasIn.Data;
using SiappGasIn.Models;

namespace SiappGasIn.Controllers
{
    [Authorize(Roles = "Super Admin, Admin, User")]
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
                            Harga = energy.Harga,
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


        [HttpGet]
        public async Task<IActionResult> SelectData(int id)
        {
            var isStatus = true;

            MstEnergy prm = await _dbContext.MstEnergy.Where(x => x.EnergyID.Equals(id)).FirstOrDefaultAsync<MstEnergy>();

            if (prm == null)
            {
                isStatus = false;
                prm = new MstEnergy();
            }

            return Ok
                    (
                        new { data = prm, status = isStatus }
                    );

        }


        [HttpPost]
        public IActionResult EditData([FromBody] MstEnergy param)
        {
            try
            {
                if (param != null)
                {
                    if (param.Energy != null && param.Energy != "")
                    {
                        if (param.EnergyID > 0)
                        {
                            var cust = _dbContext.MstEnergy.Find(param.EnergyID);
                            if (cust != null)
                            {
                                cust.Energy = param.Energy;
                                cust.Harga = param.Harga;
                                cust.NilaiKalori = param.NilaiKalori;
                                cust.Satuan = param.Satuan;
                                cust.CreatedBy = this.User.Identity.Name;
                                cust.CreatedDate = DateTimeOffset.Now;
                                _dbContext.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(data: false);
            }

            return Json(data: true);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int EnergyID)
        {

            MstEnergy std = _dbContext.MstEnergy.Where(x => x.EnergyID == EnergyID).FirstOrDefault<MstEnergy>();
            _dbContext.MstEnergy.Remove(std);
            _dbContext.SaveChanges();
           

            return RedirectToAction("List", "MstEnergy");

        }
    }
}