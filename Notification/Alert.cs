using CMS.Helper;
using System;
using System.Web;

namespace CMS.Notification
{
    public class Alert
    {
        public Alert(string Title, string Message, TypeMessage type, PositionMessage position = PositionMessage.Default, string onOkay = null)
        {
            this.Title = Title;
            this.Message = Message;
            this.Type = type;
            this.Position = position;
            this.scriptOkay = onOkay;
        }

        public string Title { get; set; }
        public string Message { get; set; }
        public Alert.TypeMessage Type { get; set; }
        public Alert.PositionMessage Position { get; set; }
        public string scriptOkay { get; set; }

        public enum TypeMessage
        {
            Success,
            Error,
            Warning
        }

        public enum PositionMessage
        {
            Default,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomCenter,
            BottomRight,
        }

        public override string ToString()
        {
            string js = "<script type=\"text/javascript\">";
            js += "$.notify({";
            js += "message:'<b>"+ this.Title.ToString().ToTitleCase() + "</b><hr class=\"mt-1 mb-1\"/>"+ this.Message.ToString() +"',";
            js += "status:'" + this.Type.ToString().ToLower() + "',";
            js += "pos:'";
            switch (this.Position)
            {
                case PositionMessage.Default:
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


            js = "<script type=\"text/javascript\">";
            js += "swal(\"" + this.Title.ToString().ToTitleCase() + "\",";
            js += "\"" + this.Message.ToString() + "\",";
            js += "\"" + this.Type.ToString().ToLower() + "\"";
            js += ")";

            if(this.scriptOkay != null)
            {
                js += ".then(okay =>  {";
                js += "if(okay){ " + this.scriptOkay.ToString() + " }";
                js += "});";
            }
            
            js += "</script>";

            return js;
        }

        public void Show()
        {
            HttpContext.Current.Session["Alert"] = ToString();
        }

    }

    
}