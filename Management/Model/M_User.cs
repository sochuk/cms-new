using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;
using CMS.Helper;
using CMS.Hubs;
using CMS.Notification.Model;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;

namespace CMS.Management.Model
{
    public class M_User
    {
        public int user_id { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string photo { get; set; }
        public string notes { get; set; }
        public int gender { get; set; }
        public string password { get; set; }
        public bool remember { get; set; }

        public int theme_id { get; set; }
        public string theme_name { get; set; }

        public int group_id { get; set; }
        public string group_name { get; set; }

        public int role_id { get; set; }
        public string role_name { get; set; }

        public int company_id { get; set; }
        public string company_name { get; set; }

        public string telegram_id { get; set; }

        //Optional for Login
        public string telegram_api { get; set; }

        public bool isactive { get; set; }
        public bool isdelete { get; set; }
        public bool isrequired_token { get; set; }

        public bool allow_create { get; set; }
        public bool allow_update { get; set; }
        public bool allow_delete { get; set; }
        public bool allow_export { get; set; }
        public bool allow_import { get; set; }
        public bool allow_enabledisable { get; set; }

        public string local_ip { get; set; }
        public string remote_ip { get; set; }
        public string user_agent { get; set; }
        public string location { get; set; }

        public int createby { get; set; }
        public int updateby { get; set; }
        public int deleteby { get; set; }
        public string createdate { get; set; }
        public string updatedate { get; set; }
        public string deletedate { get; set; }


        public static M_User Insert(M_User user, OracleConnection sqlConnection, OracleTransaction sqlTransaction)
        {
            DataTable dt = Database.queryScalar(@"
                INSERT INTO m_user 
                (
                    username,
                    fullname,
                    email,
                    phone,
                    notes,
                    gender,
                    password,
                    theme_id,
                    group_id,
                    company_id,
                    isactive,
                    isdelete,
                    createby,
                    createdate,
                    isrequired_token,
                    telegram_id
                )
                VALUES
                (
                    :username,
                    :fullname,
                    :email,
                    :phone,
                    :notes,
                    :gender,
                    :password,
                    :theme_id,
                    :group_id,
                    :company_id,
                    :isactive,
                    :isdelete,
                    :createby,
                    SYSDATE,
                    :isrequired_token,
                    :telegram_id
                )
                ",
                sqlConnection, sqlTransaction,
                new OracleParameter(":username", user.username),
                new OracleParameter(":fullname", user.fullname),
                new OracleParameter(":email", user.email),
                new OracleParameter(":phone", user.phone),
                new OracleParameter(":notes", user.notes),
                new OracleParameter(":gender", user.gender),
                new OracleParameter(":password", user.password),
                new OracleParameter(":theme_id", user.theme_id),
                new OracleParameter(":group_id", user.group_id),
                new OracleParameter(":company_id", user.company_id),
                new OracleParameter(":isactive", true.ToInteger()),
                new OracleParameter(":isdelete", false.ToInteger()),
                new OracleParameter(":isrequired_token", user.isrequired_token.ToInteger()),
                new OracleParameter(":telegram_id", user.telegram_id),
                new OracleParameter(":createby", M_User.getUserId())
                );

            return user;
        }

        public static M_User Update(M_User user, OracleConnection sqlConnection, OracleTransaction sqlTransaction)
        {
            Database.querySQL(@"
                UPDATE m_user 
                SET 
                fullname=:fullname, 
                email=:email, 
                phone=:phone,
                gender=:gender,
                theme_id=:theme_id,
                group_id=:group_id,
                company_id=:company_id,
                isrequired_token=:isrequired_token,
                telegram_id=:telegram_id,
                updateby=:updateby,
                updatedate=SYSDATE
                WHERE user_id=:user_id",
                sqlConnection, sqlTransaction,
                new OracleParameter(":user_id", user.user_id),
                new OracleParameter(":fullname", user.fullname),
                new OracleParameter(":email", user.email),
                new OracleParameter(":phone", user.phone),
                new OracleParameter(":notes", user.notes),
                new OracleParameter(":gender", user.gender),
                new OracleParameter(":theme_id", user.theme_id),
                new OracleParameter(":group_id", user.group_id),
                new OracleParameter(":company_id", user.company_id),
                new OracleParameter(":isrequired_token", user.isrequired_token.ToInteger()),
                new OracleParameter(":telegram_id", user.telegram_id),
                new OracleParameter(":updateby", M_User.getUserId())
                );

            return user;
        }

        public static M_User Delete(M_User user, OracleConnection sqlConnection, OracleTransaction sqlTransaction)
        {
            Database.querySQL(@"
                UPDATE m_user 
                SET isdelete=:isdelete, 
                deletedate=SYSDATE,
                deleteby=:deleteby
                WHERE user_id=:user_id",
                sqlConnection, sqlTransaction,
                new OracleParameter(":isdelete", true.ToInteger()),
                new OracleParameter(":user_id", user.user_id),
                new OracleParameter(":deleteby", M_User.getUserId())
                );

            return user;
        }

        public static M_User Disable(M_User user, OracleConnection sqlConnection, OracleTransaction sqlTransaction)
        {
            string sql = String.Format(@"UPDATE m_user
                SET 
                isactive=:isactive, 
                updatedate=SYSDATE,
                updateby=:updateby
                WHERE user_id=:user_id", M_User.getDatabaseName());

            Database.querySQL(sql, sqlConnection, sqlTransaction,
                new OracleParameter(":isactive", false.ToInteger()),
                new OracleParameter(":user_id", user.user_id),
                new OracleParameter(":updateby", M_User.getUserId())
                );

            return user;
        }

        public static M_User Enable(M_User user, OracleConnection sqlConnection, OracleTransaction sqlTransaction)
        {
            Database.querySQL(@"
                UPDATE m_user 
                SET isactive=:isactive, 
                updatedate=SYSDATE,
                updateby=:updateby
                WHERE user_id=:user_id",
                sqlConnection, sqlTransaction,
                new OracleParameter(":isactive", true.ToInteger()),
                new OracleParameter(":user_id", user.user_id),
                new OracleParameter(":updateby", M_User.getUserId())
                );

            return user;
        }

        public static bool IsExist(M_User data)
        {
            DataTable dt = Database.getDataTable("SELECT * FROM m_user  where username=:username",
                new OracleParameter(":username", data.username));

            if (dt.Rows.Count > 0) return true;

            return false;
        }

        public static M_User ChangePassword(M_User user, OracleConnection sqlConnection, OracleTransaction sqlTransaction)
        {
            Database.querySQL(@"
                UPDATE m_user 
                SET password=:password, 
                updatedate=SYSDATE,
                updateby=:updateby
                WHERE user_id=:user_id",
                sqlConnection, sqlTransaction,
                out OracleParameter[] param_out,
                new OracleParameter(":password", user.password),
                new OracleParameter(":user_id", user.user_id),
                new OracleParameter(":updateby", M_User.getUserId())
                );

            JObject json = param_out.toJObject();
            Log.Insert(Log.LogType.UPDATE, "Change/update password", json, sqlConnection, sqlTransaction);

            return user;
        }

        public static M_User Login(string username, string password, string local_ip, string remote_ip, string UserAgent, string Location)
        {
            DataTable dt = Database.getDataTable(@"
                SELECT U.*,COALESCE(T.theme_Name, 'Default') theme_name, G.group_name, C.company_name, R.role_id, R.role_name,
                R.allow_create,R.allow_update,R.allow_delete,R.allow_enabledisable,R.allow_export,R.allow_import,Conf.telegram_api
                FROM m_user U
                LEFT JOIN m_theme T ON U.theme_id=T.theme_id
                LEFT JOIN m_group G ON U.group_id=G.group_id
                LEFT JOIN m_role R ON G.role_id=R.role_id
                LEFT JOIN m_company C ON U.company_id=C.company_id
                LEFT JOIN m_configuration Conf ON C.company_id=Conf.company_id
                WHERE U.username=:username AND U.password=:password AND U.isactive=1 AND U.isdelete=0",
                new OracleParameter(":username", username),
                new OracleParameter(":password", password)
                );

            M_User user = null;
            if (dt.Rows.Count == 1)
            {
                user = new M_User();
                user.user_id = dt.Rows[0]["user_id"].ToString().ToInteger();
                user.username = dt.Rows[0]["username"].ToString();
                user.fullname = dt.Rows[0]["fullname"].ToString();
                user.email = dt.Rows[0]["email"].ToString();
                user.phone = dt.Rows[0]["phone"].ToString();

                dt = dt.ResolvePhotoUrl();

                //Need for Owin Session
                user.photo = Application.GetHost() + dt.Rows[0]["photo"].ToString();
                user.notes = dt.Rows[0]["notes"].ToString();
                user.gender = dt.Rows[0]["gender"].ToString().ToInteger();
                user.password = dt.Rows[0]["password"].ToString();
                user.theme_id = dt.Rows[0]["theme_id"].ToString().ToInteger();
                user.theme_name = dt.Rows[0]["theme_name"].ToString();
                user.role_id = dt.Rows[0]["role_id"].ToString().ToInteger();
                user.role_name = dt.Rows[0]["role_name"].ToString();
                user.group_id = dt.Rows[0]["group_id"].ToString().ToInteger();
                user.group_name = dt.Rows[0]["group_name"].ToString();
                user.company_id = dt.Rows[0]["company_id"].ToString().ToInteger();
                user.company_name = dt.Rows[0]["company_name"].ToString();
                user.allow_create = dt.Rows[0]["allow_create"].ToString().ToBoolean();
                user.allow_update = dt.Rows[0]["allow_update"].ToString().ToBoolean();
                user.allow_delete = dt.Rows[0]["allow_delete"].ToString().ToBoolean();
                user.allow_enabledisable = dt.Rows[0]["allow_enabledisable"].ToString().ToBoolean();
                user.allow_export = dt.Rows[0]["allow_export"].ToString().ToBoolean();
                user.allow_import = dt.Rows[0]["allow_import"].ToString().ToBoolean();
                user.isrequired_token = dt.Rows[0]["isrequired_token"].ToString().ToBoolean();
                user.telegram_id = dt.Rows[0]["telegram_id"].ToString();
                user.telegram_api = dt.Rows[0]["telegram_api"].ToString();

                user.local_ip = local_ip;
                user.remote_ip = remote_ip;
                user.user_agent = UserAgent;
                user.location = Location;

                try
                {
                    //Create session user_id used for timeout auth owin
                    M_User.GenerateSessionUser(user);
                }
                catch (Exception ex) { ex.Message.ToString(); }
            }

            dt.Dispose();

            return user;
        }

        public static M_User Get(int user_id)
        {
            DataTable dt = Database.getDataTable(@"
                SELECT U.*,COALESCE(T.theme_Name, 'Default') theme_name, G.group_name, C.company_name, R.role_id, R.role_name,
                R.allow_create, R.allow_update, R.allow_delete, R.allow_enabledisable, R.allow_export, R.allow_import, Conf.telegram_api
                FROM m_user U
                LEFT JOIN m_theme T ON U.theme_id = T.theme_id
                LEFT JOIN m_group G ON U.group_id = G.group_id
                LEFT JOIN m_role R ON G.role_id = R.role_id
                LEFT JOIN m_company C ON U.company_id = C.company_id
                LEFT JOIN m_configuration Conf ON C.company_id = Conf.company_id
                WHERE U.user_id =:user_id AND U.isactive = 1 AND U.isdelete = 0",
                new OracleParameter(":user_id", user_id));

            if (dt.Rows.Count == 1)
            {
                M_User user = new M_User();
                user.user_id = dt.Rows[0]["user_id"].ToString().ToInteger();
                user.username = dt.Rows[0]["username"].ToString();
                user.fullname = dt.Rows[0]["fullname"].ToString();
                user.email = dt.Rows[0]["email"].ToString();
                user.phone = dt.Rows[0]["phone"].ToString();

                dt = dt.ResolvePhotoUrl();

                //Need for Owin Session
                user.photo = Application.GetHost() + dt.Rows[0]["photo"].ToString();
                user.notes = dt.Rows[0]["notes"].ToString();
                user.gender = dt.Rows[0]["gender"].ToString().ToInteger();
                user.password = dt.Rows[0]["password"].ToString();
                user.theme_id = dt.Rows[0]["theme_id"].ToString().ToInteger();
                user.theme_name = dt.Rows[0]["theme_name"].ToString();
                user.role_id = dt.Rows[0]["role_id"].ToString().ToInteger();
                user.role_name = dt.Rows[0]["role_name"].ToString();
                user.group_id = dt.Rows[0]["group_id"].ToString().ToInteger();
                user.group_name = dt.Rows[0]["group_name"].ToString();
                user.company_id = dt.Rows[0]["company_id"].ToString().ToInteger();
                user.company_name = dt.Rows[0]["company_name"].ToString();
                user.allow_create = dt.Rows[0]["allow_create"].ToString().ToBoolean();
                user.allow_update = dt.Rows[0]["allow_update"].ToString().ToBoolean();
                user.allow_delete = dt.Rows[0]["allow_delete"].ToString().ToBoolean();
                user.allow_enabledisable = dt.Rows[0]["allow_enabledisable"].ToString().ToBoolean();
                user.allow_export = dt.Rows[0]["allow_export"].ToString().ToBoolean();
                user.allow_import = dt.Rows[0]["allow_import"].ToString().ToBoolean();
                user.isrequired_token = dt.Rows[0]["isrequired_token"].ToString().ToBoolean();
                user.telegram_id = dt.Rows[0]["telegram_id"].ToString();
                user.telegram_api = dt.Rows[0]["telegram_api"].ToString();

                return user;
            }

            return null;
        }

        public static void Logout()
        {
            try
            {
                //Clear all the session user
                HttpContext.Current.Session.Clear();
            }
            catch (Exception ex) { ex.Message.ToString(); }

        }

        public static void OwinSessionExpired()
        {
            if (M_User.getSessionUserId() == 0) return;

            var obj = new object[]
            {
                M_User.getUserId(),
                M_User.getUsername(),
                M_User.getFullname()
            };

            JObject json = JObject.FromObject(obj);
            Log.Insert(Log.LogType.EXPIRED, "User has expired session from system", json);

            try
            {
                //Clear all the session user
                HttpContext.Current.Session.Clear();
            }
            catch (Exception ex) { ex.Message.ToString(); }
        }

        public static void NotifyLogin(string local_ip, string username)
        {
            string message = @"<p>Another device has login using this account.</p>";
            message += "<p>Click <strong>\"Use Here\"</strong> button to using this account or <strong>\"Leave\"</strong> button to leave this session.</p>";
            message += "<div class=\"mt-3 mb-1\"><button class=\"btn mdc-button mdc-button--raised filled-button--success mdc-ripple-upgraded\" type=\"button\" id=\"useHere\">Use Here</button>";
            message += "<button class=\"btn mdc-button mdc-button--outlined outlined-button--dark mdc-ripple-upgraded ml-2\" type=\"button\" id=\"logout\">Leave</button></div>";

            var hub = GlobalHost.ConnectionManager.GetHubContext<DefaultHub>();
            hub.Clients.User(username).getDuplicateLogin(local_ip, "Duplicate login warning", message);
        }

        public static M_User UpdatePhoto(M_User user, OracleConnection sqlConnection, OracleTransaction sqlTransaction)
        {
            Database.querySQL(@"
                UPDATE m_user 
                SET 
                photo=:photo,
                updateby=:updateby,
                updatedate=SYSDATE
                WHERE user_id=:user_id",
                sqlConnection, sqlTransaction,
                new OracleParameter(":user_id", user.user_id),
                new OracleParameter(":photo", user.photo),
                new OracleParameter(":updateby", M_User.getUserId())
                );

            if (user.user_id == M_User.getUserId()) HttpContext.Current.User.Identity.UpdateClaim(UserClaim.Photo, user.photo);

            return user;
        }

        public static M_User UpdatePhoto(M_User user, byte[] photo, OracleConnection sqlConnection, OracleTransaction sqlTransaction)
        {
            DataTable old_photo = Database.getDataTable("SELECT photo FROM m_user  WHERE user_id=:user_id",
                new OracleParameter(":user_id", user.user_id));

            string directoryName = HttpContext.Current.Server.MapPath("~/Upload/Images/");
            string fileName = directoryName + Crypto.RandomString(16) + ".jpg";
            string appPath = HttpContext.Current.Server.MapPath("/").ToLower();
            string finalpath = string.Format("/{0}", fileName.ToLower().Replace(appPath, "").Replace(@"\", "/"));

            if (old_photo.Rows.Count > 0)
            {
                var old_img = Converter.GetByteArrayFromImage(HttpContext.Current.Server.MapPath(old_photo.Rows[0]["photo"].ToString()));

                if (photo != null) //photo not null
                {
                    if (photo.SequenceEqual(old_img))
                    {
                        user.photo = old_photo.Rows[0]["photo"].ToString();
                    }
                    else
                    {
                        using (MemoryStream img_stream = new MemoryStream(photo))
                        {
                            using (System.Drawing.Image image = System.Drawing.Image.FromStream(img_stream))
                            {
                                if (!System.IO.Directory.Exists(directoryName))
                                {
                                    System.IO.Directory.CreateDirectory(directoryName);
                                }
                                var img = Converter.ResizeImage(image);
                                Converter.SaveAsJpeg(img, fileName);
                            }
                        }

                        user.photo = finalpath;
                    }
                }
                else //photo is null
                {
                    user.photo = string.Empty;
                }

            }
            else
            {
                if (photo != null) //photo not null
                {
                    using (MemoryStream img_stream = new MemoryStream(photo))
                    {
                        using (System.Drawing.Image image = System.Drawing.Image.FromStream(img_stream))
                        {
                            if (!System.IO.Directory.Exists(directoryName))
                            {
                                System.IO.Directory.CreateDirectory(directoryName);
                            }
                            var img = Converter.ResizeImage(image);
                            Converter.SaveAsJpeg(img, fileName);
                        }

                        user.photo = finalpath;
                    }
                }
                else
                {
                    user.photo = string.Empty;
                }

            }

            Database.querySQL(@"
                UPDATE m_user 
                SET 
                photo=:photo,
                updateby=:updateby,
                updatedate=SYSDATE
                WHERE user_id=:user_id",
                sqlConnection, sqlTransaction,
                out OracleParameter[] param_out,
                new OracleParameter(":user_id", user.user_id),
                new OracleParameter(":photo", user.photo),
                new OracleParameter(":updateby", M_User.getUserId())
                );

            if (user.user_id == M_User.getUserId()) HttpContext.Current.User.Identity.UpdateClaim(UserClaim.Photo, user.photo);

            return user;
        }

        public static M_User UpdateNote(M_User user, OracleConnection sqlConnection, OracleTransaction sqlTransaction)
        {
            Database.querySQL(@"
                UPDATE m_user 
                SET 
                notes=:notes,
                updateby=:updateby,
                updatedate=SYSDATE
                WHERE user_id=:user_id",
                sqlConnection, sqlTransaction,
                new OracleParameter(":user_id", user.user_id),
                new OracleParameter(":notes", user.notes),
                new OracleParameter(":updateby", M_User.getUserId())
                );

            return user;
        }

        public static M_User UpdateProfile(M_User user, OracleConnection sqlConnection, OracleTransaction sqlTransaction)
        {
            Database.querySQL(@"
                UPDATE m_user 
                SET 
                fullname=:fullname, 
                email=:email, 
                phone=:phone,
                gender=:gender,
                theme_id=:theme_id,
                updateby=:updateby,
                updatedate=SYSDATE
                WHERE user_id=:user_id",
                sqlConnection, sqlTransaction,
                new OracleParameter(":user_id", user.user_id),
                new OracleParameter(":fullname", user.fullname),
                new OracleParameter(":email", user.email),
                new OracleParameter(":phone", user.phone),
                new OracleParameter(":gender", user.gender),
                new OracleParameter(":theme_id", user.theme_id),
                new OracleParameter(":updateby", M_User.getUserId())
                );

            HttpContext.Current.User.Identity.UpdateClaim(UserClaim.Fullname, user.fullname);
            HttpContext.Current.User.Identity.UpdateClaim(UserClaim.Email, user.email);
            HttpContext.Current.User.Identity.UpdateClaim(UserClaim.Theme_Id, user.theme_id.ToString());

            return user;
        }

        public static bool hasDirectEmployee(int user_id = 0)
        {
            DataTable dt = new DataTable();
            if (user_id > 0)
            {
                dt = M_User.SelectDirectEmployee(user_id);
            }
            else
            {
                dt = M_User.SelectDirectEmployee(M_User.getUserId());
            }

            if (dt.Rows.Count == 0) return false;
            return true;
        }

        /// <summary>
        /// Generate Session User on current User login
        /// </summary>
        /// <returns></returns>
        public static void GenerateSessionUser(M_User user)
        {
            try
            {
                HttpContext.Current.Session["local_ip"] = user.local_ip;
                HttpContext.Current.Session["remote_ip"] = user.remote_ip;
                HttpContext.Current.Session["user_agent"] = user.user_agent;
                HttpContext.Current.Session["location"] = user.location;

                HttpContext.Current.Session["username"] = user.username;
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Get User Id on current User login
        /// </summary>
        /// <returns></returns>
        public static int getUserId()
        {
            int user_id = (HttpContext.Current.User.Identity.Get_Id() == "" ||
                HttpContext.Current.User.Identity.Get_Id() == string.Empty)
                ? getSessionUserId()
                : HttpContext.Current.User.Identity.Get_Id().ToInteger();

            return user_id;
        }

        /// <summary>
        /// Get Session User Id on current User login
        /// </summary>
        /// <returns></returns>
        public static int getSessionUserId()
        {
            try
            {
                return HttpContext.Current.Session["user_id"] == null ? 0 : HttpContext.Current.Session["user_id"].ToString().ToInteger();
            }
            catch
            {
                return 0;
            }

        }

        /// <summary>
        /// Get Session Local IP Address on current User login
        /// </summary>
        /// <returns></returns>
        public static string getSessionLocalIPAddress()
        {
            try
            {
                return HttpContext.Current.Session["local_ip"] == null ? string.Empty : HttpContext.Current.Session["local_ip"].ToString();
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// Get Session Remote  IP Address on current User login
        /// </summary>
        /// <returns></returns>
        public static string getSessionRemoteIPAddress()
        {
            try
            {
                return HttpContext.Current.Session["remote_ip"] == null ? string.Empty : HttpContext.Current.Session["remote_ip"].ToString();
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// Get Session User Agent on current User login
        /// </summary>
        /// <returns></returns>
        public static string getSessionUserAgent()
        {
            try
            {
                return HttpContext.Current.Session["user_agent"] == null ? string.Empty : HttpContext.Current.Session["user_agent"].ToString();
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// Get Session User Agent on current User login
        /// </summary>
        /// <returns></returns>
        public static string getSessionLocation()
        {
            try
            {
                return HttpContext.Current.Session["location"] == null ? string.Empty : HttpContext.Current.Session["location"].ToString();
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// Get Company Id User on current User login
        /// </summary>
        /// <returns></returns>
        public static int getCompanyId()
        {
            return HttpContext.Current.User.Identity.Get_CompanyId().ToInteger();
        }

        /// <summary>
        /// Get Photo User on current User login
        /// </summary>
        /// <returns></returns>
        public static string getPhoto()
        {
            return HttpContext.Current.User.Identity.Get_Photo();
        }

        /// <summary>
        /// Get Phone/WA User on current User login
        /// </summary>
        /// <returns></returns>
        public static string getPhone()
        {
            return HttpContext.Current.User.Identity.Get_Phone();
        }

        /// <summary>
        /// Get M_User.getUserId() User on current User login
        /// </summary>
        /// <returns></returns>
        public static string getUsername()
        {
            try
            {
                return HttpContext.Current.User.Identity.Get_UserName();
            }
            catch
            {
                return HttpContext.Current.Session["username"].ToString();
            }

        }

        /// <summary>
        /// Get M_User.getUserId() from user_id
        /// </summary>
        /// <returns></returns>
        public static string getUsername(int user_id)
        {
            DataTable dt = Database.getDataTable("SELECT username from m_user  WHERE user_id=:user_id",
                new OracleParameter(":user_id", user_id));
            if (!string.IsNullOrEmpty(dt.Rows[0]["username"].ToString()))
            {
                return dt.Rows[0]["username"].ToString();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get Fullname User on current User login
        /// </summary>
        /// <returns></returns>
        public static string getFullname()
        {
            return HttpContext.Current.User.Identity.Get_Fullname();
        }

        /// <summary>
        /// Get Initial User on current User login
        /// </summary>
        /// <returns></returns>
        public static string getInitial()
        {
            string fullname = HttpContext.Current.User.Identity.Get_Fullname();
            string initial = string.Empty;
            var split = fullname.Split(' ');
            if (split.Count() > 1)
            {
                for (int x = 0; x <= 1; x++)
                {
                    initial += split[x].Substring(0, 1);
                }
            }
            else
            {
                initial = split[0].Substring(0, 1);
            }
            return initial;
        }

        /// <summary>
        /// Get Initial User
        /// </summary>
        /// <returns></returns>
        public static string getInitial(string fullname)
        {
            string initial = string.Empty;
            var split = fullname.Split(' ');
            if (split.Count() > 1)
            {
                for (int x = 0; x <= 1; x++)
                {
                    initial += split[x].Substring(0, 1);
                }
            }
            else
            {
                initial = split[0].Substring(0, 1);
            }

            return initial;
        }

        /// <summary>
        /// Get Fullname User on selected User with specific ID
        /// <para>ID : user_id</para>
        /// </summary>
        /// <returns></returns>
        public static string getFullname(int Id)
        {
            DataTable user = M_User.SelectUser(Id);
            return user.AsEnumerable().Select(x => x["fullname"]).FirstOrDefault().ToString();
        }

        public static string getFullname(int Id, OracleConnection sqlConnection, OracleTransaction sqlTransaction)
        {
            DataTable user = M_User.SelectUser(Id, sqlConnection, sqlTransaction);
            return user.AsEnumerable().Select(x => x["fullname"]).FirstOrDefault().ToString();
        }

        /// <summary>
        /// Get Database Name on selected User
        /// </summary>
        /// <returns></returns>
        public static string getDatabaseName()
        {
            try
            {
                return HttpContext.Current.User.Identity.Get_CompanyName();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get Remote IP Address login on selected User
        /// </summary>
        /// <returns></returns>
        public static string getRemoteIP()
        {
            return HttpContext.Current.User.Identity.Get_RemoteIP();
        }

        /// <summary>
        /// Get Local IP Address login on selected User
        /// </summary>
        /// <returns></returns>
        public static string getLocalIP()
        {
            return HttpContext.Current.User.Identity.Get_LocalIP();
        }

        /// <summary>
        /// Get User Agent login on selected User
        /// </summary>
        /// <returns></returns>
        public static string getUserAgent()
        {
            return HttpContext.Current.User.Identity.Get_UserAgent();
        }

        /// <summary>
        /// Get Login Location login on selected User
        /// </summary>
        /// <returns></returns>
        public static string getLocation()
        {
            return HttpContext.Current.User.Identity.Get_LoginLocation();
        }

        /// <summary>
        /// Get Division on current User
        /// </summary>
        /// <returns></returns>
        public static string getDivision()
        {
            string sql = string.Format(@"
              SELECT O.division_id, DI.division_name, DI.department_id, DE.department_name FROM [m_organization]  O
              LEFT JOIN m_division  DI ON O.division_id=DI.division_id
              LEFT JOIN m_department  DE ON DI.department_id=DE.department_id
              WHERE user_id=:user_id", M_User.getDatabaseName());

            DataTable dt = Database.getDataTable(sql, new OracleParameter(":user_id", M_User.getUserId()));

            if (dt.Rows.Count == 1) return dt.Rows[0]["division_name"].ToString();
            return string.Empty;
        }

        /// <summary>
        /// Get Department on current User
        /// </summary>
        /// <returns></returns>
        public static string getDepartment()
        {
            string sql = string.Format(@"
              SELECT O.division_id, DI.division_name, DI.department_id, DE.department_name FROM [m_organization]  O
              LEFT JOIN m_division  DI ON O.division_id=DI.division_id
              LEFT JOIN m_department  DE ON DI.department_id=DE.department_id
              WHERE user_id=:user_id", M_User.getDatabaseName());

            DataTable dt = Database.getDataTable(sql, new OracleParameter(":user_id", M_User.getUserId()));

            if (dt.Rows.Count == 1) return dt.Rows[0]["department_name"].ToString();
            return string.Empty;
        }

        /// <summary>
        /// Get all user in current company
        /// </summary>
        /// <returns></returns>
        public static DataTable SelectAll()
        {
            string sql = string.Format(@"SELECT U.* ,COALESCE(T.theme_name, 'Default') theme_name, T.theme_desc, G.group_name, 
                R.role_name, C.company_name, C.company_desc, CASE WHEN U.gender=0 THEN 'Laki-laki' ELSE 'Perempuan' END AS gender_desc
                FROM m_user  U 
                LEFT JOIN m_theme  T ON U.theme_id=T.theme_id
                LEFT JOIN m_group  G ON U.group_id=G.group_id
                LEFT JOIN m_role  R ON R.role_id=G.role_id
                LEFT JOIN m_company  C ON U.company_id=C.company_id
                WHERE U.isdelete=0 
                AND U.company_id=:company_id
                ORDER BY U.isactive DESC, U.user_id DESC", M_User.getDatabaseName());

            DataTable dt = Database.getDataTable(sql, new OracleParameter(":company_id", M_User.getCompanyId()));

            dt = dt.ResolvePhotoUrl();

            List<M_User> data = new List<M_User>();
            foreach (DataRow row in dt.Rows)
            {
                data.Add(
                    new M_User()
                    {
                        user_id = row["user_id"].ToInteger(),
                        username = row["username"].ToString(),
                        fullname = row["fullname"].ToString(),
                        email = row["email"].ToString(),
                        telegram_id = row["telegram_id"].ToString(),
                        phone = row["phone"].ToString(),
                        photo = row["photo"].ToString(),
                        notes = row["notes"].ToString(),
                        gender = row["gender"].ToInteger(),
                        group_id = row["group_id"].ToInteger(),
                        theme_id = row["theme_id"].ToInteger(),
                        company_id = row["company_id"].ToInteger(),
                        isrequired_token = row["isrequired_token"].ToBoolean(),
                        isactive = row["isactive"].ToBoolean(),
                        isdelete = row["isdelete"].ToBoolean(),
                        createby = row["createby"].ToInteger(),
                        updateby = row["updateby"].ToInteger(),
                        deleteby = row["deleteby"].ToInteger(),
                        createdate = row["createdate"].ToString(),
                        updatedate = row["updatedate"].ToString(),
                        deletedate = row["deletedate"].ToString(),
                    }
                );
            }

            DataTable mdt = data.ToDataTable();
            return mdt;
        }

        /// <summary>
        /// Get all user in current company
        /// </summary>
        /// <returns></returns>
        public static DataTable BindData()
        {
            string sql = string.Format(@"
                SELECT U.user_id, U.fullname FROM m_user  U
                WHERE U.company_id=:company_id", M_User.getDatabaseName());

            DataTable dt = Database.getDataTable(sql, new OracleParameter(":company_id", M_User.getCompanyId()));
            return dt;
        }

        /// <summary>
        /// Get all user in current company except login user
        /// </summary>
        /// <returns></returns>
        public static DataTable SelectAllExceptMe()
        {
            DataTable data = M_User.SelectAll();
            DataTable dt = data.Select("user_id<>" + M_User.getUserId()).CopyToDataTable();
            return dt;
        }

        /// <summary>
        /// Get data on selected user _id
        /// </summary>
        /// <returns></returns>
        public static DataTable SelectUser(int user_id)
        {
            string sql = string.Format(@"
                SELECT U.*, COALESCE(T.theme_name, 'Default') theme_name, T.theme_desc, G.group_name, R.role_name, C.company_name, C.company_desc
                FROM m_user  U 
                LEFT JOIN m_theme  T ON U.theme_id=T.theme_id
                LEFT JOIN m_group  G ON U.group_id=G.group_id
                LEFT JOIN m_role  R ON R.role_id=G.role_id
                LEFT JOIN m_company  C ON U.company_id=C.company_id
                WHERE U.isdelete=0 
                AND U.user_id=:user_id 
                AND U.company_id=:company_id", M_User.getDatabaseName());

            DataTable dt = Database.getDataTable(sql,
                new OracleParameter(":user_id", user_id),
                new OracleParameter(":company_id", M_User.getCompanyId()));

            dt = dt.ResolvePhotoUrl();

            return dt;
        }

        public static DataTable SelectUser(int user_id, OracleConnection sqlConnection, OracleTransaction sqlTransaction)
        {
            string sql = string.Format(@"
                SELECT U.*, COALESCE(T.theme_name, 'Default') theme_name, T.theme_desc, G.group_name, R.role_name, C.company_name, C.company_desc
                FROM m_user  U 
                LEFT JOIN m_theme  T ON U.theme_id=T.theme_id
                LEFT JOIN m_group  G ON U.group_id=G.group_id
                LEFT JOIN m_role  R ON R.role_id=G.role_id
                LEFT JOIN m_company  C ON U.company_id=C.company_id
                WHERE U.isdelete=0 
                AND U.user_id=:user_id 
                AND U.company_id=:company_id", M_User.getDatabaseName());

            DataTable dt = Database.getDataTable(sql, sqlConnection, sqlTransaction,
                new OracleParameter(":user_id", user_id),
                new OracleParameter(":company_id", M_User.getCompanyId()));

            dt = dt.ResolvePhotoUrl();

            return dt;
        }

        /// <summary>
        /// Get data on current login user
        /// </summary>
        /// <returns></returns>
        public static DataTable SelectCurrentUser()
        {
            string sql = @"SELECT U.*, COALESCE(T.theme_name, 'Default') theme_name, T.theme_desc, 
                G.group_name, R.role_name, C.company_name, C.company_desc
                FROM m_user U 
                LEFT JOIN m_theme T ON U.theme_id=T.theme_id
                LEFT JOIN m_group G ON U.group_id=G.group_id
                LEFT JOIN m_role R ON R.role_id=G.role_id
                LEFT JOIN m_company C ON U.company_id=C.company_id
                WHERE U.isdelete=0 AND user_id=:user_id";

            DataTable dt = Database.getDataTable(sql,
                new OracleParameter("user_id", M_User.getUserId()));

            dt = dt.ResolvePhotoUrl();

            List<M_User> data = new List<M_User>();
            foreach (DataRow row in dt.Rows)
            {
                data.Add(
                    new M_User()
                    {
                        user_id = row["user_id"].ToInteger(),
                        username = row["username"].ToString(),
                        fullname = row["fullname"].ToString(),
                        email = row["email"].ToString(),
                        telegram_id = row["telegram_id"].ToString(),
                        phone = row["phone"].ToString(),
                        photo = row["photo"].ToString(),
                        notes = row["notes"].ToString(),
                        gender = row["gender"].ToInteger(),
                        group_id = row["group_id"].ToInteger(),
                        group_name = row["group_name"].ToString(),
                        theme_id = row["theme_id"].ToInteger(),
                        theme_name = row["theme_name"].ToString(),
                        company_id = row["company_id"].ToInteger(),
                        company_name = row["company_name"].ToString(),
                        isrequired_token = row["isrequired_token"].ToBoolean(),
                        isactive = row["isactive"].ToBoolean(),
                        isdelete = row["isdelete"].ToBoolean(),
                        createby = row["createby"].ToInteger(),
                        updateby = row["updateby"].ToInteger(),
                        deleteby = row["deleteby"].ToInteger(),
                        createdate = row["createdate"].ToString(),
                        updatedate = row["updatedate"].ToString(),
                        deletedate = row["deletedate"].ToString(),
                    }
                );
            }

            DataTable mdt = data.ToDataTable();
            return mdt;
        }

        /// <summary>
        /// Get user approval (Bottom to Up) based on selected user_id
        /// </summary>
        /// <param name="user_id">Selected user_id</param>
        /// <param name="useHierarchy">Enable/Disable hierarchy approval</param>
        /// <returns></returns>
        public static DataTable SelectDirectApproval(int user_id, bool useHierarchy = false)
        {
            string sql = @"
            DECLARE @temp TABLE (
                user_id INT NOT NULL,
                user_root INT NOT NULL
            );
            DECLARE @temp2 TABLE (
                user_id INT NOT NULL,
                user_root INT NOT NULL
            );
            DECLARE @user_root INT;

            INSERT INTO @temp
	            SELECT O.user_id,O.user_root FROM m_organization  O WHERE O.user_id=:user_id;
            INSERT INTO @temp2
	            SELECT O.user_id,O.user_root FROM m_organization  O WHERE O.user_id=:user_id;

            SET @user_root = (SELECT user_root FROM @temp);
            WHILE(@user_root != 0)
            BEGIN
            DELETE FROM @temp;
            INSERT INTO @temp
	            SELECT O.user_id,O.user_root FROM m_organization  O WHERE O.user_id=@user_root;

            INSERT INTO @temp2
	            SELECT O.user_id,O.user_root FROM m_organization  O WHERE O.user_id=@user_root;

            SET @user_root = (SELECT user_root FROM @temp);
            END

            
             ";

            if (useHierarchy)
            {
                sql += @"
                SELECT TOP 1 U.*, O.can_approve FROM @temp2 A
                LEFT JOIN m_user  U ON A.user_id=U.user_id
                LEFT JOIN m_organization  O ON U.user_id=O.user_id
                WHERE A.user_id!=:user_id 
                AND U.isdelete=0 
                AND U.isactive=1
                AND U.company_id=:company_id;";
            }
            else
            {
                sql += @"
                SELECT U.*, O.can_approve FROM @temp2 A
                LEFT JOIN m_user  U ON A.user_id=U.user_id
                LEFT JOIN m_organization  O ON U.user_id=O.user_id
                WHERE A.user_id!=:user_id 
                AND U.isdelete=0 
                AND U.isactive=1
                AND U.company_id=:company_id;";
            }

            DataTable dt = Database.getDataTable(sql,
                new OracleParameter(":user_id", user_id),
                new OracleParameter(":company_id", M_User.getCompanyId()));
            DataTable final = new DataTable();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["photo"].ToString() == null || row["photo"].ToString() == string.Empty)
                    {
                        row["photo"] = Application.GetPath() + "/Content/images/unknown.jpg";
                    }
                    else
                    {
                        if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(row["photo"].ToString())))
                        {
                            row["photo"] = Application.GetPath() + "/Content/images/unknown.jpg";
                        }
                    }
                }

                //Limit until can approved is 0
                final = dt.Clone();
                foreach (DataRow row in dt.Rows)
                {
                    if ((bool)row["can_approve"] == false)
                    {
                        final.ImportRow(row);
                        break;
                    }
                    else
                    {
                        final.ImportRow(row);
                    }
                }
            }

            return final;
        }

        /// <summary>
        /// Get user approval (Bottom to Up) based on selected user_id
        /// </summary>
        /// <param name="useHierarchy">Enable/Disable hierarchy approval</param>
        /// <returns></returns>
        public static M_User SelectDirectApproval(bool useHierarchy = false)
        {
            DataTable dt = M_User.SelectDirectApproval(M_User.getUserId(), useHierarchy);
            M_User user = new M_User();
            if (dt.Rows.Count > 0)
            {
                user.user_id = dt.Rows[0]["user_id"].ToInteger();
                user.username = dt.Rows[0]["username"].ToString();
                user.fullname = dt.Rows[0]["fullname"].ToString();
                user.email = dt.Rows[0]["email"].ToString();
                user.telegram_id = dt.Rows[0]["telegram_id"].ToString();
                user.phone = dt.Rows[0]["phone"].ToString();
                user.photo = dt.Rows[0]["photo"].ToString();
                user.gender = dt.Rows[0]["gender"].ToInteger();
                user.group_id = dt.Rows[0]["group_id"].ToInteger();
                user.company_id = dt.Rows[0]["company_id"].ToInteger();
            }

            return user;
        }

        /// <summary>
        /// Get user employee (Up to Bottom) based on selected user_id
        /// </summary>
        /// <param name="user_id">Selected user_id</param>
        /// <returns></returns>
        public static DataTable SelectDirectEmployee(int user_id)
        {
            string sql = @"
            DECLARE @temp TABLE (
                user_id INT NOT NULL,
                user_root INT NOT NULL
            );
            DECLARE @temp2 TABLE (
                user_id INT NOT NULL,
                user_root INT NOT NULL
            );
            DECLARE @employee INT;

            INSERT INTO @temp 
	            SELECT O.user_id,O.user_root FROM m_organization  O WHERE O.user_root=:user_id;
            INSERT INTO @temp2 
	            SELECT O.user_id,O.user_root FROM m_organization  O WHERE O.user_root=:user_id;

            SET @employee = (SELECT TOP 1 user_id FROM @temp);
            WHILE(@employee IS NOT NULL)
            BEGIN
            DELETE FROM @temp;
            INSERT INTO @temp
	            SELECT O.user_id,O.user_root FROM m_organization  O WHERE O.user_root=@employee;

            INSERT INTO @temp2
	            SELECT O.user_id,O.user_root FROM m_organization  O WHERE O.user_root=@employee;

            SET @employee = (SELECT user_id FROM @temp);
            END

            SELECT * FROM @temp2 A
            LEFT JOIN m_user  U ON A.user_id=U.user_id
            WHERE A.user_id!=:user_id 
            AND U.isdelete=0 
            AND U.isactive=1
            AND U.company_id=:company_id;
             ";

            DataTable dt = Database.getDataTable(sql,
                new OracleParameter(":user_id", user_id),
                new OracleParameter(":company_id", M_User.getCompanyId()));

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["photo"].ToString() == null || row["photo"].ToString() == string.Empty)
                    {
                        row["photo"] = Application.GetPath() + "/Content/images/unknown.jpg";
                    }
                    else
                    {
                        if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(row["photo"].ToString())))
                        {
                            row["photo"] = Application.GetPath() + "/Content/images/unknown.jpg";
                        }
                    }
                }
            }

            return dt;
        }

        /// <summary>
        /// Get user employee (Up to Bottom) based on selected user_id
        /// </summary>
        /// <param name="user_id">Selected user_id</param>
        /// <returns></returns>
        public static DataTable SelectDirectEmployee()
        {
            DataTable dt = M_User.SelectDirectEmployee(M_User.getUserId());
            return dt;
        }

        /// <summary>
        /// Get employee activity summary
        /// </summary>
        /// <param name="Limit">Limit selection</param>
        /// <returns></returns>
        public static DataTable SelectDirectEmployeeActivitySummary(int Limit = 0)
        {
            DataTable dt = Log.SelectDirectEmployeeActivity(Limit);
            return dt;
        }

        /// <summary>
        /// Get current employee activity
        /// </summary>
        /// <param name="Limit">Limit selection</param>
        /// <returns></returns>
        public static DataTable SelectCurrentUserActivity(int Limit = 0)
        {
            DataTable dt = Log.SelectEmployeeActivity(M_User.getUserId(), Limit);
            return dt;
        }

        /// <summary>
        /// Get method to send Token
        /// </summary>
        /// <param name="Limit">Limit selection</param>
        /// <returns></returns>
        public static DataTable SelectMethodToken(int user_id)
        {
            string sql = @"
                SELECT 'Email' method FROM m_user WHERE user_id=:user_id AND email IS NOT NULL AND LENGTH(email) > 0
                UNION ALL
                SELECT 'Telegram' method FROM m_user WHERE user_id=:user_id AND telegram_id IS NOT NULL AND LENGTH(telegram_id) > 0
                UNION ALL
                SELECT 'SMS' method FROM m_user WHERE user_id=:user_id AND phone IS NOT NULL AND LENGTH(phone) > 0               
                UNION ALL
                SELECT 'Whatsapp' method FROM m_user WHERE user_id=:user_id AND phone IS NOT NULL AND LENGTH(phone) > 0               
               ";

            DataTable dt = Database.getDataTable(sql, new OracleParameter(":user_id", user_id));
            return dt;
        }

        public class SQLHelper
        {
            /// <summary>
            /// Get the Direct Employee Bottom user_id on current User. Used for SQL SELECT IN.
            /// <para>Returns format "<strong>(1,2,3,4,5,6,7)</strong>" with bracket</para>
            /// </summary>
            /// <returns></returns>
            public static string getINSqlDirectEmployee()
            {
                return M_User.SelectDirectEmployee(M_User.getUserId())
                    .ToIntSingleArray("user_id").ToDelimiterWithBracket(",");
            }

            /// <summary>
            /// Get the Approval Employee user_id on current User. Used for SQL SELECT IN.
            /// <para>Returns format "<strong>(1,2,3,4,5,6,7)</strong>" with bracket</para>
            /// </summary>
            /// <returns></returns>
            public static string getINSqlDirectApproval(bool useHierarchy = false)
            {
                return M_User.SelectDirectApproval(M_User.getUserId(), useHierarchy)
                    .ToIntSingleArray("user_id").ToDelimiterWithBracket(",");
            }
        }


        #region "Obsolete"
        /// <summary>
        /// Get data online user activity
        /// </summary>
        /// <returns></returns>
        private static DataTable SelectOnlineUserWithActivity()
        {
            string direct_employee = M_User.SQLHelper.getINSqlDirectEmployee();

            string sql = string.Format(@"SELECT U.*, COALESCE(T.theme_name, 'Default') theme_name, T.theme_desc, G.group_name, R.role_name, C.company_name, C.company_desc,L.log_date,L.log_desc
                FROM m_user U 
                LEFT JOIN m_theme T ON U.theme_id=T.theme_id
                LEFT JOIN m_group G ON U.group_id=G.group_id
                LEFT JOIN m_role R ON R.role_id=G.role_id
                LEFT JOIN m_company C ON U.company_id=C.company_id
                LEFT JOIN (SELECT * FROM (SELECT A.*, RANK() OVER (PARTITION BY A.user_id ORDER BY a.log_date desc) Rn
                FROM log A  ) A WHERE Rn=1) L ON U.user_id=L.user_id
                WHERE U.isdelete=0 AND L.log_date IS NOT NULL
                AND U.user_id IN {0} AND U.company_id=:company_id ", direct_employee);

            DataTable dt = Database.getDataTable(sql, new OracleParameter(":company_id", M_User.getCompanyId()));

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["photo"].ToString() == null || row["photo"].ToString() == string.Empty)
                    {
                        row["photo"] = Application.GetPath() + "/Content/images/unknown.jpg";
                    }
                    else
                    {
                        if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(row["photo"].ToString())))
                        {
                            row["photo"] = Application.GetPath() + "/Content/images/unknown.jpg";
                        }
                    }
                }
            }

            return dt;
        }

        /// <summary>
        /// Get data online user
        /// </summary>
        /// <returns></returns>
        private static DataTable SelectOnlineUser()
        {
            string direct_employee = M_User.SQLHelper.getINSqlDirectApproval();

            string sql = string.Format(@"SELECT U.*, COALESCE(T.theme_name, 'Default') theme_name, T.theme_desc, G.group_name, R.role_name, C.company_name, C.company_desc,L.log_date,L.log_desc
                FROM m_user U 
                LEFT JOIN m_theme T ON U.theme_id=T.theme_id
                LEFT JOIN m_group G ON U.group_id=G.group_id
                LEFT JOIN m_role R ON R.role_id=G.role_id
                LEFT JOIN m_company C ON U.company_id=C.company_id
                LEFT JOIN (SELECT * FROM (SELECT A.*, RANK() OVER (PARTITION BY A.user_id ORDER BY a.log_date desc) Rn
                FROM log A  ) A WHERE Rn=1) L ON U.user_id=L.user_id
                WHERE U.isdelete=0 AND L.log_date IS NOT NULL
                AND U.user_id IN {0} AND U.company_id=:company_id ", direct_employee);

            DataTable dt = Database.getDataTable(sql, new OracleParameter(":company_id", M_User.getCompanyId()));

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["photo"].ToString() == null || row["photo"].ToString() == string.Empty)
                    {
                        row["photo"] = Application.GetPath() + "/Content/images/unknown.jpg";
                    }
                    else
                    {
                        if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(row["photo"].ToString())))
                        {
                            row["photo"] = Application.GetPath() + "/Content/images/unknown.jpg";
                        }
                    }
                }
            }

            return dt;
        }
        #endregion

        /// <summary>
        /// Get current user list notification
        /// </summary>
        /// <param name="Limit">Limit selection</param>
        /// <returns></returns>
        public static DataTable getNotification(int offset, int limit = 10)
        {
            DataTable dt = M_Notification.SelectOffset(offset, limit).ToColumnLowerCase();
            dt = dt.ResolvePhotoUrl();
            return dt;
        }

        /// <summary>
        /// Get current user setting
        /// </summary>
        /// <returns></returns>
        public static M_Setting getSetting()
        {
            DataTable userSetting = M_Setting.SelectAll();
            if (userSetting.Rows.Count == 1)
            {
                M_Setting setting = new M_Setting()
                {
                    grid_pagesize = userSetting.Rows[0]["grid_pagesize"].ToString().ToInteger(),
                    grid_theme = (M_Setting.Theme)Enum.Parse(typeof(M_Setting.Theme), userSetting.Rows[0]["grid_theme"].ToString()),
                    grid_zebracolor = userSetting.Rows[0]["grid_zebracolor"].ToString().ToBoolean(),
                    grid_wrap_column = userSetting.Rows[0]["grid_wrap_column"].ToString().ToBoolean(),
                    grid_wrap_cell = userSetting.Rows[0]["grid_wrap_cell"].ToString().ToBoolean(),
                    grid_showfilterrow = userSetting.Rows[0]["grid_showfilterrow"].ToString().ToBoolean(),
                    grid_showfilterbar = userSetting.Rows[0]["grid_showfilterbar"].ToString().ToBoolean(),
                    grid_selectbyrow = userSetting.Rows[0]["grid_selectbyrow"].ToString().ToBoolean(),
                    grid_focuserow = userSetting.Rows[0]["grid_focuserow"].ToString().ToBoolean(),
                    grid_ellipsis = userSetting.Rows[0]["grid_ellipsis"].ToString().ToBoolean(),
                    grid_showfooter = userSetting.Rows[0]["grid_showfooter"].ToString().ToBoolean(),
                    grid_responsive = userSetting.Rows[0]["grid_responsive"].ToString().ToBoolean(),
                };
                DevExpress.Web.ASPxWebControl.GlobalTheme = setting.grid_theme.ToString();

                return setting;
            }
            else
            {
                M_Setting setting = new M_Setting()
                {
                    grid_pagesize = 10,
                    grid_theme = M_Setting.Theme.Office2010Blue,
                    grid_zebracolor = true,
                    grid_wrap_column = true,
                    grid_wrap_cell = true,
                    grid_showfilterrow = true,
                    grid_showfilterbar = true,
                    grid_selectbyrow = true,
                    grid_focuserow = true,
                    grid_ellipsis = true,
                    grid_showfooter = true,
                    grid_responsive = true,
                };
                DevExpress.Web.ASPxWebControl.GlobalTheme = setting.grid_theme.ToString();

                return setting;
            }
        }

        /// <summary>
        /// Get current user role action
        /// </summary>
        /// <returns></returns>
        public static Role_Action getRoleAction()
        {

            Role_Action action = new Role_Action()
            {
                Allow_Create = DevExpress.Utils.DefaultBoolean.True,
                Allow_Update = DevExpress.Utils.DefaultBoolean.True,
                Allow_Delete = DevExpress.Utils.DefaultBoolean.True,
                Allow_Export = DevExpress.Utils.DefaultBoolean.True,
                Allow_Import = DevExpress.Utils.DefaultBoolean.True,
                Allow_EnableDisable = DevExpress.Utils.DefaultBoolean.True
            };

            if (HttpContext.Current.User.IsInRole("Super Administrator"))
            {
                return action;
            }
            else
            {
                DataTable dt = M_User.IGetRoleAction();
                if (dt.Rows.Count == 1)
                {
                    action.Allow_Create = dt.Rows[0]["allow_create"].ToBoolean() == true ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
                    action.Allow_Update = dt.Rows[0]["allow_update"].ToBoolean() == true ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
                    action.Allow_Delete = dt.Rows[0]["allow_delete"].ToBoolean() == true ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
                    action.Allow_Export = dt.Rows[0]["allow_export"].ToBoolean() == true ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
                    action.Allow_Import = dt.Rows[0]["allow_import"].ToBoolean() == true ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
                    action.Allow_EnableDisable = dt.Rows[0]["allow_enabledisable"].ToBoolean() == true ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;

                    //action.Allow_Create = HttpContext.Current.User.Identity.Allow_Create() == true ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
                    //action.Allow_Update = HttpContext.Current.User.Identity.Allow_Update() == true ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
                    //action.Allow_Delete = HttpContext.Current.User.Identity.Allow_Delete() == true ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
                    //action.Allow_Export = HttpContext.Current.User.Identity.Allow_Export() == true ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
                    //action.Allow_Import = HttpContext.Current.User.Identity.Allow_Import() == true ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
                    //action.Allow_EnableDisable = HttpContext.Current.User.Identity.Allow_EnableDisable() == true ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
                }
                else
                {
                    action.Allow_Create = DevExpress.Utils.DefaultBoolean.False;
                    action.Allow_Update = DevExpress.Utils.DefaultBoolean.False;
                    action.Allow_Delete = DevExpress.Utils.DefaultBoolean.False;
                    action.Allow_Export = DevExpress.Utils.DefaultBoolean.False;
                    action.Allow_Import = DevExpress.Utils.DefaultBoolean.False;
                    action.Allow_EnableDisable = DevExpress.Utils.DefaultBoolean.False;
                }
                return action;
            }
        }

        /// <summary>
        /// Get all user role
        /// </summary>
        /// <returns></returns>
        public static DataTable IGetRoleAction()
        {
            string sql = string.Format(@"
                SELECT U.user_id, R.allow_create,R.allow_update,R.allow_delete,R.allow_enabledisable,R.allow_export,R.allow_import
                FROM m_user  U
                LEFT JOIN m_group  G ON U.group_id=G.group_id
                LEFT JOIN m_role  R ON G.role_id=R.role_id
                WHERE U.user_id=:user_id");

            DataTable dt = Database.getDataTable(sql, new OracleParameter(":user_id", M_User.getUserId()));
            return dt;
        }
    }
}