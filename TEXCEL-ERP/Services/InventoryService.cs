using System;
using System.Collections.Generic;
using System.Linq;
using TEXCEL_ERP.Data.Repository;
using TEXCEL_ERP.Models.Entities;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service implementation for Inventory business logic.
    /// </summary>
    public class InventoryService : IInventoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InventoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Fabric> GetAllFabrics()
        {
            return _unitOfWork.Fabrics.GetAll()
                .OrderBy(f => f.FabricType)
                .ThenBy(f => f.Color)
                .ToList();
        }

        public Fabric GetFabricById(int id)
        {
            return _unitOfWork.Fabrics.GetById(id);
        }

        public void AddFabric(Fabric fabric)
        {
            fabric.CreatedDate = DateTime.Now;
            _unitOfWork.Fabrics.Add(fabric);
            _unitOfWork.Complete();
        }

        public void UpdateFabric(Fabric fabric)
        {
            _unitOfWork.Fabrics.Update(fabric);
            _unitOfWork.Complete();
        }

        public void DeleteFabric(int id)
        {
            var fabric = _unitOfWork.Fabrics.GetById(id);
            if (fabric != null)
            {
                fabric.IsActive = false;
                _unitOfWork.Fabrics.Update(fabric);
                _unitOfWork.Complete();
            }
        }

        public IEnumerable<FabricStock> GetAllStocks()
        {
            return _unitOfWork.FabricStocks.GetAll()
                .OrderBy(s => s.FabricId)
                .ToList();
        }

        public IEnumerable<FabricStock> GetStocksByBranch(int branchId)
        {
            return _unitOfWork.FabricStocks
                .Find(s => s.BranchId == branchId)
                .OrderBy(s => s.FabricId)
                .ToList();
        }

        public FabricStock GetStockById(int id)
        {
            return _unitOfWork.FabricStocks.GetById(id);
        }

        public void AddStock(FabricStock stock)
        {
            stock.LastUpdated = DateTime.Now;
            _unitOfWork.FabricStocks.Add(stock);
            _unitOfWork.Complete();
        }

        public void UpdateStock(FabricStock stock)
        {
            stock.LastUpdated = DateTime.Now;
            _unitOfWork.FabricStocks.Update(stock);
            _unitOfWork.Complete();
        }

        public decimal GetTotalStockQuantity(int branchId)
        {
            return _unitOfWork.FabricStocks
                .Find(s => s.BranchId == branchId)
                .Sum(s => s.Quantity);
        }

        public int GetTotalRolls(int branchId)
        {
            return _unitOfWork.FabricStocks
                .Find(s => s.BranchId == branchId)
                .Count();
        }

        public IEnumerable<GRN> GetAllGRNs()
        {
            return _unitOfWork.GRNs.GetAll()
                .OrderByDescending(g => g.ReceiptDate)
                .ToList();
        }

        public GRN GetGRNById(int id)
        {
            return _unitOfWork.GRNs.GetById(id);
        }

        public void AddGRN(GRN grn)
        {
            grn.CreatedDate = DateTime.Now;
            _unitOfWork.GRNs.Add(grn);
            _unitOfWork.Complete();
        }

        public void ApproveGRN(int grnId, int approvedById)
        {
            var grn = _unitOfWork.GRNs.GetById(grnId);
            if (grn != null)
            {
                grn.IsApproved = true;
                grn.ApprovedById = approvedById;
                grn.ApprovedDate = DateTime.Now;
                _unitOfWork.GRNs.Update(grn);
                _unitOfWork.Complete();
            }
        }
    }
}