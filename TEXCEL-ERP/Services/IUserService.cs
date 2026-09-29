using System.Collections.Generic;
using TEXCEL_ERP.Models.Entities;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service interface for User business logic.
    /// </summary>
    public interface IUserService
    {
        IEnumerable<ApplicationUser> GetAllUsers();
        IEnumerable<ApplicationUser> GetUsersByBranch(int branchId);
        ApplicationUser GetUserById(string id);
        ApplicationUser GetUserByEmail(string email);
        void AddUser(ApplicationUser user, string password, string role);
        void UpdateUser(ApplicationUser user);
        void DeleteUser(string id);
        void ToggleStatus(string id);
        bool IsEmailExists(string email, string excludeId = null);
        int GetTotalUsers();
        string GetUserRole(string userId);
    }
}