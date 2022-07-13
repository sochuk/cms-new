using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using CMS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;

namespace CMS.Management.Model
{
    public static partial class UserIdentity
    {
        public static bool IsLocked(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.IsLocked);

            if (claim == null) return false;
            if (claim.ToString() == "" || claim.ToString() == string.Empty) return false;
            return claim.Value.ToString().ToBoolean();
        }

        public static string Get_Id(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.User_Id);

            return claim?.Value ?? string.Empty;
        }

        public static string Get_UserName(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.Username);

            return claim?.Value ?? string.Empty;
        }

        public static string Get_HashPassword(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.Password);

            return claim?.Value ?? string.Empty;
        }

        public static string Get_Fullname(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.Fullname);

            return claim?.Value ?? string.Empty;
        }

        public static string Get_RoleID(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.Role_Id);

            return claim?.Value ?? string.Empty;
        }

        public static string Get_RoleName(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.Role_Name);

            return claim?.Value ?? string.Empty;
        }

        public static string Get_GroupID(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.Group_Id);

            return claim?.Value ?? string.Empty;
        }

        public static string Get_GroupName(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.Group_Name);

            return claim?.Value ?? string.Empty;
        }


        public static string Get_Theme(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.Theme);

            return claim?.Value ?? string.Empty;
        }

        public static string Get_ThemeId(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.Theme);

            return claim?.Value ?? string.Empty;
        }

        public static string Get_CompanyId(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.Company_Id);

            return claim?.Value ?? string.Empty;
        }

        public static string Get_CompanyName(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.Company_Name);

            return claim?.Value ?? string.Empty;
        }

        public static string Get_Email(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.Email);

            return claim?.Value ?? string.Empty;
        }

        public static string Get_Phone(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.Phone);

            return claim?.Value ?? string.Empty;
        }

        public static string Get_Photo(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.Photo);

            return claim?.Value ?? string.Empty;
        }

        public static string Get_RemoteIP(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.RemoteIP);

            return claim?.Value ?? string.Empty;
        }

        public static string Get_LocalIP(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.LocalIP);

            return claim?.Value ?? string.Empty;
        }

        public static string Get_UserAgent(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.UserAgent);

            return claim?.Value ?? string.Empty;
        }

        public static string Get_LoginLocation(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.Location);

            return claim?.Value ?? string.Empty;
        }

        public static string Get_Gender(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.Gender);

            return claim?.Value ?? string.Empty;
        }

        public static bool Allow_Create(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.Allow_Create);
            if (claim == null) return false;
            if (claim.ToString() == "" || claim.ToString() == string.Empty) return false;
            bool value = Convert.ToBoolean(claim.Value.ToString().ToUpper() == "TRUE" ? true : false);
            return value;
        }

        public static bool Allow_Update(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.Allow_Update);
            if (claim == null) return false;
            if (claim.ToString() == "" || claim.ToString() == string.Empty) return false;
            bool value = Convert.ToBoolean(claim.Value.ToString().ToUpper() == "TRUE" ? true : false);
            return value;
        }

        public static bool Allow_Delete(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.Allow_Delete);
            if (claim == null) return false;
            if (claim.ToString() == "" || claim.ToString() == string.Empty) return false;
            bool value = Convert.ToBoolean(claim.Value.ToString().ToUpper() == "TRUE" ? true : false);
            return value;
        }

        public static bool Allow_Export(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.Allow_Export);
            if (claim == null) return false;
            if (claim.ToString() == "" || claim.ToString() == string.Empty) return false;
            bool value = Convert.ToBoolean(claim.Value.ToString().ToUpper() == "TRUE" ? true : false);
            return value;
        }

        public static bool Allow_Import(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.Allow_Import);
            if (claim == null) return false;
            if (claim.ToString() == "" || claim.ToString() == string.Empty) return false;
            bool value = Convert.ToBoolean(claim.Value.ToString().ToUpper() == "TRUE" ? true : false);
            return value;
        }

        public static bool Allow_EnableDisable(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaim.Allow_EnableDisable);
            if (claim == null) return false;
            if (claim.ToString() == "" || claim.ToString() == string.Empty) return false;
            bool value = Convert.ToBoolean(claim.Value.ToString().ToUpper() == "TRUE" ? true : false);
            return value;
        }

        public static void UpdateClaim(this IIdentity identity, string key, string value)
        {
            var Identity = (ClaimsIdentity)System.Web.HttpContext.Current.User.Identity;
            var keyClaim = Identity.FindFirst(key.ToString());
            if (keyClaim != null) Identity.RemoveClaim(keyClaim);

            Identity.AddClaim(new Claim(key, value));
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(Identity), new AuthenticationProperties() { IsPersistent = true });
        }
    }
}