using System;
using System.Collections.Generic;
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
    [SessionTimeout]
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: /Department
        public ActionResult Index(string search, bool? isActive, int? branchId)
        {
            var query = _context.Departments.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(d =>
                    d.DepartmentName.Contains(search) ||
                    d.DepartmentCode.Contains(search));
            }

            if (isActive.HasValue)
                query = query.Where(d => d.IsActive == isActive.Value);

            if (branchId.HasValue)
                query = query.Where(d => d.BranchId == branchId.Value);

            var departments = query.OrderBy(d => d.DepartmentName).ToList();

            ViewBag.Search = search;
            ViewBag.IsActive = isActive;
            ViewBag.BranchId = branchId;
            ViewBag.Branches = _context.Branches.Where(b => b.IsActive).ToList();

            return View(departments);
        }

        // POST: /Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DepartmentViewModel model)
        {
            try
            {
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

        // POST: /Department/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DepartmentViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dept = _context.Departments.Find(model.DepartmentId);
                    if (dept == null)
                    {
                        TempData["Error"] = "Department not found.";
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

        // POST: /Department/ToggleStatus
        [HttpPost]
        public JsonResult ToggleStatus(int id)
        {
            try
            {
                var dept = _context.Departments.Find(id);
                if (dept != null)
                {
                    dept.IsActive = !dept.IsActive;
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Status updated." });
                }
                return Json(new { success = false, message = "Department not found." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: /Department/Delete
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var dept = _context.Departments.Find(id);
                if (dept != null)
                {
                    // Check karein ke employees hain ya nahi
                    if (_context.Employees.Any(e => e.DepartmentId == id))
                    {
                        return Json(new { success = false, message = "Cannot delete — employees exist in this department." });
                    }

                    _context.Departments.Remove(dept);
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Department deleted." });
                }
                return Json(new { success = false, message = "Department not found." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}