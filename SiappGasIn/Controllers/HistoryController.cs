using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiappGasIn.Data;
using SiappGasIn.Models;

namespace SiappGasIn.Controllers
{
    [Authorize(Roles = "Super Admin, Admin, User")]
    public class HistoryController : Controller
    {
        private readonly GasDbContext _dbContext;

        public HistoryController(GasDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View("~/Views/Home/History.cshtml");
        }

        [HttpGet]
        public IActionResult Result(int Id)
        {

            string StoredProc = "exec SP_HeaderSimulation " + Id;

            //var data = new SP_HeaderSimulation();
            SP_HeaderSimulation model = null;
            //SP_HeaderSimulation data = _dbContext.Set<SP_HeaderSimulation>().FromSqlRaw("[dbo].[SP_HeaderSimulation] @Id", Id).AsEnumerable().FirstOrDefault();
            SP_HeaderSimulation data = _dbContext.Set<SP_HeaderSimulation>().FromSqlRaw(StoredProc).AsEnumerable().FirstOrDefault();
            return View("~/Views/SimulationCost/Result.cshtml", data);
        }

        [HttpGet]
        public IActionResult Detail(int Id)
        {

            SimulationCost data = _dbContext.SimulationCost.Where(x => x.SimulationID.Equals(Id)).FirstOrDefault<SimulationCost>();

            if (data == null)
            {
                data = new SimulationCost();
            }
            return View("~/Views/SimulationCost/Detail.cshtml", data);
        }
    }
}

namespace SiappGasIn.Controllers.Api
{

    [Authorize]
    public class HistoryController : Controller
    {
        private readonly GasDbContext _dbContext;

        public HistoryController(GasDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        [HttpPost]
        public IActionResult Retrieves(string id)
        {
            string StoredProc = "exec SP_DetailSimulation " + id;

            var data = _dbContext.Set<SP_DetailSimulation>().FromSqlRaw(StoredProc).AsEnumerable().ToList();

            return Ok
                    (
                        new { data = data }
                    );

        }

        [HttpPost]
        public IActionResult Retrieve()
        {
            var customerData = (from tempcustomer in _dbContext.HeaderSimulationCost
                                select tempcustomer);
            var history = customerData.Where(x => x.Creator == this.User.Identity.Name);
            var data = history.ToList();

            return Ok
                    (
                        new { data = data }
                    );

        }

        [HttpPost]
        //public async Task<IActionResult> SaveData(SimulationCost model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            SimulationCost parameter = model;
        //            if (model.SimulationID > 0)
        //            {
        //                parameter.ModifiedBy = this.User.Identity.Name;
        //                parameter.ModifiedDate = DateTimeOffset.Now;
        //                _dbContext.SimulationCost.Update(parameter);
        //                _dbContext.Entry<SimulationCost>(parameter).Property(x => x.CreatedBy).IsModified = false;
        //                _dbContext.Entry<SimulationCost>(parameter).Property(x => x.CreatedDate).IsModified = false;
        //            }
        //            else
        //            {
        //                parameter.CreatedBy = this.User.Identity.Name;
        //                parameter.CreatedDate = DateTimeOffset.Now;
        //                await _dbContext.SimulationCost.AddAsync(parameter);
        //            }

        //            _dbContext.SaveChanges();
        //            ModelState.Clear();

        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError("error", ex.Message);
        //            return View("~/Modules/Master/MstParameter/Form.cshtml", model);
        //        }
        //    }
        //    else
        //    {
        //        return View("~/Modules/Master/MstParameter/Form.cshtml", model);
        //    }

        //    return RedirectToAction("Index", "SimulationCost");

        //}

        [HttpPost]
        public IActionResult SaveDataHeader([FromBody] HeaderSimulationCost energy)
        {
            var isStatus = true;
            int id = 0;
            try
            {
                HeaderSimulationCost parameter = energy;
                if (energy != null)
                {


                    parameter.CreatedBy = this.User.Identity.Name;
                    parameter.CreatedDate = DateTimeOffset.Now;
                    _dbContext.HeaderSimulationCost.AddAsync(parameter);


                    _dbContext.SaveChanges();

                    id = parameter.HeaderSimulationID;
                }
            }
            catch (Exception ex)
            {
                return Json(data: false);
            }

            return Ok
                   (
                       new { data = id, status = isStatus }
                   );
        }

        [HttpPost]
        public IActionResult SaveData([FromBody] SP_CostSimulation energy)
        {
            try
            {
                if (energy != null)
                {
                    string StoredProc = "exec SP_CostSimulation " + energy.headerSimulationID + "," + energy.volume2 + "," + energy.jarak + "," + energy.operasiHari + "," + energy.operasiBulan + "," + "'" + energy.energyName + "'" + "," + "'" + energy.asalStation + "'" + "," + "'" + energy.lokasiCapel + "'" + "," + energy.minPrice + "," + energy.maxPrice;

                    var data = _dbContext.Set<SP_CostSimulation>().FromSqlRaw(StoredProc).AsEnumerable().FirstOrDefault();


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

            MstEnergy prm = await _dbContext.MstEnergy.Where(x => x.EnergyID.Equals(id)).FirstOrDefaultAsync<MstEnergy>();

            if (prm == null)
            {
                isStatus = false;
                prm = new MstEnergy();
            }

            return Ok
                    (
                        new { data = prm, status = isStatus }
                    );

        }


        [HttpPost]
        public IActionResult EditData([FromBody] MstEnergy param)
        {
            try
            {
                if (param != null)
                {
                    if (param.Energy != null && param.Energy != "")
                    {
                        if (param.EnergyID > 0)
                        {
                            var cust = _dbContext.MstEnergy.Find(param.EnergyID);
                            if (cust != null)
                            {
                                cust.Energy = param.Energy;
                                cust.Harga = param.Harga;
                                cust.NilaiKalori = param.NilaiKalori;
                                cust.Satuan = param.Satuan;
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
        public async Task<IActionResult> Delete(int EnergyID)
        {

            MstEnergy std = _dbContext.MstEnergy.Where(x => x.EnergyID == EnergyID).FirstOrDefault<MstEnergy>();
            _dbContext.MstEnergy.Remove(std);
            _dbContext.SaveChanges();


            return RedirectToAction("List", "MstEnergy");

        }

        [HttpPost]
        public IActionResult GetGaji(string NamaSPBG)
        {
            string StoredProc = "exec SP_GetGajiByLocationName '" + NamaSPBG + "'";

            var data = _dbContext.Set<SP_GetGajiByLocationName>().FromSqlRaw(StoredProc).AsEnumerable().FirstOrDefault();
            return Ok
                    (
                        new { data = data }
                    );
        }

        [HttpPost]
        public IActionResult GetPRS(int flowRate)
        {
            string StoredProc = "exec SP_GetHargaPRS" + flowRate;

            var data = _dbContext.Set<MstHargaPRS>().FromSqlRaw(StoredProc).AsEnumerable().FirstOrDefault();
            return Ok
                    (
                        new { data = data }
                    );
        }
    }
}