<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CPanel.Master" CodeBehind="Card.aspx.cs" Inherits="CMS.Master.Card" %>

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
            if (e.item.name === 'CSVEXPORT') {
                e.processOnServer = false;
                CSVEXPORT.DoClick();
            }

            if (e.item.name === 'XLSXEXPORT') {
                e.processOnServer = false;
                XLSXEXPORT.DoClick();
            }

            if (e.item.name === 'XLSXIMPORT') {
                e.processOnServer = false;
                $("input#ExcelUpload").click();
            }

            if (e.item.name === 'CSVIMPORT') {
                e.processOnServer = false;
                $("input#CSVUpload").click();
            }

            $("input#CSVUpload").on("change", function () {
                e.processOnServer = false;
                UploadButton.DoClick()
            })
        }
    </script>

    <dx:EntityServerModeDataSource runat="server" ID="CardData" ContextTypeName="CMS.Context.CMSContext" 
    TableName="M_CARD" OnSelecting="CardData_Selecting" DefaultSorting="CREATEDATE DESC" />
    <dx:ASPxGridViewExporter ID="gridExporter" GridViewID="grid" runat="server"></dx:ASPxGridViewExporter>

    <div id="sectionInfo">
        <h5 id="text-progress" style="display: none" class="my-2 font-weight-bold"></h5>
        <div role="progressbar" id="progress" class="mdc-linear-progress mdc-linear-progress--indeterminate mt-2" style="display: none">
            <div class="mdc-linear-progress__buffering-dots"></div>
            <div class="mdc-linear-progress__buffer"></div>
            <div class="mdc-linear-progress__bar mdc-linear-progress__primary-bar">
                <span class="mdc-linear-progress__bar-inner"></span>
            </div>
            <div class="mdc-linear-progress__bar mdc-linear-progress__secondary-bar">
                <span class="mdc-linear-progress__bar-inner"></span>
            </div>
        </div>
    </div>

    <asp:FileUpload ID="CSVUpload" runat="server" accept=".csv" style="display: none;" ClientIDMode="Static" />
    <asp:FileUpload ID="ExcelUpload" runat="server" accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" style="display: none;" ClientIDMode="Static" />
    <dx:ASPxButton ID="UploadButton" ClientInstanceName="UploadButton" ClientVisible="false" OnClick="UploadButton_Click" runat="server" />

    <dx:ASPxButton ID="XLSXEXPORT" ClientInstanceName="XLSXEXPORT" Visible="true" ClientVisible="false" Image-IconID="mail_sendxls_16x16office2013"
        OnClick="XLSXEXPORT_Click" runat="server" Text="Download to EXCEL File" />

    <dx:ASPxButton ID="CSVEXPORT" ClientInstanceName="CSVEXPORT" Visible="true" ClientVisible="false" Image-IconID="mail_sendcsv_16x16office2013"
        OnClick="CSVEXPORT_Click" runat="server" Text="Download to CSV File" />

    <dx:ASPxGridView runat="server" ID="grid" ClientInstanceName="grid" 
        Width="100%" CssClass="w-100"
        DataSourceID="CardData"
        KeyFieldName="CARD_ID"
        EnableCallBacks="true"        
        OnInit="grid_Init"
        OnRowInserting="grid_RowInserting"
        OnRowUpdating="grid_RowUpdating"
        OnHtmlRowPrepared="grid_HtmlRowPrepared"
        OnToolbarItemClick="grid_ToolbarItemClick"
        ClientSideEvents-EndCallback="grid_EndCallback"
        ClientSideEvents-ToolbarItemClick="onToolbarItemClick">

        <SettingsDataSecurity AllowDelete="false" AllowEdit="true" AllowInsert="true" />
        <SettingsSearchPanel CustomEditorID="tbSearch" />
        <SettingsContextMenu Enabled="true">
            <ColumnMenuItemVisibility ClearFilter="true" />
        </SettingsContextMenu>
        <SettingsPager PageSize="10">
            <PageSizeItemSettings Visible="true" Items="5, 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000" />
        </SettingsPager>
        <SettingsExport ExcelExportMode="WYSIWYG" PaperKind="A2" EnableClientSideExportAPI="true"></SettingsExport>
        <Toolbars>
            <dx:GridViewToolbar Name="Toolbar">
                <Items>
                    <%-- Image-IconID Refer to https://demos.devexpress.com/ASPxMultiUseControlsDemos/Features/IconLibraryExplorer.aspx --%>
                    <dx:GridViewToolbarItem Command="New" Name="New" Image-IconID="actions_new_16x16office2013" Text="New" />
                     <dx:GridViewToolbarItem AdaptivePriority="1" Name="Import" Text="Upload from" Image-IconID="data_addnewdatasource_16x16office2013" BeginGroup="true">
                        <Items>
                            <dx:GridViewToolbarItem Name="CSVIMPORT" Text="CSV File" Image-IconID="mail_sendcsv_16x16office2013" />
                            <dx:GridViewToolbarItem ClientVisible="false" Name="XLSXIMPORT" Text="Excel File" Image-IconID="mail_sendxls_16x16office2013" />
                        </Items>
                    </dx:GridViewToolbarItem>

                    <dx:GridViewToolbarItem Command="Refresh" Name="Refresh" BeginGroup="true" Image-IconID="actions_refresh_16x16office2013" />

                    <dx:GridViewToolbarItem AdaptivePriority="1" Name="Export" Text="Download to" Image-IconID="export_export_16x16office2013" BeginGroup="true">
                        <Items>
                            <dx:GridViewToolbarItem Name="XLSXEXPORT" Text="Export to Excel File" Image-IconID="export_exporttoxlsx_16x16office2013" />
                            <dx:GridViewToolbarItem Name="CSVEXPORT" Text="Export to CSV File" Image-IconID="mail_sendcsv_16x16office2013" />
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
                    <dx:GridViewDataTextColumn FieldName="NIK" Caption="N.I.K" Settings-AllowHeaderFilter="True" />
                    <dx:GridViewDataTextColumn FieldName="CARDUID" Caption="UID" />
                    <dx:GridViewDataTextColumn FieldName="COMMIT_DATE" Caption="COMMIT DATE" />
                    <dx:GridViewDataTextColumn FieldName="PROVINSI_CODE" Caption="PROVINSI CODE" />
                    <dx:GridViewDataTextColumn FieldName="KABKOTA_CODE" Caption="KABUPATEN / KOTA CODE" />
                    <dx:GridViewDataTextColumn FieldName="INNER" Caption="INNER" />
                    <dx:GridViewDataTextColumn FieldName="OUTER" Caption="OUTER" />

                    <dx:GridViewDataComboBoxColumn FieldName="CREATEBY" Caption="Create by" EditFormSettings-Visible="False" Settings-AllowHeaderFilter="True" MaxWidth="200">
                        <PropertiesComboBox DisplayFormatString="{1}" ValueField="user_id" TextField="fullname" />
                    </dx:GridViewDataComboBoxColumn> 
                    <dx:GridViewDataDateColumn FieldName="CREATEDATE" Caption="Create Date" EditFormSettings-Visible="False" Settings-AllowHeaderFilter="True">
                        <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd HH:mm:ss" />
                    </dx:GridViewDataDateColumn>
                    
                    <dx:GridViewDataComboBoxColumn FieldName="UPDATEBY" Caption="Update by" EditFormSettings-Visible="False" Settings-AllowHeaderFilter="True" MaxWidth="200">
                        <PropertiesComboBox DisplayFormatString="{1}" ValueField="user_id" TextField="fullname" />
                    </dx:GridViewDataComboBoxColumn> 
                    <dx:GridViewDataTextColumn FieldName="UPDATEDATE" Caption="Update Date" EditFormSettings-Visible="False" Settings-AllowHeaderFilter="True" />
                </Columns>

            </dx:GridViewBandColumn>
        </Columns>

    </dx:ASPxGridView>
</asp:Content>


<asp:Content ID="Hub" ContentPlaceHolderID="Script" runat="server">
    <script>
        $(function () {
            let progress = $.connection.userHub;
            var tryingToReconnect = false;
            var containerInfo = $("div#sectionInfo");
            var textstatus = $("h5#text-progress");
            var progressBar = $("div#progress")

            progress.client.getProgress = function (message) {
                containerInfo.show();
                textstatus.show();
                progressBar.show();
                textstatus.text(message);
            };

            progress.client.finished = function () {
                containerInfo.hide();
                textstatus.hide();
                grid.Refresh();
            };

            progress.client.started = function () {
                containerInfo.show();
                textstatus.show();
                progressBar.show();
            };

            progress.client.refresh = function () {
                grid.Refresh();
            };

            progress.client.success = function (data) {
                if ($('div.alert.notify-message.alert-success.border-danger.shadow-lg').length == 0) {
                    $('body').append(data);
                }
            };

            $.connection.hub.start(function () {
                $.get("https://www.cloudflare.com/cdn-cgi/trace", function (data) {
                    var d = data.split("\n");
                    var o = new Object();
                    d.forEach(function (itm, idx) {
                        var item = itm.split("=");
                        o[item[0]] = item[1];
                    });

                    progress.server.join(o.ip, o.uag)
                        .done(function (data) {
                            textstatus.text(data.message);
                        });
                })

            });

            $.connection.hub.disconnected(function () {
                setTimeout(function () {
                    $.connection.hub.start();
                    Pace.restart();
                }, 5000); // Re-start connection after 5 seconds
            });

            $.connection.hub.reconnecting(function () {
                tryingToReconnect = true;
            });

            $.connection.hub.reconnected(function () {
                tryingToReconnect = false;
                Pace.restart();
            });

            $.connection.hub.disconnected(function () {
                if (tryingToReconnect) {
                    console.log("Trying reconnecting...")
                }
            });

        })
    </script>
</asp:Content>