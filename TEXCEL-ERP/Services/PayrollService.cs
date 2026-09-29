using System;
using System.Collections.Generic;
using System.Linq;
using TEXCEL_ERP.Data.Repository;
using TEXCEL_ERP.Models.Entities;
using TEXCEL_ERP.Models.Enums;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service implementation for Payroll business logic.
    /// </summary>
    public class PayrollService : IPayrollService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PayrollService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Payroll> GetAllPayrolls()
        {
            return _unitOfWork.Payrolls.GetAll()
                .OrderByDescending(p => p.Year)
                .ThenByDescending(p => p.Month)
                .ToList();
        }

        public IEnumerable<Payroll> GetPayrollsByMonth(int month, int year)
        {
            return _unitOfWork.Payrolls
                .Find(p => p.Month == month && p.Year == year)
                .OrderBy(p => p.EmployeeId)
                .ToList();
        }

        public IEnumerable<Payroll> GetPayrollsByEmployee(int employeeId)
        {
            return _unitOfWork.Payrolls
                .Find(p => p.EmployeeId == employeeId)
                .OrderByDescending(p => p.Year)
                .ThenByDescending(p => p.Month)
                .ToList();
        }

        public Payroll GetPayrollById(int id)
        {
            return _unitOfWork.Payrolls.GetById(id);
        }

        public void ProcessPayroll(int month, int year, int branchId)
        {
            var employees = _unitOfWork.Employees
                .Find(e => e.BranchId == branchId && e.IsActive)
                .ToList();

            foreach (var employee in employees)
            {
                var existing = _unitOfWork.Payrolls
                    .Get(p => p.EmployeeId == employee.EmployeeId && p.Month == month && p.Year == year);

                if (existing != null && existing.Status == PayrollStatus.Finalized)
                    continue;

                var attendances = _unitOfWork.Attendances
                    .Find(a => a.EmployeeId == employee.EmployeeId
                        && a.AttendanceDate.Month == month
                        && a.AttendanceDate.Year == year)
                    .ToList();

                var presentDays = attendances.Count(a => a.Status == AttendanceStatus.Present);
                var absentDays = attendances.Count(a => a.Status == AttendanceStatus.Absent);
                var leaveDays = attendances.Count(a => a.Status == AttendanceStatus.Leave);
                var overtimeHours = attendances.Sum(a => a.OvertimeHours);

                var basicSalary = employee.BasicSalary;
                var perDaySalary = basicSalary / 30;
                var absentDeduction = perDaySalary * absentDays;
                var overtimeRate = basicSalary / 30 / 8 * 1.5m;
                var overtimeAmount = overtimeHours * overtimeRate;

                var totalEarnings = basicSalary + overtimeAmount;
                var totalDeductions = absentDeduction;
                var netSalary = totalEarnings - totalDeductions;

                if (existing == null)
                {
                    var payroll = new Payroll
                    {
                        EmployeeId = employee.EmployeeId,
                        Month = month,
                        Year = year,
                        BasicSalary = basicSalary,
                        TotalEarnings = totalEarnings,
                        TotalDeductions = totalDeductions,
                        NetSalary = netSalary,
                        PresentDays = presentDays,
                        AbsentDays = absentDays,
                        LeaveDays = leaveDays,
                        OvertimeHours = overtimeHours,
                        OvertimeAmount = overtimeAmount,
                        Status = PayrollStatus.Processed,
                        ProcessedDate = DateTime.Now,
                        CreatedDate = DateTime.Now
                    };
                    _unitOfWork.Payrolls.Add(payroll);
                }
                else
                {
                    existing.BasicSalary = basicSalary;
                    existing.TotalEarnings = totalEarnings;
                    existing.TotalDeductions = totalDeductions;
                    existing.NetSalary = netSalary;
                    existing.PresentDays = presentDays;
                    existing.AbsentDays = absentDays;
                    existing.LeaveDays = leaveDays;
                    existing.OvertimeHours = overtimeHours;
                    existing.OvertimeAmount = overtimeAmount;
                    existing.Status = PayrollStatus.Processed;
                    existing.ProcessedDate = DateTime.Now;
                    _unitOfWork.Payrolls.Update(existing);
                }
            }

            _unitOfWork.Complete();
        }

        public void FinalizePayroll(int payrollId)
        {
            var payroll = _unitOfWork.Payrolls.GetById(payrollId);
            if (payroll != null)
            {
                payroll.Status = PayrollStatus.Finalized;
                _unitOfWork.Payrolls.Update(payroll);
                _unitOfWork.Complete();
            }
        }

        public void ReversePayroll(int payrollId)
        {
            var payroll = _unitOfWork.Payrolls.GetById(payrollId);
            if (payroll != null)
            {
                payroll.Status = PayrollStatus.Reversed;
                _unitOfWork.Payrolls.Update(payroll);
                _unitOfWork.Complete();
            }
        }

        public decimal GetTotalPayrollAmount(int month, int year)
        {
            return _unitOfWork.Payrolls
                .Find(p => p.Month == month && p.Year == year && p.Status == PayrollStatus.Finalized)
                .Sum(p => p.NetSalary);
        }

        public decimal GetTotalPayrollByBranch(int branchId, int month, int year)
        {
            var employeeIds = _unitOfWork.Employees
                .Find(e => e.BranchId == branchId)
                .Select(e => e.EmployeeId)
                .ToList();

            return _unitOfWork.Payrolls
                .Find(p => employeeIds.Contains(p.EmployeeId) && p.Month == month && p.Year == year)
                .Sum(p => p.NetSalary);
        }
    }
}