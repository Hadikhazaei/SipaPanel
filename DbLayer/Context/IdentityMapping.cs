using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

// 
using DbLayer.DbTable.Identity;
using DbLayer.Enums;

namespace DbLayer.Context {
    public static class IdentityMapping {
        public static void AddIdentityMapping (this ModelBuilder builder) {
            var userName = "admin";
            var userId = Guid.NewGuid ().ToString ();
            var hasher = new PasswordHasher<AppUser> ();
            var coRoleId = Guid.NewGuid ().ToString ();
            var coRoleName = nameof (RoleType.CoRole);
            // 
            // co
            // 
            builder.Entity<IdentityRole> ().HasData (new IdentityRole {
                Id = coRoleId, Name = coRoleName, NormalizedName = coRoleName.ToUpper ()
            });
            // 
            // planning
            // 
            var planningRoleId = Guid.NewGuid ().ToString ();
            var planningRoleName = nameof (RoleType.PlanningRole);
            builder.Entity<IdentityRole> ().HasData (new IdentityRole {
                Id = planningRoleId, Name = planningRoleName, NormalizedName = planningRoleName.ToUpper ()
            });
            // 
            // production
            // 
            var productionClerkRoleId = Guid.NewGuid ().ToString ();
            var productionClerkRoleName = nameof (RoleType.ProductionClerkRole);
            builder.Entity<IdentityRole> ().HasData (new IdentityRole {
                Id = productionClerkRoleId, Name = productionClerkRoleName, NormalizedName = productionClerkRoleName.ToUpper ()
            });
            var productionManagerRoleId = Guid.NewGuid ().ToString ();
            var productionManagerRoleName = nameof (RoleType.ProductionManagerRole);
            builder.Entity<IdentityRole> ().HasData (new IdentityRole {
                Id = productionManagerRoleId, Name = productionManagerRoleName, NormalizedName = productionManagerRoleName.ToUpper ()
            });
            // 
            // qcontrol
            // 
            var qcontrolRoleId = Guid.NewGuid ().ToString ();
            var qcontrolRoleName = nameof (RoleType.QControlRole);
            builder.Entity<IdentityRole> ().HasData (new IdentityRole {
                Id = qcontrolRoleId, Name = qcontrolRoleName, NormalizedName = qcontrolRoleName.ToUpper ()
            });
            // 
            // 
            // 
            builder.Entity<AppUser> ()
                .HasData (new AppUser {
                    Id = userId,
                        UserName = userName,
                        NormalizedUserName = userName.ToUpper (),
                        PasswordHash = hasher.HashPassword (null, "pa$$w0rd")
                });
            builder.Entity<IdentityUserRole<string>> ().HasData (new IdentityUserRole<string> {
                RoleId = coRoleId,
                UserId = userId
            });
        }
    }
}