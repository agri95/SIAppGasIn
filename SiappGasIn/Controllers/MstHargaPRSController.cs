using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiappGasIn.Data;
using SiappGasIn.Models;

namespace SiappGasIn.Controllers
{
    [Authorize(Roles = "Super Admin")]
    public class MstHargaPRSController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View("~/Views/Master/MstHargaPRS/List.cshtml");
        }
    }
}

namespace SiappGasIn.Controllers.Api
{

    [Authorize]
    public class MstHargaPRSController : Controller
    {
        private readonly GasDbContext _dbContext;

        public MstHargaPRSController(GasDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        [HttpPost]
        public IActionResult Retrieve()
        {
            var tempData = (from temp in _dbContext.MstHargaPRS
                                select temp);

            var data = tempData.ToList();

            return Ok
                    (
                        new { data = data }
                    );

        }

        [HttpPost]
        public IActionResult CreateData([FromBody] MstHargaPRS prs)
        {
            try
            {
                if (prs != null)
                {
                    if (prs.PRS != null && prs.PRS != "")
                    {
                        _dbContext.MstHargaPRS.Add(new MstHargaPRS()
                        {
                            Flowrate = prs.Flowrate,
                            PRS = prs.PRS,
                            Harga = prs.Harga,
                            HargaSewa = prs.HargaSewa,
                            BiayaMobDemod = prs.BiayaMobDemod,
                            GasMonitoring = prs.GasMonitoring,
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

            MstHargaPRS prm = await _dbContext.MstHargaPRS.Where(x => x.HargaPRSID.Equals(id)).FirstOrDefaultAsync<MstHargaPRS>();

            if (prm == null)
            {
                isStatus = false;
                prm = new MstHargaPRS();
            }

            return Ok
                    (
                        new { data = prm, status = isStatus }
                    );

        }


        [HttpPost]
        public IActionResult EditData([FromBody] MstHargaPRS param)
        {
            try
            {
                if (param != null)
                {
                    if (param.PRS != null && param.PRS != "")
                    {
                        var prs = _dbContext.MstHargaPRS.Find(param.HargaPRSID);
                        if (prs != null)
                        {
                            prs.Flowrate = param.Flowrate;
                            prs.PRS = param.PRS;
                            prs.Harga = param.Harga;
                            prs.HargaSewa = param.HargaSewa;
                            prs.BiayaMobDemod = param.BiayaMobDemod;
                            prs.GasMonitoring = param.GasMonitoring;
                            prs.ModifiedBy = this.User.Identity.Name;
                            prs.ModifiedDate = DateTimeOffset.Now;
                            _dbContext.SaveChanges();
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
        public async Task<IActionResult> Delete(int HargaPRSID)
        {

            MstHargaPRS std = _dbContext.MstHargaPRS.Where(x => x.HargaPRSID == HargaPRSID).FirstOrDefault<MstHargaPRS>();
            _dbContext.MstHargaPRS.Remove(std);
            _dbContext.SaveChanges();


            return RedirectToAction("List", "MstHargaPRS");

        }
    }
}