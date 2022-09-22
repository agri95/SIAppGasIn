using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiappGasIn.Data;
using SiappGasIn.Models;

namespace SiappGasIn.Controllers
{
    [Authorize(Roles = "Super Admin, Admin")]
    public class MstCraddleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View("~/Views/Master/MstCraddle/List.cshtml");
        }
    }
}

namespace SiappGasIn.Controllers.Api
{

    [Authorize]
    public class MstCraddleController : Controller
    {
        private readonly GasDbContext _dbContext;

        public MstCraddleController(GasDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        [HttpPost]
        public IActionResult Retrieve()
        {
            var tempData = (from temp in _dbContext.MstCraddle
                            select temp);

            var data = tempData.ToList();

            return Ok
                    (
                        new { data = data }
                    );

        }

        [HttpPost]
        public IActionResult CreateData([FromBody] MstCraddle crd)
        {
            try
            {
                if (crd != null)
                {
                    if (crd.Flowrate != null && crd.Flowrate != "")
                    {
                        _dbContext.MstCraddle.Add(new MstCraddle()
                        {
                            Flowrate = crd.Flowrate,
                            UkuranGTM = crd.UkuranGTM,
                            HargaBeli = crd.HargaBeli,
                            HargaSewa = crd.HargaSewa,
                            Tahun = crd.Tahun,
                            FillingTime = crd.FillingTime,
                            WaitingTime = crd.WaitingTime,
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

            MstCraddle prm = await _dbContext.MstCraddle.Where(x => x.CraddleID.Equals(id)).FirstOrDefaultAsync<MstCraddle>();

            if (prm == null)
            {
                isStatus = false;
                prm = new MstCraddle();
            }

            return Ok
                    (
                        new { data = prm, status = isStatus }
                    );

        }


        [HttpPost]
        public IActionResult EditData([FromBody] MstCraddle param)
        {
            try
            {
                if (param != null)
                {
                    if (param.Flowrate != null && param.Flowrate != "")
                    {
                        var prs = _dbContext.MstCraddle.Find(param.CraddleID);
                        if (prs != null)
                        {
                            prs.Flowrate = param.Flowrate;
                            prs.UkuranGTM = param.UkuranGTM;
                            prs.HargaBeli = param.HargaBeli;
                            prs.HargaSewa = param.HargaSewa;
                            prs.Tahun = param.Tahun;
                            prs.FillingTime = param.FillingTime;
                            prs.WaitingTime = param.WaitingTime;
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
        public async Task<IActionResult> Delete(int CraddleID)
        {

            MstCraddle std = _dbContext.MstCraddle.Where(x => x.CraddleID == CraddleID).FirstOrDefault<MstCraddle>();
            _dbContext.MstCraddle.Remove(std);
            _dbContext.SaveChanges();


            return RedirectToAction("List", "MstCraddle");

        }
    }
}
