using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiappGasIn.Data;
using SiappGasIn.Models;

namespace SiappGasIn.Controllers
{
    [Authorize(Roles = "Super Admin, Admin")]
    public class MstHeadTruckController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View("~/Views/Master/MstHeadTruck/List.cshtml");
        }
    }
}

namespace SiappGasIn.Controllers.Api
{

    [Authorize]
    public class MstHeadTruckController : Controller
    {
        private readonly GasDbContext _dbContext;

        public MstHeadTruckController(GasDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        [HttpPost]
        public IActionResult Retrieve()
        {
            var tempData = (from temp in _dbContext.MstHeadTruck
                            select temp);

            var data = tempData.ToList();

            return Ok
                    (
                        new { data = data }
                    );

        }

        [HttpPost]
        public IActionResult CreateData([FromBody] MstHeadTruck trk)
        {
            try
            {
                if (trk != null)
                {
                    if (trk.GTM != null && trk.GTM != "")
                    {
                        _dbContext.MstHeadTruck.Add(new MstHeadTruck()
                        {
                            GTM = trk.GTM,
                            Ritase = trk.Ritase,
                            HargaSewa = trk.HargaSewa,
                            RasioBBM = trk.RasioBBM,
                            Kecepatan = trk.Kecepatan,
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

            MstHeadTruck prm = await _dbContext.MstHeadTruck.Where(x => x.HeadTruckID.Equals(id)).FirstOrDefaultAsync<MstHeadTruck>();

            if (prm == null)
            {
                isStatus = false;
                prm = new MstHeadTruck();
            }

            return Ok
                    (
                        new { data = prm, status = isStatus }
                    );

        }


        [HttpPost]
        public IActionResult EditData([FromBody] MstHeadTruck param)
        {
            try
            {
                if (param != null)
                {
                    if (param.GTM != null && param.GTM != "")
                    {
                        var prs = _dbContext.MstHeadTruck.Find(param.HeadTruckID);
                        if (prs != null)
                        {
                            prs.GTM = param.GTM;
                            prs.Ritase = param.Ritase;
                            prs.HargaSewa = param.HargaSewa;
                            prs.RasioBBM = param.RasioBBM;
                            prs.Kecepatan = param.Kecepatan;
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
        public async Task<IActionResult> Delete(int HeadTruckID)
        {

            MstHeadTruck std = _dbContext.MstHeadTruck.Where(x => x.HeadTruckID == HeadTruckID).FirstOrDefault<MstHeadTruck>();
            _dbContext.MstHeadTruck.Remove(std);
            _dbContext.SaveChanges();


            return RedirectToAction("List", "MstHeadTruck");

        }
    }
}

