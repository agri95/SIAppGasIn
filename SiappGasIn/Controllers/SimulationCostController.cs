using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SiappGasIn.Data;
using SiappGasIn.Models;
using SiappGasIn.Services;
using System.Net.Http.Headers;
using System.Text;

namespace SiappGasIn.Controllers
{
    [Authorize(Roles = "Super Admin, Admin, User")]
    public class SimulationCostController : Controller
    {
        private readonly GasDbContext _dbContext;

        public SimulationCostController(GasDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View("~/Views/Master/MstEnergy/List.cshtml");
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

        //[HttpGet]
        //public IActionResult Detail(int Id)
        //{

        //    SimulationCost data = _dbContext.SimulationCost.Where(x => x.SimulationID.Equals(Id)).FirstOrDefault<SimulationCost>();

        //    if (data == null)
        //    {
        //        data = new SimulationCost();
        //    }
        //    return View("~/Views/SimulationCost/Detail.cshtml", data);
        //}

       
        [HttpGet]
        public IActionResult DetailCNG(int Id)
        {

            string StoredProc = "exec SP_ResultDetailSimulation " + Id;

            //var data = new SP_HeaderSimulation();
            SimulationCost model = null;
            //SP_HeaderSimulation data = _dbContext.Set<SP_HeaderSimulation>().FromSqlRaw("[dbo].[SP_HeaderSimulation] @Id", Id).AsEnumerable().FirstOrDefault();
            SimulationCost data = _dbContext.Set<SimulationCost>().FromSqlRaw(StoredProc).AsEnumerable().FirstOrDefault();
           
            return View("~/Views/SimulationCost/Detail.cshtml", data);
        }
        
        [HttpGet]
        public IActionResult DetailPipe(int Id)
        {

            string StoredProc = "exec SP_ResultDetailSimulationPipeLine " + Id;

            //var data = new SP_HeaderSimulation();
            PipeCalculator model = null;
            //SP_HeaderSimulation data = _dbContext.Set<SP_HeaderSimulation>().FromSqlRaw("[dbo].[SP_HeaderSimulation] @Id", Id).AsEnumerable().FirstOrDefault();
            PipeCalculator data = _dbContext.Set<PipeCalculator>().FromSqlRaw(StoredProc).AsEnumerable().FirstOrDefault();
           
            return View("~/Views/SimulationCost/DetailPipeLine.cshtml", data);
        }
    }
}

namespace SiappGasIn.Controllers.Api
{

    [Authorize]
    public class SimulationCostController : Controller
    {
        private readonly GasDbContext _dbContext;
        private readonly APISettingOptions _apiSettingOptions;

        public SimulationCostController(GasDbContext dbContext, IOptions<APISettingOptions> apiOptions)
        {
            _dbContext = dbContext;
            _apiSettingOptions = apiOptions.Value;
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
            var customerData = (from tempcustomer in _dbContext.MstEnergy
                                select tempcustomer);

            var data = customerData.ToList();

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
                    string StoredProc = "exec SP_CostSimulation " + energy.headerSimulationID + "," + energy.volume2 + "," + energy.jarak + "," + energy.operasiHari + "," + energy.operasiBulan + "," + "'"+ energy.energyName +"'" + ","+ "'"+ energy.asalStation +"'" + ","+ "'" + energy.lokasiCapel + "'" + "," + energy.minPrice + "," + energy.maxPrice + "," + energy.latitude + "," + energy.longitude;

                    var data = _dbContext.Set<SP_CostSimulation>().FromSqlRaw(StoredProc).AsEnumerable().FirstOrDefault();


                }
            }
            catch (Exception ex)
            {
                return Json(data: false);
            }

            return Json(data: true);
        }

        [HttpPost]
        public IActionResult SaveDataPipe([FromBody] PipeCalculator pipe)
        {
            var isStatus = true;
            int id = 0;
            try
            {
                PipeCalculator parameter = pipe;
                if (pipe != null)
                {


                    parameter.CreatedBy = this.User.Identity.Name;
                    parameter.CreatedDate = DateTimeOffset.Now;
                    _dbContext.PipeCalculator.AddAsync(parameter);


                    _dbContext.SaveChanges();
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


        [HttpGet]
        public async Task<IActionResult> SelectData(int id)
        {
            var isStatus = true;

            HeaderSimulationCost prm = await _dbContext.HeaderSimulationCost.Where(x => x.HeaderSimulationID.Equals(id)).FirstOrDefaultAsync<HeaderSimulationCost>();

            if (prm == null)
            {
                isStatus = false;
                prm = new HeaderSimulationCost();
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
            string StoredProc = "exec SP_GetGajiByLocationName '"+NamaSPBG+"'" ;

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
        [HttpGet]
        public IActionResult getTokens(string user, string password)
        {
            var token = "";
            var param = new Dictionary<string, string>();
            var url = "http://10.129.10.191/nimo/api/PipePublicAPI/Login"; 
             param.Add("userName", user);
             param.Add("userPassword", password);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PostAsync(url, new FormUrlEncodedContent(param)).Result;
                token = response.Content.ReadAsStringAsync().Result;
            }
            return Ok
                    (new { data = token }
                    );
        }

        [HttpGet]
        public IActionResult getPipeCalculator(string datas, string token, int dataID)
        {
            var calculate = "";
            var tokens = token.Replace("\"", "");
            var stringContent = new StringContent(datas, UnicodeEncoding.UTF8, "application/json");
            var url = "http://10.129.10.191/nimo/api/PipeCalculatorRelyOn";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens);

                HttpResponseMessage response = client.PostAsync(url, stringContent).Result;
                calculate = response.Content.ReadAsStringAsync().Result;

                dynamic ceks = Newtonsoft.Json.JsonConvert.DeserializeObject(calculate);
                var childrenObjects = ceks.Children();

                JObject jObject = JObject.Parse(calculate);
                var jsonss = jObject["data"];
                if (jsonss.HasValues)
                {
                    PipeCalculator parameter = new PipeCalculator();
                    parameter.HeaderSimulationID = dataID;
                    parameter.type = jObject["data"]["type"].ToString();
                    var latitude = jObject["data"]["location"]["latitude"].ToString();
                    parameter.latitude = Convert.ToDouble(latitude);
                    var longitude = jObject["data"]["location"]["longitude"].ToString();
                    parameter.longitude = Convert.ToDouble(longitude);
                    parameter.postal_code = jObject["data"]["location"]["postal_code"].ToString();
                    var distanceValue = jObject["data"]["distance"]["value"].ToString();
                    parameter.distanceValue = Convert.ToDouble(distanceValue);
                    parameter.distanceUnit = jObject["data"]["distance"]["unit"].ToString();
                    var diameterValue = jObject["data"]["diameter"]["value"].ToString();
                    parameter.diameterValue = Convert.ToDouble(diameterValue);
                    parameter.diameterUnit = jObject["data"]["diameter"]["unit"].ToString();
                    var pressureValue = jObject["data"]["pressure"]["value"].ToString();
                    parameter.pressureValue = Convert.ToDouble(pressureValue);
                    parameter.pressureUnit = jObject["data"]["pressure"]["unit"].ToString();
                    var volumeValue = jObject["data"]["volume"][0]["value"].ToString();
                    parameter.volumeValue = Convert.ToDouble(volumeValue);
                    parameter.volumeUnit = jObject["data"]["volume"][0]["unit"].ToString();
                    var costValue = jObject["data"]["cost"]["value"].ToString();
                    parameter.costValue = Convert.ToDouble(costValue);
                    parameter.costUnit = jObject["data"]["cost"]["unit"].ToString();
                    parameter.route = jObject["data"]["route"].ToString();

                    _dbContext.PipeCalculator.AddAsync(parameter);

                    _dbContext.SaveChanges();
                }





                //foreach (var item in ceks)
                //{
                //   if (item.Value.id.Value == "1")
                //    {
                //        PipeCalculator parameter = new PipeCalculator();
                //        parameter.type = item.Value.type.Value;
                //        parameter.latitude = item.Value.location.latitude.Value;
                //        parameter.longitude = item.Value.location.longitude.Value;
                //        parameter.postal_code = item.Value.location.postal_code.Value;
                //        parameter.distanceValue = item.Value.distance.value.Value;
                //        parameter.distanceUnit = item.Value.distance.unit.Value;
                //        parameter.diameterValue = item.Value.diameter.value.Value;
                //        parameter.diameterUnit = item.Value.diameter.unit.Value;
                //        parameter.pressureValue = item.Value.pressure.value.Value;
                //        parameter.pressureUnit = item.Value.pressure.unit.Value;
                //        parameter.volumeValue = item.Value.volume.First.value.Value;
                //        parameter.volumeUnit = item.Value.volume.Last.unit.Value;
                //        parameter.costValue = item.Value.cost.value.Value;
                //        parameter.costUnit = item.Value.cost.unit.Value;
                //        parameter.route = item.Value.route.Value.ToString();
                //        parameter.CreatedBy = this.User.Identity.Name;
                //        parameter.CreatedDate = DateTimeOffset.Now;
                //        _dbContext.PipeCalculator.AddAsync(parameter);

                //        _dbContext.SaveChanges();
                //    }


                //}

            }
            return Ok
                    (new { data = calculate }
                    );

        }

    }
}
