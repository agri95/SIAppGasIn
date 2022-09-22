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
            var data = _dbContext.Set<MstFELs>().FromSqlRaw("[dbo].[SP_GetFEL]").ToList();

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
                            Material300A = fel.Material300A,
                            Material300B = fel.Material300B,
                            Material150A = fel.Material150A,
                            Material150B = fel.Material150B,
                            KontruksiA = fel.KontruksiA,
                            KontruksiB = fel.KontruksiB,
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

            MstFEL prm = await _dbContext.MstFEL.Where(x => x.FelID.Equals(id)).FirstOrDefaultAsync<MstFEL>();

            if (prm == null)
            {
                isStatus = false;
                prm = new MstFEL();
            }

            return Ok
                    (
                        new { data = prm, status = isStatus }
                    );

        }


        [HttpPost]
        public IActionResult EditData([FromBody] MstFEL param)
        {
            try
            {
                if (param != null)
                {
                    if (param.FelID > 0)
                    {
                        var gaj = _dbContext.MstFEL.Find(param.FelID);
                        if (gaj != null)
                        {
                            gaj.KlasifikasiID = param.KlasifikasiID;
                            gaj.ItemKlasifikasiID = param.ItemKlasifikasiID;
                            gaj.Diameter = param.Diameter;
                            gaj.UnitID = param.UnitID;
                            gaj.Material300A = param.Material300A;
                            gaj.Material300B = param.Material300B;
                            gaj.Material150A = param.Material150A;
                            gaj.Material150B = param.Material150B;
                            gaj.KontruksiA = param.KontruksiA;
                            gaj.KontruksiB = param.KontruksiB;
                            gaj.ModifiedBy = this.User.Identity.Name;
                            gaj.ModifiedDate = DateTimeOffset.Now;
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
        public async Task<IActionResult> Delete(int FelID)
        {

            MstFEL std = _dbContext.MstFEL.Where(x => x.FelID == FelID).FirstOrDefault<MstFEL>();
            _dbContext.MstFEL.Remove(std);
            _dbContext.SaveChanges();


            return RedirectToAction("List", "MstFel1");

        }
    }
}


