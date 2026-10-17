using System;
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
    /// Company controller for Company settings.
    /// Sirf SuperAdmin access kar sakta hai.
    /// </summary>
    [RoleAuthorize("SuperAdmin")]       // ✅ Sirf SuperAdmin
    [BranchAuthorize]                    // ✅ Session + Branch check
    public class CompanyController : BaseController
    {
        private readonly ICompanyService _companyService;

        public CompanyController()
        {
            var context = new ApplicationDbContext();
            var uow = new UnitOfWork(context);
            _companyService = new CompanyService(uow);
        }

        // GET: /Company
        public ActionResult Index()
        {
            var company = _companyService.GetCompany();

            // Agar company exist nahi karti toh empty ViewModel bhejein
            if (company == null)
            {
                return View(new CompanyViewModel());
            }

            var model = new CompanyViewModel
            {
                CompanyId = company.CompanyId,
                CompanyName = company.CompanyName,
                LegalName = company.LegalName,
                NTN = company.NTN,
                GSTNumber = company.GSTNumber,
                Address = company.Address,
                City = company.City,
                Country = company.Country,
                Phone = company.Phone,
                Email = company.Email,
                Website = company.Website,
                LogoPath = company.LogoPath,
                IsActive = company.IsActive
            };

            return View(model);
        }

        // POST: /Company
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CompanyViewModel model)
        {
            // ✅ Double check (attribute ke saath)
            if (!CanModify)
            {
                TempData["Error"] = "You don't have permission to modify company settings.";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var company = _companyService.GetCompany();

                // Agar company exist nahi karti toh NAYI create karein
                if (company == null)
                {
                    company = new Company
                    {
                        CompanyName = model.CompanyName,
                        LegalName = model.LegalName,
                        NTN = model.NTN,
                        GSTNumber = model.GSTNumber,
                        Address = model.Address,
                        City = model.City,
                        Country = model.Country,
                        Phone = model.Phone,
                        Email = model.Email,
                        Website = model.Website,
                        IsActive = model.IsActive,
                        CreatedDate = DateTime.Now
                    };

                    var context = new ApplicationDbContext();
                    context.Companies.Add(company);
                    context.SaveChanges();

                    TempData["Success"] = "Company created successfully!";
                    return RedirectToAction("Index");
                }

                // Update existing company
                company.CompanyName = model.CompanyName;
                company.LegalName = model.LegalName;
                company.NTN = model.NTN;
                company.GSTNumber = model.GSTNumber;
                company.Address = model.Address;
                company.City = model.City;
                company.Country = model.Country;
                company.Phone = model.Phone;
                company.Email = model.Email;
                company.Website = model.Website;
                company.IsActive = model.IsActive;

                _companyService.UpdateCompany(company);
                TempData["Success"] = "Company updated successfully!";
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}