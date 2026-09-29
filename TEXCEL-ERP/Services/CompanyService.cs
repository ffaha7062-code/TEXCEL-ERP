using System;
using System.Linq;
using TEXCEL_ERP.Data.Repository;
using TEXCEL_ERP.Models.Entities;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service implementation for Company business logic.
    /// </summary>
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Company GetCompany()
        {
            return _unitOfWork.Companies.GetAll().FirstOrDefault();
        }

        public void UpdateCompany(Company company)
        {
            company.ModifiedDate = DateTime.Now;
            _unitOfWork.Companies.Update(company);
            _unitOfWork.Complete();
        }
    }
}