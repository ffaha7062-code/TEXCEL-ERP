using System;
using System.Collections.Generic;
using System.Linq;
using TEXCEL_ERP.Data.Repository;
using TEXCEL_ERP.Models.Entities;
using TEXCEL_ERP.Models.Enums;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service implementation for Sales business logic.
    /// </summary>
    public class SalesService : ISalesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SalesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _unitOfWork.Customers.GetAll()
                .OrderBy(c => c.CustomerName)
                .ToList();
        }

        public Customer GetCustomerById(int id)
        {
            return _unitOfWork.Customers.GetById(id);
        }

        public void AddCustomer(Customer customer)
        {
            customer.CreatedDate = DateTime.Now;
            _unitOfWork.Customers.Add(customer);
            _unitOfWork.Complete();
        }

        public void UpdateCustomer(Customer customer)
        {
            _unitOfWork.Customers.Update(customer);
            _unitOfWork.Complete();
        }

        public IEnumerable<SalesOrder> GetAllSalesOrders()
        {
            return _unitOfWork.SalesOrders.GetAll()
                .OrderByDescending(o => o.OrderDate)
                .ToList();
        }

        public SalesOrder GetSalesOrderById(int id)
        {
            return _unitOfWork.SalesOrders.GetById(id);
        }

        public void AddSalesOrder(SalesOrder order)
        {
            order.CreatedDate = DateTime.Now;
            order.Status = OrderStatus.Draft;
            _unitOfWork.SalesOrders.Add(order);
            _unitOfWork.Complete();
        }

        public void UpdateSalesOrder(SalesOrder order)
        {
            _unitOfWork.SalesOrders.Update(order);
            _unitOfWork.Complete();
        }

        public void ApproveSalesOrder(int orderId, int approvedById)
        {
            var order = _unitOfWork.SalesOrders.GetById(orderId);
            if (order != null)
            {
                order.IsApproved = true;
                order.ApprovedById = approvedById;
                order.ApprovedDate = DateTime.Now;
                order.Status = OrderStatus.Approved;
                _unitOfWork.SalesOrders.Update(order);
                _unitOfWork.Complete();
            }
        }

        public IEnumerable<Invoice> GetAllInvoices()
        {
            return _unitOfWork.Invoices.GetAll()
                .OrderByDescending(i => i.InvoiceDate)
                .ToList();
        }

        public Invoice GetInvoiceById(int id)
        {
            return _unitOfWork.Invoices.GetById(id);
        }

        public void AddInvoice(Invoice invoice)
        {
            invoice.CreatedDate = DateTime.Now;
            invoice.PaymentStatus = PaymentStatus.Pending;
            _unitOfWork.Invoices.Add(invoice);
            _unitOfWork.Complete();
        }

        public void RecordPayment(int invoiceId, decimal amount)
        {
            var invoice = _unitOfWork.Invoices.GetById(invoiceId);
            if (invoice != null)
            {
                invoice.PaidAmount += amount;
                if (invoice.PaidAmount >= invoice.TotalAmount)
                {
                    invoice.PaymentStatus = PaymentStatus.Paid;
                    invoice.PaymentDate = DateTime.Now;
                }
                else
                {
                    invoice.PaymentStatus = PaymentStatus.Partial;
                }
                _unitOfWork.Invoices.Update(invoice);
                _unitOfWork.Complete();
            }
        }
    }
}