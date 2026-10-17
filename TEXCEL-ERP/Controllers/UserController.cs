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
    /// <summary>
    /// User controller — SuperAdmin + BranchAdmin.
    /// BranchAdmin = sirf apni branch ke users.
    /// Manager = view only (apni branch).
    /// Employee = access nahi.
    /// </summary>
    [RoleAuthorize("SuperAdmin", "BranchAdmin", "Manager")]
    [BranchAuthorize]
    public class UserController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public UserController()
        {
            _context = new ApplicationDbContext();
        }

        // =============================================
        // INDEX — User List
        // =============================================
        // GET: /User
        public ActionResult Index(string search, bool? isActive)
        {
            // ✅ Branch isolation: SuperAdmin = sab, baaki = apni branch
            var usersQuery = _context.Users.AsQueryable();

            if (!IsSuperAdmin && CurrentBranchId.HasValue)
            {
                int branchId = CurrentBranchId.Value;
                usersQuery = usersQuery.Where(u => u.BranchId == branchId);
            }

            var users = usersQuery.ToList();

            // Search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                users = users.Where(u =>
                    (u.FirstName ?? "").ToLower().Contains(search.ToLower()) ||
                    (u.LastName ?? "").ToLower().Contains(search.ToLower()) ||
                    (u.Email ?? "").ToLower().Contains(search.ToLower())).ToList();
            }

            // Status filter
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
            ViewBag.CanModify = CanModify;
            ViewBag.CanDelete = CanDeleteRecord;

            // ✅ Branch dropdown — SuperAdmin = sab, BranchAdmin = own only
            if (IsSuperAdmin)
            {
                ViewBag.Branches = _context.Branches.Where(b => b.IsActive).ToList();
            }
            else if (CurrentBranchId.HasValue)
            {
                int bid = CurrentBranchId.Value;
                ViewBag.Branches = _context.Branches.Where(b => b.IsActive && b.BranchId == bid).ToList();
            }
            else
            {
                ViewBag.Branches = new List<Branch>();
            }

            // ✅ Roles dropdown — SuperAdmin = sab, BranchAdmin = Manager/Employee only
            if (IsSuperAdmin)
            {
                ViewBag.Roles = _context.Roles.Select(r => r.Name).ToList();
            }
            else
            {
                ViewBag.Roles = new List<string> { "Manager", "Employee" };
            }

            return View(model);
        }

        // =============================================
        // GET BY ID — For Edit Modal (AJAX)
        // =============================================
        // GET: /User/GetById/{id}
        [HttpGet]
        public JsonResult GetById(string id)
        {
            try
            {
                var user = _context.Users.Find(id);
                if (user == null)
                {
                    return ErrorResult("User not found.");
                }

                // ✅ Branch isolation check
                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    if (user.BranchId != CurrentBranchId.Value)
                    {
                        return ErrorResult("You don't have permission to view this user.");
                    }
                }

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
                var roles = userManager.GetRoles(user.Id);

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        Id = user.Id,
                        user.FirstName,
                        user.LastName,
                        user.Email,
                        user.PhoneNumber,
                        user.BranchId,
                        RoleName = roles.FirstOrDefault() ?? "",
                        user.IsActive
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ErrorResult("Error: " + ex.Message);
            }
        }

        // =============================================
        // CREATE — POST (AJAX)
        // =============================================
        // POST: /User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(UserViewModel model, string Password)
        {
            try
            {
                if (!CanModify)
                {
                    return ErrorResult("You don't have permission to create users.");
                }

                if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(Password))
                {
                    return ErrorResult("Email and Password are required.");
                }

                // ✅ BranchAdmin sirf apni branch mein user bana sakta hai
                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    model.BranchId = CurrentBranchId.Value;
                }

                if (_context.Users.Any(u => u.Email == model.Email))
                {
                    return ErrorResult("Email already exists.");
                }

                // ✅ BranchAdmin sirf Manager/Employee bana sakta hai
                if (!IsSuperAdmin)
                {
                    if (model.RoleName != "Manager" && model.RoleName != "Employee")
                    {
                        return ErrorResult("You can only create Manager or Employee users.");
                    }
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
                    return SuccessResult("User created successfully!", new { id = user.Id });
                }

                return ErrorResult(string.Join(", ", result.Errors));
            }
            catch (Exception ex)
            {
                return ErrorResult("Error: " + ex.Message);
            }
        }

        // =============================================
        // EDIT — POST (AJAX)
        // =============================================
        // POST: /User/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(UserViewModel model)
        {
            try
            {
                if (!CanModify)
                {
                    return ErrorResult("You don't have permission to edit users.");
                }

                var user = _context.Users.Find(model.Id);
                if (user == null)
                {
                    return ErrorResult("User not found.");
                }

                // ✅ Branch isolation check
                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    if (user.BranchId != CurrentBranchId.Value)
                    {
                        return ErrorResult("You don't have permission to edit this user.");
                    }

                    // BranchAdmin apni branch nahi badal sakta
                    model.BranchId = CurrentBranchId.Value;
                }

                // ✅ BranchAdmin sirf Manager/Employee bana sakta hai
                if (!IsSuperAdmin)
                {
                    if (model.RoleName != "Manager" && model.RoleName != "Employee")
                    {
                        return ErrorResult("You can only assign Manager or Employee roles.");
                    }
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;
                user.BranchId = model.BranchId;
                user.IsActive = model.IsActive;

                _context.SaveChanges();

                // Update role
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
                var currentRoles = userManager.GetRoles(user.Id);

                if (!string.IsNullOrWhiteSpace(model.RoleName))
                {
                    // Remove old roles
                    if (currentRoles.Any())
                    {
                        userManager.RemoveFromRoles(user.Id, currentRoles.ToArray());
                    }
                    // Add new role
                    userManager.AddToRole(user.Id, model.RoleName);
                }

                return SuccessResult("User updated successfully!");
            }
            catch (Exception ex)
            {
                return ErrorResult("Error: " + ex.Message);
            }
        }

        // =============================================
        // TOGGLE STATUS — POST (AJAX)
        // =============================================
        // POST: /User/ToggleStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ToggleStatus(string id)
        {
            try
            {
                if (!CanModify)
                {
                    return ErrorResult("You don't have permission to change status.");
                }

                var user = _context.Users.Find(id);
                if (user == null)
                {
                    return ErrorResult("User not found.");
                }

                // ✅ Branch isolation check
                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    if (user.BranchId != CurrentBranchId.Value)
                    {
                        return ErrorResult("You don't have permission to change this user's status.");
                    }
                }

                user.IsActive = !user.IsActive;
                _context.SaveChanges();

                return SuccessResult("Status updated successfully.");
            }
            catch (Exception ex)
            {
                return ErrorResult("Error: " + ex.Message);
            }
        }

        // =============================================
        // DELETE (Deactivate) — POST (AJAX)
        // =============================================
        // POST: /User/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Delete(string id)
        {
            try
            {
                // ✅ Sirf SuperAdmin delete kar sakta hai
                if (!IsSuperAdmin)
                {
                    return ErrorResult("You don't have permission to delete users.");
                }

                var user = _context.Users.Find(id);
                if (user == null)
                {
                    return ErrorResult("User not found.");
                }

                // ✅ Khud ko delete na karein
                if (user.Id == CurrentUserId)
                {
                    return ErrorResult("You cannot delete your own account.");
                }

                // Soft delete — deactivate
                user.IsActive = false;
                _context.SaveChanges();

                return SuccessResult("User deactivated successfully.");
            }
            catch (Exception ex)
            {
                return ErrorResult("Error: " + ex.Message);
            }
        }
    }
}