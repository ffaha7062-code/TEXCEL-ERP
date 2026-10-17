using System;
using System.Linq;
using System.Web.Mvc;
using TEXCEL_ERP.Data;
using TEXCEL_ERP.Data.Repository;
using TEXCEL_ERP.Filters;
using TEXCEL_ERP.Models.Entities;
using TEXCEL_ERP.Models.ViewModels;
using TEXCEL_ERP.Services;

namespace TEXCEL_ERP.Controllers
{
    /// <summary>
    /// Branch controller — sirf SuperAdmin access kar sakta hai.
    /// </summary>
    [RoleAuthorize("SuperAdmin")]
    [BranchAuthorize]
    public class BranchController : BaseController
    {
        private readonly IBranchService _branchService;

        public BranchController()
        {
            var context = new ApplicationDbContext();
            var uow = new UnitOfWork(context);
            _branchService = new BranchService(uow);
        }

        // =============================================
        // INDEX — Branch List
        // =============================================
        // GET: /Branch
        public ActionResult Index(string search, bool? isActive)
        {
            var branches = _branchService.GetAllBranches().ToList();

            if (!string.IsNullOrWhiteSpace(search))
            {
                branches = branches.Where(b =>
                    (b.BranchName ?? "").ToLower().Contains(search.ToLower()) ||
                    (b.BranchCode ?? "").ToLower().Contains(search.ToLower()) ||
                    (b.City ?? "").ToLower().Contains(search.ToLower())).ToList();
            }

            if (isActive.HasValue)
                branches = branches.Where(b => b.IsActive == isActive.Value).ToList();

            ViewBag.Search = search;
            ViewBag.IsActive = isActive;
            ViewBag.CanModify = CanModify;
            ViewBag.CanDelete = CanDeleteRecord;

            return View(branches);
        }

        // =============================================
        // DETAILS — Single Branch View
        // =============================================
        // GET: /Branch/Details/5
        public ActionResult Details(int id)
        {
            var branch = _branchService.GetBranchById(id);

            if (branch == null)
            {
                TempData["Error"] = "Branch not found.";
                return RedirectToAction("Index");
            }

            return View(branch);
        }

        // =============================================
        // CREATE — GET (Modal Only — No GET Needed)
        // =============================================
        // Create modal Index page pe hai — koi GET action nahi chahiye

        // =============================================
        // CREATE — POST (AJAX)
        // =============================================
        // POST: /Branch/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(BranchViewModel model)
        {
            try
            {
                if (!CanModify)
                {
                    return ErrorResult("You don't have permission to create branches.");
                }

                if (!ModelState.IsValid)
                {
                    var errors = string.Join(" ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    return ErrorResult(string.IsNullOrEmpty(errors)
                        ? "Please fill all required fields."
                        : errors);
                }

                if (_branchService.IsBranchCodeExists(model.BranchCode))
                {
                    return ErrorResult("Branch code already exists.");
                }

                var branch = new Branch
                {
                    CompanyId = model.CompanyId,
                    BranchCode = model.BranchCode,
                    BranchName = model.BranchName,
                    Address = model.Address,
                    City = model.City,
                    Phone = model.Phone,
                    Email = model.Email,
                    ManagerName = model.ManagerName,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };

                _branchService.AddBranch(branch);

                return SuccessResult("Branch created successfully!", new { id = branch.BranchId });
            }
            catch (Exception ex)
            {
                return ErrorResult("Error: " + ex.Message);
            }
        }

        // =============================================
        // GET BY ID — For Edit Modal (AJAX)
        // =============================================
        // GET: /Branch/GetById/5
        [HttpGet]
        public JsonResult GetById(int id)
        {
            try
            {
                var branch = _branchService.GetBranchById(id);

                if (branch == null)
                {
                    return ErrorResult("Branch not found.");
                }

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        branch.BranchId,
                        branch.CompanyId,
                        branch.BranchCode,
                        branch.BranchName,
                        branch.Address,
                        branch.City,
                        branch.Phone,
                        branch.Email,
                        branch.ManagerName,
                        branch.IsActive
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ErrorResult("Error: " + ex.Message);
            }
        }

        // =============================================
        // EDIT — POST (AJAX)
        // =============================================
        // POST: /Branch/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(BranchViewModel model)
        {
            try
            {
                if (!CanModify)
                {
                    return ErrorResult("You don't have permission to edit branches.");
                }

                if (!ModelState.IsValid)
                {
                    var errors = string.Join(" ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    return ErrorResult(string.IsNullOrEmpty(errors)
                        ? "Please fill all required fields."
                        : errors);
                }

                var branch = _branchService.GetBranchById(model.BranchId);
                if (branch == null)
                {
                    return ErrorResult("Branch not found.");
                }

                // Branch code duplicate check (agar change hua hai)
                if (!string.Equals(branch.BranchCode, model.BranchCode, StringComparison.OrdinalIgnoreCase)
                    && _branchService.IsBranchCodeExists(model.BranchCode))
                {
                    return ErrorResult("Branch code already exists.");
                }

                branch.CompanyId = model.CompanyId;
                branch.BranchCode = model.BranchCode;
                branch.BranchName = model.BranchName;
                branch.Address = model.Address;
                branch.City = model.City;
                branch.Phone = model.Phone;
                branch.Email = model.Email;
                branch.ManagerName = model.ManagerName;
                branch.IsActive = model.IsActive;

                _branchService.UpdateBranch(branch);

                return SuccessResult("Branch updated successfully!");
            }
            catch (Exception ex)
            {
                return ErrorResult("Error: " + ex.Message);
            }
        }

        // =============================================
        // TOGGLE STATUS — POST (AJAX)
        // =============================================
        // POST: /Branch/ToggleStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ToggleStatus(int id)
        {
            try
            {
                if (!CanModify)
                {
                    return ErrorResult("You don't have permission to change status.");
                }

                var branch = _branchService.GetBranchById(id);
                if (branch == null)
                {
                    return ErrorResult("Branch not found.");
                }

                _branchService.ToggleStatus(id);
                return SuccessResult("Status updated successfully.");
            }
            catch (Exception ex)
            {
                return ErrorResult("Error: " + ex.Message);
            }
        }

        // =============================================
        // DELETE — POST (AJAX)
        // =============================================
        // POST: /Branch/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Delete(int id)
        {
            try
            {
                if (!CanDeleteRecord)
                {
                    return ErrorResult("You don't have permission to delete branches.");
                }

                var branch = _branchService.GetBranchById(id);
                if (branch == null)
                {
                    return ErrorResult("Branch not found.");
                }

                var beforeCount = _branchService.GetTotalBranches();
                _branchService.DeleteBranch(id);
                var afterCount = _branchService.GetTotalBranches();

                System.Diagnostics.Debug.WriteLine($"=== DELETE: {beforeCount} → {afterCount} ===");

                return SuccessResult($"Branch deleted. ({beforeCount} → {afterCount})");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"=== DELETE ERROR: {ex.Message} ===");
                System.Diagnostics.Debug.WriteLine($"=== INNER: {ex.InnerException?.Message} ===");
                return ErrorResult("Error: " + ex.Message);
            }
        }
    }
}