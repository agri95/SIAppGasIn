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
            return View("~/Views/Master/SysUser/List.cshtml");

        }

         [HttpGet]
        public IActionResult Profile()
        {
            return View("~/Views/Master/SysUser/Profile.cshtml");

        }

        [HttpGet]
        public IActionResult Form(string id)
        {
            UserProfileViewModel model = null;
            if (!string.IsNullOrEmpty(id))
            {
                model = (from u in _dbContext.ApplicationUser
                         join up in _dbContext.SysUserProfile on u.Id equals up.ApplicationUserId
                         where (u.Id == id)
                         select new UserProfileViewModel
                         {
                             //model applicationuser
                             Id = u.Id,
                             UserName = u.UserName,
                             Email = u.Email,
                             PhoneNumber = u.PhoneNumber,
                             IsLockedOut = u.LockoutEnd.HasValue ? (u.LockoutEnd.Value.AddHours(7) > DateTimeOffset.Now ? true : false) : false,

                             //model sysuserprofile
                             Firstname = up.FirstName,
                             Lastname = up.LastName,
                             ProfilePicture = up.ProfilePicture,
                             JobTitle = up.JobTitle

                         }).FirstOrDefault();
            }

            return View("~/Views/Master/SysUser/Form.cshtml", model);

        }

        [HttpGet]
        public async Task<IActionResult> UserProfile()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            return View("~/Modules/Master/SysUser/UserProfile.cshtml", user);
        }


        //[HttpGet]
        //public async Task<IActionResult> ChangePassword(string Email)
        //{
        //    var appUser = await _userManager.FindByNameAsync(this.User.Identity.Name);
        //    ChangePasswordViewModel model = new ChangePasswordViewModel
        //    {
        //        Email = appUser.Email
        //    };

        //    return View("~/Modules/Master/SysUser/ChangePassword.cshtml", model);

        //}

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RoleList(string userId)
        {
            ApplicationUser model = await _userManager.FindByIdAsync(userId);
            return View("~/Modules/Master/SysUser/RoleList.cshtml", model);

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
        private readonly DefaultUserOptions _userDefaultOptions;

        //private readonly APISettingOptions _apiSettingOptions;
        private readonly IWebHostEnvironment _env;
        public SysUserController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IWebHostEnvironment env, RoleManager<IdentityRole> roleManager, IOptions<DefaultUserOptions> userDefaultOptions/*, IOptions<APISettingOptions> apiSettingOptions*/)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _env = env;
            _userDefaultOptions = userDefaultOptions.Value;
            //_apiSettingOptions = apiSettingOptions.Value;

        }

        [HttpPost]
        public async Task<IActionResult> Form([FromBody] UserProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                        ApplicationUser newUser = new ApplicationUser();
                        newUser.Email = string.IsNullOrEmpty(model.Email) ? _userDefaultOptions.Email : model.Email;
                        newUser.UserName = model.UserName;
                        newUser.FirstName = model.Firstname;
                        newUser.LastName = model.Lastname;

                        if (newUser.Email == _userDefaultOptions.Email)
                            newUser.EmailConfirmed = false;
                        else
                            newUser.EmailConfirmed = true;

                        var result = await _userManager.CreateAsync(newUser, _userDefaultOptions.Password);


                        if (result.Succeeded)
                        {
                        //add to user profile
                        SysUserProfile userProfile = new SysUserProfile();

                            userProfile.FirstName = model.Firstname;
                            userProfile.LastName = model.Lastname;
                            userProfile.Email = model.Email;
                            userProfile.ApplicationUserId = newUser.Id;

                        userProfile.JobTitle = model.JobTitle;

                            userProfile.CreatedBy = this.User.Identity.Name;
                            userProfile.CreatedDate = DateTimeOffset.Now;

                            if (model.Image != null)
                            {
                                var extension = Path.GetExtension(model.Image.FileName);
                                var fileName = model.UserName + extension; // Rename fileName with UserName
                                var filePath = Path.Combine(_env.WebRootPath + "/adminlte/images/", fileName);

                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await model.Image.CopyToAsync(stream);
                                }
                                userProfile.ProfilePicture = "~/adminlte/images/" + fileName;
                            }

                            
                        var addUser = await _userManager.FindByIdAsync(newUser.Id);
                        var addRole = await _roleManager.FindByIdAsync(model.RoleID);
                        await _userManager.AddToRoleAsync(addUser, addRole.Name);
                        await _dbContext.SysUserProfile.AddAsync(userProfile);
                        await _dbContext.SaveChangesAsync();
                        }
                    
                }
                catch (Exception ex)
                {
                    ApplicationUser newUser = await _userManager.FindByIdAsync(model.Id);
                    await _userManager.DeleteAsync(newUser);

                    ModelState.AddModelError("error", ex.Message);
                    return Json(data: false);
                }
            }
            else
            {
                return Json(data: false);
            }

            return Json(data: true);

        }

        [HttpPost]
        public async Task<IActionResult> Retrieves()
        {
            var users = from us in _dbContext.ApplicationUser
                        join us1 in _dbContext.SysUserProfile on us.Id equals us1.ApplicationUserId
                        select new UserProfileViewModel
                        {
                            //model applicationuser
                            Id = us.Id,
                            UserName = us.UserName,
                            Email = us.Email,
                            PhoneNumber = us.PhoneNumber,
                            IsLockedOut = us.LockoutEnd.HasValue ? (us.LockoutEnd.Value.AddHours(7) > DateTimeOffset.Now ? true : false) : false,

                            //model sysuserprofile
                            Firstname = us1.FirstName,
                            Lastname = us1.LastName,
                            ProfilePicture = us1.ProfilePicture
                        };

            return Ok
                    (
                        new { data = users }
                    );


        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserProfileViewModel model)
        {
            if (model != null && model.Id != null)
            {
                try
                {
                    ApplicationUser newUser = await _userManager.FindByIdAsync(model.Id);
                    var result = await _userManager.DeleteAsync(newUser);

                    if (result.Succeeded)
                    {
                        SysUserProfile user = new SysUserProfile();
                        user = _dbContext.SysUserProfile.Where(x => x.ApplicationUserId == model.Id).FirstOrDefault();
                        _dbContext.SysUserProfile.Remove(user);
                    }
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("error", ex.Message);
                }
            }

            return RedirectToAction("List", "SysUser", new { menuid = this.HttpContext.Request.Query["menuId"] });

        }

        [HttpPost]
        public async Task<IActionResult> RetrieveRoles(string userId)
        {
            var rolesInUser = (from a in _userManager.Users
                               join b in _dbContext.UserRoles on a.Id equals b.UserId
                               join c in _dbContext.Roles on b.RoleId equals c.Id
                               where (a.Id == userId)
                               select new RoleUser
                               {
                                   Name = c.Name
                               });

            var roles = (from r in _roleManager.Roles
                         from ur in rolesInUser
                         .Where(ur => ur.Name == r.Name)
                         .DefaultIfEmpty()
                         select new RoleUserList
                         {
                             Id = r.Id,
                             Name = r.Name,
                             IsChecked = ur.Name == r.Name ? "checked" : ""
                         });

           return Ok
                    (
                        new { data = roles }
                    );

        }

        [HttpPost]
        public IActionResult RetrievesListUser(string text)
        {
            List<UserProfileViewModel> userList = null;

            var users = from us in _dbContext.ApplicationUser
                        join us1 in _dbContext.SysUserProfile on us.Id equals us1.ApplicationUserId
                        select new UserProfileViewModel
                        {
                            Id = us.Id,
                            UserName = us.UserName,
                            Email = us.Email,
                            Firstname = us1.FirstName,
                            Lastname = us1.LastName
                            //,FullName = us1.FullName
                        };

            if (!string.IsNullOrEmpty(text))
                users = users.Where(x => x.UserName.ToLower().Contains(text.ToLower()) || x.Email.ToLower().Contains(text.ToLower()) || x.FullName.ToLower().Contains(text.ToLower()));

            userList = users.ToList<UserProfileViewModel>();

            return Json(userList);
        }

        [HttpPost]
        public IActionResult RetrievesListUserPIC(string text, string pic)
        {
            IEnumerable<UserProfileViewModel> users = null;
            var roles = (from a in _userManager.Users
                         join b in _dbContext.UserRoles on a.Id equals b.UserId
                         join c in _dbContext.Roles on b.RoleId equals c.Id
                         where (c.Name == pic)
                         select new ApplicationUser
                         {
                             Id = a.Id,
                             UserName = a.UserName,
                             Email = a.Email
                         });

            users = (from u in _userManager.Users
                     join ur in roles on u.Id equals ur.Id
                     join us1 in _dbContext.SysUserProfile on ur.Id equals us1.ApplicationUserId
                     select new UserProfileViewModel
                     {
                         Id = u.Id,
                         UserName = u.UserName,
                         Email = u.Email,
                         Firstname = us1.FirstName,
                         Lastname = us1.LastName
                         //,FullName = us1.FullName
                     });

            if (!string.IsNullOrEmpty(text))
                users = users.Where(x => x.UserName.ToLower().Contains(text.ToLower()) || x.Email.ToLower().Contains(text.ToLower()) || x.FullName.ToLower().Contains(text.ToLower()));

            var list = users.Select(x => new { x.Id, x.FullName }).Distinct().OrderBy(x => x.FullName);

            return Json(list);
        }

        
        [HttpPost]
        public IActionResult RetrieveUser(string userId)
        {
            var isStatus = true;

            var user = from us in _dbContext.ApplicationUser
                       join us1 in _dbContext.SysUserProfile on us.Id equals us1.ApplicationUserId
                       where us.Id.Equals(userId)
                       select new UserProfileViewModel
                       {
                           Id = us.Id,
                           UserName = us.UserName,
                           Email = us.Email,
                           PhoneNumber = us.PhoneNumber,
                           Firstname = us1.FirstName,
                           Lastname = us1.LastName
                       };          
          
            return Ok
                    (
                        new { data = user, status = isStatus }
                    );
            return Json(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditData([FromBody] UserProfileViewModel model)
        {
            if (model != null)
            {
                try
                {
                    IdentityResult result = null;
                    if (!string.IsNullOrEmpty(model.Id)) // Update User
                    {

                        ApplicationUser user = await _userManager.FindByIdAsync(model.Id);

                        user.Email = model.Email;
                        user.PhoneNumber = model.PhoneNumber;

                        result = await _userManager.UpdateAsync(user);

                    }
                    if (result.Succeeded)
                    {
                        //add to user profile
                        SysUserProfile userProfile = _dbContext.SysUserProfile.Where(s => s.ApplicationUserId == model.Id).FirstOrDefault();

                        userProfile.FirstName = model.Firstname;
                        userProfile.LastName = model.Lastname;
                        userProfile.Email = model.Email;
                        userProfile.ApplicationUserId = model.Id;

                        userProfile.ModifiedBy = this.User.Identity.Name;
                        userProfile.ModifiedDate = DateTimeOffset.Now;

                        _dbContext.SysUserProfile.Update(userProfile);

                        _dbContext.Entry<SysUserProfile>(userProfile).Property(x => x.CreatedBy).IsModified = false;
                        _dbContext.Entry<SysUserProfile>(userProfile).Property(x => x.CreatedDate).IsModified = false;
                    }

                }
                catch (Exception ex)
                {
                   
                    return Json(data: false);
                }
            }
            else
            {
                return Json(data: false);
            }

            return Json(data: true);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoleUser(List<RoleUser> models)
        {
            string msgResult = string.Empty;
            if (models != null && models.Count > 0)
            {
                try
                {
                    var userId = models[0].UserId;
                    ApplicationUser user = await _userManager.FindByIdAsync(userId);
                    IList<string> roles = await _userManager.GetRolesAsync(user);
                    foreach (var model in models)
                    {
                        var isExists = roles.FirstOrDefault(x => x == model.Name);

                        if (isExists != null)
                        {
                            if (model.IsChecked == false)
                            {
                                var roleName = isExists;
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
                                var addRole = await _roleManager.FindByIdAsync(model.RoleId);
                                var result = await _userManager.AddToRoleAsync(user, addRole.Name);
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

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            string msgResult = string.Empty;
            
                ApplicationUser user = await _userManager.FindByNameAsync(model.Username);
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);
                if (result.Succeeded)
                    msgResult = "Password has been changed successfully!";
                else
                {
                    foreach (var err in result.Errors)
                    {
                        msgResult += err.Description + Environment.NewLine;
                    }
                }

                ModelState.AddModelError("Error", msgResult);

           

            return View("~/Views/Master/SysUser/ChangePassword.cshtml", model);

        }

       
    }
}
