using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiappGasIn.Data;
using SiappGasIn.Models;

namespace SiappGasIn.Controllers
{
    [Authorize(Roles = "Super Admin, Admin")]
    public class MstItemKlasifikasiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View("~/Views/Master/MstItemKlasifikasi/List.cshtml");
        }
    }
}

namespace SiappGasIn.Controllers.Api
{

    [Authorize]
    public class MstItemKlasifikasiController : Controller
    {
        private readonly GasDbContext _dbContext;

        public MstItemKlasifikasiController(GasDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        [HttpPost]
        public IActionResult Retrieve()
        {
            var tempData = (from temp in _dbContext.MstItemKlasifikasi
                            select temp);

            var data = tempData.ToList();

            return Ok
                    (
                        new { data = data }
                    );

        }

        [HttpPost]
        public IActionResult CreateData([FromBody] MstItemKlasifikasi kl)
        {
            try
            {
                if (kl != null)
                {
                    if (kl.ItemKlasifikasiName != null && kl.ItemKlasifikasiName != "")
                    {
                        _dbContext.MstItemKlasifikasi.Add(new MstItemKlasifikasi()
                        {
                            ItemKlasifikasiName = kl.ItemKlasifikasiName,
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

            MstItemKlasifikasi prm = await _dbContext.MstItemKlasifikasi.Where(x => x.ItemKlasifikasiID.Equals(id)).FirstOrDefaultAsync<MstItemKlasifikasi>();

            if (prm == null)
            {
                isStatus = false;
                prm = new MstItemKlasifikasi();
            }

            return Ok
                    (
                        new { data = prm, status = isStatus }
                    );

        }


        [HttpPost]
        public IActionResult EditData([FromBody] MstItemKlasifikasi param)
        {
            try
            {
                if (param != null)
                {
                    if (param.ItemKlasifikasiName != null && param.ItemKlasifikasiName != "")
                    {
                        if (param.ItemKlasifikasiID > 0)
                        {
                            var kla = _dbContext.MstItemKlasifikasi.Find(param.ItemKlasifikasiID);
                            if (kla != null)
                            {
                                kla.ItemKlasifikasiName = param.ItemKlasifikasiName;
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
        public async Task<IActionResult> Delete(int ItemKlasifikasiID)
        {

            MstItemKlasifikasi std = _dbContext.MstItemKlasifikasi.Where(x => x.ItemKlasifikasiID == ItemKlasifikasiID).FirstOrDefault<MstItemKlasifikasi>();
            _dbContext.MstItemKlasifikasi.Remove(std);
            _dbContext.SaveChanges();


            return RedirectToAction("List", "MstItemKlasifikasi");

        }
    }
}



