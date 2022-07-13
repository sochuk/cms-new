using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CMS.Hubs
{
    public class Message
    {
        public HubUser user { get; set; }
        public string message_content { get; set; }
        public string message_date { get; set; }
        public int message_unread_count { get; set; }
        public List<MessageListSummary> message_unread_userdata { get; set; }

        public class MessageListSummary
        {
            public string user_from { get; set; }
            public string username { get; set; }
            public int unread { get; set; }
        }
    }
}