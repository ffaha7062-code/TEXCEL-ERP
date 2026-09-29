using System;
using System.Collections.Generic;
using System.Linq;
using TEXCEL_ERP.Data.Repository;
using TEXCEL_ERP.Models.Entities;
using TEXCEL_ERP.Models.Enums;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service implementation for Leave business logic.
    /// </summary>
    public class LeaveService : ILeaveService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeaveService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Leave> GetAllLeaves()
        {
            return _unitOfWork.Leaves.GetAll()
                .OrderByDescending(l => l.CreatedDate)
                .ToList();
        }

        public IEnumerable<Leave> GetLeavesByEmployee(int employeeId)
        {
            return _unitOfWork.Leaves
                .Find(l => l.EmployeeId == employeeId)
                .OrderByDescending(l => l.StartDate)
                .ToList();
        }

        public IEnumerable<Leave> GetPendingLeaves()
        {
            return _unitOfWork.Leaves
                .Find(l => l.Status == LeaveStatus.Pending)
                .OrderByDescending(l => l.CreatedDate)
                .ToList();
        }

        public Leave GetLeaveById(int id)
        {
            return _unitOfWork.Leaves.GetById(id);
        }

        public void ApplyLeave(Leave leave)
        {
            leave.TotalDays = (leave.EndDate - leave.StartDate).Days + 1;
            leave.Status = LeaveStatus.Pending;
            leave.CreatedDate = DateTime.Now;
            _unitOfWork.Leaves.Add(leave);
            _unitOfWork.Complete();
        }

        public void ApproveLeave(int leaveId, int approvedById, string remarks)
        {
            var leave = _unitOfWork.Leaves.GetById(leaveId);
            if (leave != null)
            {
                leave.Status = LeaveStatus.Approved;
                leave.ApprovedById = approvedById;
                leave.ApprovedDate = DateTime.Now;
                leave.ApprovalRemarks = remarks;
                _unitOfWork.Leaves.Update(leave);
                _unitOfWork.Complete();
            }
        }

        public void RejectLeave(int leaveId, int approvedById, string remarks)
        {
            var leave = _unitOfWork.Leaves.GetById(leaveId);
            if (leave != null)
            {
                leave.Status = LeaveStatus.Rejected;
                leave.ApprovedById = approvedById;
                leave.ApprovedDate = DateTime.Now;
                leave.ApprovalRemarks = remarks;
                _unitOfWork.Leaves.Update(leave);
                _unitOfWork.Complete();
            }
        }

        public int GetLeaveBalance(int employeeId, LeaveType leaveType)
        {
            var policy = _unitOfWork.LeavePolicies.Get(p => p.LeaveType == leaveType && p.IsActive);
            if (policy == null) return 0;

            var usedLeaves = _unitOfWork.Leaves
                .Find(l => l.EmployeeId == employeeId
                    && l.LeaveType == leaveType
                    && l.Status == LeaveStatus.Approved
                    && l.StartDate.Year == DateTime.Now.Year)
                .Sum(l => l.TotalDays);

            return policy.TotalLeaves - usedLeaves;
        }
    }
}