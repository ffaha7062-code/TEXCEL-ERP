using System.Collections.Generic;
using TEXCEL_ERP.Models.Entities;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service interface for Leave business logic.
    /// </summary>
    public interface ILeaveService
    {
        IEnumerable<Leave> GetAllLeaves();
        IEnumerable<Leave> GetLeavesByEmployee(int employeeId);
        IEnumerable<Leave> GetPendingLeaves();
        Leave GetLeaveById(int id);
        void ApplyLeave(Leave leave);
        void ApproveLeave(int leaveId, int approvedById, string remarks);
        void RejectLeave(int leaveId, int approvedById, string remarks);
        int GetLeaveBalance(int employeeId, Models.Enums.LeaveType leaveType);
    }
}