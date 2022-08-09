using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiappGasIn.Data;
using SiappGasIn.Models;

namespace SiappGasIn.Controllers
{
    [Authorize(Roles = "Super Admin, Admin")]
    public class MstFel1Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View("~/Views/Master/MstFEL/fel1.cshtml");
        }
    }
}

namespace SiappGasIn.Controllers.Api
{

    [Authorize]
    public class MstFel1Controller : Controller
    {
        private readonly GasDbContext _dbContext;

        public MstFel1Controller(GasDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        [HttpPost]
        public IActionResult Retrieve()
        {
            var data = _dbContext.Set<MstFEL>().FromSqlRaw("[dbo].[SP_GajiKaryawan]").ToList();

            return Ok
                    (
                        new { data = data }
                    );

        }

        [HttpPost]
        public IActionResult CreateData([FromBody] MstFEL fel)
        {
            try
            {
                if (fel != null)
                {
                    if (fel.Diameter != null && fel.Diameter != 0)
                    {
                        _dbContext.MstFEL.Add(new MstFEL()
                        {
                            KlasifikasiID = fel.KlasifikasiID,
                            ItemKlasifikasiID = fel.ItemKlasifikasiID,
                            Diameter = fel.Diameter,
                            UnitID = fel.UnitID,
                            Material300 = fel.Material300,
                            Material150 = fel.Material150,
                            Kontruksi = fel.Kontruksi,
                            Total300 = fel.Total300,
                            Total150 = fel.Total150,
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


