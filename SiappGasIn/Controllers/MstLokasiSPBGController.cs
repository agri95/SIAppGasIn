using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiappGasIn.Data;
using SiappGasIn.Models;

namespace SiappGasIn.Controllers
{
    [Authorize]
    public class MstLokasiSPBGController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View("~/Views/Master/MstLokasiSPBG/List.cshtml");
        }
    }
}

namespace SiappGasIn.Controllers.Api
{

    [Authorize]
    public class MstLokasiSPBGController : Controller
    {
        private readonly GasDbContext _dbContext;

        public MstLokasiSPBGController(GasDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        [HttpPost]
        public IActionResult Retrieve()
        {
            var tempData = (from temp in _dbContext.MstLokasiSPBG
                            select temp);

            var data = tempData.ToList();

            return Ok
                    (
                        new { data = data }
                    );

        }

        [HttpPost]
        public IActionResult CreateData([FromBody] MstLokasiSPBG lok)
        {
            try
            {
                if (lok != null)
                {
                    if (lok.NamaSPBG != null && lok.NamaSPBG != "")
                    {
                        _dbContext.MstLokasiSPBG.Add(new MstLokasiSPBG()
                        {
                            NamaSPBG = lok.NamaSPBG,
                            LokasiSPBG = lok.LokasiSPBG,
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

            MstLokasiSPBG prm = await _dbContext.MstLokasiSPBG.Where(x => x.LokasiID.Equals(id)).FirstOrDefaultAsync<MstLokasiSPBG>();

            if (prm == null)
            {
                isStatus = false;
                prm = new MstLokasiSPBG();
            }

            return Ok
                    (
                        new { data = prm, status = isStatus }
                    );

        }


        [HttpPost]
        public IActionResult EditData([FromBody] MstLokasiSPBG param)
        {
            try
            {
                if (param != null)
                {
                    if (param.LokasiSPBG != null && param.LokasiSPBG != "")
                    {
                        if (param.LokasiID > 0)
                        {
                            var lok = _dbContext.MstLokasiSPBG.Find(param.LokasiID);
                            if (lok != null)
                            {
                                lok.NamaSPBG = param.NamaSPBG;
                                lok.LokasiSPBG = param.LokasiSPBG;
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
        public async Task<IActionResult> Delete(int LokasiID)
        {

            MstLokasiSPBG std = _dbContext.MstLokasiSPBG.Where(x => x.LokasiID == LokasiID).FirstOrDefault<MstLokasiSPBG>();
            _dbContext.MstLokasiSPBG.Remove(std);
            _dbContext.SaveChanges();


            return RedirectToAction("List", "MstLokasiSPBG");

        }
    }
}

