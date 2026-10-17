using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TEXCEL_ERP.Data;
using TEXCEL_ERP.Filters;
using TEXCEL_ERP.Models.Entities;
using TEXCEL_ERP.Models.ViewModels;

namespace TEXCEL_ERP.Controllers
{
    /// <summary>
    /// Department controller.
    /// SuperAdmin = all branches, BranchAdmin/Manager = own branch.
    /// Manager = view only, Employee = no access.
    /// </summary>
    [RoleAuthorize("SuperAdmin", "BranchAdmin", "Manager")]
    [BranchAuthorize]
    public class DepartmentController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController()
        {
            _context = new ApplicationDbContext();
        }

        // =============================================
        // INDEX — Department List
        // =============================================
        // GET: /Department
        public ActionResult Index(string search, bool? isActive, int? branchId)
        {
            var query = _context.Departments.AsQueryable();

            // ✅ Branch isolation: SuperAdmin = sab, baaki = apni branch
            if (!IsSuperAdmin && CurrentBranchId.HasValue)
            {
                int bid = CurrentBranchId.Value;
                query = query.Where(d => d.BranchId == bid);
            }
            else if (branchId.HasValue)
            {
                // SuperAdmin filter by specific branch
                query = query.Where(d => d.BranchId == branchId.Value);
            }

            // Search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(d =>
                    d.DepartmentName.Contains(search) ||
                    d.DepartmentCode.Contains(search));
            }

            // Status filter
            if (isActive.HasValue)
                query = query.Where(d => d.IsActive == isActive.Value);

            var departments = query.OrderBy(d => d.DepartmentName).ToList();

            ViewBag.Search = search;
            ViewBag.IsActive = isActive;
            ViewBag.BranchId = branchId;
            ViewBag.CanModify = CanModify;
            ViewBag.CanDelete = CanDeleteRecord;

            // ✅ Branches dropdown:
            // SuperAdmin = sab branches, BranchAdmin/Manager = own branch only
            if (IsSuperAdmin)
            {
                ViewBag.Branches = _context.Branches.Where(b => b.IsActive).ToList();
            }
            else if (CurrentBranchId.HasValue)
            {
                int bid = CurrentBranchId.Value;
                ViewBag.Branches = _context.Branches
                    .Where(b => b.IsActive && b.BranchId == bid)
                    .ToList();
            }
            else
            {
                ViewBag.Branches = new List<Branch>();
            }

            return View(departments);
        }

        // =============================================
        // GET BY ID — For Edit Modal (AJAX)
        // =============================================
        // GET: /Department/GetById/{id}
        [HttpGet]
        public JsonResult GetById(int id)
        {
            try
            {
                var dept = _context.Departments.Find(id);
                if (dept == null)
                {
                    return ErrorResult("Department not found.");
                }

                // ✅ Branch isolation check
                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    if (dept.BranchId != CurrentBranchId.Value)
                    {
                        return ErrorResult("You don't have permission to view this department.");
                    }
                }

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        dept.DepartmentId,
                        dept.BranchId,
                        dept.DepartmentCode,
                        dept.DepartmentName,
                        dept.Description,
                        dept.IsActive
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ErrorResult("Error: " + ex.Message);
            }
        }

        // =============================================
        // CREATE — POST (POST-Redirect-GET)
        // =============================================
        // POST: /Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DepartmentViewModel model)
        {
            try
            {
                // ✅ Permission check
                if (!CanModify)
                {
                    TempData["Error"] = "You don't have permission to create departments.";
                    return RedirectToAction("Index");
                }

                // ✅ BranchAdmin/Manager = own branch only
                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    model.BranchId = CurrentBranchId.Value;
                }

                if (ModelState.IsValid)
                {
                    if (_context.Departments.Any(d => d.DepartmentCode == model.DepartmentCode))
                    {
                        TempData["Error"] = "Department code already exists.";
                        return RedirectToAction("Index");
                    }

                    var dept = new Department
                    {
                        BranchId = model.BranchId,
                        DepartmentCode = model.DepartmentCode,
                        DepartmentName = model.DepartmentName,
                        Description = model.Description,
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    };

                    _context.Departments.Add(dept);
                    _context.SaveChanges();
                    TempData["Success"] = "Department created successfully!";
                }
                else
                {
                    TempData["Error"] = "Please fill all required fields.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // =============================================
        // EDIT — POST (POST-Redirect-GET)
        // =============================================
        // POST: /Department/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DepartmentViewModel model)
        {
            try
            {
                // ✅ Permission check
                if (!CanModify)
                {
                    TempData["Error"] = "You don't have permission to edit departments.";
                    return RedirectToAction("Index");
                }

                // ✅ BranchAdmin/Manager = own branch only
                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    model.BranchId = CurrentBranchId.Value;
                }

                if (ModelState.IsValid)
                {
                    var dept = _context.Departments.Find(model.DepartmentId);
                    if (dept == null)
                    {
                        TempData["Error"] = "Department not found.";
                        return RedirectToAction("Index");
                    }

                    // ✅ Branch isolation check
                    if (!IsSuperAdmin && CurrentBranchId.HasValue)
                    {
                        if (dept.BranchId != CurrentBranchId.Value)
                        {
                            TempData["Error"] = "You don't have permission to edit this department.";
                            return RedirectToAction("Index");
                        }
                    }

                    // Duplicate code check (excluding current)
                    if (_context.Departments.Any(d => d.DepartmentCode == model.DepartmentCode
                        && d.DepartmentId != model.DepartmentId))
                    {
                        TempData["Error"] = "Department code already exists.";
                        return RedirectToAction("Index");
                    }

                    dept.BranchId = model.BranchId;
                    dept.DepartmentCode = model.DepartmentCode;
                    dept.DepartmentName = model.DepartmentName;
                    dept.Description = model.Description;

                    _context.SaveChanges();
                    TempData["Success"] = "Department updated successfully!";
                }
                else
                {
                    TempData["Error"] = "Please fill all required fields.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // =============================================
        // TOGGLE STATUS — POST (AJAX)
        // =============================================
        // POST: /Department/ToggleStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ToggleStatus(int id)
        {
            try
            {
                // ✅ Permission check
                if (!CanModify)
                {
                    return ErrorResult("You don't have permission to change status.");
                }

                var dept = _context.Departments.Find(id);
                if (dept == null)
                {
                    return ErrorResult("Department not found.");
                }

                // ✅ Branch isolation check
                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    if (dept.BranchId != CurrentBranchId.Value)
                    {
                        return ErrorResult("You don't have permission to change this department's status.");
                    }
                }

                dept.IsActive = !dept.IsActive;
                _context.SaveChanges();

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
        // POST: /Department/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Delete(int id)
        {
            try
            {
                // ✅ Permission check — sirf SuperAdmin + BranchAdmin
                if (!CanDeleteRecord)
                {
                    return ErrorResult("You don't have permission to delete departments.");
                }

                var dept = _context.Departments.Find(id);
                if (dept == null)
                {
                    return ErrorResult("Department not found.");
                }

                // ✅ Branch isolation check
                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    if (dept.BranchId != CurrentBranchId.Value)
                    {
                        return ErrorResult("You don't have permission to delete this department.");
                    }
                }

                // ✅ Check ke employees exist karte hain ya nahi
                if (_context.Employees.Any(e => e.DepartmentId == id))
                {
                    return ErrorResult("Cannot delete — employees exist in this department.");
                }

                _context.Departments.Remove(dept);
                _context.SaveChanges();

                return SuccessResult("Department deleted successfully.");
            }
            catch (Exception ex)
            {
                return ErrorResult("Error: " + ex.Message);
            }
        }
    }
}