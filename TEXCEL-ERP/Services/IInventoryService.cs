using System.Collections.Generic;
using TEXCEL_ERP.Models.Entities;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service interface for Inventory business logic.
    /// </summary>
    public interface IInventoryService
    {
        IEnumerable<Fabric> GetAllFabrics();
        Fabric GetFabricById(int id);
        void AddFabric(Fabric fabric);
        void UpdateFabric(Fabric fabric);
        void DeleteFabric(int id);

        IEnumerable<FabricStock> GetAllStocks();
        IEnumerable<FabricStock> GetStocksByBranch(int branchId);
        FabricStock GetStockById(int id);
        void AddStock(FabricStock stock);
        void UpdateStock(FabricStock stock);
        decimal GetTotalStockQuantity(int branchId);
        int GetTotalRolls(int branchId);

        IEnumerable<GRN> GetAllGRNs();
        GRN GetGRNById(int id);
        void AddGRN(GRN grn);
        void ApproveGRN(int grnId, int approvedById);
    }
}