using System;
using System.Collections.Generic;
using TEXCEL_ERP.Models.Entities;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service interface for Attendance business logic.
    /// </summary>
    public interface IAttendanceService
    {
        IEnumerable<Attendance> GetAllAttendances();
        IEnumerable<Attendance> GetAttendancesByDate(DateTime date);
        IEnumerable<Attendance> GetAttendancesByEmployee(int employeeId);
        IEnumerable<Attendance> GetAttendancesByDateRange(DateTime fromDate, DateTime toDate);
        Attendance GetAttendanceById(int id);
        Attendance GetAttendanceByEmployeeAndDate(int employeeId, DateTime date);
        void AddAttendance(Attendance attendance);
        void UpdateAttendance(Attendance attendance);
        void DeleteAttendance(int id);
        void MarkAttendance(int employeeId, DateTime date, TimeSpan? checkIn, TimeSpan? checkOut);
        int GetPresentCount(DateTime date);
        int GetAbsentCount(DateTime date);
        int GetLeaveCount(DateTime date);
        decimal GetTotalOvertimeHours(DateTime fromDate, DateTime toDate);
    }
}