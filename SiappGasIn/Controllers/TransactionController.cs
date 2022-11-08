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

namespace SiappGasIn.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TransCNG()
        {
            return View("~/Views/Transaction/TransCNG.cshtml");

        }
        [HttpGet]
        public IActionResult PipeMap()
        {
            return View("~/Views/Transaction/PipeMap.cshtml");

        }
    }
}
  