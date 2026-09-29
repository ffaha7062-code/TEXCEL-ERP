using System.Collections.Generic;
using TEXCEL_ERP.Models.Entities;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service interface for Branch business logic.
    /// </summary>
    public interface IBranchService
    {
        IEnumerable<Branch> GetAllBranches();
        IEnumerable<Branch> GetActiveBranches();
        Branch GetBranchById(int id);
        Branch GetBranchByCode(string code);
        void AddBranch(Branch branch);
        void UpdateBranch(Branch branch);
        void DeleteBranch(int id);
        void ToggleStatus(int id);
        bool IsBranchCodeExists(string code, int? excludeId = null);
        int GetTotalBranches();
    }
}