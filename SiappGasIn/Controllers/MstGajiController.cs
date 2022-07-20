using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiappGasIn.Data;
using SiappGasIn.Models;

namespace SiappGasIn.Controllers
{
    public class MstGajiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View("~/Views/Master/MstGaji/List.cshtml");
        }
    }
}

namespace SiappGasIn.Controllers.Api
{

    [Authorize]
    public class MstGajiController : Controller
    {
        private readonly GasDbContext _dbContext;

        public MstGajiController(GasDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        [HttpPost]
        public IActionResult Retrieve()
        {
            var data = _dbContext.Set<MstListGaji>().FromSqlRaw("[dbo].[SP_GajiKaryawan]").ToList();
            
            return Ok
                    (
                        new { data = data }
                    );

        }

        [HttpPost]
        public IActionResult CreateData([FromBody] MstGaji gaj)
        {
            try
            {
                if (gaj != null)
                {
                    if (gaj.Gaji != null && gaj.Gaji != 0)
                    {
                        _dbContext.MstGaji.Add(new MstGaji()
                        {
                            LokasiID = gaj.LokasiID,
                            TypeID = gaj.TypeID,
                            Gaji = gaj.Gaji,
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

            MstGaji prm = await _dbContext.MstGaji.Where(x => x.GajiID.Equals(id)).FirstOrDefaultAsync<MstGaji>();

            if (prm == null)
            {
                isStatus = false;
                prm = new MstGaji();
            }

            return Ok
                    (
                        new { data = prm, status = isStatus }
                    );

        }


        [HttpPost]
        public IActionResult EditData([FromBody] MstGaji param)
        {
            try
            {
                if (param != null)
                {
                    if (param.Gaji != null && param.Gaji != 0)
                    {
                        if (param.GajiID > 0)
                        {
                            var gaj = _dbContext.MstGaji.Find(param.LokasiID);
                            if (gaj != null)
                            {
                                gaj.LokasiID = param.LokasiID;
                                gaj.TypeID = param.TypeID;
                                gaj.Gaji = param.Gaji;
                                gaj.ModifiedBy = this.User.Identity.Name;
                                gaj.ModifiedDate = DateTimeOffset.Now;
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
        public async Task<IActionResult> Delete(int GajiID)
        {

            MstGaji std = _dbContext.MstGaji.Where(x => x.GajiID == GajiID).FirstOrDefault<MstGaji>();
            _dbContext.MstGaji.Remove(std);
            _dbContext.SaveChanges();


            return RedirectToAction("List", "MstGaji");

        }
    }
}


