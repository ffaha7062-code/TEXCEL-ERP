using System;
using System.Collections.Generic;
using TEXCEL_ERP.Models.Entities;
using TEXCEL_ERP.Models.Enums;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service interface for Attendance business logic.
    /// </summary>
    public interface IAttendanceService
    {
        // =============================================
        // BASIC CRUD
        // =============================================
        IEnumerable<Attendance> GetAllAttendances();
        IEnumerable<Attendance> GetAttendancesByDate(DateTime date);
        IEnumerable<Attendance> GetAttendancesByDateAndBranch(DateTime date, int? branchId);
        IEnumerable<Attendance> GetAttendancesByEmployee(int employeeId);
        IEnumerable<Attendance> GetAttendancesByDateRange(DateTime fromDate, DateTime toDate);
        IEnumerable<Attendance> GetAttendancesByMonth(int month, int year, int? branchId);
        Attendance GetAttendanceById(int id);
        Attendance GetAttendanceByEmployeeAndDate(int employeeId, DateTime date);
        void AddAttendance(Attendance attendance);
        void UpdateAttendance(Attendance attendance);
        void DeleteAttendance(int id);

        // =============================================
        // BULK ENTRY / MARK ATTENDANCE
        // =============================================
        void SaveBulkAttendance(List<Attendance> attendanceList);
        void MarkAttendance(int employeeId, DateTime date, TimeSpan? checkIn, TimeSpan? checkOut);

        // =============================================
        // CALCULATIONS
        // =============================================
        Attendance CalculateAttendanceHours(Attendance attendance, Shift shift);
        decimal CalculateOvertimeHours(TimeSpan checkIn, TimeSpan checkOut, Shift shift);
        decimal CalculateLessTimeHours(TimeSpan checkIn, TimeSpan checkOut, Shift shift);
        decimal CalculateWorkedHours(TimeSpan checkIn, TimeSpan checkOut, Shift shift);

        // =============================================
        // STATS / COUNTS
        // =============================================
        int GetPresentCount(DateTime date);
        int GetAbsentCount(DateTime date);
        int GetLeaveCount(DateTime date);
        decimal GetTotalOvertimeHours(DateTime fromDate, DateTime toDate);
    }
}