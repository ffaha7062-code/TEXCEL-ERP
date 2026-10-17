using System;
using System.Collections.Generic;
using TEXCEL_ERP.Models.Entities;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service interface for Holiday business logic.
    /// </summary>
    public interface IHolidayService
    {
        IEnumerable<Holiday> GetAllHolidays();
        IEnumerable<Holiday> GetHolidaysByBranch(int? branchId);
        IEnumerable<Holiday> GetUpcomingHolidays(int? branchId, int count = 5);
        Holiday GetHolidayById(int id);
        void AddHoliday(Holiday holiday);
        void UpdateHoliday(Holiday holiday);
        void DeleteHoliday(int id);
        void ToggleStatus(int id);
        bool IsHoliday(DateTime date, int branchId);
    }
}