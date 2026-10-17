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
    /// Designation controller.
    /// SuperAdmin = all branches, BranchAdmin/Manager = own branch.
    /// Manager = view only, Employee = no access.
    /// Note: Designation mein BranchId nahi hai — Department ke through filter hota hai.
    /// </summary>
    [RoleAuthorize("SuperAdmin", "BranchAdmin", "Manager")]
    [BranchAuthorize]
    public class DesignationController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public DesignationController()
        {
            _context = new ApplicationDbContext();
        }

        // =============================================
        // INDEX — Designation List
        // =============================================
        // GET: /Designation
        public ActionResult Index(string search, bool? isActive, int? departmentId)
        {
            var query = _context.Designations.AsQueryable();

            // ✅ Branch isolation: SuperAdmin = sab, baaki = apni branch (via Department)
            if (!IsSuperAdmin && CurrentBranchId.HasValue)
            {
                int bid = CurrentBranchId.Value;
                var deptIds = _context.Departments
                    .Where(d => d.BranchId == bid)
                    .Select(d => d.DepartmentId)
                    .ToList();

                query = query.Where(d => deptIds.Contains(d.DepartmentId));
            }

            // Search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(d =>
                    d.DesignationName.Contains(search) ||
                    d.DesignationCode.Contains(search));
            }

            // Status filter
            if (isActive.HasValue)
                query = query.Where(d => d.IsActive == isActive.Value);

            // Department filter
            if (departmentId.HasValue)
                query = query.Where(d => d.DepartmentId == departmentId.Value);

            var designations = query.OrderBy(d => d.DesignationName).ToList();

            ViewBag.Search = search;
            ViewBag.IsActive = isActive;
            ViewBag.DepartmentId = departmentId;
            ViewBag.CanModify = CanModify;
            ViewBag.CanDelete = CanDeleteRecord;

            // ✅ Departments dropdown: SuperAdmin = sab, baaki = own branch only
            if (IsSuperAdmin)
            {
                ViewBag.Departments = _context.Departments.Where(d => d.IsActive).ToList();
            }
            else if (CurrentBranchId.HasValue)
            {
                int bid = CurrentBranchId.Value;
                ViewBag.Departments = _context.Departments
                    .Where(d => d.IsActive && d.BranchId == bid)
                    .ToList();
            }
            else
            {
                ViewBag.Departments = new List<Department>();
            }

            return View(designations);
        }

        // =============================================
        // GET BY ID — For Edit Modal (AJAX)
        // =============================================
        // GET: /Designation/GetById/{id}
        [HttpGet]
        public JsonResult GetById(int id)
        {
            try
            {
                var designation = _context.Designations.Find(id);
                if (designation == null)
                {
                    return ErrorResult("Designation not found.");
                }

                // ✅ Branch isolation check (via Department)
                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    int bid = CurrentBranchId.Value;
                    var dept = _context.Departments.Find(designation.DepartmentId);
                    if (dept == null || dept.BranchId != bid)
                    {
                        return ErrorResult("You don't have permission to view this designation.");
                    }
                }

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        designation.DesignationId,
                        designation.DepartmentId,
                        designation.DesignationCode,
                        designation.DesignationName,
                        designation.Grade,
                        designation.Description,
                        designation.IsActive
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
        // POST: /Designation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DesignationViewModel model)
        {
            try
            {
                // ✅ Permission check
                if (!CanModify)
                {
                    TempData["Error"] = "You don't have permission to create designations.";
                    return RedirectToAction("Index");
                }

                // ✅ Branch isolation — selected department apni branch ka hona chahiye
                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    int bid = CurrentBranchId.Value;
                    var dept = _context.Departments.Find(model.DepartmentId);
                    if (dept == null || dept.BranchId != bid)
                    {
                        TempData["Error"] = "Invalid department selection.";
                        return RedirectToAction("Index");
                    }
                }

                if (ModelState.IsValid)
                {
                    if (_context.Designations.Any(d => d.DesignationCode == model.DesignationCode))
                    {
                        TempData["Error"] = "Designation code already exists.";
                        return RedirectToAction("Index");
                    }

                    var designation = new Designation
                    {
                        DepartmentId = model.DepartmentId,
                        DesignationCode = model.DesignationCode,
                        DesignationName = model.DesignationName,
                        Grade = model.Grade,
                        Description = model.Description,
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    };

                    _context.Designations.Add(designation);
                    _context.SaveChanges();
                    TempData["Success"] = "Designation created successfully!";
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
        // POST: /Designation/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DesignationViewModel model)
        {
            try
            {
                // ✅ Permission check
                if (!CanModify)
                {
                    TempData["Error"] = "You don't have permission to edit designations.";
                    return RedirectToAction("Index");
                }

                if (ModelState.IsValid)
                {
                    var designation = _context.Designations.Find(model.DesignationId);
                    if (designation == null)
                    {
                        TempData["Error"] = "Designation not found.";
                        return RedirectToAction("Index");
                    }

                    // ✅ Branch isolation check
                    if (!IsSuperAdmin && CurrentBranchId.HasValue)
                    {
                        int bid = CurrentBranchId.Value;
                        var dept = _context.Departments.Find(designation.DepartmentId);
                        if (dept == null || dept.BranchId != bid)
                        {
                            TempData["Error"] = "You don't have permission to edit this designation.";
                            return RedirectToAction("Index");
                        }

                        // New department bhi apni branch ka hona chahiye
                        var newDept = _context.Departments.Find(model.DepartmentId);
                        if (newDept == null || newDept.BranchId != bid)
                        {
                            TempData["Error"] = "Invalid department selection.";
                            return RedirectToAction("Index");
                        }
                    }

                    // Duplicate code check (excluding current)
                    if (_context.Designations.Any(d => d.DesignationCode == model.DesignationCode
                        && d.DesignationId != model.DesignationId))
                    {
                        TempData["Error"] = "Designation code already exists.";
                        return RedirectToAction("Index");
                    }

                    designation.DepartmentId = model.DepartmentId;
                    designation.DesignationCode = model.DesignationCode;
                    designation.DesignationName = model.DesignationName;
                    designation.Grade = model.Grade;
                    designation.Description = model.Description;

                    _context.SaveChanges();
                    TempData["Success"] = "Designation updated successfully!";
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
        // POST: /Designation/ToggleStatus
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

                var designation = _context.Designations.Find(id);
                if (designation == null)
                {
                    return ErrorResult("Designation not found.");
                }

                // ✅ Branch isolation check
                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    int bid = CurrentBranchId.Value;
                    var dept = _context.Departments.Find(designation.DepartmentId);
                    if (dept == null || dept.BranchId != bid)
                    {
                        return ErrorResult("You don't have permission to change this designation's status.");
                    }
                }

                designation.IsActive = !designation.IsActive;
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
        // POST: /Designation/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Delete(int id)
        {
            try
            {
                if (!CanDeleteRecord)
                {
                    return ErrorResult("You don't have permission to delete designations.");
                }

                var designation = _context.Designations.Find(id);
                if (designation == null)
                {
                    return ErrorResult("Designation not found.");
                }

                // ✅ Branch isolation check
                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    int bid = CurrentBranchId.Value;
                    var dept = _context.Departments.Find(designation.DepartmentId);
                    if (dept == null || dept.BranchId != bid)
                    {
                        return ErrorResult("You don't have permission to delete this designation.");
                    }
                }

                // ✅ Check employees
                if (_context.Employees.Any(e => e.DesignationId == id))
                {
                    return ErrorResult("Cannot delete — employees exist in this designation.");
                }

                _context.Designations.Remove(designation);
                _context.SaveChanges();

                return SuccessResult("Designation deleted successfully.");
            }
            catch (Exception ex)
            {
                return ErrorResult("Error: " + ex.Message);
            }
        }
    }
}