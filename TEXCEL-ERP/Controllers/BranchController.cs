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
    public class BranchController : Controller
    {
        private readonly IBranchService _branchService;

        public BranchController()
        {
            var context = new ApplicationDbContext();
            var uow = new UnitOfWork(context);
            _branchService = new BranchService(uow);
        }

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

            return View(branches);
        }

        // POST: /Branch/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BranchViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_branchService.IsBranchCodeExists(model.BranchCode))
                    {
                        TempData["Error"] = "Branch code already exists.";
                        return RedirectToAction("Index");
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
                    TempData["Success"] = "Branch created successfully!";
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

        // POST: /Branch/ToggleStatus/5
        [HttpPost]
        public JsonResult ToggleStatus(int id)
        {
            try
            {
                _branchService.ToggleStatus(id);
                return Json(new { success = true, message = "Status updated." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: /Branch/Delete/5
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                _branchService.DeleteBranch(id);
                return Json(new { success = true, message = "Branch deleted." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}