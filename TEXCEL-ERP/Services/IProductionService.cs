using System.Collections.Generic;
using TEXCEL_ERP.Models.Entities;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service interface for Production business logic.
    /// </summary>
    public interface IProductionService
    {
        IEnumerable<ProductionPlan> GetAllPlans();
        ProductionPlan GetPlanById(int id);
        void AddPlan(ProductionPlan plan);
        void UpdatePlan(ProductionPlan plan);

        IEnumerable<ProductionOrder> GetAllOrders();
        ProductionOrder GetOrderById(int id);
        void AddOrder(ProductionOrder order);
        void UpdateOrder(ProductionOrder order);

        IEnumerable<Cutting> GetAllCuttings();
        void AddCutting(Cutting cutting);

        IEnumerable<Sewing> GetAllSewings();
        void AddSewing(Sewing sewing);

        IEnumerable<Finishing> GetAllFinishings();
        void AddFinishing(Finishing finishing);

        decimal GetProductionEfficiency(int productionOrderId);
    }
}