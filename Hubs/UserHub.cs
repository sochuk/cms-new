using CMS.Helper;
using CMS.Management.Model;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Hubs
{
    public class UserHub : Hub
    {
        private HubUser user = new HubUser();

        public object Join(string IPAddress, string UserAgent)
        {
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

            return new
            {
                message = "Connected to server"
            };
        }

    }
}