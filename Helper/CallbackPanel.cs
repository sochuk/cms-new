using DevExpress.Web;
using CMS.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Helper
{
    public static class CallbackPanel
    {
        public static void alertSuccess(this ASPxCallbackPanel obj)
        {
            Alert alert = new Alert("Success", "Data updated successfully", Alert.TypeMessage.Success, Alert.PositionMessage.BottomRight);
            obj.JSProperties["cpSuccess"] = alert.ToString();
        }

        public static void alertSuccess(this ASPxCallbackPanel obj, string message)
        {
            Alert alert = new Alert("Success", message, Alert.TypeMessage.Success, Alert.PositionMessage.BottomRight);
            obj.JSProperties["cpSuccess"] = alert.ToString();
        }

        public static void alertError(this ASPxCallbackPanel obj, string error)
        {
            Alert alert = new Alert("Error", error, Alert.TypeMessage.Error, Alert.PositionMessage.BottomRight);
            obj.JSProperties["cpError"] = alert.ToString();
        }

        public static void alertError(this ASPxCallbackPanel obj, string error, string scriptOkay)
        {
            Alert alert = new Alert("Error", error, Alert.TypeMessage.Error, Alert.PositionMessage.BottomRight, scriptOkay);
            obj.JSProperties["cpError"] = alert.ToString();
        }

        public static void toastSuccess(this ASPxCallbackPanel grid)
        {
            Toast alert = new Toast("Success", "Data updated successfully", Toast.TypeMessage.Success, Toast.PositionMessage.BottomRight);
            grid.JSProperties["cpSuccess"] = alert.ToString();
        }

        public static void toastSuccess(this ASPxCallbackPanel grid, string message)
        {
            Toast alert = new Toast("Success", message, Toast.TypeMessage.Success, Toast.PositionMessage.BottomRight);
            grid.JSProperties["cpSuccess"] = alert.ToString();
        }

        public static void toastError(this ASPxCallbackPanel grid, string error)
        {
            Toast alert = new Toast("Error", error, Toast.TypeMessage.Error, Toast.PositionMessage.BottomRight);
            grid.JSProperties["cpError"] = alert.ToString();
        }
    }
}