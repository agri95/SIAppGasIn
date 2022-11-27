using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
        public IActionResult Cluster()
        {
            return View("~/Views/SimulationCost/Cluster.cshtml");
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
        public IActionResult ResultCluster(int Id)
        {
            //string StoredProc = "exec SP_ClusterSimulation " + Id;

            string StoredProc = "exec SP_ClusterSimulation " +
                     "@HeaderClusterID = " + Id + "";

            //var datass = _dbContext.Set<SP_ClusterSimulations>().FromSqlRaw(StoredProc).ToListAsync();

            HeaderCluster data = _dbContext.HeaderCluster.Where(x => x.HeaderClusterID.Equals(Id)).FirstOrDefault<HeaderCluster>();

            return View("~/Views/SimulationCost/ResultCluster.cshtml", data);
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
        public IActionResult DetailCluster(int Id)
        {
            string StoredProc = "exec SP_DetailClusterSingle " + Id;

            ClusterSimulation data = _dbContext.Set<ClusterSimulation>().FromSqlRaw(StoredProc).AsEnumerable().FirstOrDefault();

            return View("~/Views/SimulationCost/DetailCluster.cshtml", data);
        }

        [HttpGet]
        public IActionResult DetailPipe(int Id)
        {

            PipeCalculator data = _dbContext.PipeCalculator.Where(x => x.PipeCalculatorID.Equals(Id)).FirstOrDefault<PipeCalculator>();

            return View("~/Views/SimulationCost/DetailPipeLine.cshtml", data);
        }

        [HttpGet]
        public IActionResult DetailClusterPipe(int Id)
        {

            PipeCalculator data = _dbContext.PipeCalculator.Where(x => x.PipeCalculatorID.Equals(Id)).FirstOrDefault<PipeCalculator>();

            return View("~/Views/SimulationCost/DetailClusterPipeline.cshtml", data);
        }

        [HttpGet]
        public IActionResult DetailLNG(int Id)
        {
            HeaderRetail data = _dbContext.HeaderRetail.Where(x => x.HeaderRetailId.Equals(Id)).FirstOrDefault<HeaderRetail>();
                       
            return View("~/Views/SimulationCost/DetailLNG.cshtml", data);
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
        public IActionResult RetrievesCluster(int id)
        {
            string StoredProc = "exec SP_DetailClusterSimulation " + id;

            var data = _dbContext.Set<SP_DetailClusterSimulation>().FromSqlRaw(StoredProc).AsEnumerable().ToList();

            return Ok
                    (
                        new { data = data }
                    );

        }

        [HttpPost]
        public IActionResult RetrievesRouteCluster(int id)
        {
            string StoredProc = "exec SP_DetailRouteClusterSimulation " + id;

            var data = _dbContext.Set<SP_DetailRouteClusterSimulation>().FromSqlRaw(StoredProc).AsEnumerable().ToList();

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
                    string StoredProc = "exec SP_CostSimulation " + energy.headerSimulationID + "," + energy.volume2 + "," + energy.jarak + "," + energy.operasiHari + "," + energy.operasiBulan + "," + "'" + energy.energyName + "'" + "," + "'" + energy.asalStation + "'" + "," + "'" + energy.lokasiCapel + "'" + "," + energy.minPrice + "," + energy.maxPrice + "," + energy.latitude + "," + energy.longitude;

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
        public IActionResult SaveDataRetailSupply(int headerSimulationID, decimal demand, decimal jarakSumber, decimal jarakHub, decimal jumlahPelanggan, decimal size, decimal volume, decimal speed, decimal qtyTruck, decimal estRoundTrip, decimal m3Day, decimal isoTankDay, decimal qtyVGL, decimal isoTank, string asalSumber, string latitude, string longitude, string latitudePelanggan, string longitudePelanggan)
        {
            try
            {
                if (headerSimulationID != null)
                {
                    //string StoredProc = "exec SP_RetailSupply " + headerSimulationID;
                    string StoredProc = "exec SP_RetailSupply " + headerSimulationID + "," + demand + "," + jarakSumber + "," + jarakHub + "," + jumlahPelanggan + "," + size + "," + volume + "," + speed + "," + qtyTruck + "," + estRoundTrip + "," + m3Day + "," + isoTankDay + "," + qtyVGL + "," + isoTank + "," + "'" + asalSumber + "'" + "," + latitude + "," + longitude + "," + latitudePelanggan + "," + longitudePelanggan;

                    var data = _dbContext.Set<SP_RetailSupply>().FromSqlRaw(StoredProc).AsEnumerable().FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                return Json(data: false);
            }

            return Json(data: true);
        }

        [HttpPost]
        public ActionResult SaveDatas(HeaderSimulationCost _header, List<SP_CostSimulation> _cng)
        {
            return Json(false);
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
        public IActionResult GetCapexOpex(int Id, string Type, string Item)
        {

            var details = _dbContext.RetailSupplyChain.Where(x => x.HeaderSimulationID == Id && x.Type == Type && x.Item == Item).ToListAsync<RetailSupplyChain>();
         
            return Ok
                    (
                        new { data = details }
                    );
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
        [HttpGet]
        public IActionResult getToken(string user, string password)
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

            return Json(token);
        }
        public static string getTokens(string user, string password)
        {

            var param = new Dictionary<string, string>();
            var url = "http://10.129.10.191/nimo/api/PipePublicAPI/Login";
            param.Add("userName", user);
            param.Add("userPassword", password);
            string token;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PostAsync(url, new FormUrlEncodedContent(param)).Result;
                token = response.Content.ReadAsStringAsync().Result;
            }
            return token;
        }

        [HttpGet]
        public IActionResult getRouteMap(string user, string password)
        {
            string token = getTokens(user, password);
            var tokens = token.Replace("\"", "");
            var url = "http://10.129.10.191/nimo/api/pipecalculatormap?category=industri";
            var dataMap = "";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens);

                HttpResponseMessage response = client.GetAsync(url).Result;
                dataMap = response.Content.ReadAsStringAsync().Result;

                //dynamic ceks = Newtonsoft.Json.JsonConvert.DeserializeObject(calculate);
                //var childrenObjects = ceks.Children();

                //JObject jObject = JObject.Parse(calculate);
                //var jsonss = jObject["data"];

            }
            return Json(dataMap);

        }

        [HttpGet]
        public IActionResult getPipeCalculator(string datas, string user, string password, int dataID, decimal volume2, int operasiHari, int operasiBulan, string energyName, decimal pressure)
        {
            var calculate = "";
            string token = getTokens(user, password);
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

                    var type = jObject["data"]["type"].ToString();
                    var latitude = jObject["data"]["location"]["latitude"].ToString();
                    var longitude = jObject["data"]["location"]["longitude"].ToString();
                    var postal_code = jObject["data"]["location"]["postal_code"].ToString();
                    var distanceValues = jObject["data"]["distance"]["value"].ToString();
                    var distanceValue = Convert.ToDouble(distanceValues);
                    var diameterValues = jObject["data"]["diameter"]["value"].ToString();
                    var diameterValue = Convert.ToDouble(diameterValues);
                    var pressureValues = jObject["data"]["pressure"]["value"].ToString();
                    var pressureValue = Convert.ToDouble(pressureValues);
                    var volumeValues = jObject["data"]["volume"][0]["value"].ToString();
                    var volumeValue = Convert.ToDouble(volumeValues);
                    var route = jObject["data"]["route"].ToString();

                    //string StoredProc = "exec SP_PipeCalculator " +
                    //"@headerSimulationID = " + dataID + "," +
                    //"@type = '" + type + "'," +
                    //"@latitude= '" + latitude + "'," +
                    //"@longitude= '" + longitude + "'," +
                    //"@postal_code= " + postal_code + "," +
                    //"@distanceValue = " + distanceValue + "," +
                    //"@diameterValue = " + diameterValue + "," +
                    //"@pressureValue = " + pressureValue + "," +
                    //"@volumeValue = " + volumeValue + "," +                    
                    //"@route= '" + route + "'," +
                    //"@volume2 = " + volume2 + "," +
                    //"@operasiHari = " + operasiHari + "," +
                    //"@operasiBulan = " + operasiBulan + "," +
                    //"@energyName = '" + energyName + "'," +
                    //"@pressure= " + pressure + "";

                    //var datass =  _dbContext.Set<SP_PipeCalculator>().FromSqlRaw(StoredProc).ToListAsync();

                    //string StoredProcs = "exec SP_PipeCalculator " + dataID + "," + "'" + type + "'" + "," + "'" + latitude + "'" + "," + "'" + longitude + "'" + "," + postal_code + "," + diameterValue + "," + distanceValue + "," + pressureValue + "," + volumeValue + "," + "'" + route + "'" + "," + volume2 + "," + operasiHari + "," + operasiBulan + "," + "'" + energyName + "'" + "," + pressure;
                    //var data = _dbContext.Set<SP_PipeCalculatorData>().FromSqlRaw(StoredProcs).AsEnumerable().FirstOrDefault();
                    _dbContext.Set<SP_PipeCalculatorData>().FromSqlRaw("SP_PipeCalculator @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14", dataID, type, latitude, longitude, postal_code, diameterValue, distanceValue, pressureValue, volumeValue, route, volume2, operasiHari, operasiBulan, energyName, pressure).ToList();

                }
            }
            return Ok
                    (new { data = calculate }
                    );

        }
        [HttpGet]
        public IActionResult getPipeCalculatorCluster(string datas, string user, string password, int dataID, decimal volume2, int operasiHari, int operasiBulan, string energyName, decimal pressure, string customer)
        {
            var calculate = "";
            string token = getTokens(user, password);
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

                    var type = jObject["data"]["type"].ToString();
                    var latitude = jObject["data"]["location"]["latitude"].ToString();
                    var longitude = jObject["data"]["location"]["longitude"].ToString();
                    var postal_code = jObject["data"]["location"]["postal_code"].ToString();
                    var distanceValues = jObject["data"]["distance"]["value"].ToString();
                    var distanceValue = Convert.ToDouble(distanceValues);
                    var diameterValues = jObject["data"]["diameter"]["value"].ToString();
                    var diameterValue = Convert.ToDouble(diameterValues);
                    var pressureValues = jObject["data"]["pressure"]["value"].ToString();
                    var pressureValue = Convert.ToDouble(pressureValues);
                    var volumeValues = jObject["data"]["volume"][0]["value"].ToString();
                    var volumeValue = Convert.ToDouble(volumeValues);
                    var route = jObject["data"]["route"].ToString();

                    //string StoredProc = "exec SP_PipeCalculator " +
                    //"@headerSimulationID = " + dataID + "," +
                    //"@type = '" + type + "'," +
                    //"@latitude= '" + latitude + "'," +
                    //"@longitude= '" + longitude + "'," +
                    //"@postal_code= " + postal_code + "," +
                    //"@distanceValue = " + distanceValue + "," +
                    //"@diameterValue = " + diameterValue + "," +
                    //"@pressureValue = " + pressureValue + "," +
                    //"@volumeValue = " + volumeValue + "," +                    
                    //"@route= '" + route + "'," +
                    //"@volume2 = " + volume2 + "," +
                    //"@operasiHari = " + operasiHari + "," +
                    //"@operasiBulan = " + operasiBulan + "," +
                    //"@energyName = '" + energyName + "'," +
                    //"@pressure= " + pressure + "";

                    //var datass =  _dbContext.Set<SP_PipeCalculator>().FromSqlRaw(StoredProc).ToListAsync();

                    //string StoredProcs = "exec SP_PipeCalculator " + dataID + "," + "'" + type + "'" + "," + "'" + latitude + "'" + "," + "'" + longitude + "'" + "," + postal_code + "," + diameterValue + "," + distanceValue + "," + pressureValue + "," + volumeValue + "," + "'" + route + "'" + "," + volume2 + "," + operasiHari + "," + operasiBulan + "," + "'" + energyName + "'" + "," + pressure;
                    //var data = _dbContext.Set<SP_PipeCalculatorData>().FromSqlRaw(StoredProcs).AsEnumerable().FirstOrDefault();
                    _dbContext.Set<SP_PipeCalculatorData>().FromSqlRaw("SP_PipeCalculatorCluster @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15", dataID, type, latitude, longitude, postal_code, diameterValue, distanceValue, pressureValue, volumeValue, route, volume2, operasiHari, operasiBulan, energyName, pressure, customer).ToList();

                }
            }
            return Ok
                    (new { data = calculate }
                    );

        }
        [HttpGet]
        public IActionResult getPipeSektor(string datas, string user, string password, int dataID, decimal volume2, int operasiHari, int operasiBulan, string energyName, decimal pressure, string customer, int countCapel)
        {
            var calculate = "";
            string token = getTokens(user, password);
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

                    var type = jObject["data"]["type"].ToString();
                    var latitude = jObject["data"]["location"]["latitude"].ToString();
                    var longitude = jObject["data"]["location"]["longitude"].ToString();
                    var postal_code = jObject["data"]["location"]["postal_code"].ToString();
                    var distanceValues = jObject["data"]["distance"]["value"].ToString();
                    var distanceValue = Convert.ToDouble(distanceValues);
                    var diameterValues = jObject["data"]["diameter"]["value"].ToString();
                    var diameterValue = Convert.ToDouble(diameterValues);
                    var pressureValues = jObject["data"]["pressure"]["value"].ToString();
                    var pressureValue = Convert.ToDouble(pressureValues);
                    var volumeValues = jObject["data"]["volume"][0]["value"].ToString();
                    var volumeValue = Convert.ToDouble(volumeValues);
                    var route = jObject["data"]["route"].ToString();

                    //string StoredProc = "exec SP_PipeCalculator " +
                    //"@headerSimulationID = " + dataID + "," +
                    //"@type = '" + type + "'," +
                    //"@latitude= '" + latitude + "'," +
                    //"@longitude= '" + longitude + "'," +
                    //"@postal_code= " + postal_code + "," +
                    //"@distanceValue = " + distanceValue + "," +
                    //"@diameterValue = " + diameterValue + "," +
                    //"@pressureValue = " + pressureValue + "," +
                    //"@volumeValue = " + volumeValue + "," +                    
                    //"@route= '" + route + "'," +
                    //"@volume2 = " + volume2 + "," +
                    //"@operasiHari = " + operasiHari + "," +
                    //"@operasiBulan = " + operasiBulan + "," +
                    //"@energyName = '" + energyName + "'," +
                    //"@pressure= " + pressure + "";

                    //var datass =  _dbContext.Set<SP_PipeCalculator>().FromSqlRaw(StoredProc).ToListAsync();

                    //string StoredProcs = "exec SP_PipeCalculator " + dataID + "," + "'" + type + "'" + "," + "'" + latitude + "'" + "," + "'" + longitude + "'" + "," + postal_code + "," + diameterValue + "," + distanceValue + "," + pressureValue + "," + volumeValue + "," + "'" + route + "'" + "," + volume2 + "," + operasiHari + "," + operasiBulan + "," + "'" + energyName + "'" + "," + pressure;
                    //var data = _dbContext.Set<SP_PipeCalculatorData>().FromSqlRaw(StoredProcs).AsEnumerable().FirstOrDefault();
                    _dbContext.Set<SP_PipeCalculatorData>().FromSqlRaw("SP_PipeSektorCalculator @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16", dataID, type, latitude, longitude, postal_code, diameterValue, distanceValue, pressureValue, volumeValue, route, volume2, operasiHari, operasiBulan, energyName, pressure, customer, countCapel).ToList();

                }
            }
            return Ok
                    (new { data = calculate }
                    );

        }

        [HttpPost]
        public IActionResult SaveDataHeaderCluster([FromBody] HeaderCluster cluster)
        {
            var isStatus = true;
            int id = 0;
            try
            {
                HeaderCluster parameter = cluster;
                if (cluster != null)
                {
                    parameter.CreatedBy = this.User.Identity.Name;
                    parameter.CreatedDate = DateTimeOffset.Now;
                    _dbContext.HeaderCluster.AddAsync(parameter);

                    _dbContext.SaveChanges();

                    id = parameter.HeaderClusterID;
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
        public IActionResult SaveListCluster([FromBody] ClusterSimulation cluster)
        {
            var isStatus = true;
            int id = 0;
            try
            {
                ClusterSimulation parameter = cluster;
                if (cluster != null)
                {
                    parameter.CreatedBy = this.User.Identity.Name;
                    parameter.CreatedDate = DateTimeOffset.Now;
                    _dbContext.ClusterSimulation.AddAsync(parameter);

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

        [HttpPost]
        public IActionResult SaveDataCluster([FromBody] SP_ClusterSimulation energy)
        {
            var createdDate = energy.ProjectDate.ToString("yyyyMMdd");
            var targetDate = energy.TargetCOD.ToString("yyyyMMdd");
            var isStatus = true;

            try
            {
                if (energy != null)
                {
                    //string StoredProc = "exec SP_ClusterCostSimulation " + energy.HeaderClusterID + "," + "'" + energy.ProjectName + "'" + "," + "'" + energy.Creator + "'" + "," + "'" + createdDate + "'" + "," + "'" + energy.CustomerName + "'" + "," + energy.Volume + "," + "'" + targetDate + "'" + "," + energy.OperasiHari + "," + energy.OperasiBulan + "," + "'" + energy.EnergyName + "'" + "," + "'" + energy.Latitude + "'" + "," + "'" + energy.Longitude + "'" + "," + energy.Jarak + "," + "'" + energy.Latitude2 + "'" +  "," + "'" + energy.Longitude2 + "'" + "," + "'" + energy.AsalStation + "'" + "," + "'" + energy.LokasiCapel + "'" + "," + energy.TotalVolume + "," + energy.CountCapel;

                    //var data = _dbContext.Set<SP_ClusterSimulation>().FromSqlRaw(StoredProc).ToList();

                    _dbContext.Set<SP_ClusterSimulation>().FromSqlRaw("SP_ClusterCostSimulation @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18", energy.HeaderClusterID, energy.ProjectName, energy.Creator, createdDate, energy.CustomerName, energy.Volume, targetDate, energy.OperasiHari, energy.OperasiBulan, energy.EnergyName, energy.Latitude, energy.Longitude, energy.Jarak, energy.Latitude2, energy.Longitude2, energy.AsalStation, energy.LokasiCapel, energy.TotalVolume, energy.CountCapel).ToList();

                }
            }
            catch (Exception ex)
            {
                return Json(data: false);
            }

            return Ok
                  (
                      new { status = isStatus }
                  );
        }

    }
}
