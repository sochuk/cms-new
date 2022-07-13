<%@ Page Language="C#" MasterPageFile="~/CPanel.Master" AutoEventWireup="true" CodeBehind="Configuration.aspx.cs" Inherits="CMS.Management.Configuration" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        function Save() {
            if ($('form').valid()) {                
                $('[data-id=btnSave]').prop("disabled", true).val("Saving..");
                cPanel.PerformCallback('save');
            }            
        }
        function CpEndCallback(s, e) {
            $('[data-id=btnSave]').prop("disabled", false).val("Save");
            EndCallback(s, e);
        };
    </script>
    <div class="mdc-layout-grid">
        <div class="mdc-layout-grid__inner">
            <div class="mdc-layout-grid__cell mdc-layout-grid__cell--span-12-desktop mdc-layout-grid__cell mdc-layout-grid__cell--span-12-tablet d-none">
                <div class="d-flex align-items-stretch justify-content-between">
                <dx:ASPxCallbackPanel runat="server" ID="cPanel" ClientInstanceName="cPanel" OnCallback="cPanel_Callback" ClientSideEvents-EndCallback="CpEndCallback">
                    <PanelCollection>
                        <dx:PanelContent>
                            <dx:ASPxHiddenField ID="content_id" ClientInstanceName="content_id" runat="server"></dx:ASPxHiddenField>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxCallbackPanel>
                </div>
            </div>
            
            <div class="mdc-layout-grid__cell stretch-card mdc-layout-grid__cell--span-12">
                <div class="mdc-card">
                    <div class="d-flex align-items-stretch justify-content-between mb-5">
                        <asp:LinkButton ID="btnBack" OnClick="btnBack_Click" runat="server" class="btn btn-danger btn-sm mdc-ripple-upgraded mr-2">
                        <i class="material-icons mdc-button__icon">keyboard_backspace</i>&nbsp;Back
                        </asp:LinkButton>
                    </div>

                    <div class="form-group bmd-form-group bmd-form-group-sm">
                        <asp:Label AssociatedControlID="txtTelegram" CssClass="bmd-label-floating" runat="server">Telegram API</asp:Label>
                        <asp:TextBox ID="txtTelegram" data-id="txtTelegram" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" onfocus="maxlength(this, 500)" />
                        <span class="bmd-help text-info">Telegram API of your application</span>
                    </div>

                    <div class="row">
                        <div class="form-group bmd-form-group bmd-form-group-sm col-xl-3">
                            <asp:Label AssociatedControlID="txtSMTPMail" CssClass="bmd-label-floating" runat="server">SMTP Email</asp:Label>
                            <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtSMTPMail" MaxLength="100" />
                            <span class="bmd-help text-info">SMTP Email. Example automation@nai.co.id</span>
                        </div>

                        <div class="form-group bmd-form-group bmd-form-group-sm col-xl-3">
                            <asp:Label AssociatedControlID="txtSMTPPassword" CssClass="bmd-label-floating" runat="server">SMTP Password</asp:Label>
                            <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtSMTPPassword" type="password" />
                            <span class="bmd-help text-info">Your email SMTP password </span>
                        </div>

                        <div class="form-group bmd-form-group bmd-form-group-sm col-xl-3">
                            <asp:Label AssociatedControlID="txtSMTPServer" CssClass="bmd-label-floating" runat="server">SMTP Server</asp:Label>
                            <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtSMTPServer" MaxLength="100" />
                            <span class="bmd-help text-info">SMTP server</span>
                        </div>

                        <div class="form-group bmd-form-group bmd-form-group-sm col-xl-1">
                            <asp:Label AssociatedControlID="txtSMTPPort" CssClass="bmd-label-floating" runat="server">SMTP Port</asp:Label>
                            <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtSMTPPort" type="number" />
                            <span class="bmd-help text-info">Port number</span>
                        </div>
                        
                    </div>
                    

                    <div class="form-group bmd-form-group bmd-form-group-sm">
                        <button type="button" class="btn btn-sm btn-success" id="btnSave" data-id="btnSave" onclick="Save(); return false;">SAVE</button>
                    </div>
                    
                </div>
            </div>
            
        </div>
    </div>
</asp:Content>

