using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiappGasIn.Data;
using SiappGasIn.Models;

namespace SiappGasIn.Controllers
{
    [Authorize(Roles = "Super Admin, Admin, User")]
    public class MstParameterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View("~/Views/Master/MstParameter/List.cshtml");
        }
    }
}

namespace SiappGasIn.Controllers.Api
{

    [Authorize]
    public class MstParameterController : Controller
    {
        private readonly GasDbContext _dbContext;

        public MstParameterController(GasDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        [HttpPost]
        public IActionResult Retrieve()
        {
            var customerData = (from tempcustomer in _dbContext.MstParameter
                                select tempcustomer);

            var data = customerData.ToList();

            return Ok
                    (
                        new { data = data }
                    );

        }

        [HttpPost]
        public IActionResult CreateData([FromBody] MstParameter param)
        {
            try
            {
                if (param != null)
                {
                    if (param.ParamName != null && param.ParamName != "")
                    {
                        _dbContext.MstParameter.Add(new MstParameter()
                        {
                            ParamName = param.ParamName,
                            ParamValue = param.ParamValue,
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

            MstParameter prm = await _dbContext.MstParameter.Where(x => x.ParamId.Equals(id)).FirstOrDefaultAsync<MstParameter>();

            if (prm == null)
            {
                isStatus = false;
                prm = new MstParameter();
            }

            return Ok
                    (
                        new { data = prm, status = isStatus }
                    );

        }


        [HttpPost]
        public IActionResult EditData([FromBody] MstParameter param)
        {
            try
            {
                if (param != null)
                {
                    if (param.ParamName != null && param.ParamName != "")
                    {
                        if (param.ParamId > 0)
                        {
                            var cust = _dbContext.MstParameter.Find(param.ParamId);
                            if (cust != null)
                            {
                                cust.ParamName = param.ParamName;
                                cust.ParamValue = param.ParamValue;
                                cust.CreatedBy = this.User.Identity.Name;
                                cust.CreatedDate = DateTimeOffset.Now;
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
        public async Task<IActionResult> Delete(int ParamId)
        {

            MstParameter std = _dbContext.MstParameter.Where(x => x.ParamId == ParamId).FirstOrDefault<MstParameter>();
            _dbContext.MstParameter.Remove(std);
            _dbContext.SaveChanges();


            return RedirectToAction("List", "MstParameter");

        }
    }
}