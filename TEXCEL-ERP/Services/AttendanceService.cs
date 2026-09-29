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
                var attendance = new Attendance
                {
                    EmployeeId = employeeId,
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