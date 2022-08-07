using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiappGasIn.Data;
using SiappGasIn.Models;

namespace SiappGasIn.Controllers
{
    [Authorize(Roles = "Super Admin, Admin")]
    public class MstUnitController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View("~/Views/Master/MstUnit/List.cshtml");
        }
    }
}

namespace SiappGasIn.Controllers.Api
{

    [Authorize]
    public class MstUnitController : Controller
    {
        private readonly GasDbContext _dbContext;

        public MstUnitController(GasDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        [HttpPost]
        public IActionResult Retrieve()
        {
            var tempData = (from temp in _dbContext.MstUnit
                            select temp);

            var data = tempData.ToList();

            return Ok
                    (
                        new { data = data }
                    );

        }

        [HttpPost]
        public IActionResult CreateData([FromBody] MstUnit ut)
        {
            try
            {
                if (ut != null)
                {
                    if (ut.UnitName != null && ut.UnitName != "")
                    {
                        _dbContext.MstUnit.Add(new MstUnit()
                        {
                            UnitName = ut.UnitName,
                            UnitDesc = ut.UnitDesc,
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

            MstUnit prm = await _dbContext.MstUnit.Where(x => x.UnitID.Equals(id)).FirstOrDefaultAsync<MstUnit>();

            if (prm == null)
            {
                isStatus = false;
                prm = new MstUnit();
            }

            return Ok
                    (
                        new { data = prm, status = isStatus }
                    );

        }


        [HttpPost]
        public IActionResult EditData([FromBody] MstUnit param)
        {
            try
            {
                if (param != null)
                {
                    if (param.UnitName != null && param.UnitName != "")
                    {
                        if (param.UnitID > 0)
                        {
                            var gaj = _dbContext.MstUnit.Find(param.UnitID);
                            if (gaj != null)
                            {
                                gaj.UnitName = param.UnitName;
                                gaj.UnitDesc = param.UnitDesc;
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
        public async Task<IActionResult> Delete(int UnitID)
        {

            MstUnit std = _dbContext.MstUnit.Where(x => x.UnitID == UnitID).FirstOrDefault<MstUnit>();
            _dbContext.MstUnit.Remove(std);
            _dbContext.SaveChanges();


            return RedirectToAction("List", "MstUnit");

        }
    }
}



