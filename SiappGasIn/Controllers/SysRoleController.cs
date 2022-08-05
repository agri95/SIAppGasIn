using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiappGasIn.Data;
using SiappGasIn.Models;

namespace SiappGasIn.Controllers
{
    [Authorize(Roles = "Super Admin")]
    public class SysRoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public SysRoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }


        [HttpGet]
        public async Task<IActionResult> Form(string id)
        {
            SysRoleViewModel model = null;
            if (!string.IsNullOrEmpty(id))
            {
                model = new SysRoleViewModel();
                IdentityRole role = await _roleManager.FindByIdAsync(id);
                model.Id = role.Id;
                model.Name = role.Name;
            }

            return View("~/Modules/Master/SysRole/Form.cshtml", model);
        }

        [HttpGet]
        public IActionResult List()
        {
            return View("~/Views/Master/SysRole/List.cshtml");

        }

        [HttpGet]
        public async Task<IActionResult> UserList(string roleId)
        {
            IdentityRole model = await _roleManager.FindByIdAsync(roleId);
            return View("~/Modules/Master/SysRole/UserList.cshtml", model);

        }

    }
}



namespace SiappGasIn.Controllers.Api
{
    [Authorize]
    public class SysRoleController : Controller
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public SysRoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Form([FromBody] SysRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    IdentityResult result = null;
                    if (model != null && !string.IsNullOrEmpty(model.Id))
                    {
                        IdentityRole identityRole = await _roleManager.FindByIdAsync(model.Id);
                        identityRole.Name = model.Name;
                        result = await _roleManager.UpdateAsync(identityRole);
                    }
                    else
                    {
                        IdentityRole identityRole = new IdentityRole
                        {
                            Name = model.Name
                        };
                        result = await _roleManager.CreateAsync(identityRole);
                    }

                    if (result.Succeeded)
                    {
                        return Json(data: true);
                    }

                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                catch (Exception ex)
                {
                    return Json(data: false);
                }
            }

            return Json(data: true);
        }

        [HttpPost]
        public IActionResult RetrieveList()
        {
            var roleList = _roleManager.Roles;

            return Ok
            (
                new { list = roleList }
            );

        }

        //[HttpPost]
        //public async Task<IActionResult> RetrieveUsers(KendoUI.DataSourceRequest request, string roleId)
        //{
        //    var usersInRole = (from a in _userManager.Users
        //                       join b in _dbContext.UserRoles on a.Id equals b.UserId
        //                       join c in _dbContext.Roles on b.RoleId equals c.Id
        //                       where (c.Id == roleId)
        //                       select new ApplicationUser
        //                       {
        //                           Id = a.Id,
        //                           UserName = a.UserName,
        //                           Email = a.Email
        //                       });

        //    var users = (from u in _userManager.Users
        //                 join us1 in _dbContext.SysUserProfile on u.Id equals us1.ApplicationUserId
        //                 from ur in usersInRole
        //                 .Where(ur => ur.Id == u.Id)
        //                 .DefaultIfEmpty()
        //                 select new UserRoleList
        //                 {
        //                     Id = u.Id,
        //                     UserName = u.UserName,
        //                     FirstName = us1.FirstName,
        //                     LastName = us1.LastName,
        //                     Email = u.Email,
        //                     IsChecked = u.Id == ur.Id ? "checked" : ""
        //                 });

        //    var strSearch = request.Search;

        //    if (!string.IsNullOrEmpty(strSearch))
        //        users = users.Where(x => x.UserName.ToLower().Contains(strSearch.ToLower()) || x.Email.ToLower().Contains(strSearch.ToLower()) || x.FirstName.ToLower().Contains(strSearch.ToLower()) || x.LastName.ToLower().Contains(strSearch.ToLower()));

        //    KendoUI.PagingList<UserRoleList> usersRole = await KendoUI.PagingList<UserRoleList>.CreateAsync(users.AsNoTracking(), request.Page, request.PageSize);

        //    return Ok
        //    (
        //        new { list = usersRole, totalItems = usersRole.TotalItems }
        //    );

        //}

        [HttpPost]
        public IActionResult Retrieves()
        {
            List<SysRoleViewModel> roles = new List<SysRoleViewModel>();
            foreach (var role in _roleManager.Roles)
            {
                SysRoleViewModel obj = new SysRoleViewModel();
                obj.Id = role.Id;
                obj.Name = role.Name;
                roles.Add(obj);
            }

            var results = from r in roles
                          select r;


            return Ok
                     (
                         new { data = results }
                     );

        }

        [HttpPost]
        public async Task<IActionResult> Delete(SysRoleViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Id))
            {
                try
                {
                    var _role = await _roleManager.FindByIdAsync(model.Id);
                    await _roleManager.DeleteAsync(_role);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("error", ex.Message);
                }
            }

            return View("~/Modules/Master/SysRole/List.cshtml" + "?menuid" + this.HttpContext.Request.Query["menuId"], model);

        }

        [HttpPost]
        public async Task<IActionResult> AddUserRole(List<UserRole> models)
        {
            string msgResult = string.Empty;
            if (models != null && models.Count > 0)
            {
                try
                {
                    var roleName = models[0].RoleName;
                    IList<ApplicationUser> users = await _userManager.GetUsersInRoleAsync(roleName);
                    foreach (var model in models)
                    {
                        var isExists = users.FirstOrDefault(x => x.UserName == model.UserName);

                        if (isExists != null)
                        {
                            if (model.IsChecked == false)
                            {
                                var user = isExists;
                                var result = await _userManager.RemoveFromRoleAsync(user, roleName);
                                if (result.Succeeded)
                                    msgResult = "Data has been updated successfully";
                                else
                                    msgResult = "Data fail updated";
                            }
                        }
                        else
                        {
                            if (model.IsChecked)
                            {
                                var addUser = await _userManager.FindByIdAsync(model.UserId);
                                var result = await _userManager.AddToRoleAsync(addUser, roleName);
                                if (result.Succeeded)
                                    msgResult = "Data has been updated successfully";
                                else
                                    msgResult = "Data fail updated";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("error", ex.Message);
                    msgResult = "Data fail updated with errror " + ex.Message;
                }
            }
            else
                msgResult = "No data has been updated!";


            return Ok
                    (
                        new { status = msgResult }
                    );

        }
    }

}

