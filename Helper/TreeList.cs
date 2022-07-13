using DevExpress.Web;
using DevExpress.Web.ASPxTreeList;
using CMS.Management.Model;
using CMS.Notification;
using System;
using static CMS.Notification.Alert;

namespace CMS.Helper
{
    public static class TreeList
    {        
        public static void initializeTreeList(this ASPxTreeList treelist, EventArgs e)
        {
            Role_Action roleAction = M_User.getRoleAction();
            M_Setting setting = M_User.getSetting();
            try
            {
                TreeListToolbar toolbar = (TreeListToolbar)treelist.Toolbars.FindByName("Toolbar");
                if (toolbar != null)
                {
                    var btnNew = ((TreeListToolbarItem)toolbar.Items.FindByName("New"));
                    var btnExport = ((TreeListToolbarItem)toolbar.Items.FindByName("Export"));
                    if (btnNew != null && btnNew.Visible) btnNew.Visible = (roleAction.Allow_Create == DevExpress.Utils.DefaultBoolean.True ? true : false);
                    if (btnExport != null && btnExport.Visible) btnExport.Visible = (roleAction.Allow_Export == DevExpress.Utils.DefaultBoolean.True ? true : false);
                }

                //Global settings
                treelist.SettingsPager.EnableAdaptivity = true;
                treelist.SettingsPopup.CustomizationWindow.Height = 300;
                treelist.SettingsPopup.CustomizationWindow.VerticalAlign = PopupVerticalAlign.Middle;
                treelist.SettingsExport.EnableClientSideExportAPI = true;

                treelist.SettingsEditing.Mode = TreeListEditMode.EditForm;
                treelist.SettingsBehavior.FilterRowMode = GridViewFilterRowMode.OnClick;
                treelist.SettingsBehavior.EnableCustomizationWindow = true;                
                treelist.Theme = treelist.Theme == null ? setting.grid_theme.ToString() : treelist.Theme;

                var import_button = ((TreeListToolbarItem)toolbar.Items.FindByName("Import"));
                if (import_button.Visible)
                {
                    import_button.Visible = (roleAction.Allow_Import == DevExpress.Utils.DefaultBoolean.True ? true : false);
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            treelist.BeforeGetCallbackResult += (obj, arg) =>
            {
                ASPxTreeList treelistView = (ASPxTreeList)obj;
                if (treelistView.SearchPanelFilter != string.Empty && treelistView.TotalNodeCount > 0)
                {
                    treelistView.ExpandAll();
                }
            };

            treelist.StartNodeEditing += (obj, arg) =>
            {
                ASPxTreeList treelistView = (ASPxTreeList)obj;
                if (roleAction.Allow_Update == DevExpress.Utils.DefaultBoolean.False)
                {
                    treelist.JSProperties["cpShowDeleteConfirm"] = false;
                    treelist.JSProperties["cpShowEnableConfirm"] = false;
                    treelist.JSProperties["cpShowDisableConfirm"] = false;
                    treelist.JSProperties["cpShowPopup"] = false;
                    treelist.JSProperties["cpRefresh"] = false;

                    Alert alert = new Alert("Denied", "You dont have access to do this", TypeMessage.Error, PositionMessage.BottomRight);
                    treelistView.JSProperties["cpError"] = alert.ToString();
                    treelistView.CancelEdit();
                }
            };

            treelist.InitNewNode += (obj, arg) =>
            {
                ASPxTreeList treelistView = (ASPxTreeList)obj;
                if (roleAction.Allow_Create == DevExpress.Utils.DefaultBoolean.False)
                {
                    treelist.JSProperties["cpShowDeleteConfirm"] = false;
                    treelist.JSProperties["cpShowEnableConfirm"] = false;
                    treelist.JSProperties["cpShowDisableConfirm"] = false;
                    treelist.JSProperties["cpShowPopup"] = false;
                    treelist.JSProperties["cpRefresh"] = false;

                    Alert alert = new Alert("Denied", "You dont have access to do this", TypeMessage.Error, PositionMessage.BottomRight);
                    treelistView.JSProperties["cpError"] = alert.ToString();
                    treelistView.CancelEdit();
                }

            };

        }

        public static void initializeCommandButton(this ASPxTreeList treelist, TreeListCommandColumnButtonEventArgs e)
        {
            Role_Action roleAction = M_User.getRoleAction();
            if (e.ButtonType == TreeListCommandColumnButtonType.New && e.Visible == DevExpress.Utils.DefaultBoolean.True) e.Visible = roleAction.Allow_Create;
            if (e.ButtonType == TreeListCommandColumnButtonType.Edit && e.Visible == DevExpress.Utils.DefaultBoolean.True) e.Visible = roleAction.Allow_Update;
            if (e.ButtonType == TreeListCommandColumnButtonType.Delete && e.Visible == DevExpress.Utils.DefaultBoolean.True) e.Visible = roleAction.Allow_Delete;
        }
        
        public static void initializeCustomCallback(this ASPxTreeList treelist, TreeListCustomCallbackEventArgs e)
        {
            treelist.JSProperties["cpShowDeleteConfirm"] = false;
            treelist.JSProperties["cpShowEnableConfirm"] = false;
            treelist.JSProperties["cpShowDisableConfirm"] = false;
            treelist.JSProperties["cpRefresh"] = true;


            TreeListToolbar toolbar = (TreeListToolbar)treelist.Toolbars.FindByName("Toolbar");
            if (toolbar != null)
            {
                ASPxCheckBox filter = (ASPxCheckBox)toolbar.Items.FindByName("Filter").FindControl("checkShowFilter");
                switch (e.Argument.ToUpper())
                {
                    case "CHECKSHOWFILTER":
                        treelist.Settings.ShowFilterRow = filter.Checked;
                        treelist.JSProperties["cpRefresh"] = true;
                        break;
                }
            }
        }                
    }
}