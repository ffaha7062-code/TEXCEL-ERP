using System.Collections.Generic;
using TEXCEL_ERP.Models.Entities;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service interface for Payroll business logic.
    /// </summary>
    public interface IPayrollService
    {
        IEnumerable<Payroll> GetAllPayrolls();
        IEnumerable<Payroll> GetPayrollsByMonth(int month, int year);
        IEnumerable<Payroll> GetPayrollsByEmployee(int employeeId);
        Payroll GetPayrollById(int id);
        void ProcessPayroll(int month, int year, int branchId);
        void FinalizePayroll(int payrollId);
        void ReversePayroll(int payrollId);
        decimal GetTotalPayrollAmount(int month, int year);
        decimal GetTotalPayrollByBranch(int branchId, int month, int year);
    }
}