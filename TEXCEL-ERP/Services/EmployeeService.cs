using System;
using System.Collections.Generic;
using System.Linq;
using TEXCEL_ERP.Data.Repository;
using TEXCEL_ERP.Models.Entities;
using TEXCEL_ERP.Models.Enums;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service implementation for Employee business logic.
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _unitOfWork.Employees.GetAll()
                .OrderBy(e => e.FirstName)
                .ToList();
        }

        public IEnumerable<Employee> GetEmployeesByBranch(int branchId)
        {
            return _unitOfWork.Employees
                .Find(e => e.BranchId == branchId)
                .OrderBy(e => e.FirstName)
                .ToList();
        }

        public IEnumerable<Employee> GetEmployeesByDepartment(int departmentId)
        {
            return _unitOfWork.Employees
                .Find(e => e.DepartmentId == departmentId)
                .OrderBy(e => e.FirstName)
                .ToList();
        }

        public Employee GetEmployeeById(int id)
        {
            return _unitOfWork.Employees.GetById(id);
        }

        public Employee GetEmployeeByCode(string code)
        {
            return _unitOfWork.Employees.Get(e => e.EmployeeCode == code);
        }

        public Employee GetEmployeeByCNIC(string cnic)
        {
            return _unitOfWork.Employees.Get(e => e.CNIC == cnic);
        }

        public void AddEmployee(Employee employee)
        {
            employee.CreatedDate = DateTime.Now;
            _unitOfWork.Employees.Add(employee);
            _unitOfWork.Complete();
        }

        public void UpdateEmployee(Employee employee)
        {
            employee.ModifiedDate = DateTime.Now;
            _unitOfWork.Employees.Update(employee);
            _unitOfWork.Complete();
        }

        public void DeleteEmployee(int id)
        {
            var employee = _unitOfWork.Employees.GetById(id);
            if (employee != null)
            {
                employee.IsActive = false;
                employee.Status = EmployeeStatus.Inactive;
                employee.ModifiedDate = DateTime.Now;
                _unitOfWork.Employees.Update(employee);
                _unitOfWork.Complete();
            }
        }

        public bool IsEmployeeCodeExists(string code, int? excludeId = null)
        {
            if (excludeId.HasValue)
                return _unitOfWork.Employees.Any(e => e.EmployeeCode == code && e.EmployeeId != excludeId.Value);
            return _unitOfWork.Employees.Any(e => e.EmployeeCode == code);
        }

        public bool IsCNICExists(string cnic, int? excludeId = null)
        {
            if (excludeId.HasValue)
                return _unitOfWork.Employees.Any(e => e.CNIC == cnic && e.EmployeeId != excludeId.Value);
            return _unitOfWork.Employees.Any(e => e.CNIC == cnic);
        }

        public int GetTotalEmployees()
        {
            return _unitOfWork.Employees.Count(e => e.IsActive);
        }

        public int GetTotalEmployeesByBranch(int branchId)
        {
            return _unitOfWork.Employees.Count(e => e.BranchId == branchId && e.IsActive);
        }
    }
}