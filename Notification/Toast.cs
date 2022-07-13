using CMS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Notification
{
    //based on jquery plugin https://github.com/kamranahmedse/jquery-toast-plugin
    public class Toast
    {
        public Toast(string Title, string Message, Toast.TypeMessage type, Toast.PositionMessage position = PositionMessage.Default)
        {
            this.Title = Title;
            this.Message = Message;
            this.Type = type;
            this.Position = position;
            this.HideAfterSecond = 0;
        }

        public Toast(string Title, string Message, Toast.TypeMessage type, Toast.PositionMessage position = PositionMessage.Default, int HideAfterSecond = 15000)
        {
            this.Title = Title;
            this.Message = Message;
            this.Type = type;
            this.Position = position;
            this.HideAfterSecond = HideAfterSecond;
        }

        public string Title { get; set; }
        public string Message { get; set; }
        public int HideAfterSecond { get; set; }
        public Toast.TypeMessage Type { get; set; }
        public Toast.PositionMessage Position { get; set; }

        public enum TypeMessage
        {
            Success,
            Error,
            Warning,
            Info
        }

        public enum PositionMessage
        {
            Default,
            TopLeft,
            TopCenter,
            TopRight,
            BottomLeft,
            BottomCenter,
            BottomRight,
        }

        public override string ToString()
        {
            string js = "<script type=\"text/javascript\">";
            js += "$.notify({";
            js += "heading:'" + this.Title.ToString().ToTitleCase() + "',";
            js += "text:'" + this.Message.ToString()+ "',";
            js += "icon:'" + this.Type.ToString().ToLower() + "',";
            js += "allowToastClose:true,";
            js += "showHideTransition:'slide',";
            js += "hideAfter:"+ (this.HideAfterSecond.ToString() != "0" ? (this.HideAfterSecond.ToInteger() * 1000).ToString() : "false") + ",";
            js += "position:'";
            switch (this.Position)
            {
                case PositionMessage.Default:
                    js += "mid-center";
                    break;
                case PositionMessage.TopCenter:
                    js += "top-center";
                    break;
                case PositionMessage.TopLeft:
                    js += "top-left";
                    break;
                case PositionMessage.TopRight:
                    js += "top-right";
                    break;
                case PositionMessage.BottomLeft:
                    js += "bottom-left";
                    break;
                case PositionMessage.BottomCenter:
                    js += "bottom-center";
                    break;
                case PositionMessage.BottomRight:
                    js += "bottom-right";
                    break;
                default:
                    js += "top-center";
                    break;
            }
            js += "'";
            js += "})";
            js += "</script>";

            return js;
        }

        public void Show()
        {
            HttpContext.Current.Session["Toast"] = ToString();
        }
    }
}