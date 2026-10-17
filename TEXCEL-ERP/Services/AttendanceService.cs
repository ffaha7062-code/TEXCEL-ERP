using System;
using System.Collections.Generic;
using System.Linq;
using TEXCEL_ERP.Data.Repository;
using TEXCEL_ERP.Models.Entities;
using TEXCEL_ERP.Models.Enums;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service implementation for Attendance business logic.
    /// </summary>
    public class AttendanceService : IAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttendanceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =============================================
        // BASIC CRUD
        // =============================================

        public IEnumerable<Attendance> GetAllAttendances()
        {
            return _unitOfWork.Attendances.GetAll()
                .OrderByDescending(a => a.AttendanceDate)
                .ToList();
        }

        public IEnumerable<Attendance> GetAttendancesByDate(DateTime date)
        {
            return _unitOfWork.Attendances
                .Find(a => a.AttendanceDate == date.Date)
                .OrderBy(a => a.EmployeeId)
                .ToList();
        }

        // ✅ NEW: Branch-specific
        public IEnumerable<Attendance> GetAttendancesByDateAndBranch(DateTime date, int? branchId)
        {
            var query = _unitOfWork.Attendances
                .Find(a => a.AttendanceDate == date.Date);

            if (branchId.HasValue)
            {
                int bid = branchId.Value;
                query = query.Where(a => a.BranchId == bid);
            }

            return query.OrderBy(a => a.EmployeeId).ToList();
        }

        public IEnumerable<Attendance> GetAttendancesByEmployee(int employeeId)
        {
            return _unitOfWork.Attendances
                .Find(a => a.EmployeeId == employeeId)
                .OrderByDescending(a => a.AttendanceDate)
                .ToList();
        }

        public IEnumerable<Attendance> GetAttendancesByDateRange(DateTime fromDate, DateTime toDate)
        {
            return _unitOfWork.Attendances
                .Find(a => a.AttendanceDate >= fromDate.Date && a.AttendanceDate <= toDate.Date)
                .OrderBy(a => a.AttendanceDate)
                .ToList();
        }

        // ✅ NEW: Monthly
        public IEnumerable<Attendance> GetAttendancesByMonth(int month, int year, int? branchId)
        {
            var query = _unitOfWork.Attendances
                .Find(a => a.AttendanceDate.Month == month && a.AttendanceDate.Year == year);

            if (branchId.HasValue)
            {
                int bid = branchId.Value;
                query = query.Where(a => a.BranchId == bid);
            }

            return query.OrderBy(a => a.AttendanceDate).ThenBy(a => a.EmployeeId).ToList();
        }

        public Attendance GetAttendanceById(int id)
        {
            return _unitOfWork.Attendances.GetById(id);
        }

        public Attendance GetAttendanceByEmployeeAndDate(int employeeId, DateTime date)
        {
            return _unitOfWork.Attendances
                .Get(a => a.EmployeeId == employeeId && a.AttendanceDate == date.Date);
        }

        public void AddAttendance(Attendance attendance)
        {
            attendance.CreatedDate = DateTime.Now;
            _unitOfWork.Attendances.Add(attendance);
            _unitOfWork.Complete();
        }

        public void UpdateAttendance(Attendance attendance)
        {
            attendance.ModifiedDate = DateTime.Now;
            _unitOfWork.Attendances.Update(attendance);
            _unitOfWork.Complete();
        }

        public void DeleteAttendance(int id)
        {
            var attendance = _unitOfWork.Attendances.GetById(id);
            if (attendance != null)
            {
                _unitOfWork.Attendances.Remove(attendance);
                _unitOfWork.Complete();
            }
        }

        // =============================================
        // BULK ENTRY
        // =============================================
        public void SaveBulkAttendance(List<Attendance> attendanceList)
        {
            if (attendanceList == null || !attendanceList.Any()) return;

            foreach (var att in attendanceList)
            {
                // Existing record check
                var existing = GetAttendanceByEmployeeAndDate(att.EmployeeId, att.AttendanceDate);

                // Shift fetch karein (calc ke liye)
                Shift shift = null;
                if (att.ShiftId.HasValue)
                {
                    shift = _unitOfWork.Shifts.GetById(att.ShiftId.Value);
                }

                // Calculate hours
                if (shift != null && att.CheckIn.HasValue && att.CheckOut.HasValue)
                {
                    att.WorkedHours = CalculateWorkedHours(att.CheckIn.Value, att.CheckOut.Value, shift);
                    att.OvertimeHours = CalculateOvertimeHours(att.CheckIn.Value, att.CheckOut.Value, shift);
                    att.LessTimeHours = CalculateLessTimeHours(att.CheckIn.Value, att.CheckOut.Value, shift);

                    // Late calc
                    var lateMinutes = (att.CheckIn.Value - shift.StartTime).TotalMinutes;
                    att.LateMinutes = lateMinutes > shift.LateAllowance ? (int)lateMinutes : 0;

                    // Early going calc
                    var earlyMinutes = (shift.EndTime - att.CheckOut.Value).TotalMinutes;
                    att.EarlyGoingMinutes = earlyMinutes > 0 ? (int)earlyMinutes : 0;
                }

                if (existing != null)
                {
                    // Update existing
                    existing.CheckIn = att.CheckIn;
                    existing.CheckOut = att.CheckOut;
                    existing.Status = att.Status;
                    existing.WorkedHours = att.WorkedHours;
                    existing.OvertimeHours = att.OvertimeHours;
                    existing.LessTimeHours = att.LessTimeHours;
                    existing.LateMinutes = att.LateMinutes;
                    existing.EarlyGoingMinutes = att.EarlyGoingMinutes;
                    existing.Remarks = att.Remarks;
                    existing.IsVerified = att.IsVerified;
                    existing.ModifiedDate = DateTime.Now;

                    _unitOfWork.Attendances.Update(existing);
                }
                else
                {
                    // New record
                    att.CreatedDate = DateTime.Now;
                    _unitOfWork.Attendances.Add(att);
                }
            }

            _unitOfWork.Complete();
        }

        public void MarkAttendance(int employeeId, DateTime date, TimeSpan? checkIn, TimeSpan? checkOut)
        {
            var existing = GetAttendanceByEmployeeAndDate(employeeId, date);

            if (existing != null)
            {
                existing.CheckIn = checkIn;
                existing.CheckOut = checkOut;
                existing.Status = DetermineStatus(checkIn, checkOut);
                UpdateAttendance(existing);
            }
            else
            {
                var employee = _unitOfWork.Employees.GetById(employeeId);

                var attendance = new Attendance
                {
                    EmployeeId = employeeId,
                    BranchId = employee != null ? employee.BranchId : 1,
                    AttendanceDate = date.Date,
                    CheckIn = checkIn,
                    CheckOut = checkOut,
                    Status = DetermineStatus(checkIn, checkOut),
                    CreatedDate = DateTime.Now
                };
                AddAttendance(attendance);
            }
        }

        private AttendanceStatus DetermineStatus(TimeSpan? checkIn, TimeSpan? checkOut)
        {
            if (checkIn == null) return AttendanceStatus.Absent;
            if (checkOut == null) return AttendanceStatus.Present;

            var workedHours = (checkOut.Value - checkIn.Value).TotalHours;
            if (workedHours >= 8) return AttendanceStatus.Present;
            if (workedHours >= 4) return AttendanceStatus.HalfDay;
            return AttendanceStatus.Absent;
        }

        // =============================================
        // CALCULATIONS
        // =============================================
        public Attendance CalculateAttendanceHours(Attendance attendance, Shift shift)
        {
            if (attendance == null || shift == null) return attendance;

            if (attendance.CheckIn.HasValue && attendance.CheckOut.HasValue)
            {
                attendance.WorkedHours = CalculateWorkedHours(attendance.CheckIn.Value, attendance.CheckOut.Value, shift);
                attendance.OvertimeHours = CalculateOvertimeHours(attendance.CheckIn.Value, attendance.CheckOut.Value, shift);
                attendance.LessTimeHours = CalculateLessTimeHours(attendance.CheckIn.Value, attendance.CheckOut.Value, shift);

                // Late
                var lateMin = (attendance.CheckIn.Value - shift.StartTime).TotalMinutes;
                attendance.LateMinutes = lateMin > shift.LateAllowance ? (int)lateMin : 0;

                // Early going
                var earlyMin = (shift.EndTime - attendance.CheckOut.Value).TotalMinutes;
                attendance.EarlyGoingMinutes = earlyMin > 0 ? (int)earlyMin : 0;
            }

            return attendance;
        }

        public decimal CalculateWorkedHours(TimeSpan checkIn, TimeSpan checkOut, Shift shift)
        {
            var totalHours = (checkOut - checkIn).TotalHours;
            var breakHours = shift.BreakDuration / 60.0; // minutes to hours
            var worked = totalHours - breakHours;
            return worked > 0 ? Math.Round((decimal)worked, 2) : 0;
        }

        public decimal CalculateOvertimeHours(TimeSpan checkIn, TimeSpan checkOut, Shift shift)
        {
            var shiftHours = (shift.EndTime - shift.StartTime).TotalHours - (shift.BreakDuration / 60.0);
            var worked = CalculateWorkedHours(checkIn, checkOut, shift);

            if (worked > (decimal)shiftHours)
            {
                return Math.Round(worked - (decimal)shiftHours, 2);
            }
            return 0;
        }

        public decimal CalculateLessTimeHours(TimeSpan checkIn, TimeSpan checkOut, Shift shift)
        {
            var shiftHours = (shift.EndTime - shift.StartTime).TotalHours - (shift.BreakDuration / 60.0);
            var worked = CalculateWorkedHours(checkIn, checkOut, shift);

            if (worked < (decimal)shiftHours)
            {
                return Math.Round((decimal)shiftHours - worked, 2);
            }
            return 0;
        }

        // =============================================
        // STATS
        // =============================================
        public int GetPresentCount(DateTime date)
        {
            return _unitOfWork.Attendances.Count(a => a.AttendanceDate == date.Date && a.Status == AttendanceStatus.Present);
        }

        public int GetAbsentCount(DateTime date)
        {
            return _unitOfWork.Attendances.Count(a => a.AttendanceDate == date.Date && a.Status == AttendanceStatus.Absent);
        }

        public int GetLeaveCount(DateTime date)
        {
            return _unitOfWork.Attendances.Count(a => a.AttendanceDate == date.Date && a.Status == AttendanceStatus.Leave);
        }

        public decimal GetTotalOvertimeHours(DateTime fromDate, DateTime toDate)
        {
            return _unitOfWork.Attendances
                .Find(a => a.AttendanceDate >= fromDate.Date && a.AttendanceDate <= toDate.Date)
                .Sum(a => a.OvertimeHours);
        }
    }
}