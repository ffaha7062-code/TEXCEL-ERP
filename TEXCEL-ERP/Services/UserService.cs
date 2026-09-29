using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TEXCEL_ERP.Data;
using TEXCEL_ERP.Models.Entities;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service implementation for User business logic.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        }

        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            return _context.Users
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .ToList();
        }

        public IEnumerable<ApplicationUser> GetUsersByBranch(int branchId)
        {
            return _context.Users
                .Where(u => u.BranchId == branchId)
                .OrderBy(u => u.FirstName)
                .ToList();
        }

        public ApplicationUser GetUserById(string id)
        {
            return _context.Users.Find(id);
        }

        public ApplicationUser GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public void AddUser(ApplicationUser user, string password, string role)
        {
            user.CreatedDate = DateTime.Now;
            user.IsActive = true;
            user.EmailConfirmed = true;

            var result = _userManager.Create(user, password);
            if (result.Succeeded)
            {
                _userManager.AddToRole(user.Id, role);
            }
            else
            {
                var errors = string.Join(", ", result.Errors);
                throw new Exception($"Failed to create user: {errors}");
            }
        }

        public void UpdateUser(ApplicationUser user)
        {
            _context.Entry(user).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteUser(string id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                user.IsActive = false;
                _context.SaveChanges();
            }
        }

        public void ToggleStatus(string id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                user.IsActive = !user.IsActive;
                _context.SaveChanges();
            }
        }

        public bool IsEmailExists(string email, string excludeId = null)
        {
            if (excludeId != null)
                return _context.Users.Any(u => u.Email == email && u.Id != excludeId);
            return _context.Users.Any(u => u.Email == email);
        }

        public int GetTotalUsers()
        {
            return _context.Users.Count();
        }

        public string GetUserRole(string userId)
        {
            var userRole = _context.Set<IdentityUserRole>()
                .FirstOrDefault(ur => ur.UserId == userId);

            if (userRole == null) return "No Role";

            var role = _context.Set<IdentityRole>()
                .FirstOrDefault(r => r.Id == userRole.RoleId);

            return role?.Name ?? "No Role";
        }
    }
}