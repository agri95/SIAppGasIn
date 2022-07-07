using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using SiappGasIn.Data;
using SiappGasIn.Models;
//using SiappGasIn.Helpers;

namespace SiappGasIn.Controllers
{
    [Authorize]
    public class TransportCostController : Controller
    {
        private readonly GasDbContext _dbContext;

        public TransportCostController(GasDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("~/Views/Transport/Index.cshtml");

        }

        [HttpGet]
        public IActionResult Form(int id)
        {
            MstParameter model = null;
            if (id > 0)
                model = _dbContext.MstParameter.Where(x => x.ParamId.Equals(id)).FirstOrDefault<MstParameter>();

            return View("~/Modules/Master/MstParameter/Form.cshtml", model);

        }

    }
}

//namespace SiappGasIn.Controllers.Api
//{

//    [Authorize]
//    public class TransportCostController : Controller
//    {
//        private readonly GasDbContext _dbContext;

//        public TransportCostController(GasDbContext dbContext)
//        {
//            _dbContext = dbContext;

//        }


//        [HttpPost]
//        public IActionResult LoadData()
//        {

//            try
//            {
//                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
//                // Skiping number of Rows count
//                var start = Request.Form["start"].FirstOrDefault();
//                // Paging Length 10,20
//                var length = Request.Form["length"].FirstOrDefault();
//                // Sort Column Name
//                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
//                // Sort Column Direction ( asc ,desc)
//                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
//                // Search Value from (Search box)
//                var searchValue = Request.Form["search[value]"].FirstOrDefault();

//                //Paging Size (10,20,50,100)
//                int pageSize = length != null ? Convert.ToInt32(length) : 0;
//                int skip = start != null ? Convert.ToInt32(start) : 0;
//                int recordsTotal = 0;

//                // Getting all Customer data
//                var customerData = (from tempcustomer in _dbContext.MstParameter
//                                    select tempcustomer);

//                //Sorting
//                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
//                {
//                    customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
//                }
//                //Search
//                if (!string.IsNullOrEmpty(searchValue))
//                {
//                    customerData = customerData.Where(m => (m.ParamName != null && m.ParamName.Contains(searchValue)) || (m.ParamValue != null && m.ParamValue.Contains(searchValue)));
//                }

//                //total number of rows count 
//                recordsTotal = customerData.Count();
//                //Paging 
//                var data = customerData.Skip(skip).Take(pageSize).ToList();
//                //Returning Json Data
//                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

//            }
//            catch (Exception)
//            {
//                throw;
//            }

//        }

//        [HttpGet]
//        public IActionResult GetParam(int paramId)
//        {
//            MstParameter param = new MstParameter();

//            try
//            {
//                if (paramId > 0)
//                {
//                    var prm = _dbContext.MstParameter.Find(paramId);
//                    if (prm != null)
//                    {
//                        param.ParamName = prm.ParamName;
//                        param.ParamValue = prm.ParamValue;
//                        param.ParamGroup = prm.ParamGroup;
//                        param.ParamCode = prm.ParamCode;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                return Json(new { response = false, data = param });
//            }

//            return Json(new { response = true, data = param });
//        }

//        [HttpPost]
//        public IActionResult EditParameter([FromBody] MstParameter param)
//        {
//            try
//            {
//                if (param != null)
//                {
//                    if (param.ParamName != null && param.ParamName != "")
//                    {
//                        if (param.ParamId > 0)
//                        {
//                            var cust = _dbContext.MstParameter.Find(param.ParamId);
//                            if (cust != null)
//                            {
//                                cust.ParamName = param.ParamName;
//                                cust.ParamValue = param.ParamValue;
//                                cust.ParamGroup = param.ParamGroup;
//                                cust.ParamCode = param.ParamCode;

//                                _dbContext.SaveChanges();
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                return Json(data: false);
//            }

//            return Json(data: true);
//        }

//        [HttpPost]
//        public IActionResult CreateData([FromBody] MstParameter param)
//        {
//            try
//            {
//                if (param != null)
//                {
//                    if (param.ParamName != null && param.ParamName != "")
//                    {
//                        _dbContext.MstParameter.Add(new MstParameter()
//                        {
//                            ParamCode = param.ParamCode,
//                            ParamName = param.ParamName,
//                            ParamValue = param.ParamValue,
//                            ParamGroup = param.ParamGroup,
//                            CreatedBy = this.User.Identity.Name,
//                            CreatedDate = DateTimeOffset.Now
//                        });

//                        _dbContext.SaveChanges();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                return Json(data: false);
//            }

//            return Json(data: true);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Form(MstParameter model)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    MstParameter parameter = model;
//                    if (model.ParamId > 0)
//                    {
//                        parameter.ModifiedBy = this.User.Identity.Name;
//                        parameter.ModifiedDate = DateTimeOffset.Now;
//                        _dbContext.MstParameter.Update(parameter);
//                        _dbContext.Entry<MstParameter>(parameter).Property(x => x.CreatedBy).IsModified = false;
//                        _dbContext.Entry<MstParameter>(parameter).Property(x => x.CreatedDate).IsModified = false;
//                    }
//                    else
//                    {
//                        parameter.CreatedBy = this.User.Identity.Name;
//                        parameter.CreatedDate = DateTimeOffset.Now;
//                        await _dbContext.MstParameter.AddAsync(parameter);
//                    }

//                    _dbContext.SaveChanges();
//                    ModelState.Clear();

//                }
//                catch (Exception ex)
//                {
//                    ModelState.AddModelError("error", ex.Message);
//                    return View("~/Modules/Master/MstParameter/Form.cshtml", model);
//                }
//            }
//            else
//            {
//                return View("~/Modules/Master/MstParameter/Form.cshtml", model);
//            }

//            return RedirectToAction("List", "MstParameter", new { menuid = this.HttpContext.Request.Query["menuid"] });

//        }



//        [HttpPost]
//        public async Task<IActionResult> Retrieve(int id)
//        {
//            var isStatus = true;

//            MstParameter prm = await _dbContext.MstParameter.Where(x => x.ParamId.Equals(id)).FirstOrDefaultAsync<MstParameter>();

//            if (prm == null)
//            {
//                isStatus = false;
//                prm = new MstParameter();
//            }

//            return Ok
//                    (
//                        new { data = prm, status = isStatus }
//                    );

//        }


//        [HttpPost]
//        public async Task<IActionResult> Delete(MstParameter model)
//        {
//            if (model != null && model.ParamId > 0)
//            {
//                try
//                {
//                    model = _dbContext.MstParameter
//                        .Where(x => x.ParamId == model.ParamId)
//                        .FirstOrDefault();
//                    _dbContext.MstParameter.Remove(model);
//                    await _dbContext.SaveChangesAsync();
//                }
//                catch (Exception ex)
//                {
//                    ModelState.AddModelError("error", ex.Message);
//                }
//            }

//            return RedirectToAction("List", "MstParameter", new { menuid = this.HttpContext.Request.Query["menuid"] });

//        }

//        [HttpPost]
//        public async Task<IActionResult> RetrievesList(string text, string group)
//        {
//            List<MstParameter> paramList = null;

//            if (!string.IsNullOrEmpty(text))
//                paramList = await _dbContext.MstParameter.Where(x => x.ParamGroup.ToLower().Equals(group.ToLower()) && x.ParamName.ToLower().Contains(text)).OrderBy(x => x.ParamName).ToListAsync<MstParameter>();
//            else
//                paramList = await _dbContext.MstParameter.Where(x => x.ParamGroup.ToLower() == group.ToLower()).OrderBy(x => x.ParamName).ToListAsync<MstParameter>();


//            return Json(paramList);
//        }

//    }
//}
