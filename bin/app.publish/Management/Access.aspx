<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CPanel.Master" CodeBehind="Access.aspx.cs" Inherits="CMS.Management.Access" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var user_id = [];
        var access_id= [];
        function onToolbarItemClick(s, e) {
            if (e.item.name === 'Group' || e.item.name === 'Role' || e.item.name === 'None') {
                e.processOnServer = true;
            }
        }

        function showPopup(s, e) {
            user_id = s.cpGroup_Id;
            Grid_Access.PerformCallback(s.cpGroup_Id);
            Access_Control.Show();
        }

        function saveAccess(s, e) {
            Grid_Access.GetSelectedFieldValues("module_id", AccessOnGetRowValues);
        }

        function AccessOnGetRowValues(values) {
            var result = [[user_id], values]
            cpCallback.PerformCallback(result);
        }

        function grid_EndCallback(s, e) {
            if (s.cpShowPopup) {
                s.cpShowPopup = false;
                showPopup(s, e);
            }
            EndCallback(s, e);
        }

    </script>

    <dx:ASPxGridView runat="server" ID="grid" ClientInstanceName="grid_group" 
        Width="100%" CssClass="w-100"
        KeyFieldName="group_id"
        EnableCallBacks="true"
        OnInit="grid_Init"
        OnCustomButtonInitialize="grid_CustomButtonInitialize"
        OnCustomButtonCallback="grid_CustomButtonCallback"
        OnToolbarItemClick="grid_ToolbarItemClick">
        <ClientSideEvents ToolbarItemClick="function(s,e){ onToolbarItemClick(s,e) }" />
        <SettingsSearchPanel CustomEditorID="tbSearch" />
        <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
        <Toolbars>
            <dx:GridViewToolbar Name="Toolbar">
                <Items>
                    <%-- Image-IconID Refer to https://demos.devexpress.com/ASPxMultiUseControlsDemos/Features/IconLibraryExplorer.aspx --%>
                    <dx:GridViewToolbarItem Command="Refresh" Name="Refresh" BeginGroup="true" Image-IconID="actions_refresh_16x16office2013" />
                    <dx:GridViewToolbarItem Text="Group by" Image-IconID="snap_groupby_16x16" BeginGroup="true">
                        <Items>
                            <dx:GridViewToolbarItem Command="Custom" Image-IconID="reports_none_16x16office2013" Name="None" Text="None" />
                            <dx:GridViewToolbarItem Command="Custom" Image-IconID="people_usergroup_16x16office2013" Name="Group" Text="User Group" />
                            <dx:GridViewToolbarItem Command="Custom" Image-IconID="people_role_16x16office2013" Name="Role" Text="Role" />
                        </Items>
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
                    <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="30" />
                    <dx:GridViewDataTextColumn FieldName="group_id" Caption="Group Id" Visible="false" />
                    <dx:GridViewDataTextColumn FieldName="group_name" Caption="Group" Settings-AllowHeaderFilter="True" />
                    <dx:GridViewDataTextColumn FieldName="group_desc" Caption="Group Description" />
                    <dx:GridViewDataTextColumn FieldName="role_name" Caption="Role" Settings-AllowHeaderFilter="True" />
                    <dx:GridViewDataTextColumn FieldName="role_desc" Caption="Role Description" />
                </Columns>

            </dx:GridViewBandColumn>
            <dx:GridViewCommandColumn HeaderStyle-HorizontalAlign="Center"
                AdaptivePriority="2"
                VisibleIndex="0"
                ButtonRenderMode="Button"
                ShowEditButton="false"
                ShowDeleteButton="false"
                ShowApplyFilterButton="false"
                ShowClearFilterButton="false"
                Width="100"
                Caption="Action">
                <CustomButtons>                    
                    <dx:GridViewCommandColumnCustomButton ID="Access_Right_Button" Text="Enroll" />
                </CustomButtons>
            </dx:GridViewCommandColumn>
        </Columns>
        <SettingsPager EnableAdaptivity="true" />        
        <SettingsBehavior FilterRowMode="OnClick" AllowEllipsisInText="true" AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
        <Settings ShowPreview="True" ShowFooter="true" ShowFilterRow="true" />
        <Styles>
            <AlternatingRow Enabled="true" />
        </Styles>
        <ClientSideEvents EndCallback="function(s,e){ grid_EndCallback(s,e) }" />
    </dx:ASPxGridView>

    <dx:ASPxPopupControl ID="Access_Control" runat="server" ClientInstanceName="Access_Control"
        Width="700" Height="200" ClientSideEvents-PopUp="function(s,e){s.ShowAtPos(s.Left, 20)}"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="BottomSides"
        CloseAction="CloseButton" CloseOnEscape="true" AllowDragging="true" PopupAnimationType="None"
        EnableViewState="false" AutoUpdatePosition="true" ShowFooter="false"         
        HeaderText="Access Right Control" >
        <ContentCollection>
            <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
                <dx:ASPxGridView runat="server" ID="Grid_Access" ClientInstanceName="Grid_Access" 
                    KeyFieldName="module_id"
                    EnableCallBacks="true"
                    OnDataBinding="Grid_Access_DataBinding"
                    OnCustomCallback="Grid_Access_CustomCallback">
                    <Columns>
                        <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="30" ShowClearFilterButton="true" SelectAllCheckboxMode="AllPages" />
                        <dx:GridViewDataTextColumn FieldName="module_id" Caption="Module Id" Visible="false"/>
                        <dx:GridViewDataTextColumn FieldName="module_name" Caption="Module Name" Settings-AllowHeaderFilter="True" />
                        <dx:GridViewDataTextColumn FieldName="module_desc" Caption="Module Description" />
                        <dx:GridViewDataTextColumn FieldName="module_name_group" Caption="Module Root" />
                    </Columns>
                    <SettingsPager EnableAdaptivity="true" PageSize="25" />
                    <SettingsBehavior FilterRowMode="OnClick" AllowEllipsisInText="true" />
                    <SettingsEditing Mode="EditForm" />
                    <Settings ShowPreview="True" ShowFooter="true" ShowFilterRow="true" />
                    <Styles>
                        <AlternatingRow Enabled="true" />
                    </Styles>
                    <Settings VerticalScrollBarMode="Visible" VerticalScrollBarStyle="VirtualSmooth" />
                    <SettingsPager>
                        <PageSizeItemSettings Visible="true" />
                    </SettingsPager>
                    <ClientSideEvents CustomButtonClick="function(s,e){showPopup(s,e)}" />
                </dx:ASPxGridView>

                <dx:ASPxCallbackPanel ID="cpCallback" runat="server" Width="200px" 
                    ClientInstanceName="cpCallback"
                    CssClass="text-center w-100"
                    OnCallback="cpCallback_Callback"
                    ClientSideEvents-EndCallback="function(s,e){Access_Control.Hide(); EndCallback(s, e);}" />

                <table class="w-100 mt-2">
                    <tr>
                        <td class="d-flex justify-content-end p-2">                            
                            <dx:ASPxButton CssClass="mr-2" ID="YesButton" runat="server" Text="Save" AutoPostBack="False" 
                                ClientSideEvents-Click="function(s, e) { saveAccess(s,e) }" />
                            <dx:ASPxButton ID="CancelButton" runat="server" Text="Close" AutoPostBack="False" 
                                ClientSideEvents-Click="function(s, e) { Access_Control.Hide(); }" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>    
</asp:Content>
