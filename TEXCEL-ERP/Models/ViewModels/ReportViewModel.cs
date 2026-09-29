using System;
using System.Collections.Generic;
using TEXCEL_ERP.Models.Entities;

namespace TEXCEL_ERP.Models.ViewModels
{
    /// <summary>
    /// ViewModel for reports and filters.
    /// </summary>
    public class ReportViewModel
    {
        public string ReportType { get; set; } // Employee, Attendance, Payroll, Inventory, Sales, Purchase, Production

        // Date Filters
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }

        // Branch & Department Filters
        public int? BranchId { get; set; }
        public int? DepartmentId { get; set; }
        public int? DesignationId { get; set; }
        public int? EmployeeId { get; set; }

        // Status Filters
        public string Status { get; set; }

        // Search
        public string SearchTerm { get; set; }

        // Report Data (Generic)
        public dynamic ReportData { get; set; }

        // Dropdowns
        public List<Branch> Branches { get; set; }
        public List<Department> Departments { get; set; }
        public List<Designation> Designations { get; set; }
        public List<Employee> Employees { get; set; }

        // Summary Data
        public decimal TotalAmount { get; set; }
        public int TotalRecords { get; set; }
    }
}