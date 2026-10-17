using System;
using System.Collections.Generic;
using System.Linq;
using TEXCEL_ERP.Data.Repository;
using TEXCEL_ERP.Models.Entities;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service implementation for Holiday business logic.
    /// </summary>
    public class HolidayService : IHolidayService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HolidayService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Holiday> GetAllHolidays()
        {
            return _unitOfWork.Holidays.GetAll()
                .OrderByDescending(h => h.HolidayDate)
                .ToList();
        }

        public IEnumerable<Holiday> GetHolidaysByBranch(int? branchId)
        {
            var query = _unitOfWork.Holidays.GetAll().AsQueryable();

            if (branchId.HasValue)
            {
                int bid = branchId.Value;
                query = query.Where(h => h.BranchId == bid);
            }

            return query.OrderByDescending(h => h.HolidayDate).ToList();
        }

        public IEnumerable<Holiday> GetUpcomingHolidays(int? branchId, int count = 5)
        {
            var today = DateTime.Now.Date;
            var query = _unitOfWork.Holidays
                .Find(h => h.HolidayDate >= today && h.IsActive);

            if (branchId.HasValue)
            {
                int bid = branchId.Value;
                query = query.Where(h => h.BranchId == bid);
            }

            return query.OrderBy(h => h.HolidayDate).Take(count).ToList();
        }

        public Holiday GetHolidayById(int id)
        {
            return _unitOfWork.Holidays.GetById(id);
        }

        public void AddHoliday(Holiday holiday)
        {
            holiday.CreatedDate = DateTime.Now;
            _unitOfWork.Holidays.Add(holiday);
            _unitOfWork.Complete();
        }

        public void UpdateHoliday(Holiday holiday)
        {
            _unitOfWork.Holidays.Update(holiday);
            _unitOfWork.Complete();
        }

        public void DeleteHoliday(int id)
        {
            var holiday = _unitOfWork.Holidays.GetById(id);
            if (holiday != null)
            {
                _unitOfWork.Holidays.Remove(holiday);
                _unitOfWork.Complete();
            }
        }

        public void ToggleStatus(int id)
        {
            var holiday = _unitOfWork.Holidays.GetById(id);
            if (holiday != null)
            {
                holiday.IsActive = !holiday.IsActive;
                _unitOfWork.Holidays.Update(holiday);
                _unitOfWork.Complete();
            }
        }

        public bool IsHoliday(DateTime date, int branchId)
        {
            return _unitOfWork.Holidays
                .Any(h => h.HolidayDate == date.Date
                    && h.BranchId == branchId
                    && h.IsActive);
        }
    }
}