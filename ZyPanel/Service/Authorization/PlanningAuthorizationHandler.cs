using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

// 
using DbLayer.DbTable;
using DbLayer.DbTable.Identity;

namespace ZyPanel.Service.Authorization {
    public class PlanningAuthorizationHandler:
        AuthorizationHandler<OperationAuthorizationRequirement, TblPlanning> {
            private UserManager<AppUser> _userManager;

            public PlanningAuthorizationHandler (UserManager<AppUser> userManager) {
                _userManager = userManager;
            }

            protected override Task HandleRequirementAsync (AuthorizationHandlerContext context,
                OperationAuthorizationRequirement requirement, TblPlanning resource) {
                if (context.User == null || resource == null) {
                    return Task.CompletedTask;
                }

                // If not asking for CRUD permission, return.

                // if (requirement.Name != Constants.CreateOperationName &&
                //     requirement.Name != Constants.ReadOperationName &&
                //     requirement.Name != Constants.UpdateOperationName &&
                //     requirement.Name != Constants.DeleteOperationName) {
                //     return Task.CompletedTask;
                // }

                // if (resource.UserId == _userManager.GetUserId (context.User)) {
                //     context.Succeed (requirement);
                // }

                return Task.CompletedTask;
            }
        }
}