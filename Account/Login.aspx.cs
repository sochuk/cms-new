using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using CMS.Management.Model;
using System;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using CMS.Helper;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using CMS.Notification;
using System.Net.Mail;
using Antlr4.StringTemplate;

namespace CMS.Account
{
    public partial class Login : System.Web.UI.Page
    {        
        private CustomUserManager CustomUserManager { get; set; }
        private IAuthenticationManager AuthenticationManager => HttpContext.Current.GetOwinContext().Authentication;
        private string returnurl = "";
        private int max_trylogin = 0;
        private int block_trylogin = 0;

        public static bool blocked = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Login";
            //ChangePassword();
            HttpContext.Current.Session["error"] = null;
            returnurl = Request.QueryString["returnurl"];

            max_trylogin = ConfigurationManager.AppSettings.Get("TryLogin").ToString().ToInteger();
            block_trylogin = ConfigurationManager.AppSettings.Get("TryLoginBlockInMinutes").ToString().ToInteger();

            blocked = (HttpContext.Current.Session["trylogin_datetime"] != null);
            if (blocked)
            {
                DateTime trylogin_datetime = (DateTime)HttpContext.Current.Session["trylogin_datetime"]; //client time
                DateTime trylogin_timeout = (DateTime)HttpContext.Current.Session["trylogin_timeout"]; //client time tiemout

                DateTime trylogin_datetime_local = trylogin_datetime.ToLocalTime();

                TimeSpan timeout = DateTime.Now.ToLocalTime().Subtract(trylogin_datetime_local);

                if (timeout.TotalMinutes < block_trylogin)
                {
                    string format = trylogin_timeout.AddSeconds(5).ToString("yyyy/MM/dd HH:mm:ss");
                    HttpContext.Current.Session["error"] = "Access login blocked.<br/> You can try login again in <h1 id=\"timeout\" class=\"h1 mt-2 font-weight-bold\">" + format + "</h1>";                    
                    return;
                }
                else
                {
                    HttpContext.Current.Session["trylogin_datetime"] = null;
                    HttpContext.Current.Session["trylogin_timeout"] = null;
                    HttpContext.Current.Session["trylogin"] = 0;
                }
            }

            
            CustomUserManager = new CustomUserManager();
                        

            if (User.Identity.IsAuthenticated)
            {
                if (returnurl != "")
                {
                    Response.Redirect("~" + returnurl);
                }
                Response.Redirect("~/");
            }

            if (IsPostBack)
            {
                string username = txtUsername.Text.Trim();
                string pass = Crypto.EncryptPassword(txtPassword.Text.Trim());
                bool remember = chkRemember.Checked;
                string remote_ip = Request.Form["ip"].ToString();
                string useragent = Request.Form["ua"].ToString();
                string location = Request.Form["loc"].ToString();

                if (username == "" || pass == "")
                {
                    HttpContext.Current.Session["error"] = "Username or password required";
                    return;
                }

                // validate the Captcha to check we're not dealing with a bot
                if (CMS.CPanel.UseCaptcha)
                {
                    bool isHuman = Captcha.Validate(txtCaptcha.Text);
                    txtCaptcha.Text = null; // clear previous user input
                    if (!isHuman)
                    {
                        HttpContext.Current.Session["error"] = "Captcha not valid";
                    }
                    else
                    {
                        Validate_Login(username, pass, remember, remote_ip, useragent, location);
                    }
                }
                else
                {
                    Validate_Login(username, pass, remember, remote_ip, useragent, location);
                }             
            }
        }

        public void Validate_Login(string username, string password, bool rememberMe, string remote_ip, string useragent, string location)
        {
            string local_ip = IPAddress.GetLocalIPAddress();
            M_User loginfo = new M_User();
            using (var cnn = new OracleConnection(Database.getConnectionString("Default")))
            {
                cnn.Open();
                using (OracleTransaction sqlTransaction = cnn.BeginTransaction())
                {                    
                    loginfo = M_User.Login(username, password, local_ip, remote_ip, useragent, location);
                }
            }
                    

            if (loginfo != null)
            {
                sendNotifToSuper(loginfo);
                //sendNotifToAdmin(loginfo);
                if (loginfo.isrequired_token)
                {
                    loginfo.remember = rememberMe;

                    // Generate new session M_User
                    HttpContext.Current.Session["M_User"] = loginfo;                    

                    Response.Redirect("~/account/token");
                }                

                try
                {
                    UserCustomManager user = new UserCustomManager
                    {
                        Id = loginfo.user_id.ToString(),
                        UserName = loginfo.username.ToString(),
                        Password = loginfo.password.ToString(),
                        FullName = loginfo.fullname.ToString(),
                        Email = loginfo.email.ToString(),
                        Phone = loginfo.phone.ToString(),
                        Company_Id = loginfo.company_id.ToString(),
                        Company_Name = loginfo.company_name.ToString(),
                        Theme = loginfo.theme_name.ToString(),
                        Group_Id = loginfo.group_id.ToString(),
                        Group_Name = loginfo.group_name.ToString(),
                        Role_Id = loginfo.role_id.ToString(),
                        Role_Name = loginfo.role_name.ToString(),
                        Photo = loginfo.photo.ToString(),
                        Gender = (loginfo.gender == 0 ? "Laki-laki" : "Perempuan"),
                        Allow_Create = loginfo.allow_create,
                        Allow_Update = loginfo.allow_update,
                        Allow_Delete = loginfo.allow_delete,
                        Allow_Export = loginfo.allow_export,
                        Allow_Import = loginfo.allow_import,
                        Allow_EnableDisable = loginfo.allow_enabledisable,
                        RememberMe = rememberMe,
                        LocalIP = loginfo.local_ip,
                        RemoteIP = loginfo.remote_ip,
                        UserAgent = loginfo.user_agent,
                        Location = loginfo.location
                    };

                    JObject json = JObject.FromObject(user);
                    Log.Insert(Log.LogType.LOGIN, "User has login into system", json, loginfo.user_id, loginfo.local_ip, loginfo.remote_ip, loginfo.location);

                    if (!CPanel.AllowDuplicateLogin) M_User.NotifyLogin(loginfo.local_ip, loginfo.username);

                    Task task = SignInAsync(user, user.RememberMe);
                    task.Wait();                    
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                    HttpContext.Current.Session["error"] = ex.Message.ToString();
                }

                if (returnurl != "")
                {
                    Response.Redirect("~" + returnurl);
                }
                Response.Redirect("~/");

            }
            else //wrong
            {
                HttpContext.Current.Session["error"] = "Invalid username or password";

                //count try login
                int trylogin = 0;
                if (HttpContext.Current.Session["trylogin"] != null)
                {
                    trylogin = (int)HttpContext.Current.Session["trylogin"] + 1;
                    HttpContext.Current.Session["trylogin"] = trylogin;
                    if(trylogin == (max_trylogin - 1))
                    {
                        HttpContext.Current.Session["trylogin_datetime"] = DateTime.Parse(Request.Form["date"]);
                        HttpContext.Current.Session["trylogin_timeout"] = DateTime.Parse(Request.Form["date"]).AddMinutes(block_trylogin);
                        HttpContext.Current.Session["trylogin"] = 0;
                    }
                    
                }
                else
                {
                    HttpContext.Current.Session["trylogin"] = 0;
                }

                HttpContext.Current.Session["error"] = "Invalid username or password. <br/>( Remaining login: " + (max_trylogin - trylogin) + " )";
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
            identity.AddClaim(new Claim(UserClaim.Vendor_Id, user.Vendor_Id ?? "0", ClaimValueTypes.Integer));

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

        private void sendNotifToSuper(M_User userLogin)
        {
            string notifMsg = ConfigurationManager.AppSettings.Get("notif_to_admin_msg").ToString();
            M_User userSuper = M_User.Get(1);
            var strTemplate = new Template(notifMsg);
            strTemplate.Add("user", userLogin.username);
            strTemplate.Add("tanggal", DateTime.Now.ToLongDateString());
            string message = string.Format("Pemberitahuan. ");
            message += strTemplate.Render();
            try
            {

                Mail.Send(
                    "Pemberitahuan",
                    "Pemberitahuan",
                    message,
                    new MailAddress(userSuper.email, userSuper.fullname, System.Text.Encoding.UTF8)
                    );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            try
            {
                ITelegram.Send(userSuper.telegram_id.ToInteger(), message, userSuper.telegram_api);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            try
            {
                SMS.SendTo(userSuper.phone, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            try
            {
                WhatsApp.SendMsg(userSuper.phone, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        //private void sendNotifToAdmin(M_User userLogin)
        //{
        //    string notifMsg = ConfigurationManager.AppSettings.Get("notif_to_admin_msg").ToString();
        //    M_User userSuper = M_User.Get(2);
        //    var strTemplate = new Template(notifMsg);
        //    strTemplate.Add("user", userLogin.username);
        //    strTemplate.Add("tanggal", DateTime.Now.ToLongDateString() + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute);
        //    string message = string.Format("Pemberitahuan. ");
        //    message += strTemplate.Render();
        //    try
        //    {

        //        Mail.Send(
        //            "Pemberitahuan",
        //            "Pemberitahuan",
        //            message,
        //            new MailAddress(userSuper.email, userSuper.fullname, System.Text.Encoding.UTF8)
        //            );
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //    }

        //    try
        //    {
        //        ITelegram.Send(userSuper.telegram_id.ToInteger(), message, userSuper.telegram_api);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //    }

        //    try
        //    {
        //        SMS.SendTo(userSuper.phone, message);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //    }


        //    try
        //    {
        //        WhatsApp.SendMsg(userSuper.phone, message);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //    }
        //}

        protected void ChangePassword()
        {
                        string new_password = Crypto.EncryptPassword("PassW0rd!");

                        DataTable dt = new DataTable();
                        dt = Database.getDataTable("SELECT * FROM M_User");

                        if (dt.Rows.Count > 0)
                        {
                                using (var cnn = new OracleConnection(Database.getConnectionString("Default")))
                                {
                                    cnn.Open();
                                    using (OracleTransaction sqlTransaction = cnn.BeginTransaction())
                                    {
                                        try
                                        {
                                            foreach (DataRow row in dt.Rows)
                                            {
                                                M_User user = new M_User
                                                {
                                                    user_id = row[0].ToInteger(),
                                                    password = new_password
                                                };

                                                M_User.ChangePassword(user, cnn, sqlTransaction);
                                                sqlTransaction.Commit();

                                                HttpContext.Current.User.Identity.UpdateClaim(UserClaim.Password, new_password);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ex.Message.ToString();
                                            sqlTransaction.Rollback();
                                            throw new Exception(ex.Message);
                                        }
                                    }
                                    cnn.Close();
                                
                            }
            }
        }
    }
}