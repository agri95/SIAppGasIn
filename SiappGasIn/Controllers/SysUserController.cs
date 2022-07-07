using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Net.Http;
using System.Threading.Tasks;
using SiappGasIn.Data;
//using SiappGasIn.Helpers;
using SiappGasIn.Models;
using SiappGasIn.Services;

namespace SiappGasIn.Controllers
{
    [Authorize]
    public class SysUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        public SysUserController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;

        }

        [HttpGet]
        public IActionResult List()
        {
            return View("~/Views/User/List.cshtml");

        }


        [HttpGet]
        public IActionResult Form(string id)
        {
            UserProfileViewModel model = null;
            if (!string.IsNullOrEmpty(id))
            {
                model = (from u in _dbContext.ApplicationUser
                         where (u.Id == id)
                         select new UserProfileViewModel
                         {
                             //model applicationuser
                             Id = u.Id,
                             UserName = u.UserName,
                             Email = u.Email,
                             PhoneNumber = u.PhoneNumber,
                             IsLockedOut = u.LockoutEnd.HasValue ? (u.LockoutEnd.Value.AddHours(7) > DateTimeOffset.Now ? true : false) : false

                             //model sysuserprofile

                         }).FirstOrDefault();
            }

            return View("~/Modules/Master/SysUser/Form.cshtml", model);

        }

        [HttpGet]
        public async Task<IActionResult> UserProfile()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            return View("~/Modules/Master/SysUser/UserProfile.cshtml", user);
        }


    }
}

namespace SiappGasIn.Controllers.Api
{

    [Authorize]
    public class SysUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _env;
        public SysUserController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IWebHostEnvironment env, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _env = env;

        }

        


    }

    

}

