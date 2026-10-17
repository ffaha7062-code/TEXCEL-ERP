using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using TEXCEL_ERP.Data;
using TEXCEL_ERP.Filters;
using TEXCEL_ERP.Models.Entities;
using TEXCEL_ERP.Models.Enums;
using TEXCEL_ERP.Models.ViewModels;

namespace TEXCEL_ERP.Controllers
{
    /// <summary>
    /// Employee controller.
    /// SuperAdmin = all branches (CRUD)
    /// BranchAdmin = own branch (CRUD)
    /// Manager = own branch (View only)
    /// Employee = auto-redirect to own profile
    /// </summary>
    [RoleAuthorize("SuperAdmin", "BranchAdmin", "Manager", "Employee")]
    [BranchAuthorize]
    public class EmployeeController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController()
        {
            _context = new ApplicationDbContext();
        }

        // =============================================
        // INDEX — Employee List
        // =============================================
        public ActionResult Index(
            string search,
            int? branchId,
            int? departmentId,
            int? designationId,
            EmployeeStatus? status)
        {
            // ✅ Employee role = auto-redirect to own profile
            if (IsEmployee && CurrentEmployeeId.HasValue)
            {
                return RedirectToAction("Details", new { id = CurrentEmployeeId.Value });
            }

            var query = _context.Employees
                .Include(e => e.Branch)
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .Include(e => e.Shift)
                .AsQueryable();

            // ✅ Branch isolation: SuperAdmin = sab, baaki = own branch
            if (!IsSuperAdmin && CurrentBranchId.HasValue)
            {
                int bid = CurrentBranchId.Value;
                query = query.Where(e => e.BranchId == bid);
            }
            else if (branchId.HasValue)
            {
                query = query.Where(e => e.BranchId == branchId.Value);
            }

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(e =>
                    e.EmployeeCode.Contains(search) ||
                    e.FirstName.Contains(search) ||
                    (e.LastName != null && e.LastName.Contains(search)) ||
                    e.CNIC.Contains(search) ||
                    e.MobileNumber.Contains(search));
            }

            // Other filters
            if (departmentId.HasValue)
                query = query.Where(e => e.DepartmentId == departmentId.Value);

            if (designationId.HasValue)
                query = query.Where(e => e.DesignationId == designationId.Value);

            if (status.HasValue)
                query = query.Where(e => e.Status == status.Value);

            var employees = query.OrderBy(e => e.FirstName).ToList();

            ViewBag.Search = search;
            ViewBag.BranchId = branchId;
            ViewBag.DepartmentId = departmentId;
            ViewBag.DesignationId = designationId;
            ViewBag.Status = status;
            ViewBag.CanModify = CanModify;
            ViewBag.CanDelete = CanDeleteRecord;

            LoadDropdowns();

            return View(employees);
        }

        // =============================================
        // DETAILS — Single Employee View (My Profile)
        // =============================================
        public ActionResult Details(int id)
        {
            var employee = _context.Employees
                .Include(e => e.Branch)
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .Include(e => e.Shift)
                .Include(e => e.ReportingManager)
                .FirstOrDefault(e => e.EmployeeId == id);

            if (employee == null)
            {
                TempData["Error"] = "Employee not found.";
                return RedirectToAction("Index");
            }

            // ✅ Branch isolation check
            if (!IsSuperAdmin && CurrentBranchId.HasValue)
            {
                if (employee.BranchId != CurrentBranchId.Value)
                {
                    TempData["Error"] = "You don't have permission to view this employee.";
                    return RedirectToAction("Index");
                }
            }

            // ✅ Employee role: sirf apna profile dekh sakta hai
            if (IsEmployee)
            {
                if (!CurrentEmployeeId.HasValue || CurrentEmployeeId.Value != id)
                {
                    TempData["Error"] = "You can only view your own profile.";
                    return RedirectToAction("Index");
                }
            }

            ViewBag.CanModify = CanModify;
            return View(employee);
        }

        // =============================================
        // GET BY ID — For Edit Modal (AJAX)
        // =============================================
        [HttpGet]
        public JsonResult GetById(int id)
        {
            try
            {
                var employee = _context.Employees.Find(id);
                if (employee == null)
                {
                    return ErrorResult("Employee not found.");
                }

                // ✅ Branch isolation check
                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    if (employee.BranchId != CurrentBranchId.Value)
                    {
                        return ErrorResult("You don't have permission to view this employee.");
                    }
                }

                // ✅ Employee role restriction
                if (IsEmployee)
                {
                    return ErrorResult("You don't have permission to edit employee records.");
                }

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        employee.EmployeeId,
                        employee.EmployeeCode,
                        employee.BranchId,
                        employee.DepartmentId,
                        employee.DesignationId,
                        employee.ShiftId,
                        employee.FirstName,
                        employee.LastName,
                        employee.CNIC,
                        DateOfBirth = employee.DateOfBirth.ToString("yyyy-MM-dd"),
                        Gender = (int)employee.Gender,
                        employee.MobileNumber,
                        employee.Email,
                        employee.CurrentAddress,
                        JoiningDate = employee.JoiningDate.ToString("yyyy-MM-dd"),
                        Status = (int)employee.Status,
                        SkillType = employee.SkillType.HasValue ? (int?)employee.SkillType.Value : null,
                        employee.MachineAssigned,
                        employee.BasicSalary,
                        employee.PieceRate,
                        employee.BankName,
                        employee.BankAccountNumber
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ErrorResult("Error: " + ex.Message);
            }
        }

        // =============================================
        // CREATE — AJAX
        // =============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateAjax(EmployeeViewModel model)
        {
            try
            {
                // ✅ Permission check
                if (!CanModify)
                {
                    return ErrorResult("You don't have permission to create employees.");
                }

                // ✅ BranchAdmin = own branch only
                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    model.BranchId = CurrentBranchId.Value;
                }

                // Validation
                if (string.IsNullOrWhiteSpace(model.FirstName))
                    return ErrorResult("First Name is required.");

                if (string.IsNullOrWhiteSpace(model.CNIC))
                    return ErrorResult("CNIC is required.");

                if (string.IsNullOrWhiteSpace(model.MobileNumber))
                    return ErrorResult("Mobile Number is required.");

                if (model.BranchId == 0)
                    return ErrorResult("Please select Branch.");

                if (model.DepartmentId == 0)
                    return ErrorResult("Please select Department.");

                if (model.DesignationId == 0)
                    return ErrorResult("Please select Designation.");

                if (model.BasicSalary <= 0)
                    return ErrorResult("Basic Salary must be greater than 0.");

                if (_context.Employees.Any(e => e.CNIC == model.CNIC))
                    return ErrorResult("CNIC already exists.");

                var company = _context.Companies.FirstOrDefault();
                if (company == null)
                    return ErrorResult("No company found. Please create company first.");

                // ✅ Branch-wise Employee Code
                var employeeCode = GenerateEmployeeCode(model.BranchId);

                var employee = new Employee
                {
                    EmployeeCode = employeeCode,
                    CompanyId = company.CompanyId,
                    BranchId = model.BranchId,
                    DepartmentId = model.DepartmentId,
                    DesignationId = model.DesignationId,
                    ShiftId = model.ShiftId,

                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    CNIC = model.CNIC,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,

                    MobileNumber = model.MobileNumber,
                    Email = model.Email,
                    CurrentAddress = model.CurrentAddress,

                    JoiningDate = model.JoiningDate,
                    Status = model.Status,
                    SkillType = model.SkillType,
                    MachineAssigned = model.MachineAssigned,

                    BasicSalary = model.BasicSalary,
                    PieceRate = model.PieceRate,
                    BankName = model.BankName,
                    BankAccountNumber = model.BankAccountNumber,

                    IsActive = true,
                    CreatedDate = DateTime.Now
                };

                _context.Employees.Add(employee);
                _context.SaveChanges();

                return SuccessResult("Employee created successfully! Code: " + employeeCode);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                var errors = new System.Text.StringBuilder();
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errors.AppendLine($"❌ {validationError.PropertyName}: {validationError.ErrorMessage}");
                    }
                }
                return Json(new { success = false, message = "Validation: " + errors.ToString() });
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null)
                    msg += " | Inner: " + ex.InnerException.Message;
                return Json(new { success = false, message = "Error: " + msg });
            }
        }

        // =============================================
        // EDIT — AJAX
        // =============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditAjax(EmployeeViewModel model)
        {
            try
            {
                if (!CanModify)
                {
                    return ErrorResult("You don't have permission to edit employees.");
                }

                var employee = _context.Employees.Find(model.EmployeeId);
                if (employee == null)
                    return ErrorResult("Employee not found.");

                // ✅ Branch isolation
                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    if (employee.BranchId != CurrentBranchId.Value)
                    {
                        return ErrorResult("You don't have permission to edit this employee.");
                    }
                    model.BranchId = CurrentBranchId.Value;
                }

                if (string.IsNullOrWhiteSpace(model.FirstName))
                    return ErrorResult("First Name is required.");

                if (string.IsNullOrWhiteSpace(model.CNIC))
                    return ErrorResult("CNIC is required.");

                if (_context.Employees.Any(e => e.CNIC == model.CNIC && e.EmployeeId != model.EmployeeId))
                    return ErrorResult("CNIC already exists for another employee.");

                // ✅ Branch change detect karein
                int oldBranchId = employee.BranchId;
                bool branchChanged = (oldBranchId != model.BranchId);

                // ✅ Fields update karein
                employee.BranchId = model.BranchId;
                employee.DepartmentId = model.DepartmentId;
                employee.DesignationId = model.DesignationId;
                employee.ShiftId = model.ShiftId;

                employee.FirstName = model.FirstName;
                employee.LastName = model.LastName;
                employee.CNIC = model.CNIC;
                employee.DateOfBirth = model.DateOfBirth;
                employee.Gender = model.Gender;

                employee.MobileNumber = model.MobileNumber;
                employee.Email = model.Email;
                employee.CurrentAddress = model.CurrentAddress;

                employee.JoiningDate = model.JoiningDate;
                employee.Status = model.Status;
                employee.SkillType = model.SkillType;
                employee.MachineAssigned = model.MachineAssigned;

                employee.BasicSalary = model.BasicSalary;
                employee.PieceRate = model.PieceRate;
                employee.BankName = model.BankName;
                employee.BankAccountNumber = model.BankAccountNumber;

                // ✅ Agar branch change hua toh naya code generate karein
                if (branchChanged)
                {
                    employee.EmployeeCode = GenerateEmployeeCode(model.BranchId);
                }

                employee.ModifiedDate = DateTime.Now;
                _context.SaveChanges();

                if (branchChanged)
                {
                    return SuccessResult($"Employee updated successfully! New code: {employee.EmployeeCode}");
                }
                return SuccessResult("Employee updated successfully!");
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null)
                    msg += " | Inner: " + ex.InnerException.Message;
                return ErrorResult("Error: " + msg);
            }
        }

        // =============================================
        // TOGGLE STATUS — AJAX
        // =============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ToggleStatusAjax(int id)
        {
            try
            {
                if (!CanModify)
                {
                    return ErrorResult("You don't have permission to change status.");
                }

                var employee = _context.Employees.Find(id);
                if (employee == null)
                    return ErrorResult("Employee not found.");

                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    if (employee.BranchId != CurrentBranchId.Value)
                    {
                        return ErrorResult("You don't have permission to change this employee's status.");
                    }
                }

                employee.IsActive = !employee.IsActive;
                employee.ModifiedDate = DateTime.Now;
                _context.SaveChanges();

                return SuccessResult("Status updated successfully.");
            }
            catch (Exception ex)
            {
                return ErrorResult("Error: " + ex.Message);
            }
        }

        // =============================================
        // DELETE (Deactivate) — AJAX
        // =============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteAjax(int id)
        {
            try
            {
                // ✅ Sirf SuperAdmin delete kar sakta hai
                if (!IsSuperAdmin)
                {
                    return ErrorResult("You don't have permission to delete employees.");
                }

                var employee = _context.Employees.Find(id);
                if (employee == null)
                    return ErrorResult("Employee not found.");

                employee.IsActive = false;
                employee.Status = EmployeeStatus.Inactive;
                employee.ModifiedDate = DateTime.Now;
                _context.SaveChanges();

                return SuccessResult("Employee deactivated successfully.");
            }
            catch (Exception ex)
            {
                return ErrorResult("Error: " + ex.Message);
            }
        }

        // =============================================
        // HELPERS
        // =============================================
        private string GenerateEmployeeCode(int branchId)
        {
            // ✅ Branch-wise: EMP-LHR-2026-0001
            var branch = _context.Branches.Find(branchId);
            string branchCode = branch != null ? branch.BranchCode : "GEN";

            int currentYear = DateTime.Now.Year;

            // Last employee of this branch in current year
            var lastEmployee = _context.Employees
                .Where(e => e.BranchId == branchId
                    && e.EmployeeCode.StartsWith("EMP-" + branchCode + "-" + currentYear))
                .OrderByDescending(e => e.EmployeeId)
                .FirstOrDefault();

            int nextNumber = 1;
            if (lastEmployee != null && !string.IsNullOrEmpty(lastEmployee.EmployeeCode))
            {
                var parts = lastEmployee.EmployeeCode.Split('-');
                if (parts.Length > 0 && int.TryParse(parts[parts.Length - 1], out int lastNum))
                {
                    nextNumber = lastNum + 1;
                }
            }

            return $"EMP-{branchCode}-{currentYear}-{nextNumber:D4}";
        }

        private void LoadDropdowns()
        {
            // ✅ Branch isolation for dropdowns
            if (IsSuperAdmin)
            {
                ViewBag.Branches = _context.Branches.Where(b => b.IsActive).OrderBy(b => b.BranchName).ToList();
                ViewBag.Departments = _context.Departments.Where(d => d.IsActive).OrderBy(d => d.DepartmentName).ToList();
                ViewBag.Designations = _context.Designations.Where(d => d.IsActive).OrderBy(d => d.DesignationName).ToList();
                ViewBag.Shifts = _context.Shifts.Where(s => s.IsActive).OrderBy(s => s.ShiftName).ToList();
            }
            else if (CurrentBranchId.HasValue)
            {
                int bid = CurrentBranchId.Value;

                ViewBag.Branches = _context.Branches
                    .Where(b => b.IsActive && b.BranchId == bid)
                    .OrderBy(b => b.BranchName).ToList();

                var deptIds = _context.Departments.Where(d => d.BranchId == bid).Select(d => d.DepartmentId).ToList();

                ViewBag.Departments = _context.Departments
                    .Where(d => d.IsActive && d.BranchId == bid)
                    .OrderBy(d => d.DepartmentName).ToList();

                ViewBag.Designations = _context.Designations
                    .Where(d => d.IsActive && deptIds.Contains(d.DepartmentId))
                    .OrderBy(d => d.DesignationName).ToList();

                ViewBag.Shifts = _context.Shifts
                    .Where(s => s.IsActive && s.BranchId == bid)
                    .OrderBy(s => s.ShiftName).ToList();
            }
            else
            {
                ViewBag.Branches = new List<Branch>();
                ViewBag.Departments = new List<Department>();
                ViewBag.Designations = new List<Designation>();
                ViewBag.Shifts = new List<Shift>();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _context.Dispose();
            base.Dispose(disposing);
        }
    }
}