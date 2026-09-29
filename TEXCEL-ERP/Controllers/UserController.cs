using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TEXCEL_ERP.Data;
using TEXCEL_ERP.Filters;
using TEXCEL_ERP.Models.Entities;
using TEXCEL_ERP.Models.ViewModels;

namespace TEXCEL_ERP.Controllers
{
    [SessionTimeout]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: /User
        public ActionResult Index(string search, bool? isActive)
        {
            var users = _context.Users.ToList();

            if (!string.IsNullOrWhiteSpace(search))
            {
                users = users.Where(u =>
                    (u.FirstName ?? "").ToLower().Contains(search.ToLower()) ||
                    (u.LastName ?? "").ToLower().Contains(search.ToLower()) ||
                    (u.Email ?? "").ToLower().Contains(search.ToLower())).ToList();
            }

            if (isActive.HasValue)
                users = users.Where(u => u.IsActive == isActive.Value).ToList();

            // Get user roles
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            var model = new List<UserViewModel>();

            foreach (var u in users)
            {
                var roles = userManager.GetRoles(u.Id);
                model.Add(new UserViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    BranchId = u.BranchId,
                    RoleName = roles.FirstOrDefault() ?? "No Role",
                    IsActive = u.IsActive,
                    LastLogin = u.LastLogin,
                    CreatedDate = u.CreatedDate
                });
            }

            ViewBag.Search = search;
            ViewBag.IsActive = isActive;
            ViewBag.Branches = _context.Branches.Where(b => b.IsActive).ToList();
            ViewBag.Roles = _context.Roles.Select(r => r.Name).ToList();

            return View(model);
        }

        // POST: /User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserViewModel model, string Password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(Password))
                {
                    TempData["Error"] = "Email and Password are required.";
                    return RedirectToAction("Index");
                }

                if (_context.Users.Any(u => u.Email == model.Email))
                {
                    TempData["Error"] = "Email already exists.";
                    return RedirectToAction("Index");
                }

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    BranchId = model.BranchId,
                    IsActive = true,
                    EmailConfirmed = true,
                    CreatedDate = DateTime.Now
                };

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
                var result = userManager.Create(user, Password);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(model.RoleName))
                    {
                        userManager.AddToRole(user.Id, model.RoleName);
                    }
                    TempData["Success"] = "User created successfully!";
                }
                else
                {
                    TempData["Error"] = string.Join(", ", result.Errors);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // POST: /User/ToggleStatus
        [HttpPost]
        public JsonResult ToggleStatus(string id)
        {
            try
            {
                var user = _context.Users.Find(id);
                if (user != null)
                {
                    user.IsActive = !user.IsActive;
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Status updated." });
                }
                return Json(new { success = false, message = "User not found." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: /User/Delete
        [HttpPost]
        public JsonResult Delete(string id)
        {
            try
            {
                var user = _context.Users.Find(id);
                if (user != null)
                {
                    user.IsActive = false;
                    _context.SaveChanges();
                    return Json(new { success = true, message = "User deactivated." });
                }
                return Json(new { success = false, message = "User not found." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}