using Microsoft.AspNet.SignalR;
using CMS.Helper;
using CMS.Hubs;
using CMS.Management.Model;
using CMS.Notification.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace CMS.Notification
{
    public class Notify
    {
        private static void SendToHub(int to_user_id, M_Notification notif)
        {
            var data = new
            {
                notification = new
                {
                    notif.notif_id,
                    notif.notif_title,
                    notif.notif_message,
                    notif.url,
                    notif.notif_type,
                    unread = 1,
                },
                user = new
                {
                    username = M_User.getUsername().To64Byte(),
                    fullname = M_User.getFullname()
                }

            };

            var hub = GlobalHost.ConnectionManager.GetHubContext<DefaultHub>();
            hub.Clients.User(M_User.getUsername(to_user_id)).getNotification(data);
        }

        public static void Send(int to_user_id, string title, string message, string url)
        {
            M_Notification notif = new M_Notification();
            notif.to_user_id = to_user_id;
            notif.notif_title = title;
            notif.notif_message = message;
            notif.url = url;
            notif.notif_type = NotificationType.DEFAULT;
            
            using (var sqlConnection = new OracleConnection(Database.getConnectionString("Default")))
            {
                sqlConnection.Open();
                using (OracleTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    M_Notification.Insert(notif, sqlConnection, sqlTransaction);
                    sqlTransaction.Commit();

                    SendToHub(to_user_id, notif);               
                }
            }
        }

        public static void Send(NotificationType notif_type, int to_user_id, string title, string message, string url)
        {
            M_Notification notif = new M_Notification();
            notif.to_user_id = to_user_id;
            notif.notif_title = title;
            notif.notif_message = message;
            notif.url = url;
            notif.notif_type = notif_type;

            using (var sqlConnection = new OracleConnection(Database.getConnectionString("Default")))
            {
                sqlConnection.Open();
                using (OracleTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    M_Notification.Insert(notif, sqlConnection, sqlTransaction);
                    sqlTransaction.Commit();

                    SendToHub(to_user_id, notif);
                }
            }
        }

        public static void Send(int to_user_id, int from_user_id, string title, string message, string url)
        {
            M_Notification notif = new M_Notification();
            notif.to_user_id = to_user_id;
            notif.from_user_id = from_user_id;
            notif.notif_title = title;            
            notif.notif_message = message;
            notif.url = url;
            notif.notif_type = NotificationType.DEFAULT;

            using (var sqlConnection = new OracleConnection(Database.getConnectionString("Default")))
            {
                sqlConnection.Open();
                using (OracleTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    M_Notification.Insert(notif, sqlConnection, sqlTransaction);
                    sqlTransaction.Commit();

                    SendToHub(to_user_id, notif);
                }
            }
        }

        public static void Send(NotificationType notif_type, int to_user_id, int from_user_id, string title, string message, string url)
        {
            M_Notification notif = new M_Notification();
            notif.to_user_id = to_user_id;
            notif.from_user_id = from_user_id;
            notif.notif_title = title;
            notif.notif_message = message;
            notif.url = url;
            notif.notif_type = notif_type;

            using (var sqlConnection = new OracleConnection(Database.getConnectionString("Default")))
            {
                sqlConnection.Open();
                using (OracleTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    M_Notification.Insert(notif, sqlConnection, sqlTransaction);
                    sqlTransaction.Commit();

                    SendToHub(to_user_id, notif);
                }
            }
        }
    }
}