<%@ Page Language="C#" MasterPageFile="~/CPanel.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="CMS.Account.ChangePassword" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var passwordMinLength = 6;
        function GetPasswordStrength(password) {
            var result = 0;
            if (password) {
                result++;
                if (password.length >= passwordMinLength) {
                    if (/[a-z]/.test(password))
                        result++;
                    if (/[A-Z]/.test(password))
                        result++;
                    if (/\d/.test(password))
                        result++;
                    if (!(/^[a-z0-9]+$/i.test(password)))
                        result++;
                }
            }
            $('#password_strength').barrating('set', result);
            ApplyPasswordStrength(result)
            return result;
        }

        function ApplyPasswordStrength(value) {
            var label = $('label#label_password_strength');
            switch (value) {
                case 0:
                    label.html("")
                    break;
                case 1:
                    label.html("Password too simple")
                    break;
                case 2:
                    label.html("Password not safe")
                    break;
                case 3:
                    label.html("Password normal")
                    break;
                case 4:
                    label.html("Password safe")
                    break;
                case 5:
                    label.html("Password very safe")
                    break;
                default:
                    label.html("Password not safe")
            }
        }

        function CpEndCallback(s, e) {
            $('[data-id=save]').prop("disabled", false).val("Change Password");
            EndCallback(s, e);
        }

        function ChangePassword() {
            if ($('input[data-id]').valid()) {                
                cPanel.PerformCallback('save');
                $('[data-id=save]').prop("disabled", true).val("Please wait..");
            }            
        }

    </script>

    <div class="mdc-layout-grid">
        <div class="mdc-layout-grid__inner">
            <div class="mdc-layout-grid__cell stretch-card mdc-layout-grid__cell--span-6">
                <div class="mdc-card">

                    <dx:ASPxCallbackPanel runat="server" ID="cPanel" ClientInstanceName="cPanel" OnCallback="cPanel_Callback" ClientSideEvents-EndCallback="CpEndCallback">
                    </dx:ASPxCallbackPanel>

                    <div class="form-group">
                        <label class="bmd-label-floating">Old Password</label>
                        <asp:TextBox TextMode="Password" ID="old_passwords" data-id="old_passwords" required="required" CssClass="form-control form-control-sm" runat="server" min="6" />
                    </div>

                    <div class="form-group">
                        <label class="bmd-label-floating">New Password</label>
                        <asp:TextBox TextMode="Password" ID="new_passwords" data-id="new_passwords" data-rule-minlength="6" required="required" CssClass="form-control form-control-sm" runat="server"
                            onkeyup="GetPasswordStrength(this.value)" />
                    </div>                                   

                    <div class="form-group">
                        <label class="bmd-label-floating">Retype New Password</label>
                        <asp:TextBox TextMode="Password" ID="renew_passwords" data-id="renew_passwords" data-rule-minlength="6" data-msg-equalTo="Password didn't match with new password" data-rule-equalTo="[data-id=new_passwords]" required="required" CssClass="form-control form-control-sm" runat="server" />
                    </div>

                     <div class="form-group text-center">
                        <label id="label_password_strength" class="bmd-label-floating">Password strength: </label>
                        <select id="password_strength" name="password_strength">
                            <option value="1">1</option>
                            <option value="2">2</option>
                            <option value="3">3</option>
                            <option value="4">4</option>
                            <option value="5">5</option>
                        </select>
                    </div>    

                    <button type="button" data-id="save" onclick="ChangePassword(); return false;" class="btn btn-block btn-success btn-lg">Change Password</button>
                </div>
            </div>
        </div>
    </div>

    
</asp:Content>

<asp:Content ContentPlaceHolderID="Script" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Themes/Material/js/jquery.bar.rating.min.js") %>"></script>
    <script type="text/javascript">
        $(function() {
          $('#password_strength').barrating({
              theme: 'fontawesome-stars',
              showValues: false,
              readonly:true
          });
        });       
        
    </script>
</asp:Content>