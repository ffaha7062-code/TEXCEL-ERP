using System;
using System.Collections.Generic;
using System.Linq;
using TEXCEL_ERP.Data;
using TEXCEL_ERP.Data.Repository;
using TEXCEL_ERP.Models.Entities;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service implementation for Branch business logic.
    /// </summary>
    public class BranchService : IBranchService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BranchService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Branch> GetAllBranches()
        {
            return _unitOfWork.Branches.Find(b => !b.IsDeleted)   // ✅ Sirf non-deleted
                .OrderBy(b => b.BranchName)
                .ToList();
        }

        public IEnumerable<Branch> GetActiveBranches()
        {
            return _unitOfWork.Branches.Find(b => b.IsActive && !b.IsDeleted)
                .OrderBy(b => b.BranchName)
                .ToList();
        }

        public Branch GetBranchById(int id)
        {
            var branch = _unitOfWork.Branches.GetById(id);
            if (branch != null && branch.IsDeleted) return null;   // ✅ Deleted = not found
            return branch;
        }

        public Branch GetBranchByCode(string code)
        {
            return _unitOfWork.Branches.Get(b => b.BranchCode == code);
        }

        public void AddBranch(Branch branch)
        {
            branch.CreatedDate = DateTime.Now;
            _unitOfWork.Branches.Add(branch);
            _unitOfWork.Complete();
        }

        public void UpdateBranch(Branch branch)
        {
            _unitOfWork.Branches.Update(branch);
            _unitOfWork.Complete();
        }

        public void DeleteBranch(int id)
        {
            var branch = _unitOfWork.Branches.GetById(id);
            if (branch != null)
            {
                branch.IsDeleted = true;                   // ✅ Soft delete
                _unitOfWork.Branches.Update(branch);
                _unitOfWork.Complete();
            }
        }

        public void ToggleStatus(int id)
        {
            var branch = _unitOfWork.Branches.GetById(id);
            if (branch != null)
            {
                branch.IsActive = !branch.IsActive;
                _unitOfWork.Branches.Update(branch);
                _unitOfWork.Complete();
            }
        }

        public bool IsBranchCodeExists(string code, int? excludeId = null)
        {
            if (excludeId.HasValue)
                return _unitOfWork.Branches.Any(b => b.BranchCode == code
                    && b.BranchId != excludeId.Value
                    && !b.IsDeleted);
            return _unitOfWork.Branches.Any(b => b.BranchCode == code && !b.IsDeleted);
        }

        public int GetTotalBranches()
        {
            return _unitOfWork.Branches.Count(b => !b.IsDeleted);   // ✅ Sirf non-deleted
        }
    }
}