<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CPanel.Master" CodeBehind="User.aspx.cs" Inherits="CMS.Management.User" %>

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

        function onToolbarItemClick(s, e) {
            if (e.item.name === 'Hystory') {
                e.processOnServer = true;
            }
        }
    </script>
    <dx:ASPxGridView runat="server" ID="grid" ClientInstanceName="grid" Width="100%" CssClass="w-100"
        KeyFieldName="user_id"
        EnableCallBacks="true"
        OnInit="grid_Init"
        OnDataBinding="grid_DataBinding"
        OnCustomButtonInitialize="grid_CustomButtonInitialize"
        OnCustomButtonCallback="grid_CustomButtonCallback"
        OnCustomCallback="grid_CustomCallback"
        OnRowValidating="grid_RowValidating"
        OnRowInserting="grid_RowInserting"
        OnRowUpdating="grid_RowUpdating"
        OnRowDeleting="grid_RowDeleting"
        OnHtmlRowPrepared="grid_HtmlRowPrepared"
        OnCellEditorInitialize="grid_CellEditorInitialize">
        <ClientSideEvents ToolbarItemClick="function(s,e){ onToolbarItemClick(s,e) }" />
        <ClientSideEvents EndCallback="function(s,e){grid_EndCallback(s,e)}" />
        <SettingsSearchPanel CustomEditorID="tbSearch" />
        <Toolbars>
            <dx:GridViewToolbar Name="Toolbar">
                <Items>
                    <%-- Image-IconID Refer to https://demos.devexpress.com/ASPxMultiUseControlsDemos/Features/IconLibraryExplorer.aspx --%>
                    <dx:GridViewToolbarItem Command="New" Name="New" Image-IconID="actions_new_16x16office2013" Text="New" />
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
                    <dx:GridViewToolbarItem>
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
                    <dx:GridViewDataTextColumn FieldName="username" Caption="Username" Width="100" MaxWidth="100" Settings-AllowHeaderFilter="True">
                        <PropertiesTextEdit MaxLength="20">
                            <ValidationSettings ValidationGroup="Validate" Display="Dynamic" ErrorTextPosition="Bottom" ErrorDisplayMode="Text">
                                <RequiredField IsRequired="true" ErrorText="* Data required" />
                                <ErrorFrameStyle CssClass="small pl-0"></ErrorFrameStyle>
                            </ValidationSettings>
                            <ReadOnlyStyle BackColor="Transparent"></ReadOnlyStyle>
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn FieldName="fullname" Caption="Full Name" MaxWidth="200" Settings-AllowHeaderFilter="True">
                        <PropertiesTextEdit MaxLength="100">
                            <ValidationSettings ValidationGroup="Validate" Display="Dynamic" ErrorTextPosition="Bottom" ErrorDisplayMode="Text">
                                <RequiredField IsRequired="true" ErrorText="* Data required" />
                                <ErrorFrameStyle CssClass="small pl-0"></ErrorFrameStyle>
                            </ValidationSettings>
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataComboBoxColumn FieldName="group_id" Caption="Group" MaxWidth="150" Settings-AllowHeaderFilter="True" UnboundType="Integer">
                        <PropertiesComboBox ValueField="group_id" TextField="group_name" ValueType="System.Int32"
                            TextFormatString="{0}" DisplayFormatString="{0}">
                        </PropertiesComboBox>
                        <EditItemTemplate>
                            <dx:ASPxGridLookup ID="lookup_group" ClientInstanceName="lookup_group" runat="server"
                                KeyFieldName="group_id" TextFormatString="{1}" AutoGenerateColumns="False" 
                                OnInit="lookup_group_Init" SelectionMode="Single" ValidationGroup="Validate"
                                MultiTextSeparator=";" SelectInputTextOnClick="false" >
                                <ValidationSettings ValidationGroup="Validate"  Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
                                    <RequiredField IsRequired="true" ErrorText="* Data required" />
                                    <ErrorFrameStyle CssClass="small p-0"></ErrorFrameStyle>
                                </ValidationSettings>
                                <Columns>
                                    <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="30" SelectAllCheckboxMode="Page" ShowCancelButton="true" ShowApplyFilterButton="true" />
                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="group_id" Visible="false" ReadOnly="True" />
                                    <dx:GridViewDataTextColumn Caption="Group Name" FieldName="group_name" />
                                    <dx:GridViewDataTextColumn Caption="Description" FieldName="group_desc" />
                                    <dx:GridViewDataTextColumn Caption="Role" FieldName="role_name" />
                                </Columns>
                                <GridViewProperties>                                    
                                    <Settings ShowFilterRow="True" ShowStatusBar="Visible" />
                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="true" />
                                    <SettingsPager EnableAdaptivity="true"></SettingsPager>
                                    <Templates>
                                        <StatusBar>
                                            <div class="row">
                                                <div class="col">                                                    
                                                    <dx:ASPxButton runat="server" AutoPostBack="false" Text="Clear" Image-IconID="reports_none_16x16office2013"
                                                    ClientSideEvents-Click="function(s,e){ 
                                                        lookup_group.SetValue(null);
                                                        }" />
                                                    <dx:ASPxButton runat="server" AutoPostBack="false" Text="Refresh" Image-IconID="actions_refresh_16x16office2013"
                                                    ClientSideEvents-Click="function(s,e){ 
                                                        lookup_group.GetGridView().Refresh(); 
                                                        }" />                                                    
                                                </div>
                                                <div class="col-auto float-right">
                                                    <dx:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" 
                                                    ClientSideEvents-Click="function(s,e){ 
                                                        lookup_group.ConfirmCurrentSelection();
                                                        lookup_group.HideDropDown();
                                                        lookup_group.Focus(); 
                                                        }" />
                                                </div>
                                            </div>
                                        </StatusBar>
                                    </Templates>
                                </GridViewProperties>
                                <ValidationSettings ValidationGroup="Validate"  Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
                                    <ErrorFrameStyle CssClass="small p-0"></ErrorFrameStyle>
                                </ValidationSettings>
                            </dx:ASPxGridLookup>                            
                        </EditItemTemplate>
                    </dx:GridViewDataComboBoxColumn>        

                    <dx:GridViewDataTextColumn FieldName="email" Caption="Email" MaxWidth="150">
                        <PropertiesTextEdit MaxLength="100">
                            <ValidationSettings ValidationGroup="Validate" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
                                <RegularExpression ErrorText="* Invalid email address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                <RequiredField IsRequired="true" ErrorText="* Data required" />                                
                                <ErrorFrameStyle CssClass="small pl-0"></ErrorFrameStyle>
                            </ValidationSettings>
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataComboBoxColumn FieldName="gender" Caption="Gender" MaxWidth="75">
                        <PropertiesComboBox ValueField="gender_id" TextField="gender_desc"
                            TextFormatString="{0}" DisplayFormatString="{0}">
                            <ValidationSettings ValidationGroup="Validate" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
                                <RequiredField IsRequired="true" ErrorText="* Data required" />
                                <ErrorFrameStyle CssClass="small pl-0"></ErrorFrameStyle>
                            </ValidationSettings>
                        </PropertiesComboBox>
                        <EditItemTemplate>
                            <dx:ASPxGridLookup ID="lookup_gender" ClientInstanceName="lookup_gender" runat="server"
                                KeyFieldName="gender_id" TextFormatString="{1}" AutoGenerateColumns="False" 
                                OnInit="lookup_gender_Init" SelectionMode="Single" ValidationGroup="Validate"
                                MultiTextSeparator=";">
                                <ValidationSettings ValidationGroup="Validate"  Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
                                    <RequiredField IsRequired="true" ErrorText="* Data required" />
                                    <ErrorFrameStyle CssClass="small p-0"></ErrorFrameStyle>
                                </ValidationSettings>
                                <Columns>
                                    <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="30" SelectAllCheckboxMode="Page" ShowCancelButton="true" ShowApplyFilterButton="true" />
                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="gender_id" Visible="false" ReadOnly="True" />
                                    <dx:GridViewDataTextColumn Caption="Gender" FieldName="gender_desc" />
                                </Columns>
                                <GridViewProperties>                                    
                                    <Settings ShowFilterRow="True" ShowStatusBar="Visible" />
                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="true" />
                                    <SettingsPager EnableAdaptivity="true"></SettingsPager>
                                    <Templates>
                                        <StatusBar>
                                            <div class="row">
                                                <div class="col">                                                    
                                                    <dx:ASPxButton runat="server" AutoPostBack="false" Text="Clear" Image-IconID="reports_none_16x16office2013"
                                                    ClientSideEvents-Click="function(s,e){ 
                                                        lookup_gender.SetValue(null);
                                                        }" />
                                                    <dx:ASPxButton runat="server" AutoPostBack="false" Text="Refresh" Image-IconID="actions_refresh_16x16office2013"
                                                    ClientSideEvents-Click="function(s,e){ 
                                                        lookup_gender.GetGridView().Refresh(); 
                                                        }" />                                                    
                                                </div>
                                                <div class="col-auto float-right">
                                                    <dx:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" 
                                                    ClientSideEvents-Click="function(s,e){ 
                                                        lookup_gender.ConfirmCurrentSelection();
                                                        lookup_gender.HideDropDown();
                                                        lookup_gender.Focus(); 
                                                        }" />
                                                </div>
                                            </div>
                                        </StatusBar>
                                    </Templates>
                                </GridViewProperties>
                                <ValidationSettings ValidationGroup="Validate"  Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
                                    <ErrorFrameStyle CssClass="small p-0"></ErrorFrameStyle>
                                </ValidationSettings>
                            </dx:ASPxGridLookup>                            
                        </EditItemTemplate>
                    </dx:GridViewDataComboBoxColumn>                    

                    <dx:GridViewDataTextColumn FieldName="phone" Caption="Phone/WhatsApp" Visible="false" EditFormSettings-Visible="True" EditFormSettings-VisibleIndex="5" MaxWidth="130" PropertiesTextEdit-MaskSettings-Mask="+99-9999-9999-9999">
                        <PropertiesTextEdit MaxLength="18">
                            <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
                                <ErrorFrameStyle CssClass="small pl-0"></ErrorFrameStyle>
                            </ValidationSettings>
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataComboBoxColumn FieldName="theme_id" Caption="Theme" MaxWidth="75">
                        <PropertiesComboBox TextField="theme_name" ValueField="theme_id" ValueType="System.Int32"
                            TextFormatString="{0}" DisplayFormatString="{0}">
                            <ValidationSettings ValidationGroup="Validate" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
                                <RequiredField IsRequired="true" ErrorText="* Data required" />
                                <ErrorFrameStyle CssClass="small pl-0"></ErrorFrameStyle>
                            </ValidationSettings>
                        </PropertiesComboBox>
                        <EditItemTemplate>
                            <dx:ASPxGridLookup ID="lookup_theme" ClientInstanceName="lookup_theme" runat="server"
                                KeyFieldName="theme_id" TextFormatString="{1}" AutoGenerateColumns="False" 
                                OnInit="lookup_theme_Init" SelectionMode="Single" ValidationGroup="Validate"
                                MultiTextSeparator=";">
                                <ValidationSettings ValidationGroup="Validate"  Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
                                    <RequiredField IsRequired="true" ErrorText="* Data required" />
                                    <ErrorFrameStyle CssClass="small p-0"></ErrorFrameStyle>
                                </ValidationSettings>
                                <Columns>
                                    <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="30" SelectAllCheckboxMode="Page" ShowCancelButton="true" ShowApplyFilterButton="true" />
                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="theme_id" Visible="false" ReadOnly="True" />
                                    <dx:GridViewDataTextColumn Caption="Theme" FieldName="theme_name" />
                                    <dx:GridViewDataTextColumn Caption="Description" FieldName="theme_desc" />
                                </Columns>
                                <GridViewProperties>                                    
                                    <Settings ShowFilterRow="True" ShowStatusBar="Visible" />
                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="true" />
                                    <SettingsPager EnableAdaptivity="true"></SettingsPager>
                                    <Templates>
                                        <StatusBar>
                                            <div class="row">
                                                <div class="col">                                                    
                                                    <dx:ASPxButton runat="server" AutoPostBack="false" Text="Clear" Image-IconID="reports_none_16x16office2013"
                                                    ClientSideEvents-Click="function(s,e){ 
                                                        lookup_theme.SetValue(null);
                                                        }" />
                                                    <dx:ASPxButton runat="server" AutoPostBack="false" Text="Refresh" Image-IconID="actions_refresh_16x16office2013"
                                                    ClientSideEvents-Click="function(s,e){ 
                                                        lookup_theme.GetGridView().Refresh(); 
                                                        }" />                                                    
                                                </div>
                                                <div class="col-auto float-right">
                                                    <dx:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" 
                                                    ClientSideEvents-Click="function(s,e){ 
                                                        lookup_theme.ConfirmCurrentSelection();
                                                        lookup_theme.HideDropDown();
                                                        lookup_theme.Focus(); 
                                                        }" />
                                                </div>
                                            </div>
                                        </StatusBar>
                                    </Templates>
                                </GridViewProperties>
                                <ValidationSettings ValidationGroup="Validate"  Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
                                    <ErrorFrameStyle CssClass="small p-0"></ErrorFrameStyle>
                                </ValidationSettings>
                            </dx:ASPxGridLookup>
                        </EditItemTemplate>
                    </dx:GridViewDataComboBoxColumn>
                    
                    <dx:GridViewDataMemoColumn FieldName="notes" Caption="Notes" Visible="false" EditFormSettings-Visible="True"
                        VisibleIndex="10">
                        <PropertiesMemoEdit MaxLength="500" Rows="5">
                        </PropertiesMemoEdit>
                    </dx:GridViewDataMemoColumn>

                    <dx:GridViewDataTextColumn FieldName="telegram_id" MinWidth="50" Caption="Telegram ID" EditFormSettings-Visible="True" Visible="false" EditFormSettings-VisibleIndex="8">
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataComboBoxColumn FieldName="company_id" Caption="Company" MaxWidth="75" Visible="false" EditFormSettings-Visible="True" EditFormSettings-VisibleIndex="9">
                        <PropertiesComboBox TextField="company_name" ValueField="company_id" ValueType="System.Int32"
                            TextFormatString="{0}" DisplayFormatString="{0}">
                            <ValidationSettings ValidationGroup="Validate" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
                                <RequiredField IsRequired="true" ErrorText="* Data required" />
                                <ErrorFrameStyle CssClass="small pl-0"></ErrorFrameStyle>
                            </ValidationSettings>
                            <Columns>
                                <dx:ListBoxColumn Visible="false" FieldName="company_id"></dx:ListBoxColumn>
                                <dx:ListBoxColumn Caption="Company Name" FieldName="company_name"></dx:ListBoxColumn>
                                <dx:ListBoxColumn Caption="Description" FieldName="company_desc"></dx:ListBoxColumn>
                            </Columns>
                        </PropertiesComboBox>
                        <EditItemTemplate>
                            <dx:ASPxGridLookup ID="lookup_company" ClientInstanceName="lookup_company" runat="server"
                                KeyFieldName="company_id" TextFormatString="{1}" AutoGenerateColumns="False" 
                                OnInit="lookup_company_Init" SelectionMode="Single" ValidationGroup="Validate"
                                MultiTextSeparator=";">
                                <ValidationSettings ValidationGroup="Validate"  Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
                                    <RequiredField IsRequired="true" ErrorText="* Data required" />
                                    <ErrorFrameStyle CssClass="small p-0"></ErrorFrameStyle>
                                </ValidationSettings>
                                <Columns>
                                    <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="30" SelectAllCheckboxMode="Page" ShowCancelButton="true" ShowApplyFilterButton="true" />
                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="company_id" Visible="false" ReadOnly="True" />
                                    <dx:GridViewDataTextColumn Caption="Company" FieldName="company_name" />
                                    <dx:GridViewDataTextColumn Caption="Description" FieldName="company_desc" />
                                </Columns>
                                <GridViewProperties>                                    
                                    <Settings ShowFilterRow="True" ShowStatusBar="Visible" />
                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="true" />
                                    <SettingsPager EnableAdaptivity="true"></SettingsPager>
                                    <Templates>
                                        <StatusBar>
                                            <div class="row">
                                                <div class="col">                                                    
                                                    <dx:ASPxButton runat="server" AutoPostBack="false" Text="Clear" Image-IconID="reports_none_16x16office2013"
                                                    ClientSideEvents-Click="function(s,e){ 
                                                        lookup_company.SetValue(null);
                                                        }" />
                                                    <dx:ASPxButton runat="server" AutoPostBack="false" Text="Refresh" Image-IconID="actions_refresh_16x16office2013"
                                                    ClientSideEvents-Click="function(s,e){ 
                                                        lookup_company.GetGridView().Refresh(); 
                                                        }" />                                                    
                                                </div>
                                                <div class="col-auto float-right">
                                                    <dx:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" 
                                                    ClientSideEvents-Click="function(s,e){ 
                                                        lookup_company.ConfirmCurrentSelection();
                                                        lookup_company.HideDropDown();
                                                        lookup_company.Focus(); 
                                                        }" />
                                                </div>
                                            </div>
                                        </StatusBar>
                                    </Templates>
                                </GridViewProperties>
                                <ValidationSettings ValidationGroup="Validate"  Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
                                    <ErrorFrameStyle CssClass="small p-0"></ErrorFrameStyle>
                                </ValidationSettings>
                            </dx:ASPxGridLookup>                            
                        </EditItemTemplate>
                    </dx:GridViewDataComboBoxColumn>                    

                    <dx:GridViewDataCheckColumn FieldName="isrequired_token" Caption="Required token" EditFormSettings-Visible="True" Visible="false" EditFormSettings-VisibleIndex="10">
                    </dx:GridViewDataCheckColumn>

                    <dx:GridViewDataCheckColumn MinWidth="50" Width="60" FieldName="isactive" Caption="Active" EditFormSettings-Visible="False" />
                    <dx:GridViewDataComboBoxColumn FieldName="createby" Caption="Create by" Visible="false" Settings-AllowHeaderFilter="True" MaxWidth="100">
                        <PropertiesComboBox DisplayFormatString="{1}" ValueField="user_id" TextField="fullname" />
                    </dx:GridViewDataComboBoxColumn> 
                    <dx:GridViewDataDateColumn FieldName="createdate" Caption="Create Date" Visible="false" Settings-AllowHeaderFilter="True"  />
                    <dx:GridViewDataComboBoxColumn FieldName="updateby" Caption="Update by" Visible="false" Settings-AllowHeaderFilter="True" MaxWidth="100">
                        <PropertiesComboBox DisplayFormatString="{1}" ValueField="user_id" TextField="fullname" />
                    </dx:GridViewDataComboBoxColumn> 
                    <dx:GridViewDataTextColumn FieldName="updatedate" Caption="Update Date" Visible="false" Settings-AllowHeaderFilter="True" />
                </Columns>

            </dx:GridViewBandColumn>
            <dx:GridViewCommandColumn HeaderStyle-HorizontalAlign="Center"
                AdaptivePriority="2"
                Width="230"
                VisibleIndex="0"
                ButtonRenderMode="Button"
                ShowEditButton="true"
                ShowDeleteButton="true"
                ShowApplyFilterButton="true"
                ShowClearFilterButton="true"
                Caption="Action">
                <CustomButtons>                    
                    <dx:GridViewCommandColumnCustomButton Text="Unhold" ID="ButtonEnable"></dx:GridViewCommandColumnCustomButton>
                    <dx:GridViewCommandColumnCustomButton Text="Hold" ID="ButtonDisable"></dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dx:GridViewCommandColumn>
        </Columns>
        <SettingsEditing EditFormColumnCount="2" />
        <Templates>
            <EditForm>
                <div class="d-flex flex-row">
                    <% if (!grid.IsNewRowEditing) { %>
                    <div class="col-auto">
                        <div style="border: solid 1px #c0c0c0; padding: 2px;">
                            <asp:HiddenField runat="server" ID="hPhoto" Value='<%# Eval("Photo") %>'></asp:HiddenField>
                            <dx:ASPxBinaryImage ID="photo" ClientInstanceName="photo" Width="250" Height="250" runat="server"
                                ShowLoadingImage="true"
                                EditingSettings-AllowDropOnPreview="true"
                                LoadingImageUrl="~/Content/images/Icon/load.gif"
                                BinaryStorageMode="Session"
                                Value='<%# (Eval("photo") == null ? new byte[0] : CMS.Helper.Converter.GetByteArrayFromImage(Server.MapPath(Eval("photo").ToString())))%>'>
                                <EditingSettings Enabled="true">
                                    <UploadSettings>
                                        <UploadValidationSettings MaxFileSize="4194304"></UploadValidationSettings>
                                    </UploadSettings>
                                    <ButtonPanelSettings ButtonPosition="Center" Visibility="OnMouseOver" />
                                </EditingSettings>
                            </dx:ASPxBinaryImage>
                        </div>
                    </div>
                    <% } %>
                    <div style="width:100%">
                        <dx:ASPxGridViewTemplateReplacement ID="Editors" ReplacementType="EditFormEditors" runat="server" />
                        <div class="d-flex justify-content-end mt-3 mb-3">
                            <dx:ASPxButton CssClass="mr-2" ID="UpdateButton" runat="server" Text="Update" AutoPostBack="false">
                                <ClientSideEvents Click="function(s,e) { if(ASPxClientEdit.ValidateGroup('Validate'))grid.UpdateEdit();}" />
                            </dx:ASPxButton>
                            <dx:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton" runat="server" />
                        </div>
                    </div>
                </div>
            </EditForm>
        </Templates>
    </dx:ASPxGridView>

    <dx:ASPxPopupControl ID="Confirm" runat="server" ClientInstanceName="Confirm"
        Width="450" Height="150" ClientSideEvents-PopUp="function(s,e){s.ShowAtPos(s.Left, 100)}"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="BottomSides"
        CloseAction="CloseButton" CloseOnEscape="true" AllowDragging="true" PopupAnimationType="None"
        EnableViewState="false" AutoUpdatePosition="true" ShowFooter="true" FooterImage-AlternateText="FS"
        HeaderText="Confirmation" >
        <ContentCollection>
            <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
                <p>Are you sure want delete selected data?</p>
                <dx:ASPxGridView runat="server" ID="grid_delete" ClientInstanceName="grid_delete"  AutoGenerateColumns="true" CssClass="w-100">
                    <SettingsPager EnableAdaptivity="true" />
                    <SettingsBehavior AllowEllipsisInText="true" />
                </dx:ASPxGridView>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <FooterTemplate>
            <table class="w-100">
                <tr>
                    <td class="d-flex justify-content-end p-2">
                        <dx:ASPxButton CssClass="mr-2" ID="YesButton" runat="server" Text="Yes" AutoPostBack="False"
                            ClientSideEvents-Click="function(s, e) { 
                            var k = grid.GetSelectedKeysOnPage();
                            grid.PerformCallback('Delete');
                            Confirm.Hide();                            
                            }" />
                        <dx:ASPxButton ID="CancelButton" runat="server" Text="Cancel" AutoPostBack="False"
                            ClientSideEvents-Click="function(s, e) { Confirm.Hide();  }" />
                    </td>
                </tr>
            </table>
        </FooterTemplate>
    </dx:ASPxPopupControl>

</asp:Content>
