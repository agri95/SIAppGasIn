using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiappGasIn.Data;
using SiappGasIn.Models;

namespace SiappGasIn.Controllers
{
    [Authorize(Roles = "Super Admin, Admin, User")]
    public class MstParameterLNGController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View("~/Views/Master/MstParameterLNG/List.cshtml");
        }
        
        public IActionResult PriceList()
        {
            return View("~/Views/Master/MstParameterLNG/PriceList.cshtml");
        }
    }
}

namespace SiappGasIn.Controllers.Api
{

    [Authorize]
    public class MstParameterLNGController : Controller
    {
        private readonly GasDbContext _dbContext;

        public MstParameterLNGController(GasDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        [HttpPost]
        public IActionResult Retrieve()
        {
            var customerData = (from tempcustomer in _dbContext.MstParameterLNG
                                select tempcustomer);

            var data = customerData.ToList();

            return Ok
                    (
                        new { data = data }
                    );

        }

        [HttpPost]
        public IActionResult CreateData([FromBody] MstParameterLNG param)
        {
            try
            {
                if (param != null)
                {
                    if (param.ParamName != null && param.ParamName != "")
                    {
                        _dbContext.MstParameterLNG.Add(new MstParameterLNG()
                        {
                            ParamName = param.ParamName,
                            Size = param.Size,
                            Volume = param.Volume,
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

            MstParameterLNG prm = await _dbContext.MstParameterLNG.Where(x => x.ParamId.Equals(id)).FirstOrDefaultAsync<MstParameterLNG>();

            if (prm == null)
            {
                isStatus = false;
                prm = new MstParameterLNG();
            }

            return Ok
                    (
                        new { data = prm, status = isStatus }
                    );

        }


        [HttpPost]
        public IActionResult EditData([FromBody] MstParameterLNG param)
        {
            try
            {
                if (param != null)
                {
                    if (param.ParamName != null && param.ParamName != "")
                    {
                        if (param.ParamId > 0)
                        {
                            var cust = _dbContext.MstParameterLNG.Find(param.ParamId);
                            if (cust != null)
                            {
                                cust.ParamName = param.ParamName;
                                cust.Size = param.Size;
                                cust.Volume = param.Volume;
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

            MstParameterLNG std = _dbContext.MstParameterLNG.Where(x => x.ParamId == ParamId).FirstOrDefault<MstParameterLNG>();
            _dbContext.MstParameterLNG.Remove(std);
            _dbContext.SaveChanges();


            return RedirectToAction("List", "MstParameterLNG");

        }

        ///////////////// Price List ///////////////////

        [HttpPost]
        public IActionResult RetrievePrice()
        {
            var customerData = (from tempcustomer in _dbContext.MstPriceList
                                select tempcustomer);

            var data = customerData.ToList();

            return Ok
                    (
                        new { data = data }
                    );

        }

        [HttpPost]
        public IActionResult CreateDataPrice([FromBody] MstPriceList param)
        {
            try
            {
                if (param != null)
                {
                    if (param.Description != null && param.Description != "")
                    {
                        _dbContext.MstPriceList.Add(new MstPriceList()
                        {
                            Description = param.Description,
                            Reference = param.Reference,
                            Value = param.Value,
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
        public async Task<IActionResult> SelectDataPrice(int id)
        {
            var isStatus = true;

            MstPriceList prm = await _dbContext.MstPriceList.Where(x => x.PriceListId.Equals(id)).FirstOrDefaultAsync<MstPriceList>();

            if (prm == null)
            {
                isStatus = false;
                prm = new MstPriceList();
            }

            return Ok
                    (
                        new { data = prm, status = isStatus }
                    );

        }


        [HttpPost]
        public IActionResult EditDataPrice([FromBody] MstPriceList param)
        {
            try
            {
                if (param != null)
                {
                    if (param.Description != null && param.Description != "")
                    {
                        if (param.PriceListId > 0)
                        {
                            var cust = _dbContext.MstPriceList.Find(param.PriceListId);
                            if (cust != null)
                            {
                                cust.Description = param.Description;
                                cust.Reference = param.Reference;
                                cust.Value = param.Value;
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
        public async Task<IActionResult> DeletePrice(int PriceListId)
        {

            MstPriceList std = _dbContext.MstPriceList.Where(x => x.PriceListId == PriceListId).FirstOrDefault<MstPriceList>();
            _dbContext.MstPriceList.Remove(std);
            _dbContext.SaveChanges();


            return RedirectToAction("PriceList", "MstParameterLNG");

        }

    }
}