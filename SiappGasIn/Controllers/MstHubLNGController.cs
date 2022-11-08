using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiappGasIn.Data;
using SiappGasIn.Models;

namespace SiappGasIn.Controllers
{
    [Authorize(Roles = "Super Admin, Admin")]
    public class MstHubLNGController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View("~/Views/Master/MstHubLNG/List.cshtml");
        }
    }
}

namespace SiappGasIn.Controllers.Api
{

    [Authorize]
    public class MstHubLNGController : Controller
    {
        private readonly GasDbContext _dbContext;

        public MstHubLNGController(GasDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        [HttpPost]
        public IActionResult Retrieve()
        {
            var tempData = (from temp in _dbContext.MstHubLNG
                            select temp);

            var data = tempData.ToList();

            return Ok
                    (
                        new { data = data }
                    );

        }

        [HttpPost]
        public IActionResult CreateData([FromBody] MstHubLNG lok)
        {
            try
            {
                if (lok != null)
                {
                    if (lok.NamaHub != null && lok.NamaHub != "")
                    {
                        _dbContext.MstHubLNG.Add(new MstHubLNG()
                        {
                            NamaHub = lok.NamaHub,
                            LokasiHub = lok.LokasiHub,
                            Longitude = lok.Longitude,
                            Latitude = lok.Latitude,
                            HargaUS = lok.HargaUS,
                            HargaIDR = lok.HargaIDR,
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

            MstHubLNG prm = await _dbContext.MstHubLNG.Where(x => x.HubID.Equals(id)).FirstOrDefaultAsync<MstHubLNG>();

            if (prm == null)
            {
                isStatus = false;
                prm = new MstHubLNG();
            }

            return Ok
                    (
                        new { data = prm, status = isStatus }
                    );

        }


        [HttpPost]
        public IActionResult EditData([FromBody] MstHubLNG param)
        {
            try
            {
                if (param != null)
                {
                    if (param.LokasiHub != null && param.LokasiHub != "")
                    {
                        if (param.HubID > 0)
                        {
                            var lok = _dbContext.MstHubLNG.Find(param.HubID);
                            if (lok != null)
                            {
                                lok.NamaHub = param.NamaHub;
                                lok.LokasiHub = param.LokasiHub;
                                lok.Longitude = param.Longitude;
                                lok.Latitude = param.Latitude;
                                lok.HargaUS = param.HargaUS;
                                lok.HargaIDR = param.HargaIDR;
                                lok.ModifiedBy = this.User.Identity.Name;
                                lok.ModifiedDate = DateTimeOffset.Now;
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
        public async Task<IActionResult> Delete(int HubID)
        {

            MstHubLNG std = _dbContext.MstHubLNG.Where(x => x.HubID == HubID).FirstOrDefault<MstHubLNG>();
            _dbContext.MstHubLNG.Remove(std);
            _dbContext.SaveChanges();


            return RedirectToAction("List", "MstHubLNG");

        }
    }
}


