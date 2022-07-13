using DevExpress.Web;
using DevExpress.Web.ASPxTreeList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace CMS.Helper
{
    public static class GridLookup
    {
        public static ASPxGridLookup FindID(this ASPxGridView gridView, string column_name, string ASPxGridLookup_ID)
        {
            var column = gridView.Columns[column_name] as GridViewDataComboBoxColumn;
            ASPxGridLookup lookup = (ASPxGridLookup)gridView.FindEditRowCellTemplateControl(column, ASPxGridLookup_ID) as ASPxGridLookup;
            return lookup;
        }

        public static ASPxGridLookup FindToolbar(this ASPxGridView gridView, string toolbar_name, string toolbar_item, string toolbar_control)
        {
            GridViewToolbar toolbar = (GridViewToolbar)gridView.Toolbars.FindByName(toolbar_name);
            ASPxGridLookup aSPxGridLookup;
            if (toolbar != null)
            {
                aSPxGridLookup = (ASPxGridLookup)toolbar.Items.FindByName(toolbar_item).FindControl(toolbar_control);
                return aSPxGridLookup;
            }
            else
            {
                return null;
            }            
        }

        public static void Bind(this ASPxGridLookup lookup, string column_name)
        {
            lookup.Load += (obj, arg) =>
            {
                ASPxGridLookup _lookup = obj as ASPxGridLookup;
                _lookup.GridView.Width = new System.Web.UI.WebControls.Unit(420, UnitType.Pixel);
                _lookup.IncrementalFilteringDelay = 100;
                GridViewEditItemTemplateContainer container = _lookup.NamingContainer as GridViewEditItemTemplateContainer;
                if (container != null)
                {
                    if (container.Grid.IsNewRowEditing) return;
                    _lookup.GridView.Selection.SelectRowByKey(container.Grid.GetRowValues(container.VisibleIndex, column_name));
                }                
            };
        }

        public static void Bind(this ASPxGridLookup lookup, string column_name, int width)
        {
            lookup.Load += (obj, arg) =>
            {
                ASPxGridLookup _lookup = obj as ASPxGridLookup;
                _lookup.GridView.Width = new System.Web.UI.WebControls.Unit(width, UnitType.Pixel);
                _lookup.IncrementalFilteringDelay = 100;
                GridViewEditItemTemplateContainer container = _lookup.NamingContainer as GridViewEditItemTemplateContainer;
                if (container != null)
                {
                    if (container.Grid.IsNewRowEditing) return;
                    _lookup.GridView.Selection.SelectRowByKey(container.Grid.GetRowValues(container.VisibleIndex, column_name));
                }
            };
        }

        public static void Initialize(this ASPxGridLookup lookup)
        {
            lookup.GridView.Init += (obj, arg) =>
            {
                ASPxGridView gridView = (ASPxGridView)obj;
                gridView.Styles.Cell.Wrap = DevExpress.Utils.DefaultBoolean.False;
                gridView.Styles.Header.Wrap = DevExpress.Utils.DefaultBoolean.False;
            };

            lookup.GridView.CommandButtonInitialize += (obj, arg) =>
            {
                ASPxGridView gridView = (ASPxGridView)obj;

                if (arg.ButtonType == ColumnCommandButtonType.SelectCheckbox)
                {
                    bool active = gridView.GetRowValues(arg.VisibleIndex, "isactive").ToString().ToBoolean();
                    if (!active)
                    {
                        arg.Enabled = false;
                    }
                }
                gridView.ClientSideEvents.RowClick = "function(s,e){e.cancel=true}";
            };

            lookup.GridView.HtmlRowPrepared += (obj, arg) =>
            {
                ASPxGridView gridView = (ASPxGridView)obj;
                if (arg.RowType != GridViewRowType.Data) return;
                bool active = Convert.ToBoolean(arg.GetValue("isactive"));
                if (!active)
                {
                    arg.Row.ForeColor = Color.Gray;
                    arg.Row.BackColor = Color.WhiteSmoke;
                    arg.Row.Font.Italic = true;
                }
            };
        }

        public static void SetReadonlyColumn(this ASPxGridLookup lookup, string fieldname, string value)
        {
            lookup.GridView.CommandButtonInitialize += (obj, arg) =>
            {
                ASPxGridView gridView = (ASPxGridView)obj;

                if (arg.ButtonType == ColumnCommandButtonType.SelectCheckbox)
                {
                    bool valid = gridView.GetRowValues(arg.VisibleIndex, fieldname).ToString().ToLower() == value.ToLower();
                    if (valid)
                    {
                        arg.Enabled = false;
                    }
                }
                gridView.ClientSideEvents.RowClick = "function(s,e){ e.cancel=true }";
            };

            lookup.GridView.HtmlRowPrepared += (obj, arg) =>
            {
                ASPxGridView gridView = (ASPxGridView)obj;
                if (arg.RowType != GridViewRowType.Data) return;
                bool valid = arg.GetValue(fieldname).ToString().ToLower() == value.ToLower();
                if (valid)
                {
                    arg.Row.ForeColor = Color.Gray;
                    arg.Row.BackColor = Color.WhiteSmoke;
                    arg.Row.Font.Italic = true;
                }
            };
        }
    }
}