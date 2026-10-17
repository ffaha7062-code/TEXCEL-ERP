using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TEXCEL_ERP.Data;
using TEXCEL_ERP.Data.Repository;
using TEXCEL_ERP.Filters;
using TEXCEL_ERP.Models.Entities;
using TEXCEL_ERP.Services;

namespace TEXCEL_ERP.Controllers
{
    [RoleAuthorize("SuperAdmin", "BranchAdmin", "Manager")]
    [BranchAuthorize]
    public class HolidayController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IHolidayService _holidayService;

        public HolidayController()
        {
            _context = new ApplicationDbContext();
            var uow = new UnitOfWork(_context);
            _holidayService = new HolidayService(uow);
        }

        public ActionResult Index()
        {
            int? branchId = IsSuperAdmin ? null : CurrentBranchId;
            var holidays = _holidayService.GetHolidaysByBranch(branchId).ToList();

            ViewBag.CanModify = CanModify;
            ViewBag.CanDelete = CanDeleteRecord;

            if (IsSuperAdmin)
            {
                ViewBag.Branches = _context.Branches.Where(b => b.IsActive).ToList();
            }
            else if (CurrentBranchId.HasValue)
            {
                int bid = CurrentBranchId.Value;
                ViewBag.Branches = _context.Branches.Where(b => b.IsActive && b.BranchId == bid).ToList();
            }

            return View(holidays);
        }

        [HttpGet]
        public JsonResult GetById(int id)
        {
            try
            {
                var holiday = _context.Holidays.Find(id);
                if (holiday == null) return ErrorResult("Holiday not found.");

                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    if (holiday.BranchId != CurrentBranchId.Value)
                        return ErrorResult("Permission denied.");
                }

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        holiday.HolidayId,
                        holiday.BranchId,
                        holiday.HolidayName,
                        HolidayDate = holiday.HolidayDate.ToString("yyyy-MM-dd"),
                        holiday.Description,
                        holiday.IsPaid,
                        holiday.IsActive
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ErrorResult("Error: " + ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(Holiday model)
        {
            try
            {
                if (!CanModify)
                    return ErrorResult("Permission denied.");

                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                    model.BranchId = CurrentBranchId.Value;

                if (string.IsNullOrWhiteSpace(model.HolidayName))
                    return ErrorResult("Holiday name is required.");

                var holiday = new Holiday
                {
                    BranchId = model.BranchId,
                    HolidayName = model.HolidayName,
                    HolidayDate = model.HolidayDate.Date,
                    Description = model.Description,
                    IsPaid = model.IsPaid,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };

                _holidayService.AddHoliday(holiday);
                return SuccessResult("Holiday created successfully!");
            }
            catch (Exception ex)
            {
                return ErrorResult("Error: " + ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(Holiday model)
        {
            try
            {
                if (!CanModify)
                    return ErrorResult("Permission denied.");

                var holiday = _context.Holidays.Find(model.HolidayId);
                if (holiday == null) return ErrorResult("Holiday not found.");

                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    if (holiday.BranchId != CurrentBranchId.Value)
                        return ErrorResult("Permission denied.");
                    model.BranchId = CurrentBranchId.Value;
                }

                holiday.BranchId = model.BranchId;
                holiday.HolidayName = model.HolidayName;
                holiday.HolidayDate = model.HolidayDate.Date;
                holiday.Description = model.Description;
                holiday.IsPaid = model.IsPaid;

                _holidayService.UpdateHoliday(holiday);
                return SuccessResult("Holiday updated successfully!");
            }
            catch (Exception ex)
            {
                return ErrorResult("Error: " + ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ToggleStatus(int id)
        {
            try
            {
                if (!CanModify)
                    return ErrorResult("Permission denied.");

                var holiday = _context.Holidays.Find(id);
                if (holiday == null) return ErrorResult("Holiday not found.");

                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    if (holiday.BranchId != CurrentBranchId.Value)
                        return ErrorResult("Permission denied.");
                }

                _holidayService.ToggleStatus(id);
                return SuccessResult("Status updated successfully.");
            }
            catch (Exception ex)
            {
                return ErrorResult("Error: " + ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Delete(int id)
        {
            try
            {
                if (!CanDeleteRecord)
                    return ErrorResult("Permission denied.");

                var holiday = _context.Holidays.Find(id);
                if (holiday == null) return ErrorResult("Holiday not found.");

                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    if (holiday.BranchId != CurrentBranchId.Value)
                        return ErrorResult("Permission denied.");
                }

                _holidayService.DeleteHoliday(id);
                return SuccessResult("Holiday deleted successfully.");
            }
            catch (Exception ex)
            {
                return ErrorResult("Error: " + ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _context.Dispose();
            base.Dispose(disposing);
        }
    }
}