<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CPanel.Master" CodeBehind="Organization.aspx.cs" Inherits="KMS.Management.Structure.Organization" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v19.2, Version=19.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxDiagram.v19.2, Version=19.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxDiagram" TagPrefix="dx" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function grid_EndCallback(s, e) {
            if (s.cpRefresh) {
                grid.Refresh();
                s.cpRefresh = false;
            }
            if (s.cpSuccess) {
                treelist.Refresh();
                cpDiagram.PerformCallback();
            }

            EndCallback(s, e);
        }
    </script>
    <dx:ASPxPageControl ID="TabOrganization" CssClass="w-100 mb-3" runat="server" ActiveTabIndex="0">
        <TabPages>
            <dx:TabPage Text="Data">
                <ContentCollection>
                    <dx:ContentControl runat="server">
                        <dx:ASPxGridView runat="server" ID="grid" ClientInstanceName="grid" Width="100%"
                            KeyFieldName="user_id"
                            EnableCallBacks="true"
                            OnInit="grid_Init"
                            OnCustomButtonInitialize="grid_CustomButtonInitialize"
                            OnCustomButtonCallback="grid_CustomButtonCallback"
                            OnHtmlRowPrepared="grid_HtmlRowPrepared"
                            OnCustomCallback="grid_CustomCallback"
                            OnRowUpdating="grid_RowUpdating"
                            OnCellEditorInitialize="grid_CellEditorInitialize"
                            OnCustomColumnDisplayText="grid_CustomColumnDisplayText"
                            ClientSideEvents-EndCallback="function(s,e){grid_EndCallback(s,e)}">
                            <SettingsSearchPanel CustomEditorID="tbSearch" />
                            <Toolbars>
                                <dx:GridViewToolbar Name="Toolbar">
                                    <Items>
                                        <%-- Image-IconID Refer to https://demos.devexpress.com/ASPxMultiUseControlsDemos/Features/IconLibraryExplorer.aspx --%>
                                        <dx:GridViewToolbarItem Visible="false" Command="New" Name="New" Image-IconID="actions_new_16x16office2013" Text="New" />
                                        <dx:GridViewToolbarItem Command="Refresh" Name="Refresh" BeginGroup="true" Image-IconID="actions_refresh_16x16office2013" />
                                        <dx:GridViewToolbarItem AdaptivePriority="1" Name="Export" Text="Download" Image-IconID="export_export_16x16office2013" BeginGroup="true">
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
                                        <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="30" ShowClearFilterButton="true" SelectAllCheckboxMode="Page" />

                                        <dx:GridViewDataComboBoxColumn FieldName="user_id" Caption="User" Width="8%" ReadOnly="false" Settings-AllowHeaderFilter="True">
                                            <PropertiesComboBox ValueField="user_id" TextField="fullname" ValueType="System.Int32" ReadOnlyStyle-BackColor="Transparent"
                                                TextFormatString="{0}" DisplayFormatString="{0}">
                                                <ValidationSettings ValidationGroup="Validate" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
                                                    <RequiredField ErrorText="* Data required" IsRequired="true" />
                                                    <ErrorFrameStyle CssClass="small pl-0"></ErrorFrameStyle>
                                                </ValidationSettings>
                                            </PropertiesComboBox>
                                        </dx:GridViewDataComboBoxColumn>

                                        <dx:GridViewDataComboBoxColumn FieldName="user_root" Caption="Direct" Width="8%" ReadOnly="false" Settings-AllowHeaderFilter="True">
                                            <PropertiesComboBox ValueField="user_id" TextField="fullname" ValueType="System.Int32"
                                                TextFormatString="{0}" DisplayFormatString="{0}">
                                            </PropertiesComboBox>
                                            <EditItemTemplate>
                                                <dx:ASPxGridLookup ID="lookup_user_root" ClientInstanceName="lookup_user_root" runat="server" SelectionMode="Single"
                                                    KeyFieldName="user_id" TextFormatString="{0}" DisplayFormatString="{0}" AutoGenerateColumns="False"
                                                    OnInit="lookup_user_root_Init">
                                                    <GridViewProperties>
                                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True" AllowSelectByRowClick="True" />
                                                    </GridViewProperties>
                                                    <Columns>
                                                        <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="30" />
                                                        <dx:GridViewDataTextColumn FieldName="fullname" Caption="User" />
                                                        <dx:GridViewDataTextColumn FieldName="gender_desc" Caption="Gender" />
                                                    </Columns>
                                                    <GridViewProperties>
                                                        <Settings ShowFilterRow="True" ShowStatusBar="Visible" />
                                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="true" />
                                                        <SettingsPager EnableAdaptivity="true"></SettingsPager>
                                                        <Templates>
                                                            <StatusBar>
                                                                <div class="d-flex justify-content-between">
                                                                    <div class="col px-0">
                                                                        <dx:ASPxButton CssClass="align-self-center" runat="server" AutoPostBack="false" Text="Clear" Image-IconID="reports_none_16x16office2013"
                                                                            ClientSideEvents-Click="function(s,e){ 
                                                                            lookup_user_root.SetValue(null);
                                                                            }" />
                                                                        <dx:ASPxButton CssClass="align-self-center" runat="server" AutoPostBack="false" Text="Refresh" Image-IconID="actions_refresh_16x16office2013"
                                                                            ClientSideEvents-Click="function(s,e){ 
                                                                            lookup_user_root.GetGridView().Refresh(); 
                                                                            }" />
                                                                    </div>
                                                                    <div class="col-auto float-right">
                                                                        <dx:ASPxButton CssClass="align-self-center" ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Close"
                                                                            ClientSideEvents-Click="function(s,e){ 
                                                                            lookup_user_root.ConfirmCurrentSelection();
                                                                            lookup_user_root.HideDropDown();
                                                                            lookup_user_root.Focus(); 
                                                                            }" />
                                                                    </div>
                                                                </div>
                                                            </StatusBar>
                                                        </Templates>
                                                    </GridViewProperties>
                                                    <ValidationSettings ValidationGroup="Validate" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
                                                        <ErrorFrameStyle CssClass="small p-0"></ErrorFrameStyle>
                                                    </ValidationSettings>
                                                </dx:ASPxGridLookup>
                                            </EditItemTemplate>
                                        </dx:GridViewDataComboBoxColumn>

                                        <dx:GridViewDataComboBoxColumn FieldName="division_id" Caption="Division" Width="8%" ReadOnly="false" Settings-AllowHeaderFilter="True">
                                            <PropertiesComboBox ValueField="division_id" TextField="division_name" ValueType="System.Int32"
                                                TextFormatString="{0}" DisplayFormatString="{0}">
                                                <ValidationSettings ValidationGroup="Validate" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
                                                    <RequiredField ErrorText="* Data required" IsRequired="true" />
                                                    <ErrorFrameStyle CssClass="small pl-0"></ErrorFrameStyle>
                                                </ValidationSettings>
                                            </PropertiesComboBox>
                                            <EditItemTemplate>
                                                <dx:ASPxGridLookup ID="lookup_division" ClientInstanceName="lookup_division" runat="server" SelectionMode="Single"
                                                    KeyFieldName="division_id" TextFormatString="{1}" DisplayFormatString="{1}" AutoGenerateColumns="False"
                                                    OnInit="lookup_division_Init">
                                                    <ValidationSettings ValidationGroup="Validate" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
                                                        <RequiredField IsRequired="true" ErrorText="* Data required" />
                                                        <ErrorFrameStyle CssClass="small p-0"></ErrorFrameStyle>
                                                    </ValidationSettings>
                                                    <GridViewProperties>
                                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True" AllowSelectByRowClick="True" />
                                                    </GridViewProperties>
                                                    <Columns>
                                                        <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="30" />
                                                        <dx:GridViewDataTextColumn FieldName="division_id" Visible="false" ReadOnly="True" />
                                                        <dx:GridViewDataTextColumn FieldName="division_name" Caption="Division" />
                                                        <dx:GridViewDataTextColumn FieldName="department_name" Caption="Department" />
                                                    </Columns>
                                                    <GridViewProperties>
                                                        <Settings ShowFilterRow="True" ShowStatusBar="Visible" />
                                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="true" />
                                                        <SettingsPager EnableAdaptivity="true"></SettingsPager>
                                                        <Templates>
                                                            <StatusBar>
                                                                <div class="d-flex justify-content-between">
                                                                    <div class="col px-0">
                                                                        <dx:ASPxButton CssClass="align-self-center" runat="server" AutoPostBack="false" Text="Clear" Image-IconID="reports_none_16x16office2013"
                                                                            ClientSideEvents-Click="function(s,e){ 
                                                                            lookup_division.SetValue(null);
                                                                            }" />
                                                                        <dx:ASPxButton CssClass="align-self-center" runat="server" AutoPostBack="false" Text="Refresh" Image-IconID="actions_refresh_16x16office2013"
                                                                            ClientSideEvents-Click="function(s,e){ 
                                                                            lookup_division.GetGridView().Refresh(); 
                                                                            }" />
                                                                    </div>
                                                                    <div class="col-auto float-right">
                                                                        <dx:ASPxButton CssClass="align-self-center" runat="server" AutoPostBack="false" Text="Close"
                                                                            ClientSideEvents-Click="function(s,e){ 
                                                                            lookup_division.ConfirmCurrentSelection();
                                                                            lookup_division.HideDropDown();
                                                                            lookup_division.Focus(); 
                                                                            }" />
                                                                    </div>
                                                                </div>
                                                            </StatusBar>
                                                        </Templates>
                                                    </GridViewProperties>
                                                    <ValidationSettings ValidationGroup="Validate" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
                                                        <ErrorFrameStyle CssClass="small p-0"></ErrorFrameStyle>
                                                    </ValidationSettings>
                                                </dx:ASPxGridLookup>
                                            </EditItemTemplate>
                                        </dx:GridViewDataComboBoxColumn>

                                        <dx:GridViewDataCheckColumn FieldName="can_approve" Caption="Can Approve" Width="30" />

                                        <dx:GridViewDataComboBoxColumn FieldName="createby" Caption="Create by" Visible="false" Settings-AllowHeaderFilter="True" MaxWidth="100">
                                            <PropertiesComboBox DisplayFormatString="{1}" ValueField="user_id" TextField="fullname" />
                                        </dx:GridViewDataComboBoxColumn>
                                        <dx:GridViewDataDateColumn FieldName="createdate" Caption="Create Date" Visible="false" Settings-AllowHeaderFilter="True" />
                                        <dx:GridViewDataComboBoxColumn FieldName="updateby" Caption="Update by" Visible="false" Settings-AllowHeaderFilter="True" MaxWidth="100">
                                            <PropertiesComboBox DisplayFormatString="{1}" ValueField="user_id" TextField="fullname" />
                                        </dx:GridViewDataComboBoxColumn>
                                        <dx:GridViewDataTextColumn FieldName="updatedate" Caption="Update Date" Visible="false" Settings-AllowHeaderFilter="True" />

                                    </Columns>
                                </dx:GridViewBandColumn>

                                <dx:GridViewCommandColumn HeaderStyle-HorizontalAlign="Center"
                                    AdaptivePriority="2"
                                    VisibleIndex="0"
                                    ButtonRenderMode="Button"
                                    ShowEditButton="true"
                                    ShowDeleteButton="false"
                                    ShowApplyFilterButton="true"
                                    ShowClearFilterButton="true"
                                    Width="30"
                                    Caption="Action">
                                </dx:GridViewCommandColumn>
                            </Columns>

                            <Templates>
                                <EditForm>
                                    <dx:ASPxGridViewTemplateReplacement ID="Editors" ReplacementType="EditFormEditors" runat="server" />
                                    <div class="d-flex justify-content-end mt-3 mb-3">
                                        <dx:ASPxButton CssClass="mr-2" runat="server" Text="Update" AutoPostBack="false">
                                            <ClientSideEvents Click="function(s,e) { if(ASPxClientEdit.ValidateGroup('Validate'))grid.UpdateEdit();}" />
                                        </dx:ASPxButton>
                                        <dx:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton" runat="server" />
                                    </div>
                                </EditForm>
                            </Templates>
                        </dx:ASPxGridView>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>

            <dx:TabPage Text="Tree Diagram">
                <ContentCollection>
                    <dx:ContentControl runat="server">
                        <dx:ASPxTreeList ID="treelist" ClientInstanceName="treelist" runat="server" Theme="Default"
                            KeyFieldName="user_id" ParentFieldName="user_root"
                            AutoGenerateColumns="false" Width="100%"
                            OnInit="treelist_Init" 
                            OnCustomCallback="treelist_CustomCallback"
                            OnCommandColumnButtonInitialize="treelist_CommandColumnButtonInitialize">
                            <Settings GridLines="Both" ShowFilterBar="Hidden" />
                            <SettingsBehavior AllowFocusedNode="True" ExpandCollapseAction="NodeDblClick" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsSearchPanel CustomEditorID="tbSearch" />

                            <Toolbars>
                                <dx:TreeListToolbar Name="Toolbar">
                                    <SettingsAdaptivity Enabled="true" EnableCollapseRootItemsToIcons="true" />
                                    <Items>
                                        <%-- Image-IconID Refer to https://demos.devexpress.com/ASPxMultiUseControlsDemos/Features/IconLibraryExplorer.aspx --%>
                                        <dx:TreeListToolbarItem Visible="false" Command="New" Name="New" Image-IconID="actions_new_16x16office2013" Text="New" />
                                        <dx:TreeListToolbarItem Command="Refresh" Name="Refresh" BeginGroup="true" Image-IconID="actions_refresh_16x16office2013" />
                                        <dx:TreeListToolbarItem AdaptivePriority="1" Name="Export" Text="Download" Image-IconID="export_export_16x16office2013" BeginGroup="true">
                                            <Items>
                                                <dx:TreeListToolbarItem Command="ExportToXls" Text="Export to XLS" Image-IconID="mail_sendxls_16x16office2013" />
                                                <dx:TreeListToolbarItem Command="ExportToXlsx" Text="Export to XLSX" Image-IconID="export_exporttoxlsx_16x16office2013" />
                                                <dx:TreeListToolbarItem Command="ExportToPdf" Visible="true" />
                                                <dx:TreeListToolbarItem Command="ExportToDocx" Visible="true" />
                                                <dx:TreeListToolbarItem Command="ExportToRtf" Visible="true" />
                                            </Items>
                                        </dx:TreeListToolbarItem>
                                        <dx:TreeListToolbarItem AdaptivePriority="1" Name="Import" Visible="false" Text="Upload from" Image-IconID="actions_download_16x16office2013" BeginGroup="true">
                                            <Items>
                                                <dx:TreeListToolbarItem Text="CSV" Image-IconID="mail_sendcsv_16x16office2013" />
                                                <dx:TreeListToolbarItem Text="Excel File" Image-IconID="mail_sendxls_16x16office2013" />
                                            </Items>
                                        </dx:TreeListToolbarItem>
                                        <dx:TreeListToolbarItem Command="ShowCustomizationWindow" Text="Column chooser" BeginGroup="true" Image-IconID="spreadsheet_pivottablegroupselectioncontextmenuitem_16x16" />
                                        <dx:TreeListToolbarItem AdaptivePriority="1" Name="Filter">
                                            <Template>
                                                <dx:ASPxCheckBox ID="checkShowFilter" runat="server" Text="Show filter column">
                                                    <ClientSideEvents CheckedChanged="function(s, e) { treelist.PerformCallback('checkShowFilter'); }" />
                                                </dx:ASPxCheckBox>
                                            </Template>
                                        </dx:TreeListToolbarItem>
                                        <dx:TreeListToolbarItem BeginGroup="true">
                                            <Template>
                                                <dx:ASPxButtonEdit ID="tbSearch" runat="server" NullText="Search" Height="100%" />
                                            </Template>
                                        </dx:TreeListToolbarItem>
                                    </Items>
                                </dx:TreeListToolbar>
                            </Toolbars>
                            <Columns>
                                <dx:TreeListComboBoxColumn FieldName="fullname" Caption="User" AllowHeaderFilter="True">
                                </dx:TreeListComboBoxColumn>

                                <dx:TreeListComboBoxColumn FieldName="division_name" Caption="Division" AllowHeaderFilter="True">
                                </dx:TreeListComboBoxColumn>

                                <dx:TreeListComboBoxColumn FieldName="department_name" Caption="Department" AllowHeaderFilter="True">
                                </dx:TreeListComboBoxColumn>

                            </Columns>
                            <Templates>
                                <EditForm>
                                    <dx:ASPxTreeListTemplateReplacement ID="Editors" ReplacementType="Editors" runat="server" />
                                    <div class="d-flex justify-content-end mt-3 mb-3">
                                        <dx:ASPxTreeListTemplateReplacement ID="UpdateButton" ReplacementType="UpdateButton" runat="server" />
                                        <dx:ASPxTreeListTemplateReplacement ID="CancelButton" ReplacementType="CancelButton" runat="server" />
                                    </div>
                                </EditForm>
                            </Templates>
                        </dx:ASPxTreeList>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>

            <dx:TabPage Text="Layout Diagram">
                <ContentCollection>
                    <dx:ContentControl runat="server">
                        <dx:ASPxButton runat="server" Text="Refresh" ID="btnAdd" CssClass="mb-2"
                            ClientSideEvents-Click='function(s,e){e.processOnServer=false;cpDiagram.PerformCallback()}'
                            Image-IconID="actions_refresh_16x16office2013">
                        </dx:ASPxButton>
                        <dx:ASPxCallbackPanel runat="server" ID="cpDiagram" ClientInstanceName="cpDiagram" OnCallback="cpDiagram_Callback">
                            <PanelCollection>
                                <dx:PanelContent>
                                    <dx:ASPxDiagram ID="diagram" ClientInstanceName="diagram" runat="server" Width="100%" Height="600px"
                                        SimpleView="true" ReadOnly="true" ViewUnits="Px">
                                        <SettingsAutoLayout Type="Layered" Orientation="Vertical" />
                                        <SettingsGrid SnapToGrid="true" Visible="false" />
                                        <SettingsToolbox Visibility="Disabled" />
                                        <Mappings>
                                            <Node Key="user_id" ParentKey="user_root" Text="fullname" />
                                        </Mappings>
                                    </dx:ASPxDiagram>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxCallbackPanel>

                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>


        </TabPages>
    </dx:ASPxPageControl>
</asp:Content>
