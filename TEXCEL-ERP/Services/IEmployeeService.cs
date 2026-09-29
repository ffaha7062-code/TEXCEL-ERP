using System.Collections.Generic;
using TEXCEL_ERP.Models.Entities;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service interface for Employee business logic.
    /// </summary>
    public interface IEmployeeService
    {
        IEnumerable<Employee> GetAllEmployees();
        IEnumerable<Employee> GetEmployeesByBranch(int branchId);
        IEnumerable<Employee> GetEmployeesByDepartment(int departmentId);
        Employee GetEmployeeById(int id);
        Employee GetEmployeeByCode(string code);
        Employee GetEmployeeByCNIC(string cnic);
        void AddEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(int id);
        bool IsEmployeeCodeExists(string code, int? excludeId = null);
        bool IsCNICExists(string cnic, int? excludeId = null);
        int GetTotalEmployees();
        int GetTotalEmployeesByBranch(int branchId);
    }
}