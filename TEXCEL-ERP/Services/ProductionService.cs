using System;
using System.Collections.Generic;
using System.Linq;
using TEXCEL_ERP.Data.Repository;
using TEXCEL_ERP.Models.Entities;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service implementation for Production business logic.
    /// </summary>
    public class ProductionService : IProductionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ProductionPlan> GetAllPlans()
        {
            return _unitOfWork.ProductionPlans.GetAll()
                .OrderByDescending(p => p.PlanDate)
                .ToList();
        }

        public ProductionPlan GetPlanById(int id)
        {
            return _unitOfWork.ProductionPlans.GetById(id);
        }

        public void AddPlan(ProductionPlan plan)
        {
            plan.CreatedDate = DateTime.Now;
            plan.Status = "Pending";
            _unitOfWork.ProductionPlans.Add(plan);
            _unitOfWork.Complete();
        }

        public void UpdatePlan(ProductionPlan plan)
        {
            _unitOfWork.ProductionPlans.Update(plan);
            _unitOfWork.Complete();
        }

        public IEnumerable<ProductionOrder> GetAllOrders()
        {
            return _unitOfWork.ProductionOrders.GetAll()
                .OrderByDescending(o => o.CreatedDate)
                .ToList();
        }

        public ProductionOrder GetOrderById(int id)
        {
            return _unitOfWork.ProductionOrders.GetById(id);
        }

        public void AddOrder(ProductionOrder order)
        {
            order.CreatedDate = DateTime.Now;
            order.Status = "Pending";
            _unitOfWork.ProductionOrders.Add(order);
            _unitOfWork.Complete();
        }

        public void UpdateOrder(ProductionOrder order)
        {
            _unitOfWork.ProductionOrders.Update(order);
            _unitOfWork.Complete();
        }

        public IEnumerable<Cutting> GetAllCuttings()
        {
            return _unitOfWork.Cuttings.GetAll()
                .OrderByDescending(c => c.CuttingDate)
                .ToList();
        }

        public void AddCutting(Cutting cutting)
        {
            cutting.CreatedDate = DateTime.Now;
            _unitOfWork.Cuttings.Add(cutting);
            _unitOfWork.Complete();
        }

        public IEnumerable<Sewing> GetAllSewings()
        {
            return _unitOfWork.Sewings.GetAll()
                .OrderByDescending(s => s.SewingDate)
                .ToList();
        }

        public void AddSewing(Sewing sewing)
        {
            sewing.CreatedDate = DateTime.Now;
            _unitOfWork.Sewings.Add(sewing);
            _unitOfWork.Complete();
        }

        public IEnumerable<Finishing> GetAllFinishings()
        {
            return _unitOfWork.Finishings.GetAll()
                .OrderByDescending(f => f.FinishingDate)
                .ToList();
        }

        public void AddFinishing(Finishing finishing)
        {
            finishing.CreatedDate = DateTime.Now;
            _unitOfWork.Finishings.Add(finishing);
            _unitOfWork.Complete();
        }

        public decimal GetProductionEfficiency(int productionOrderId)
        {
            var order = _unitOfWork.ProductionOrders.GetById(productionOrderId);
            if (order == null || order.PlannedQuantity == 0) return 0;

            return (decimal)order.ProducedQuantity / order.PlannedQuantity * 100;
        }
    }
}