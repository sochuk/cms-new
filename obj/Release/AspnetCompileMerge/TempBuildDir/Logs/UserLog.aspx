<%@ Page Language="C#" MasterPageFile="~/CPanel.Master" AutoEventWireup="true" CodeBehind="UserLog.aspx.cs" Inherits="CMS.Logs.UserLog" %>
<%@ Register Assembly="DevExpress.XtraCharts.v19.2.Web, Version=19.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function grid_EndCallback(s, e) {
            if (s.cpRefresh) {
                grid.Refresh();
                s.cpRefresh = false;
            }
            if (s.cpShowDeleteConfirm) {
                grid_delete.Refresh();
                s.cpShowDeleteConfirm = false;
                Confirm.Show();
            }

            EndCallback(s, e);
        }
    </script>

    <dx:ASPxGridView runat="server" ID="grid" ClientInstanceName="grid"
        Width="100%" CssClass="w-100" DataSourceID="UserLogData"
        KeyFieldName="LOG_ID"
        EnableCallBacks="true"
        OnInit="grid_Init"
        OnHtmlRowPrepared="grid_HtmlRowPrepared"
        OnCustomColumnDisplayText="grid_CustomColumnDisplayText">
        <ClientSideEvents EndCallback="grid_EndCallback" />
        <SettingsSearchPanel CustomEditorID="tbSearch" />
        <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
        <SettingsPager>
            <PageSizeItemSettings Visible="true" Items="10, 20, 50, 100" />
        </SettingsPager>
        <SettingsDetail ShowDetailRow="true" />
        <SettingsContextMenu Enabled="true">
            <ColumnMenuItemVisibility ClearFilter="true" />
        </SettingsContextMenu>
        <Toolbars>
            <dx:GridViewToolbar Name="Toolbar">
                <Items>
                    <dx:GridViewToolbarItem Command="Refresh" Name="Refresh" BeginGroup="true" Image-IconID="actions_refresh_16x16office2013" />
                    <dx:GridViewToolbarItem AdaptivePriority="1" Name="Export" Text="Download to" Image-IconID="export_export_16x16office2013" BeginGroup="true">
                        <Items>
                            <dx:GridViewToolbarItem Command="ExportToPdf" Visible="false" />
                            <dx:GridViewToolbarItem Command="ExportToDocx" Visible="false" />
                            <dx:GridViewToolbarItem Command="ExportToRtf" Visible="false" />
                            <dx:GridViewToolbarItem Command="ExportToCsv" Image-IconID="mail_sendcsv_16x16office2013" />
                            <dx:GridViewToolbarItem Command="ExportToXls" Text="Export to XLS" Image-IconID="mail_sendxls_16x16office2013" />
                            <dx:GridViewToolbarItem Command="ExportToXlsx" Text="Export to XLSX" Image-IconID="export_exporttoxlsx_16x16office2013" />
                            <dx:GridViewToolbarItem Name="CustomExportToXLS" Text="Export to XLS(WYSIWYG)" Visible="false" Image-IconID="export_exporttoxls_16x16office2013" />
                            <dx:GridViewToolbarItem Name="CustomExportToXLSX" Text="Export to XLSX(WYSIWYG)" Visible="false" Image-IconID="export_exporttoxlsx_16x16office2013" />
                        </Items>
                    </dx:GridViewToolbarItem>
                    <dx:GridViewToolbarItem AdaptivePriority="1" Name="Import" Visible="false" Text="Upload from" Image-IconID="actions_download_16x16office2013" BeginGroup="true">
                        <Items>
                            <dx:GridViewToolbarItem Text="CSV" Image-IconID="mail_sendcsv_16x16office2013" />
                            <dx:GridViewToolbarItem Text="Excel File" Image-IconID="mail_sendxls_16x16office2013" />
                        </Items>
                    </dx:GridViewToolbarItem>
                    <dx:GridViewToolbarItem Command="ShowCustomizationWindow" Text="Column chooser" BeginGroup="true" Image-IconID="spreadsheet_pivottablegroupselectioncontextmenuitem_16x16" />
                    <dx:GridViewToolbarItem AdaptivePriority="1" Name="Filter">
                        <Template>
                            <dx:ASPxCheckBox ID="checkShowFilter" runat="server" Text="Show filter column">
                                <ClientSideEvents CheckedChanged="function(s, e) { grid.PerformCallback('checkShowFilter'); }" />
                            </dx:ASPxCheckBox>
                        </Template>
                    </dx:GridViewToolbarItem>
                    <dx:GridViewToolbarItem BeginGroup="true">
                        <Template>
                            <dx:ASPxButtonEdit ID="tbSearch" runat="server" NullText="Search" Height="100%" />
                        </Template>
                    </dx:GridViewToolbarItem>
                </Items>
            </dx:GridViewToolbar>
        </Toolbars>

        <Columns>
            <dx:GridViewBandColumn Caption="Data" VisibleIndex="0">
                <HeaderStyle HorizontalAlign="Center" />
                <Columns>
                    <dx:GridViewDataComboBoxColumn FieldName="MODULE_ID" Caption="Module" Width="100" MaxWidth="100" Settings-AllowHeaderFilter="True">
                        <PropertiesComboBox DisplayFormatString="{1}" ValueField="module_id" TextField="module_title" />
                    </dx:GridViewDataComboBoxColumn>

                    <dx:GridViewDataDateColumn FieldName="LOG_DATE" Caption="Date" Width="70" MaxWidth="70" Settings-AllowHeaderFilter="True">
                        <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd HH:mm:ss">
                        </PropertiesDateEdit>
                    </dx:GridViewDataDateColumn>
                    
                    <dx:GridViewDataComboBoxColumn FieldName="LOG_TYPE" Caption="Log Type" Width="100" MaxWidth="100" Settings-AllowHeaderFilter="True">
                        <PropertiesComboBox DisplayFormatString="{1}" ValueField="log_type" TextField="log_type_desc" />
                    </dx:GridViewDataComboBoxColumn>
                    
                    <dx:GridViewDataComboBoxColumn FieldName="USER_ID" Caption="Log By" Width="75" MaxWidth="75" Settings-AllowHeaderFilter="True">
                        <PropertiesComboBox DisplayFormatString="{1}" ValueField="user_id" TextField="fullname" />
                    </dx:GridViewDataComboBoxColumn>
                    
                    <dx:GridViewDataTextColumn FieldName="LOCAL_IP" Caption="Local IP" Width="70" MaxWidth="75" Settings-AllowHeaderFilter="True" />
                    
                    <dx:GridViewDataTextColumn FieldName="REMOTE_IP" Caption="Remote IP" Width="70" MaxWidth="75" Settings-AllowHeaderFilter="True" />
                    <dx:GridViewDataTextColumn FieldName="LOCATION" Caption="Location" Width="50" MaxWidth="50" Settings-AllowHeaderFilter="True" />
                </Columns>


            </dx:GridViewBandColumn>
        </Columns>

        <Templates>
            <DetailRow>
                <dx:ASPxGridView ID="grid_detail" runat="server" AutoGenerateColumns="false" Width="100%"
                    OnBeforePerformDataSelect="grid_detail_BeforePerformDataSelect">
                    <Columns>
                        <dx:GridViewDataTextColumn FieldName="Title" Caption="Title" Width="100" MaxWidth="100" />
                        <dx:GridViewDataTextColumn FieldName="Data" Settings-AllowEllipsisInText="True" Caption="Data" />
                    </Columns>
                </dx:ASPxGridView>
            </DetailRow>
        </Templates>
    </dx:ASPxGridView>

    <dx:EntityServerModeDataSource runat="server" ID="UserLogData" ContextTypeName="CMS.Logs.Context.Entities" TableName="LOG_USER" 
        OnSelecting="UserLogData_Selecting" DefaultSorting="LOG_ID DESC" />

</asp:Content>
