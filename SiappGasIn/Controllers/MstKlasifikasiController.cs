using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiappGasIn.Data;
using SiappGasIn.Models;

namespace SiappGasIn.Controllers
{
    [Authorize(Roles = "Super Admin, Admin")]
    public class MstKlasifikasiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View("~/Views/Master/MstKlasifikasi/List.cshtml");
        }
    }
}

namespace SiappGasIn.Controllers.Api
{

    [Authorize]
    public class MstKlasifikasiController : Controller
    {
        private readonly GasDbContext _dbContext;

        public MstKlasifikasiController(GasDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        [HttpPost]
        public IActionResult Retrieve()
        {
            var tempData = (from temp in _dbContext.MstKlasifikasi
                            select temp);

            var data = tempData.ToList();

            return Ok
                    (
                        new { data = data }
                    );

        }

        [HttpPost]
        public IActionResult CreateData([FromBody] MstKlasifikasi kl)
        {
            try
            {
                if (kl != null)
                {
                    if (kl.KlasifikasiName != null && kl.KlasifikasiName != "")
                    {
                        _dbContext.MstKlasifikasi.Add(new MstKlasifikasi()
                        {
                            KlasifikasiName = kl.KlasifikasiName,
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

            MstKlasifikasi prm = await _dbContext.MstKlasifikasi.Where(x => x.KlasifikasiID.Equals(id)).FirstOrDefaultAsync<MstKlasifikasi>();

            if (prm == null)
            {
                isStatus = false;
                prm = new MstKlasifikasi();
            }

            return Ok
                    (
                        new { data = prm, status = isStatus }
                    );

        }


        [HttpPost]
        public IActionResult EditData([FromBody] MstKlasifikasi param)
        {
            try
            {
                if (param != null)
                {
                    if (param.KlasifikasiName != null && param.KlasifikasiName != "")
                    {
                        if (param.KlasifikasiID > 0)
                        {
                            var kla = _dbContext.MstKlasifikasi.Find(param.KlasifikasiID);
                            if (kla != null)
                            {
                                kla.KlasifikasiName = param.KlasifikasiName;
                                kla.ModifiedBy = this.User.Identity.Name;
                                kla.ModifiedDate = DateTimeOffset.Now;
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
        public async Task<IActionResult> Delete(int KlasifikasiID)
        {

            MstKlasifikasi std = _dbContext.MstKlasifikasi.Where(x => x.KlasifikasiID == KlasifikasiID).FirstOrDefault<MstKlasifikasi>();
            _dbContext.MstKlasifikasi.Remove(std);
            _dbContext.SaveChanges();


            return RedirectToAction("List", "MstKlasifikasi");

        }
    }
}



