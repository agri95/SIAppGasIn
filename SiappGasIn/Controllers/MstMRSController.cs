using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiappGasIn.Data;
using SiappGasIn.Models;

namespace SiappGasIn.Controllers
{
    [Authorize(Roles = "Super Admin, Admin")]
    public class MstMRSController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View("~/Views/Master/MstMRS/List.cshtml");
        }
    }
}

namespace SiappGasIn.Controllers.Api
{

    [Authorize]
    public class MstMRSController : Controller
    {
        private readonly GasDbContext _dbContext;

        public MstMRSController(GasDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        [HttpPost]
        public IActionResult Retrieve()
        {
            var customerData = (from tempcustomer in _dbContext.MstMRS
                                select tempcustomer);

            var data = customerData.ToList();

            return Ok
                    (
                        new { data = data }
                    );
        }

        [HttpPost]
        public IActionResult CreateData([FromBody] MstMRS fel)
        {
            try
            {
                if (fel != null)
                {
                    _dbContext.MstMRS.Add(new MstMRS()
                    {
                        ClassID = fel.ClassID,
                        TypePelanggan = fel.TypePelanggan,
                        MaxPress = fel.MaxPress,
                        MinPress = fel.MinPress,
                        InletMaxBarg = fel.InletMaxBarg,
                        InletMinBarg = fel.InletMinBarg,
                        OutletMaxBarg = fel.OutletMaxBarg,
                        OutletMinBarg = fel.OutletMinBarg,
                        InletInch = fel.InletInch,
                        OutletInch = fel.OutletInch,
                        InletNPS = fel.InletNPS,
                        OutletNPS = fel.OutletNPS,
                        CreatedBy = this.User.Identity.Name,
                        CreatedDate = DateTimeOffset.Now
                    });

                    _dbContext.SaveChanges();
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


