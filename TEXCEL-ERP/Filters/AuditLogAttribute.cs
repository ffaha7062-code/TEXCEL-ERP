using System;
using System.Web.Mvc;
using TEXCEL_ERP.Data;
using TEXCEL_ERP.Helpers;
using TEXCEL_ERP.Models.Entities;

namespace TEXCEL_ERP.Filters
{
    /// <summary>
    /// Audit logging filter.
    /// </summary>
    public class AuditLogAttribute : ActionFilterAttribute
    {
        public string ActionType { get; set; } = "Access";
        public string Module { get; set; } = "General";

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var log = new AuditLog
                    {
                        UserId = SessionHelper.UserId,
                        UserName = SessionHelper.UserName,
                        BranchId = SessionHelper.BranchId,
                        ActionType = ActionType,
                        Module = Module,
                        Description = $"{filterContext.ActionDescriptor.ControllerDescriptor.ControllerName}.{filterContext.ActionDescriptor.ActionName}",
                        IPAddress = filterContext.HttpContext.Request.UserHostAddress,
                        Timestamp = DateTime.Now
                    };
                    context.AuditLogs.Add(log);
                    context.SaveChanges();
                }
            }
            catch { }
        }
    }
}