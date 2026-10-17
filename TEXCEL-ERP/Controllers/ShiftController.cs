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
    /// Shift controller.
    /// SuperAdmin = all branches, BranchAdmin/Manager = own branch.
    /// Manager = view only, Employee = no access.
    /// </summary>
    [RoleAuthorize("SuperAdmin", "BranchAdmin", "Manager")]
    [BranchAuthorize]
    public class ShiftController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public ShiftController()
        {
            _context = new ApplicationDbContext();
        }

        // =============================================
        // INDEX — Shift List
        // =============================================
        // GET: /Shift
        public ActionResult Index(string search, bool? isActive, string shiftType)
        {
            var query = _context.Shifts.AsQueryable();

            // ✅ Branch isolation
            if (!IsSuperAdmin && CurrentBranchId.HasValue)
            {
                int bid = CurrentBranchId.Value;
                query = query.Where(s => s.BranchId == bid);
            }

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s => s.ShiftName.Contains(search));
            }

            // Status
            if (isActive.HasValue)
                query = query.Where(s => s.IsActive == isActive.Value);

            // Shift type
            if (!string.IsNullOrWhiteSpace(shiftType))
                query = query.Where(s => s.ShiftType == shiftType);

            var shifts = query.OrderBy(s => s.ShiftName).ToList();

            ViewBag.Search = search;
            ViewBag.IsActive = isActive;
            ViewBag.ShiftType = shiftType;
            ViewBag.CanModify = CanModify;
            ViewBag.CanDelete = CanDeleteRecord;

            // ✅ Branches dropdown
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

            return View(shifts);
        }

        // =============================================
        // GET BY ID — For Edit Modal (AJAX)
        // =============================================
        [HttpGet]
        public JsonResult GetById(int id)
        {
            try
            {
                var shift = _context.Shifts.Find(id);
                if (shift == null)
                {
                    return ErrorResult("Shift not found.");
                }

                // ✅ Branch isolation check
                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    if (shift.BranchId != CurrentBranchId.Value)
                    {
                        return ErrorResult("You don't have permission to view this shift.");
                    }
                }

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        shift.ShiftId,
                        shift.BranchId,
                        shift.ShiftName,
                        StartTime = shift.StartTime.ToString(@"hh\:mm"),
                        EndTime = shift.EndTime.ToString(@"hh\:mm"),
                        shift.BreakDuration,
                        shift.ShiftType,
                        shift.OvertimeRate,
                        shift.LateAllowance,
                        shift.IsActive
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ErrorResult("Error: " + ex.Message);
            }
        }

        // =============================================
        // CREATE — POST
        // =============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ShiftViewModel model)
        {
            try
            {
                if (!CanModify)
                {
                    TempData["Error"] = "You don't have permission to create shifts.";
                    return RedirectToAction("Index");
                }

                // ✅ BranchAdmin/Manager = own branch only
                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    model.BranchId = CurrentBranchId.Value;
                }

                if (ModelState.IsValid)
                {
                    // Duplicate check within same branch
                    if (_context.Shifts.Any(s => s.ShiftName == model.ShiftName
                        && s.BranchId == model.BranchId))
                    {
                        TempData["Error"] = "Shift name already exists in this branch.";
                        return RedirectToAction("Index");
                    }

                    var shift = new Shift
                    {
                        BranchId = model.BranchId,
                        ShiftName = model.ShiftName,
                        StartTime = model.StartTime,
                        EndTime = model.EndTime,
                        BreakDuration = model.BreakDuration,
                        ShiftType = model.ShiftType,
                        OvertimeRate = model.OvertimeRate,
                        LateAllowance = model.LateAllowance,
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    };

                    _context.Shifts.Add(shift);
                    _context.SaveChanges();
                    TempData["Success"] = "Shift created successfully!";
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
        // EDIT — POST
        // =============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ShiftViewModel model)
        {
            try
            {
                if (!CanModify)
                {
                    TempData["Error"] = "You don't have permission to edit shifts.";
                    return RedirectToAction("Index");
                }

                // ✅ BranchAdmin/Manager = own branch only
                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    model.BranchId = CurrentBranchId.Value;
                }

                if (ModelState.IsValid)
                {
                    var shift = _context.Shifts.Find(model.ShiftId);
                    if (shift == null)
                    {
                        TempData["Error"] = "Shift not found.";
                        return RedirectToAction("Index");
                    }

                    // ✅ Branch isolation check
                    if (!IsSuperAdmin && CurrentBranchId.HasValue)
                    {
                        if (shift.BranchId != CurrentBranchId.Value)
                        {
                            TempData["Error"] = "You don't have permission to edit this shift.";
                            return RedirectToAction("Index");
                        }
                    }

                    shift.BranchId = model.BranchId;
                    shift.ShiftName = model.ShiftName;
                    shift.StartTime = model.StartTime;
                    shift.EndTime = model.EndTime;
                    shift.BreakDuration = model.BreakDuration;
                    shift.ShiftType = model.ShiftType;
                    shift.OvertimeRate = model.OvertimeRate;
                    shift.LateAllowance = model.LateAllowance;

                    _context.SaveChanges();
                    TempData["Success"] = "Shift updated successfully!";
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
        // TOGGLE STATUS — AJAX
        // =============================================
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

                var shift = _context.Shifts.Find(id);
                if (shift == null)
                {
                    return ErrorResult("Shift not found.");
                }

                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    if (shift.BranchId != CurrentBranchId.Value)
                    {
                        return ErrorResult("You don't have permission to change this shift's status.");
                    }
                }

                shift.IsActive = !shift.IsActive;
                _context.SaveChanges();

                return SuccessResult("Status updated successfully.");
            }
            catch (Exception ex)
            {
                return ErrorResult("Error: " + ex.Message);
            }
        }

        // =============================================
        // DELETE — AJAX
        // =============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Delete(int id)
        {
            try
            {
                if (!CanDeleteRecord)
                {
                    return ErrorResult("You don't have permission to delete shifts.");
                }

                var shift = _context.Shifts.Find(id);
                if (shift == null)
                {
                    return ErrorResult("Shift not found.");
                }

                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    if (shift.BranchId != CurrentBranchId.Value)
                    {
                        return ErrorResult("You don't have permission to delete this shift.");
                    }
                }

                // Check employees
                if (_context.Employees.Any(e => e.ShiftId == id))
                {
                    return ErrorResult("Cannot delete — employees exist in this shift.");
                }

                _context.Shifts.Remove(shift);
                _context.SaveChanges();

                return SuccessResult("Shift deleted successfully.");
            }
            catch (Exception ex)
            {
                return ErrorResult("Error: " + ex.Message);
            }
        }
    }
}