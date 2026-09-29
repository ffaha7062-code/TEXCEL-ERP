using System.Collections.Generic;
using TEXCEL_ERP.Models.Entities;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service interface for Sales business logic.
    /// </summary>
    public interface ISalesService
    {
        IEnumerable<Customer> GetAllCustomers();
        Customer GetCustomerById(int id);
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);

        IEnumerable<SalesOrder> GetAllSalesOrders();
        SalesOrder GetSalesOrderById(int id);
        void AddSalesOrder(SalesOrder order);
        void UpdateSalesOrder(SalesOrder order);
        void ApproveSalesOrder(int orderId, int approvedById);

        IEnumerable<Invoice> GetAllInvoices();
        Invoice GetInvoiceById(int id);
        void AddInvoice(Invoice invoice);
        void RecordPayment(int invoiceId, decimal amount);
    }
}