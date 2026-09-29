using TEXCEL_ERP.Models.Entities;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service interface for Company business logic.
    /// </summary>
    public interface ICompanyService
    {
        Company GetCompany();
        void UpdateCompany(Company company);
    }
}