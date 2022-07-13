using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Newtonsoft.Json.Linq;
using CMS.Helper;
using CMS.Management.Model;
using CMS.Notification;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace CMS.Account
{
    public partial class Token : System.Web.UI.Page
    {
        private CustomUserManager CustomUserManager { get; set; }
        private IAuthenticationManager AuthenticationManager => HttpContext.Current.GetOwinContext().Authentication;
        private M_User user;
        enum ViewType { method, submit };

        public static DataTable Method = new DataTable();

        protected async void Page_Load(object sender, EventArgs e)
        {
            CustomUserManager = new CustomUserManager();

            Page.Title = "Security Login";
            if (HttpContext.Current.Session["M_User"] != null)
            {
                user = (M_User)HttpContext.Current.Session["M_User"];
                await LoadView();
            }
            else
            {
                Response.Redirect("~/", false);
            }
        }

        protected void cpToken_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
        }

        async Task LoadView()
        {
            if (!IsPostBack)
            {
                hfView["pageIndex"] = 0;
                hfView["method"] = "";                
                hfView["isSubmit"] = false;
            }

            if (Request.Form["method"] != null) hfView.Set("method", Request.Form["method"].ToString());

            mvToken.ActiveViewIndex = (int)hfView["pageIndex"];

            hfView.Set("isSubmit", true);
            switch ((ViewType)mvToken.ActiveViewIndex)
            {
                case ViewType.method:
                    Method = M_User.SelectMethodToken(user.user_id);
                    break;

                case ViewType.submit:
                    string token = "";
                    switch ((string)hfView["method"].ToString().ToUpper())
                    {
                        case "EMAIL":
                            if ((bool)hfView["isSubmit"] == false)
                            {
                                using (var sqlConnection = new OracleConnection(Database.getConnectionString("Default")))
                                {
                                    sqlConnection.Open();
                                    using (OracleTransaction sqlTransaction = sqlConnection.BeginTransaction())
                                    {
                                        token = M_Token.GenerateToken(user.user_id, sqlConnection, sqlTransaction);
                                        sqlTransaction.Commit();
                                    }
                                }

                                txtHeader.Text = "Security token has been sent to your " + hfView["method"].ToString();

                                try
                                {
                                    string message = string.Format("Hi, {0}, ", user.fullname);
                                    message += "Please don't tell below code to anyone.<br/><br/>";
                                    message += "Your security login token code is: <b>" + token + "</b>";

                                    Mail.Send(
                                        "KMS OTP Security Login",
                                        "Security Login Token",
                                        message,
                                        new MailAddress(user.email, user.fullname, System.Text.Encoding.UTF8)
                                        );

                                    hfView.Set("isSubmit", true);
                                }
                                catch (Exception ex)
                                {
                                    App_Log log = new App_Log();
                                    log.log_title = "KMS.Notification.Mail.Send";
                                    log.log_content = ex.Message.ToString();
                                    log.Save();

                                    cpToken.alertError("Configuration error", "window.location.reload()");
                                }
                            }

                            break;

                        case "TELEGRAM":
                            if ((bool)hfView["isSubmit"] == false)
                            {
                                using (var sqlConnection = new OracleConnection(Database.getConnectionString("Default")))
                                {
                                    sqlConnection.Open();
                                    using (OracleTransaction sqlTransaction = sqlConnection.BeginTransaction())
                                    {
                                        token = M_Token.GenerateToken(user.user_id, sqlConnection, sqlTransaction);
                                        sqlTransaction.Commit();
                                    }
                                }

                                txtHeader.Text = "Security token has been sent to your " + hfView["method"].ToString();

                                try
                                {
                                    string message = string.Format("Hi {0}, Please dont tell below code to anyone. <br/>", user.fullname);
                                    message += "Your security login token code is: <b>" + token + "</b>";

                                    await ITelegram.Send(user.telegram_id.ToInteger(), message, user.telegram_api);
                                }
                                catch (Exception ex)
                                {
                                    App_Log log = new App_Log();
                                    log.log_title = "KMS.Notification.Telegram.Send";
                                    log.log_content = ex.Message.ToString();
                                    log.Save();

                                    cpToken.alertError("Configuration error", "window.location.reload()");
                                }

                                hfView.Set("isSubmit", true);
                            }

                            break;

                        case "WHATSAPP":
                            if ((bool)hfView["isSubmit"] == false)
                            {
                                bool result = false;
                                using (var sqlConnection = new OracleConnection(Database.getConnectionString("Default")))
                                {
                                    sqlConnection.Open();
                                    using (OracleTransaction sqlTransaction = sqlConnection.BeginTransaction())
                                    {
                                        token = M_Token.GenerateToken(user.user_id, sqlConnection, sqlTransaction);
                                        sqlTransaction.Commit();
                                    }
                                }

                                txtHeader.Text = "Security token has been sent to your " + hfView["method"].ToString();

                                try
                                {
                                    string message = string.Format("Hi {0}, Please dont tell below code to anyone. <br/>", user.fullname);
                                    message += "Your security login token code is: *" + token + "*";

                                    result = await WhatsApp.SendMsg(user.phone, message);
                                }
                                catch (Exception ex)
                                {
                                    App_Log log = new App_Log();
                                    log.log_title = "KMS.Notification.Mail.Send";
                                    log.log_content = ex.Message.ToString();
                                    log.Save();

                                    cpToken.alertError("Configuration error", "window.location.reload()");
                                }

                                hfView.Set("isSubmit", result);
                            }

                            break;

                        case "SMS":
                            if ((bool)hfView["isSubmit"] == false)
                            {
                                bool result = false;
                                using (var sqlConnection = new OracleConnection(Database.getConnectionString("Default")))
                                {
                                    sqlConnection.Open();
                                    using (OracleTransaction sqlTransaction = sqlConnection.BeginTransaction())
                                    {
                                        token = M_Token.GenerateToken(user.user_id, sqlConnection, sqlTransaction);
                                        sqlTransaction.Commit();
                                    }
                                }

                                txtHeader.Text = "Security token has been sent to your phone";

                                try
                                {
                                    string message = string.Format("Hi {0}, Please dont tell below code to anyone." + Environment.NewLine + Environment.NewLine, user.fullname);
                                    message += "Your security login token code is: " + token + "";

                                    result = await SMS.SendTo(user.phone, message);
                                }
                                catch (Exception ex)
                                {
                                    App_Log log = new App_Log();
                                    log.log_title = "KMS.Notification.SMS.Send";
                                    log.log_content = ex.Message.ToString();
                                    log.Save();

                                    cpToken.alertError("Configuration error", "window.location.reload()");
                                }

                                hfView.Set("isSubmit", result);
                            }

                            break;
                    }
                    break;
            }
        }

        private async Task SignInAsync(UserCustomManager user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            var identity = await CustomUserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            identity.AddClaim(new Claim(UserClaim.User_Id, user.Id ?? user.Id, ClaimValueTypes.Integer));
            identity.AddClaim(new Claim(UserClaim.Username, user.UserName ?? user.UserName, ClaimValueTypes.String));
            identity.AddClaim(new Claim(UserClaim.Password, user.Password ?? user.Password, ClaimValueTypes.String));
            identity.AddClaim(new Claim(UserClaim.Fullname, user.FullName ?? user.FullName, ClaimValueTypes.String));
            identity.AddClaim(new Claim(UserClaim.Email, user.Email ?? user.Email, ClaimValueTypes.String));
            identity.AddClaim(new Claim(UserClaim.Phone, user.Phone ?? user.Phone, ClaimValueTypes.String));
            identity.AddClaim(new Claim(UserClaim.Theme, user.Theme ?? "Material", ClaimValueTypes.String));
            identity.AddClaim(new Claim(UserClaim.Role_Id, user.Role_Id ?? user.Role_Id, ClaimValueTypes.Integer));
            identity.AddClaim(new Claim(UserClaim.Role_Name, user.Role_Name ?? user.Role_Name, ClaimValueTypes.String));
            identity.AddClaim(new Claim(UserClaim.Group_Id, user.Group_Id ?? user.Group_Id, ClaimValueTypes.Integer));
            identity.AddClaim(new Claim(UserClaim.Group_Name, user.Group_Name ?? user.Group_Name, ClaimValueTypes.String));
            identity.AddClaim(new Claim(UserClaim.Photo, user.Photo ?? user.Photo, ClaimValueTypes.String));
            identity.AddClaim(new Claim(UserClaim.Company_Id, user.Company_Id ?? user.Company_Id, ClaimValueTypes.String));
            identity.AddClaim(new Claim(UserClaim.Company_Name, user.Company_Name ?? user.Company_Name, ClaimValueTypes.String));
            identity.AddClaim(new Claim(UserClaim.Gender, user.Gender ?? user.Gender, ClaimValueTypes.String));

            identity.AddClaim(new Claim(UserClaim.Allow_Create, user.Allow_Create.ToString(), ClaimValueTypes.Boolean));
            identity.AddClaim(new Claim(UserClaim.Allow_Update, user.Allow_Update.ToString(), ClaimValueTypes.Boolean));
            identity.AddClaim(new Claim(UserClaim.Allow_Delete, user.Allow_Delete.ToString(), ClaimValueTypes.Boolean));
            identity.AddClaim(new Claim(UserClaim.Allow_EnableDisable, user.Allow_EnableDisable.ToString(), ClaimValueTypes.Boolean));
            identity.AddClaim(new Claim(UserClaim.Allow_Export, user.Allow_Export.ToString(), ClaimValueTypes.Boolean));
            identity.AddClaim(new Claim(UserClaim.Allow_Import, user.Allow_Import.ToString(), ClaimValueTypes.Boolean));

            identity.AddClaim(new Claim(UserClaim.LocalIP, user.LocalIP, ClaimValueTypes.String));
            identity.AddClaim(new Claim(UserClaim.RemoteIP, user.RemoteIP, ClaimValueTypes.String));
            identity.AddClaim(new Claim(UserClaim.UserAgent, user.UserAgent.ToString(), ClaimValueTypes.String));
            identity.AddClaim(new Claim(UserClaim.Location, user.Location.ToString(), ClaimValueTypes.String));

            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //if (M_Token.ValidateToken(user.user_id, txtCode.Text.Trim().ToUpper()))
            //{
                UserCustomManager userManager = new UserCustomManager
                {
                    Id = user.user_id.ToString(),
                    UserName = user.username.ToString(),
                    Password = user.password.ToString(),
                    FullName = user.fullname.ToString(),
                    Email = user.email.ToString(),
                    Phone = user.phone.ToString(),
                    Company_Id = user.company_id.ToString(),
                    Company_Name = user.company_name.ToString(),
                    Theme = user.theme_name.ToString(),
                    Group_Id = user.group_id.ToString(),
                    Group_Name = user.group_name.ToString(),
                    Role_Id = user.role_id.ToString(),
                    Role_Name = user.role_name.ToString(),
                    Photo = user.photo.ToString(),
                    Gender = (user.gender == 0 ? "Laki-laki" : "Perempuan"),
                    Allow_Create = user.allow_create,
                    Allow_Update = user.allow_update,
                    Allow_Delete = user.allow_delete,
                    Allow_Export = user.allow_export,
                    Allow_Import = user.allow_import,
                    Allow_EnableDisable = user.allow_enabledisable,
                    RememberMe = user.remember,
                    LocalIP = user.local_ip,
                    RemoteIP = user.remote_ip,
                    UserAgent = user.user_agent,
                    Location = user.location
                };

                JObject json = JObject.FromObject(user);
                Log.Insert(Log.LogType.LOGIN, "User has login into system", json, user.user_id, user.local_ip, user.remote_ip, user.location);

                try
                {
                    if (!CPanel.AllowDuplicateLogin) M_User.NotifyLogin(user.local_ip, user.username);
                    Task task = SignInAsync(userManager, user.remember);
                    task.Wait();
                }
                catch (Exception ex) { ex.Message.ToString(); }

                //Clear session M_User
                HttpContext.Current.Session["M_User"] = null;

                Response.Redirect("~/");
            //}
            //else
            //{
            //    txtHeader.Text = "* The code you are enter is not valid";
            //    txtHeader.CssClass = "text-danger";
            //    btnSubmit.Visible = false;
            //    btnGoBack.Visible = true;

            //    //Clear session M_User
            //    HttpContext.Current.Session["M_User"] = null;
            //}
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["M_User"] = null;
            Response.Redirect("~/account/login");
        }
    }
}