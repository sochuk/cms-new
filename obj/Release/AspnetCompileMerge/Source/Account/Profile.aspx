<%@ Page Language="C#" MasterPageFile="~/CPanel.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="CMS.Account.Profile" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .dxeBinImgSys{
        }
    </style>
    <script type="text/javascript">
        function ValueChanged(s, e) {
            var img = s.GetMainElement().querySelector("img");
            console.log(img.src);
            console.log(e);
            grid.PerformCallback('ChangePhoto');
        }

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

    <div class="mb-5 w-auto">
        <dx:ASPxGridView runat="server" ID="grid" ClientInstanceName="grid" Width="95%"
            AutoGenerateColumns="false"
            KeyFieldName="user_id"
            EnableCallBacks="true"
            OnInit="grid_Init"
            OnCustomButtonCallback="grid_CustomButtonCallback"
            OnRowUpdating="grid_RowUpdating"
            OnHtmlRowPrepared="grid_HtmlRowPrepared"
            ClientSideEvents-EndCallback="function(s,e){ grid_EndCallback(s,e) }">
            <SettingsDataSecurity AllowInsert="false" AllowDelete="false" />
            <Columns>
                <dx:GridViewDataTextColumn FieldName="username" Caption="Username" Width="100" MinWidth="100" Visible="false" EditFormSettings-Visible="True" ReadOnly="true" EditFormSettings-VisibleIndex="0">
                    <PropertiesTextEdit MaxLength="20">
                        <ValidationSettings ValidationGroup="Validate" Display="Dynamic" ErrorTextPosition="Bottom" ErrorDisplayMode="Text">
                            <RequiredField IsRequired="true" ErrorText="* Data required" />
                            <ErrorFrameStyle CssClass="small pl-0"></ErrorFrameStyle>
                        </ValidationSettings>
                        <ReadOnlyStyle BackColor="Transparent"></ReadOnlyStyle>
                    </PropertiesTextEdit>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn FieldName="group_name" Caption="Group" Width="100" MinWidth="100" Visible="false" EditFormSettings-Visible="True" ReadOnly="true" EditFormSettings-VisibleIndex="1">
                    <PropertiesTextEdit MaxLength="20">
                        <ValidationSettings ValidationGroup="Validate" Display="Dynamic" ErrorTextPosition="Bottom" ErrorDisplayMode="Text">
                            <RequiredField IsRequired="true" ErrorText="* Data required" />
                            <ErrorFrameStyle CssClass="small pl-0"></ErrorFrameStyle>
                        </ValidationSettings>
                        <ReadOnlyStyle BackColor="Transparent"></ReadOnlyStyle>
                    </PropertiesTextEdit>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataComboBoxColumn FieldName="group_id" Caption="Group" Width="100" MinWidth="100" Visible="false">
                    <PropertiesComboBox ValueField="group_id" TextField="group_name" ValueType="System.Int32"
                        TextFormatString="{0}" DisplayFormatString="{1}">
                    </PropertiesComboBox>
                    <EditItemTemplate>
                        <dx:ASPxGridLookup ID="lookup_group" ClientInstanceName="lookup_group" runat="server"
                            KeyFieldName="group_id" TextFormatString="{1}" AutoGenerateColumns="False"
                            OnInit="lookup_group_Init" SelectionMode="Single" ValidationGroup="Validate"
                            MultiTextSeparator=";">
                            <ValidationSettings ValidationGroup="Validate" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
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
                            <ValidationSettings ValidationGroup="Validate" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
                                <ErrorFrameStyle CssClass="small p-0"></ErrorFrameStyle>
                            </ValidationSettings>
                        </dx:ASPxGridLookup>
                    </EditItemTemplate>
                </dx:GridViewDataComboBoxColumn>

                <dx:GridViewDataTextColumn FieldName="fullname" Caption="User Profile" Width="200" MinWidth="200" EditFormSettings-Visible="True" EditFormSettings-VisibleIndex="3">
                    <PropertiesTextEdit MaxLength="100">
                        <ValidationSettings ValidationGroup="Validate" Display="Dynamic" ErrorTextPosition="Bottom" ErrorDisplayMode="Text">
                            <RequiredField IsRequired="true" ErrorText="* Data required" />
                            <ErrorFrameStyle CssClass="small pl-0"></ErrorFrameStyle>
                        </ValidationSettings>
                    </PropertiesTextEdit>
                    <EditFormSettings Caption="Fullname" />
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataComboBoxColumn FieldName="gender" Caption="Gender" Visible="false" EditFormSettings-Visible="True" EditFormSettings-VisibleIndex="6">
                    <PropertiesComboBox ValueField="gender_id" TextField="gender_desc" TextFormatString="{0}" DisplayFormatString="{1}">
                        <ValidationSettings ValidationGroup="Validate" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
                            <RequiredField IsRequired="true" ErrorText="* Data required" />
                            <ErrorFrameStyle CssClass="small pl-0"></ErrorFrameStyle>
                        </ValidationSettings>
                    </PropertiesComboBox>
                </dx:GridViewDataComboBoxColumn>

                <dx:GridViewDataTextColumn FieldName="telegram_id" Caption="Telegram ID" Width="200" MinWidth="200" Visible="false" EditFormSettings-Visible="True" EditFormSettings-VisibleIndex="5">
                    <PropertiesTextEdit MaxLength="100">
                    </PropertiesTextEdit>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn FieldName="email" Caption="Email" Width="200" MinWidth="200" Visible="false" EditFormSettings-Visible="True" EditFormSettings-VisibleIndex="5">
                    <PropertiesTextEdit MaxLength="100">
                        <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
                            <RegularExpression ErrorText="* Invalid email address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                            <RequiredField ErrorText="* Data required" IsRequired="true" />
                            <ErrorFrameStyle CssClass="small pl-0"></ErrorFrameStyle>
                        </ValidationSettings>
                    </PropertiesTextEdit>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn FieldName="phone" Caption="Phone" Width="100" MinWidth="100" PropertiesTextEdit-MaskSettings-Mask="+99-9999-9999-9999" Visible="false" EditFormSettings-Visible="True" EditFormSettings-VisibleIndex="4">
                    <PropertiesTextEdit MaxLength="18">
                        <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
                            <ErrorFrameStyle CssClass="small pl-0"></ErrorFrameStyle>
                        </ValidationSettings>
                    </PropertiesTextEdit>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataComboBoxColumn FieldName="theme_id" Caption="Theme" Visible="false" EditFormSettings-Visible="True" EditFormSettings-VisibleIndex="7">
                    <PropertiesComboBox TextField="theme_name" ValueField="theme_id" ValueType="System.Int32"
                        TextFormatString="{0}" DisplayFormatString="{1}">
                        <ValidationSettings ValidationGroup="Validate" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
                            <RequiredField IsRequired="true" ErrorText="* Data required" />
                            <ErrorFrameStyle CssClass="small pl-0"></ErrorFrameStyle>
                        </ValidationSettings>
                    </PropertiesComboBox>
                </dx:GridViewDataComboBoxColumn>

                <dx:GridViewDataComboBoxColumn FieldName="company_id" Caption="Company" Width="100" MinWidth="100" Visible="false" EditFormSettings-Visible="False">
                    <PropertiesComboBox TextField="company_name" ValueField="company_id" ValueType="System.Int32"
                        TextFormatString="{0}" DisplayFormatString="{1}">
                        <ValidationSettings ValidationGroup="Validate" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
                            <RequiredField IsRequired="true" ErrorText="* Data required" />
                            <ErrorFrameStyle CssClass="small pl-0"></ErrorFrameStyle>
                        </ValidationSettings>
                    </PropertiesComboBox>
                    <EditItemTemplate>
                        <dx:ASPxGridLookup ID="lookup_company" ClientInstanceName="lookup_company" runat="server"
                            KeyFieldName="company_id" TextFormatString="{1}" AutoGenerateColumns="False"
                            OnInit="lookup_company_Init" SelectionMode="Single" ValidationGroup="Validate"
                            MultiTextSeparator=";">
                            <ValidationSettings ValidationGroup="Validate" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
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
                            <ValidationSettings ValidationGroup="Validate" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true" ErrorDisplayMode="Text">
                                <ErrorFrameStyle CssClass="small p-0"></ErrorFrameStyle>
                            </ValidationSettings>
                        </dx:ASPxGridLookup>
                    </EditItemTemplate>
                </dx:GridViewDataComboBoxColumn>

                <dx:GridViewCommandColumn Width="25" MinWidth="25" ButtonRenderMode="Button" ShowEditButton="true">
                    <HeaderStyle HorizontalAlign="Center" />
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                </dx:GridViewCommandColumn>
            </Columns>
            <Templates>
                <EditForm>
                    <div class="mdc-layout-grid">
                        <div class="mdc-layout-grid__inner">
                            <% if (!grid.IsNewRowEditing){ 
                            %>
                            <div class="mdc-layout-grid__cell mdc-layout-grid__cell--span-3 mdc-layout-grid__cell mdc-layout-grid__cell--span-12-tablet">
                                <dx:ASPxBinaryImage ID="photo" runat="server"
                                    ClientInstanceName="photo" 
                                    Width="250" 
                                    Height="250" 
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

                            <% } %>

                            <div class="mdc-layout-grid__cell mdc-layout-grid__cell--span-9 mdc-layout-grid__cell mdc-layout-grid__cell--span-12-tablet email-right-wrapper">
                                <dx:ASPxGridViewTemplateReplacement ID="Editors" ReplacementType="EditFormEditors" runat="server" />
                                <div class="d-flex justify-content-end mt-3 mb-3">
                                    <dx:ASPxButton CssClass="mr-2" ID="UpdateButton" runat="server" Text="Update" AutoPostBack="false">
                                        <ClientSideEvents Click="function(s,e) { if(ASPxClientEdit.ValidateGroup('Validate'))grid.UpdateEdit();}" />
                                    </dx:ASPxButton>
                                    <dx:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </EditForm>
                <PreviewRow>
                    <div class="mdc-layout-grid">
                        <div class="mdc-layout-grid__inner">
                            <div class="mdc-layout-grid__cell mdc-layout-grid__cell--span-4 mdc-layout-grid__cell mdc-layout-grid__cell--span-12-tablet">
                                <div class="mdc-card">
                                    <div class="border-bottom text-center pb-4">
                                        <dx:ASPxBinaryImage CssClass="img-xl img-rouded mb-3" ID="ASPxBinaryImage1" ClientInstanceName="photo" Width="250" Height="250" runat="server"
                                            ShowLoadingImage="true"
                                            EditingSettings-AllowDropOnPreview="true"
                                            LoadingImageUrl="~/Content/images/Icon/load.gif"
                                            BinaryStorageMode="Session"
                                            Value='<%# (Eval("photo") == null ? new byte[0] : CMS.Helper.Converter.GetByteArrayFromImage(Server.MapPath(Eval("photo").ToString())))%>'>
                                            <EditingSettings Enabled="true">
                                                <UploadSettings>
                                                    <UploadValidationSettings MaxFileSize="4194304"></UploadValidationSettings>
                                                </UploadSettings>
                                                <ButtonPanelSettings ButtonPosition="Center" Visibility="None" />
                                            </EditingSettings>
                                        </dx:ASPxBinaryImage>
                                    </div>

                                    <div class="py-4">
                                        <p class="d-flex justify-content-between">
                                            <span>Username</span>
                                            <span class="text-muted"><%# Eval("username") %></span>
                                        </p>
                                        <p class="d-flex justify-content-between">
                                            <span>Email</span>
                                            <span class="text-muted"><%# Eval("email") %></span>
                                        </p>
                                        <p class="d-flex justify-content-between">
                                            <span>Phone </span>
                                            <span class="text-muted"><%# Eval("phone") %></span>
                                        </p>
                                        <p class="d-flex justify-content-between">
                                            <span>Telegram ID</span>
                                            <span class="text-muted">
                                                <a href="#"><%# Eval("telegram_id") %></a>
                                            </span>
                                        </p>
                                    </div>

                                    <button type="button" runat="server" class="mdc-button mdc-button--unelevated filled-button--primary mdc-ripple-upgraded btn-block" style="--mdc-ripple-fg-size: 194px; --mdc-ripple-fg-scale: 1.73551; --mdc-ripple-fg-translate-start: 130.6px, -87.4px; --mdc-ripple-fg-translate-end: 65.35px, -79px;">
                                        <i class="fa fa-key" aria-hidden="true"></i>&nbsp;Change Password
                                            <a href="~/account/changepassword.aspx" runat="server"></a>
                                    </button>
                                </div>
                            </div>
                            <div class="mdc-layout-grid__cell mdc-layout-grid__cell--span-8 mdc-layout-grid__cell mdc-layout-grid__cell--span-12-tablet email-right-wrapper">
                                <div class="mdc-card">
                                    <div class="d-lg-flex justify-content-between">
                                        <div>
                                            <h3><%# Eval("fullname") %></h3>
                                            <div class="d-flex align-items-center">
                                                <h5 class="mb-0 mr-2 text-muted"><%# Eval("company_name") %></h5>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="mt-4 py-2 border-top border-bottom">
                                        <ul class="profile-navbar">
                                            <li>
                                                <i class="material-icons">people</i>&nbsp;<%# Eval("group_name") %>
                                            </li>
                                        </ul>
                                    </div>
                                    <p class="d-flex justify-content-between mt-2">
                                        <span>Gender</span>
                                        <span class="text-muted">
                                            <%# Eval("gender").ToString() == "0"  ? "Laki-Laki" : "Perempuan" %>
                                        </span>
                                    </p>
                                    <p class="d-flex justify-content-between">
                                        <span>Theme</span>
                                        <span class="text-muted">
                                            <%# Eval("theme_name").ToString() %>
                                        </span>
                                    </p>

                                    
                                </div>
                            </div>

                        </div>
                    </div>
                </PreviewRow>
            </Templates>
            <Settings ShowPreview="true" />
        </dx:ASPxGridView>
    </div>
</asp:Content>
