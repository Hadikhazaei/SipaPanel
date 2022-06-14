using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ZyPanel.Service.IdentityService {
    public static class IdExtensions {
        public static string GetUserId (this IIdentity identity) {
            return identity?.GetUserClaimValue (ClaimTypes.NameIdentifier);
        }

        public static T GetUserId<T> (this IIdentity identity) where T : IConvertible {
            var firstValue = identity?.GetUserClaimValue (ClaimTypes.NameIdentifier);
            return firstValue != null ? (T) Convert.ChangeType (firstValue, typeof (T), CultureInfo.InvariantCulture) : default (T);
        }

        public static string GetUserName (this IIdentity identity) {
            return identity?.GetUserClaimValue (ClaimTypes.Name);
        }

        public static string GetUserRole (this IIdentity identity) {
            return identity?.GetUserClaimValue (ClaimTypes.Role);
        }

        public static string GetUserFirstName (this IIdentity identity) {
            return identity?.GetUserClaimValue (ClaimTypes.GivenName);
        }

        public static string GetUserLastName (this IIdentity identity) {
            return identity?.GetUserClaimValue (ClaimTypes.Surname);
        }

        public static string GetUserFullName (this IIdentity identity) {
            return $"{GetUserFirstName(identity)} {GetUserLastName(identity)}";
        }

        public static string GetUserDisplayName (this IIdentity identity) {
            var fullName = GetUserFullName (identity);
            return string.IsNullOrWhiteSpace (fullName) ? GetUserName (identity) : fullName;
        }

        public static string GetUserClaimValue (this IIdentity identity, string claimType) {
            var _identity = identity as ClaimsIdentity;
            return _identity?.FindFirstValue (claimType);
        }

        public static string FindFirstValue (this ClaimsIdentity identity, string claimType) {
            return identity?.FindFirst (claimType)?.Value;
        }

        // Errors
        public static string IdentityResultErrors (this IdentityResult result, bool useHtmlNewLine = false) {
            var results = new StringBuilder ();
            if (!result.Succeeded) {
                foreach (var error in result.Errors) {
                    var errDescription = error.Description;
                    if (string.IsNullOrWhiteSpace (errDescription)) {
                        continue;
                    }
                    if (!useHtmlNewLine) {
                        results.AppendLine (errDescription);
                    } else {
                        results.AppendLine ($"{errDescription}<br/>");
                    }
                }
            }
            return results.ToString ();
        }

        public static void AddIdentityResultErrors (this ModelStateDictionary modelState, IdentityResult result) {
            foreach (var error in result.Errors) {
                modelState.AddModelError ("", error.Description);
            }
        }
    }
}