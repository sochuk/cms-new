<%@ Page Language="C#" MasterPageFile="~/CPanel.Master" AutoEventWireup="true" CodeBehind="Setting.aspx.cs" Inherits="CMS.Account.Setting" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex">
        <div class="col-auto">
            <dx:ASPxFormLayout runat="server" ID="FormLayout" ColumnCount="1" Width="100%" CssClass="w-100">
                <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="600" />
                <Styles>
                    <LayoutGroup BackColor="Transparent"></LayoutGroup>
                    <LayoutGroupBox Caption-BackColor="#e4e5e6"></LayoutGroupBox>
                </Styles>
                <Items>
                    <dx:LayoutGroup Caption="Data User Setting" GroupBoxDecoration="Box" ColCount="1">
                        <Items>
                            <dx:LayoutItem Caption="Theme">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxGridLookup ID="Theme" runat="server" TextFormatString="{1}" DisplayFormatString="{1}"
                                            KeyFieldName="Id" CssClass="" AutoPostBack="false">
                                            <GridViewProperties>
                                                <SettingsBehavior AllowHeaderFilter="true" />
                                                <SettingsPager PageSize="50"></SettingsPager>
                                            </GridViewProperties>
                                            <Columns>
                                                <dx:GridViewCommandColumn ShowSelectCheckbox="true"></dx:GridViewCommandColumn>
                                                <dx:GridViewDataColumn Caption="ID" FieldName="Id" Visible="false"></dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn Caption="Description" FieldName="Description" Width="250"></dx:GridViewDataColumn>
                                            </Columns>
                                        </dx:ASPxGridLookup>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Data Page Size">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxSpinEdit ID="PageSize" TabIndex="1" runat="server" CssClass="" Width="100" MinValue="10" MaxValue="1000">
                                            <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" ErrorDisplayMode="Text">
                                                <RequiredField IsRequired="true" ErrorText="* Required" />
                                                <ErrorFrameStyle CssClass="small"></ErrorFrameStyle>
                                            </ValidationSettings>
                                        </dx:ASPxSpinEdit>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>

                            <dx:LayoutItem Caption="Use Zebra Row">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxCheckBox runat="server" ID="Zebra" CssClass="">
                                        </dx:ASPxCheckBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>

                            <dx:LayoutItem Caption="Wrap Data Column">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxCheckBox runat="server" ID="WrapColumn" CssClass="">
                                        </dx:ASPxCheckBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>

                            <dx:LayoutItem Caption="Wrap Data Cell">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxCheckBox runat="server" ID="WrapCell" CssClass="">
                                        </dx:ASPxCheckBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>

                            <dx:LayoutItem Caption="Show Filter Bar">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxCheckBox runat="server" ID="FilterBar" CssClass="">
                                        </dx:ASPxCheckBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>

                            <dx:LayoutItem Caption="Selecting Row Click">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxCheckBox runat="server" ID="ClickSelect" CssClass="">
                                        </dx:ASPxCheckBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>

                            <dx:LayoutItem Caption="Use Focused Row">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxCheckBox runat="server" ID="Focused" CssClass="">
                                        </dx:ASPxCheckBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>

                            <dx:LayoutItem Caption="Use Ellipsis Text">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxCheckBox runat="server" ID="Ellipsis" CssClass="">
                                        </dx:ASPxCheckBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>

                            <dx:LayoutItem Caption="Show Footer">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxCheckBox runat="server" ID="Footer" CssClass="">
                                        </dx:ASPxCheckBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>

                            <dx:LayoutItem Caption="Responsive Layout">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxCheckBox runat="server" ID="Responsive" CssClass="">
                                        </dx:ASPxCheckBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>

                            <dx:LayoutItem Caption=" " HorizontalAlign="Left">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxButton ID="Save" runat="server" TabIndex="4" Text="Save Setting" CssClass="mt-2"></dx:ASPxButton>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:LayoutGroup>
                </Items>
            </dx:ASPxFormLayout>
        </div>
    </div>

    <div class="mb-5 mt-3">
        <p>Preview :</p>
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
        <dx:ASPxGridView runat="server" ID="GridPreview" ClientInstanceName="grid" Width="100%"
            KeyFieldName="No"
            OnInit="GridPreview_Init"
            OnRowInserting="GridPreview_RowInserting"
            OnRowDeleting="GridPreview_RowDeleting"
            OnRowUpdating="GridPreview_RowUpdating"
            OnCustomCallback="GridPreview_CustomCallback"
            OnCustomButtonCallback="GridPreview_CustomButtonCallback"
            OnCustomButtonInitialize="GridPreview_CustomButtonInitialize"
            OnHtmlRowPrepared="GridPreview_HtmlRowPrepared"
            ClientSideEvents-EndCallback="function(s,e){grid_EndCallback(s,e)}">
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
                        <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="30" MinWidth="30"></dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn FieldName="No" Width="30" MinWidth="30" />
                        <dx:GridViewDataTextColumn FieldName="Column 1" Settings-AllowHeaderFilter="True" Width="100" MinWidth="100" />
                        <dx:GridViewDataTextColumn FieldName="Column 2" Width="100" MinWidth="100" />
                        <dx:GridViewDataTextColumn FieldName="Column 3" Width="100" MinWidth="100" />
                        <dx:GridViewDataTextColumn FieldName="Column 4" />
                        <dx:GridViewDataTextColumn FieldName="Column 5" />
                        <dx:GridViewDataCheckColumn MinWidth="50" Width="60" FieldName="isactive" Caption="Active" />
                    </Columns>
                </dx:GridViewBandColumn>
                <dx:GridViewCommandColumn HeaderStyle-HorizontalAlign="Center"
                    AdaptivePriority="2"
                    Width="150"
                    VisibleIndex="0"
                    ButtonRenderMode="Button"
                    ShowEditButton="true"
                    ShowDeleteButton="false"
                    ShowApplyFilterButton="true"
                    ShowClearFilterButton="true"
                    Caption="Action">
                    <CustomButtons>
                        <dx:GridViewCommandColumnCustomButton Text="Delete" ID="ButtonDelete"></dx:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dx:GridViewCommandColumn>
            </Columns>
        </dx:ASPxGridView>
    </div>
</asp:Content>
