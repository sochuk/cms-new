using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using CMS.Helper;
using CMS.Management.Model;
using CMS.Notification.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Web;

namespace CMS.Hubs
{
    public class DefaultHub : Hub
    {
        private HubUser user = new HubUser();
        private Message message = new Message();
        //private static readonly ConcurrentDictionary<string, HubUser> registeredUser = new ConcurrentDictionary<string, HubUser>(StringComparer.OrdinalIgnoreCase);

        public void SendToAll(string message)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
                Clients.All.getBroadcastMessage(Clients.Caller.fullname, message);
        }

        public void SendTo(string user_id, string username, string message)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                user_id = Crypto.Decode64Byte(user_id);
                username = Crypto.Decode64Byte(username);
                string caller_username = Crypto.Decode64Byte(Clients.Caller.username);
                List<string> sender = new List<string>();
                sender.Add(username);
                if(caller_username != username) sender.Add(caller_username);

                //Sender info
                Message data = new Message();
                user = new HubUser();
                user.connection_id = Clients.Caller.connection_id;
                user.user_id = Clients.Caller.user_id;
                user.username = Clients.Caller.username;
                user.fullname = Clients.Caller.fullname;
                user.company_name = Clients.Caller.company_name;
                user.gender = Clients.Caller.gender;
                user.photo = Clients.Caller.photo;
                user.local_ipaddress = Clients.Caller.local_ipaddress;
                user.remote_ipaddress = Clients.Caller.remote_ipaddress;
                user.user_agent = Clients.Caller.user_agent;

                data.message_content = message;
                data.message_date = DateTime.Now.ToString("HH:mm");
                data.message_unread_count = 1;
                data.user = user;

                M_Message.Insert(user_id.ToInteger(), message);
                
                Clients.Users(sender).getMessage(
                    new {
                        message = data
                    });
            }                
        }

        public void SendToGroup(string groupname, string message)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
                Clients.Group(groupname).getMessage(Clients.Caller.fullname, message);
        }

        public void LogoutAnother()
        {
            Clients.User(M_User.getUsername()).logoutAnother(Clients.Caller.local_ipaddress);
        }        

        public object Join(string IPAddress, string UserAgent)
        {
            //message = new Message();
            user = new HubUser();

            user.connection_id = Context.ConnectionId;
            user.user_id = Crypto.Encode64Byte(M_User.getUserId().ToString());
            user.username = Crypto.Encode64Byte(M_User.getUsername());
            user.fullname = M_User.getFullname();
            user.company_name = HttpContext.Current.User.Identity.Get_CompanyName();
            user.gender = HttpContext.Current.User.Identity.Get_Gender();
            user.photo = M_User.getPhoto();
            user.local_ipaddress = M_User.getLocalIP();
            user.remote_ipaddress = IPAddress;
            user.user_agent = UserAgent;

            //message.user = user;
            //message.message_content = null;
            //message.message_date = null;            
            //message.message_unread_count = M_Message.getMessage_Unread();

            //var unencrypt = M_Message.getMessage_UnreadUserData().ToList<Message.MessageListSummary>();
            //var message_unread_userdata = new List<Message.MessageListSummary>();
            //foreach (var x in unencrypt)
            //{
            //    message_unread_userdata.Add(
            //        new Message.MessageListSummary()
            //        {
            //            user_from = Crypto.Encode64Byte(x.user_from.ToString()),
            //            username = Crypto.Encode64Byte(x.username),
            //            unread = x.unread
            //        }
            //    );
            //}

            //message.message_unread_userdata = message_unread_userdata;

            if (user != null)
            {
                Clients.Caller.user_id = user.user_id;
                Clients.Caller.connection_id = user.connection_id;
                Clients.Caller.username = user.username;
                Clients.Caller.fullname = user.fullname;
                Clients.Caller.company_name = user.company_name;
                Clients.Caller.gender = user.gender;
                Clients.Caller.photo = user.photo;
                Clients.Caller.local_ipaddress = user.local_ipaddress;
                Clients.Caller.remote_ipaddress = user.remote_ipaddress;
                Clients.Caller.user_agent = user.user_agent;
            }

            var data = new
            {
                message,
                notification = new
                {
                    unread = M_Notification.getNotification_Unread()
                }
            };
            
            return data;
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (stopCalled)
            {
                Console.WriteLine(String.Format("Client {0} explicitly closed the connection.", Context.ConnectionId));
            }
            else
            {
                Console.WriteLine(String.Format("Client {0} timed out .", Context.ConnectionId));
            }

            return base.OnDisconnected(stopCalled);
        }

        public void getMessageData(string user_from)
        {
            int user_id = Crypto.Decode64Byte(user_from).ToString().ToInteger();
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                DataTable list = M_Message.getMessage_Data(user_id);
                var data = (from row in list.AsEnumerable()
                            select new
                            {
                                fullname = row["fullname"].ToString(),
                                message_content = row["message_content"].ToString(),
                                message_date = Convert.ToDateTime(row["createdate"]).ToString("HH:mm"),

                            });
                Clients.User(M_User.getUsername()).getMessageUser(JsonConvert.SerializeObject(data));
            }
        }

        public void getNotificationData(int offset, int limit = 10)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                DataTable list = M_User.getNotification(offset, limit);
                list.Columns.Add("initial", typeof(string));
                list.Columns.Add("prettydate", typeof(string));
                foreach (DataRow row in list.Rows)
                {
                    row["initial"] = M_User.getInitial(row["fullname"].ToString());
                    row["prettydate"] = PrettyDate.GetPrettyDate(Convert.ToDateTime(row["createdate"].ToString()));
                }
                list.AcceptChanges();

                var data = new
                {
                    notification = list
                };

                Clients.User(M_User.getUsername()).getNotificationData(JsonConvert.SerializeObject(data));
            }
        }

        public void setNotificationRead(int id, bool all)
        {
            M_Notification.setNotificationRead(id, all);
        }

    }
}