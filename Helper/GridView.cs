using DevExpress.Web;
using CMS.Management.Model;
using CMS.Notification;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace CMS.Helper
{
    public static class GridView
    {

        public static void bindDelete(this ASPxGridView grid_source, ASPxGridView grid_target, DataTable dt, string[] column, string[] row)
        {
            grid_target.DataBind();            
            grid_target.DataBinding += (obj, _event) =>
            {
                List<object> selected = grid_source.GetSelectedFieldValues(row);
                dt.Columns.Clear();
                dt.Rows.Clear();
                foreach (var field in column) dt.Columns.Add((string)field.ToString());
                try
                {
                    foreach (object[] item in selected)
                    {
                        DataRow _row = dt.NewRow();
                        for (int i = 0; i < item.Length; i++)
                            _row[i] = item[i].ToString();
                        dt.Rows.Add(_row);
                    }
                }
                catch
                {
                    foreach (string item in selected)
                    {
                        DataRow _row = dt.NewRow();
                        _row[0] = item.ToString();
                        dt.Rows.Add(_row);
                    }
                }

                grid_target = (ASPxGridView)obj;
                grid_target.DataSource = dt;
                grid_target.SettingsBehavior.AllowSort = false;
            };
        }

        public static void SelectionCopyTo(this ASPxGridView source, ASPxGridView target, string[] column, string[] row)
        {
            DataTable dt = new DataTable();
            target.DataBind();
            target.DataBinding += (obj, _event) =>
            {
                List<object> selected = source.GetSelectedFieldValues(row);
                dt.Columns.Clear();
                dt.Rows.Clear();
                foreach (var field in column) dt.Columns.Add((string)field.ToString());
                try
                {
                    foreach (object[] item in selected)
                    {
                        DataRow _row = dt.NewRow();
                        for (int i = 0; i < item.Length; i++)
                            _row[i] = item[i].ToString();
                        dt.Rows.Add(_row);
                    }
                }
                catch
                {
                    foreach (string item in selected)
                    {
                        DataRow _row = dt.NewRow();
                        _row[0] = item.ToString();
                        dt.Rows.Add(_row);
                    }
                }

                target = (ASPxGridView)obj;
                target.DataSource = dt;
                target.Selection.SelectAll();
                target.KeyFieldName = source.KeyFieldName;
                target.SettingsBehavior.AllowSort = false;
            };
        }

        public static void initializeGrid(this ASPxGridView grid, bool show_hystory = true)
        {
            Role_Action roleAction = M_User.getRoleAction();
            M_Setting setting = M_User.getSetting();
            try
            {
                GridViewToolbar toolbar = (GridViewToolbar)grid.Toolbars.FindByName("Toolbar");
                if (toolbar != null)
                {
                    var btnNew = ((GridViewToolbarItem)toolbar.Items.FindByName("New"));
                    var btnExport = ((GridViewToolbarItem)toolbar.Items.FindByName("Export"));
                    if (btnNew != null && btnNew.Visible) btnNew.Visible = (roleAction.Allow_Create == DevExpress.Utils.DefaultBoolean.True ? true : false);
                    if (btnExport != null && btnExport.Visible) btnExport.Visible = (roleAction.Allow_Export == DevExpress.Utils.DefaultBoolean.True ? true : false);
                }

                if (grid.SettingsDataSecurity.AllowEdit)
                {
                    grid.SettingsDataSecurity.AllowEdit = (roleAction.Allow_Update == DevExpress.Utils.DefaultBoolean.True ? true : false);
                    if(grid.SettingsDataSecurity.AllowEdit)
                    {
                        grid.SettingsDataSecurity.AllowEdit = (roleAction.Allow_Update == DevExpress.Utils.DefaultBoolean.True ? true : false);
                    }                    
                }

                if (grid.SettingsDataSecurity.AllowDelete)
                {
                    grid.SettingsDataSecurity.AllowDelete = (roleAction.Allow_Delete == DevExpress.Utils.DefaultBoolean.True ? true : false);
                }

                grid.ClientSideEvents.RowClick = "function(s, e) { focusedGrid=s; }";
                grid.ClientSideEvents.RowFocusing = "function(s, e) { focusedGrid=s; }";
                if (string.IsNullOrEmpty(grid.ClientSideEvents.ToolbarItemClick))
                {
                    grid.ClientSideEvents.ToolbarItemClick = "function(s, e) { ToolbarItemClick(s,e) }";
                }
                

                GridViewToolbarItem toolbarItem = new GridViewToolbarItem();
                toolbarItem.Name = "Hystory";
                toolbarItem.Text = "Hystory";
                toolbarItem.Image.IconID = "history_historyitem_16x16office2013";
                toolbarItem.ItemStyle.CssClass = "position-absolute toolbar-right";
                toolbar.Items.Add(toolbarItem);

                if(show_hystory)
                {
                    toolbarItem.Visible = true;
                }
                else
                {
                    toolbarItem.Visible = false;
                }


                //Global settings
                grid.KeyboardSupport = true;
                grid.Theme = (grid.Theme == null || string.IsNullOrEmpty(grid.Theme)) ? setting.grid_theme.ToString() : grid.Theme;
                grid.SettingsAdaptivity.AdaptivityMode = GridViewAdaptivityMode.HideDataCells;
                grid.SettingsAdaptivity.AdaptiveDetailColumnCount = 1;
                grid.SettingsAdaptivity.AllowOnlyOneAdaptiveDetailExpanded = true;
                grid.EditFormLayoutProperties.SettingsAdaptivity.AdaptivityMode = FormLayoutAdaptivityMode.SingleColumnWindowLimit;
                grid.EditFormLayoutProperties.SettingsAdaptivity.SwitchToSingleColumnAtWindowInnerWidth = 600;
                grid.SettingsPager.EnableAdaptivity = true;

                grid.SettingsPopup.CustomizationWindow.ShowShadow = true;
                grid.SettingsPopup.CustomizationWindow.CloseOnEscape = AutoBoolean.True;
                grid.SettingsPopup.CustomizationWindow.VerticalOffset = 50;
                grid.SettingsPopup.CustomizationWindow.Height = 300;
                grid.SettingsPopup.CustomizationWindow.VerticalAlign = PopupVerticalAlign.Middle;

                grid.SettingsCommandButton.RenderMode = GridCommandButtonRenderMode.Button;

                grid.SettingsExport.EnableClientSideExportAPI = true;
                grid.SettingsExport.ExcelExportMode = DevExpress.Export.ExportType.DataAware;

                if (grid.SettingsEditing == null) grid.SettingsEditing.Mode = GridViewEditingMode.EditForm;

                grid.SettingsBehavior.FilterRowMode = GridViewFilterRowMode.OnClick;

                grid.SettingsBehavior.EnableCustomizationWindow = true;

                //User settings
                if(grid.SettingsPager.PageSize.ToInteger() == 0)  grid.SettingsPager.PageSize = setting.grid_pagesize;
                grid.Styles.AlternatingRow.Enabled = setting.grid_zebracolor == true ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
                grid.Styles.Cell.Wrap = setting.grid_wrap_cell == true ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
                grid.Styles.Header.Wrap = setting.grid_wrap_column == true ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;

                grid.Settings.ShowPreview = true;
                grid.Settings.ShowFooter = setting.grid_showfooter;
                grid.Settings.ShowFilterBar = setting.grid_showfilterbar == true ? GridViewStatusBarMode.Visible : GridViewStatusBarMode.Hidden;

                //grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;

                grid.SettingsBehavior.AllowSelectByRowClick = setting.grid_selectbyrow;
                grid.SettingsBehavior.AllowFocusedRow = setting.grid_focuserow;
                grid.SettingsBehavior.AllowEllipsisInText = setting.grid_ellipsis;                

                if (setting.grid_responsive)
                {
                    grid.SettingsResizing.ColumnResizeMode = ColumnResizeMode.Disabled;
                    grid.Width = new Unit(100, UnitType.Percentage);
                }
                else
                {
                    grid.Width = new Unit(100, UnitType.Percentage);
                    grid.SettingsResizing.ColumnResizeMode = ColumnResizeMode.Control;
                    grid.SettingsResizing.Visualization = ResizingMode.Postponed;
                    grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                }

                var import_button = ((GridViewToolbarItem)toolbar.Items.FindByName("Import"));
                if(import_button != null)
                {
                    if (import_button.Visible)
                    {
                        import_button.Visible = (roleAction.Allow_Import == DevExpress.Utils.DefaultBoolean.True ? true : false);
                    }
                }
                

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            grid.ToolbarItemClick += (obj, arg) =>
            {
                ASPxGridView gridView = obj as ASPxGridView;
                if (arg.Item.Name == "Hystory")
                {
                    gridView.JSProperties["cpShowHystory"] = true;
                }
            };

            grid.BeforeGetCallbackResult += (obj, arg) =>
            {
                ASPxGridView gridView = (ASPxGridView)obj;
                if (gridView.SearchPanelFilter != string.Empty && gridView.VisibleRowCount > 0)
                {
                    gridView.ExpandAll();
                }
            };

            grid.StartRowEditing += (obj, arg) =>
            {
                ASPxGridView gridView = (ASPxGridView)obj;
                try
                {
                    var isVisible = grid.GetRowValuesByKeyValue(arg.EditingKeyValue, "allow_edit");
                    if(isVisible != null)
                    {
                        var allowEditing = isVisible.ToBoolean();
                        if (!allowEditing) arg.Cancel = true;
                    }
                }
                catch { }

                if (roleAction.Allow_Update == DevExpress.Utils.DefaultBoolean.False)
                {
                    gridView.JSProperties["cpShowDeleteConfirm"] = false;
                    gridView.JSProperties["cpShowEnableConfirm"] = false;
                    gridView.JSProperties["cpShowDisableConfirm"] = false;
                    gridView.JSProperties["cpShowPopup"] = false;
                    gridView.JSProperties["cpRefresh"] = false;

                    Toast alert = new Toast("Denied", "You dont have access to do this", Toast.TypeMessage.Error, Toast.PositionMessage.BottomRight);
                    gridView.JSProperties["cpError"] = alert.ToString();
                    gridView.CancelEdit();
                }

                gridView.Selection.UnselectAll();
                gridView.Selection.SetSelection(grid.FocusedRowIndex, true);
            };

            grid.InitNewRow += (obj, arg) =>
            {
                ASPxGridView gridView = (ASPxGridView)obj;
                if (roleAction.Allow_Create == DevExpress.Utils.DefaultBoolean.False)
                {
                    gridView.JSProperties["cpShowDeleteConfirm"] = false;
                    gridView.JSProperties["cpShowEnableConfirm"] = false;
                    gridView.JSProperties["cpShowDisableConfirm"] = false;
                    gridView.JSProperties["cpShowPopup"] = false;
                    gridView.JSProperties["cpRefresh"] = false;

                    Toast alert = new Toast("Denied", "You dont have access to do this", Toast.TypeMessage.Error, Toast.PositionMessage.BottomRight);
                    gridView.JSProperties["cpError"] = alert.ToString();
                    gridView.CancelEdit();
                }

                if (gridView.IsNewRowEditing) gridView.SettingsCommandButton.UpdateButton.Text = "Add";
            };

            grid.HtmlRowPrepared += (object sender, ASPxGridViewTableRowEventArgs e) => {
                ASPxGridView _grid = (ASPxGridView)sender;
                _grid.initializeHtmlRowPrepared(e);
            };

            grid.CustomCallback += (object sender, ASPxGridViewCustomCallbackEventArgs e) => {
                ASPxGridView _grid = (ASPxGridView)sender;
                _grid.initializeCustomCallback(e);
            };

            grid.CustomButtonCallback += (object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e) => {
                ASPxGridView _grid = (ASPxGridView)sender;
                _grid.initializeCustomButtonCallback(e);
            };

            grid.FillContextMenuItems += (object sender, DevExpress.Web.ASPxGridViewContextMenuEventArgs e) => {
                ASPxGridView _grid = (ASPxGridView)sender;
                if (e.MenuType == GridViewContextMenuType.Rows)
                {                   
                    e.Items.FindByName("EditRow").Image.IconID = "actions_edit_16x16devav";
                    e.Items.FindByName("Refresh").Image.IconID = "actions_refresh_16x16office2013";
                }
            };
        }

        public static void initializeCustomButton(this ASPxGridView grid, ASPxGridViewCustomButtonEventArgs e)
        {
            Role_Action roleAction = M_User.getRoleAction();
            try
            {
                grid.ToolbarItemClick += (obj, _event) =>
                {
                    grid.JSProperties["cpShowDeleteConfirm"] = false;
                };
            }
            catch
            {

            }

            try
            {
                if (e.ButtonID.ToUpper() == "BUTTONEDIT" && e.VisibleIndex >= 0)
                {
                    e.Visible = roleAction.Allow_Update;
                }

                if (e.ButtonID.ToUpper() == "BUTTONDELETE" && e.VisibleIndex >= 0)
                {
                    e.Visible = roleAction.Allow_Delete;
                }

                if (e.ButtonID.ToUpper() == "BUTTONENABLE" && e.VisibleIndex >= 0)
                {
                    e.Visible = roleAction.Allow_EnableDisable;
                    if (e.Visible == DevExpress.Utils.DefaultBoolean.True)
                    {
                        string text = "False";
                        try
                        {
                            text = grid.GetRowValues(e.VisibleIndex, "isactive").ToString().ToBoolean().ToString();
                        }
                        catch
                        {
                            text = "False";
                        }

                        if (text == "False")
                        {
                            e.Visible = DevExpress.Utils.DefaultBoolean.True;
                        }
                        else
                        {
                            e.Visible = DevExpress.Utils.DefaultBoolean.False;
                        }
                    }
                }

                if (e.ButtonID.ToUpper() == "BUTTONDISABLE" && e.VisibleIndex >= 0)
                {
                    e.Visible = roleAction.Allow_EnableDisable;
                    if (e.Visible == DevExpress.Utils.DefaultBoolean.True)
                    {
                        string text = "False";
                        try
                        {
                            text = grid.GetRowValues(e.VisibleIndex, "isactive").ToString().ToBoolean().ToString();
                        }
                        catch
                        {
                            text = "False";
                        }

                        if (text == "True")
                        {
                            e.Visible = DevExpress.Utils.DefaultBoolean.True;
                        }
                        else
                        {
                            e.Visible = DevExpress.Utils.DefaultBoolean.False;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        private static void initializeCustomButton(this ASPxGridView grid, Role_Action roleAction, ASPxGridViewCustomButtonEventArgs e)
        {
            try
            {
                grid.ToolbarItemClick += (obj, _event) =>
                {
                    grid.JSProperties["cpShowDeleteConfirm"] = false;
                };
            }
            catch
            {

            }

            try
            {
                if (e.ButtonID.ToUpper() == "BUTTONEDIT" && e.VisibleIndex >= 0)
                {
                    e.Visible = roleAction.Allow_Update;
                }

                if (e.ButtonID.ToUpper() == "BUTTONDELETE" && e.VisibleIndex >= 0)
                {
                    e.Visible = roleAction.Allow_Delete;
                }

                if (e.ButtonID.ToUpper() == "BUTTONENABLE" && e.VisibleIndex >= 0)
                {
                    e.Visible = roleAction.Allow_EnableDisable;
                    if (e.Visible == DevExpress.Utils.DefaultBoolean.True)
                    {
                        string text = "False";
                        try
                        {
                            text = grid.GetRowValues(e.VisibleIndex, "isactive").ToString().ToBoolean().ToString();
                        }
                        catch
                        {
                            text = "False";
                        }

                        if (text == "False")
                        {
                            e.Visible = DevExpress.Utils.DefaultBoolean.True;
                        }
                        else
                        {
                            e.Visible = DevExpress.Utils.DefaultBoolean.False;
                        }
                    }
                }

                if (e.ButtonID.ToUpper() == "BUTTONDISABLE" && e.VisibleIndex >= 0)
                {
                    e.Visible = roleAction.Allow_EnableDisable;
                    if (e.Visible == DevExpress.Utils.DefaultBoolean.True)
                    {
                        string text = "False";
                        try
                        {
                            text = grid.GetRowValues(e.VisibleIndex, "isactive").ToString().ToBoolean().ToString();
                        }
                        catch
                        {
                            text = "False";
                        }

                        if (text == "True")
                        {
                            e.Visible = DevExpress.Utils.DefaultBoolean.True;
                        }
                        else
                        {
                            e.Visible = DevExpress.Utils.DefaultBoolean.False;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        public static void initializeCustomCallback(this ASPxGridView grid, ASPxGridViewCustomCallbackEventArgs e)
        {
            grid.JSProperties["cpShowDeleteConfirm"] = false;
            grid.JSProperties["cpShowEnableConfirm"] = false;
            grid.JSProperties["cpShowDisableConfirm"] = false;
            grid.JSProperties["cpRefresh"] = true;


            GridViewToolbar toolbar = (GridViewToolbar)grid.Toolbars.FindByName("Toolbar");
            if (toolbar != null)
            {
                var toolItem = toolbar.Items.FindByName("Filter") as GridViewToolbarItem;
                if(toolItem != null)
                {
                    ASPxCheckBox filter = (ASPxCheckBox)toolItem.FindControl("checkShowFilter");
                    switch (e.Parameters.ToUpper())
                    {
                        case "CHECKSHOWFILTER":
                            grid.Settings.ShowFilterRow = filter.Checked;
                            grid.JSProperties["cpRefresh"] = true;
                            break;
                    }
                }
                
            }
        }

        public static void initializeCustomButtonCallback(this ASPxGridView grid, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            grid.JSProperties["cpShowDeleteConfirm"] = false;
            grid.JSProperties["cpShowEnableConfirm"] = false;
            grid.JSProperties["cpShowDisableConfirm"] = false;
            grid.JSProperties["cpRefresh"] = true;

            switch (e.ButtonID.ToUpper())
            {
                case "BUTTONEDIT":
                    grid.SettingsLoadingPanel.Mode = GridViewLoadingPanelMode.ShowAsPopup;
                    grid.StartEdit(e.VisibleIndex);
                    break;
                case "BUTTONEDIT2":
                    grid.SettingsLoadingPanel.Mode = GridViewLoadingPanelMode.ShowAsPopup;
                    grid.StartEdit(e.VisibleIndex);
                    break;
                case "BUTTONEDIT3":
                    grid.SettingsLoadingPanel.Mode = GridViewLoadingPanelMode.ShowAsPopup;
                    grid.StartEdit(e.VisibleIndex);
                    break;
                case "BUTTONEDIT4":
                    grid.SettingsLoadingPanel.Mode = GridViewLoadingPanelMode.ShowAsPopup;
                    grid.StartEdit(e.VisibleIndex);
                    break;

                case "BUTTONDELETE":
                    grid.SettingsLoadingPanel.Mode = GridViewLoadingPanelMode.ShowOnStatusBar;
                    grid.Selection.SelectRow(e.VisibleIndex);
                    grid.JSProperties["cpShowDeleteConfirm"] = true;
                    break;
                case "BUTTONDELETE2":
                    grid.SettingsLoadingPanel.Mode = GridViewLoadingPanelMode.ShowOnStatusBar;
                    grid.Selection.SelectRow(e.VisibleIndex);
                    grid.JSProperties["cpShowDeleteConfirm"] = true;
                    break;
                case "BUTTONDELETE3":
                    grid.SettingsLoadingPanel.Mode = GridViewLoadingPanelMode.ShowOnStatusBar;
                    grid.Selection.SelectRow(e.VisibleIndex);
                    grid.JSProperties["cpShowDeleteConfirm"] = true;
                    break;
                case "BUTTONDELETE4":
                    grid.SettingsLoadingPanel.Mode = GridViewLoadingPanelMode.ShowOnStatusBar;
                    grid.Selection.SelectRow(e.VisibleIndex);
                    grid.JSProperties["cpShowDeleteConfirm"] = true;
                    break;

                case "BUTTONENABLE":
                    grid.SettingsLoadingPanel.Mode = GridViewLoadingPanelMode.ShowAsPopup;
                    grid.Selection.SelectRow(e.VisibleIndex);
                    break;
                case "BUTTONDISABLE":
                    grid.SettingsLoadingPanel.Mode = GridViewLoadingPanelMode.ShowAsPopup;
                    grid.Selection.SelectRow(e.VisibleIndex);
                    break;
            }
        }

        public static void initializeHtmlRowPrepared(this ASPxGridView sender, ASPxGridViewTableRowEventArgs e)
        {
            try
            {
                bool active = false;
                if (e.RowType != GridViewRowType.Data) return;
                var isactive = e.GetValue("isactive");
                if(isactive != null)
                {
                    active = Convert.ToBoolean(e.GetValue("isactive").ToString().ToBoolean());
                    if (!active)
                    {
                        e.Row.ForeColor = System.Drawing.Color.Gray;
                        e.Row.BackColor = System.Drawing.Color.WhiteSmoke;
                        e.Row.Font.Italic = true;
                    }
                    else
                    {
                        e.Row.ForeColor = System.Drawing.Color.Black;
                    }
                }                
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        public static void alertSuccess(this ASPxGridView grid)
        {
            Alert alert = new Alert("Success", "Data updated successfully", Alert.TypeMessage.Success, Alert.PositionMessage.BottomRight);
            grid.JSProperties["cpSuccess"] = alert.ToString();
        }

        public static void alertSuccess(this ASPxGridView grid, string message)
        {
            Alert alert = new Alert("Success", message, Alert.TypeMessage.Success, Alert.PositionMessage.BottomRight);
            grid.JSProperties["cpSuccess"] = alert.ToString();
        }

        public static void alertError(this ASPxGridView grid, string error)
        {
            Alert alert = new Alert("Error", error, Alert.TypeMessage.Error, Alert.PositionMessage.BottomRight);
            grid.JSProperties["cpError"] = alert.ToString();
        }

        public static void alertError(this ASPxGridView grid, string title, string message)
        {
            Alert alert = new Alert(title, message, Alert.TypeMessage.Error, Alert.PositionMessage.BottomRight);
            grid.JSProperties["cpError"] = alert.ToString();
        }

        public static void toastSuccess(this ASPxGridView grid)
        {
            Toast alert = new Toast("Success", "Data updated successfully", Toast.TypeMessage.Success, Toast.PositionMessage.BottomRight);
            alert.HideAfterSecond = 10;
            grid.JSProperties["cpSuccess"] = alert.ToString();
        }

        public static void toastSuccess(this ASPxGridView grid, string message)
        {
            Toast alert = new Toast("Success", message, Toast.TypeMessage.Success, Toast.PositionMessage.BottomRight);
            alert.HideAfterSecond = 10;
            grid.JSProperties["cpSuccess"] = alert.ToString();
        }

        public static void toastError(this ASPxGridView grid, string error)
        {
            Toast alert = new Toast("Error", error, Toast.TypeMessage.Error, Toast.PositionMessage.BottomRight);
            grid.JSProperties["cpError"] = alert.ToString();
        }

        public static void openTab(this ASPxGridView grid, string url)
        {
            grid.JSProperties["cpOpenTab"] = url;
        }

        public static void openURL(this ASPxGridView grid, string url)
        {
            grid.JSProperties["cpOpenUrl"] = url;
        }

        public static void Refresh(this ASPxGridView grid, bool value = true)
        {
            grid.JSProperties["cpShowDeleteConfirm"] = false;
            grid.JSProperties["cpRefresh"] = value;
        }

        public static void ShowDeleteConfirm(this ASPxGridView grid, bool value = true)
        {
            grid.Selection.SelectRow(grid.FocusedRowIndex);
            grid.JSProperties["cpShowDeleteConfirm"] = value;
        }

        public static void ShowDeleteConfirm(this ASPxGridView grid, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            grid.Selection.SelectRow(grid.FocusedRowIndex);
            grid.JSProperties["cpShowDeleteConfirm"] = true;
            grid.CancelEdit();
            e.Cancel = true;
        }

        public static void ShowDeleteConfirm(this ASPxGridView grid, string JSProterties, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            grid.Selection.SelectRow(grid.FocusedRowIndex);
            grid.JSProperties[JSProterties] = true;
            grid.CancelEdit();
            e.Cancel = true;
        }

        public static List<object> GetSelectedKey(this ASPxGridView grid, string key_fieldname)
        {
            List<object> selected = grid.GetSelectedFieldValues(key_fieldname);             
            
            return selected.Distinct().ToList();
        }

        public static List<object> GetSelectedAndFocusRow(this ASPxGridView grid, string key_fieldname)
        {
            List<object> selected = grid.GetSelectedFieldValues(key_fieldname);
            try
            {
                var object_focus = grid.GetRowValues(grid.FocusedRowIndex, key_fieldname);
                if (object_focus != null)
                {
                    int focus = object_focus.ToString().ToInteger();
                    selected.Add(focus);
                }
            }
            catch
            {
                return selected;
            }

            return selected.Distinct().ToList();
        }
    }
}