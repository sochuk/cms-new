using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;

namespace CMS.Management.Model
{
    public class UserCustomManager : IUser
    {
        public string Id { get; set; }
        [DisplayName("M_User.getUserId()")]

        public string UserName { get; set; }
        [DisplayName("Password")]

        public string Password { get; set; }

        [DisplayName("Full Name")]
        public string FullName { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Phone")]
        public string Phone { get; set; }

        [DisplayName("Theme")]
        public string Theme { get; set; }

        public string Role_Id { get; set; }
        [DisplayName("Role Name")]
        public string Role_Name { get; set; }

        public string Group_Id { get; set; }
        [DisplayName("Group Name")]
        public string Group_Name { get; set; }

        public string Company_Id { get; set; }
        [DisplayName("Company Name")]
        public string Company_Name { get; set; }

        public string Vendor_Id { get; set; }

        public string Photo { get; set; }
        public string Gender { get; set; }

        public bool Allow_Create { get; set; }
        public bool Allow_Update { get; set; }
        public bool Allow_Delete { get; set; }
        public bool Allow_EnableDisable { get; set; }
        public bool Allow_Export { get; set; }
        public bool Allow_Import { get; set; }

        [DisplayName("Remember Me")]
        public bool RememberMe { get; set; }

        public string LocalIP { get; set; }
        public string RemoteIP { get; set; }
        public string UserAgent { get; set; }
        public string Location { get; set; }
    }

    public class UserClaim
    {
        public const string IsLocked = "IsLocked";
        public const string User_Id = "User_Id";
        public const string Username = "Username";
        public const string Password = "Password";
        public const string Fullname = "Fullname";
        public const string Email = "Email";
        public const string Phone = "Phone";
        public const string Company_Id = "Company_Id";
        public const string Company_Name = "Company_Name";
        public const string Theme_Id = "Theme_Id";
        public const string Theme = "Theme";
        public const string Role_Id = "Role_Id";
        public const string Role_Name = "Role_Name";
        public const string Group_Id = "Group_Id";
        public const string Vendor_Id = "Vendor_Id";
        public const string Group_Name = "Group_Name";
        public const string Photo = "Photo";
        public const string Gender = "Gender";
        public const string Allow_Create = "Allow_Create";
        public const string Allow_Update = "Allow_Update";
        public const string Allow_Delete = "Allow_Delete";
        public const string Allow_EnableDisable = "Allow_EnableDisable";
        public const string Allow_Export = "Allow_Export";
        public const string Allow_Import = "Allow_Import";
        public const string Last_Login = "Last_Login";

        public const string LocalIP = "LocalIP";
        public const string RemoteIP = "RemoteIP";
        public const string UserAgent = "UserAgent";
        public const string Location = "Location";
    }    

    public class CustomUserManager : UserManager<UserCustomManager>
    {
        public CustomUserManager() : base(new UserStore<UserCustomManager>())
        {
        }
    }

    public class UserStore<T> : IUserStore<T> where T : UserCustomManager
    {
        public Task CreateAsync(T user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(T user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public Task<T> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T user)
        {
            throw new NotImplementedException();
        }

    }
}