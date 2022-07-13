<%@ Page Language="C#" MasterPageFile="~/Themes/Material/Login2.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CMS.Account.Login" Async="true"%>

<%@ Register Assembly="BotDetect" Namespace="BotDetect.Web.UI" TagPrefix="BotDetect" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="mdc-layout-grid">
        <input type="hidden" name="date" />
        <input type="hidden" name="ip" />
        <input type="hidden" name="ua" />
        <input type="hidden" name="loc" />
        <% if (!blocked) { %>
        <div class="text-center mb-4">
            <img src="../Content/images/kemendagri.png" runat="server" width="100" class="img-fluid" />
            <h3 class="font-weight-bold">KEMENTERIAN DALAM NEGERI REPUBLIK INDONESIA</h3>
        </div>
        <div class="mdc-layout-grid__cell stretch-card mdc-layout-grid__cell--span-12 px-2">
            <div class="mdc-text-field mdc-text-field--outlined mdc-text-field--with-trailing-icon">
                <i class="material-icons mdc-text-field__icon text-black">account_circle</i>
                <asp:TextBox ID="txtUsername" data-id="username" CssClass="mdc-text-field__input" required="required" runat="server" autofocus="autofocus" />
                <div class="mdc-notched-outline mdc-notched-outline--upgraded">
                    <div class="mdc-notched-outline__leading"></div>
                    <div class="mdc-notched-outline__notch" style="">
                        <label for="text-username" class="mdc-floating-label text-black" style="">Username</label>
                    </div>
                    <div class="mdc-notched-outline__trailing"></div>
                </div>
            </div>
        </div>
        <div class="mdc-layout-grid__cell stretch-card mdc-layout-grid__cell--span-12 mt-3 px-2">
            <div class="mdc-text-field mdc-text-field--outlined mdc-text-field--with-trailing-icon">
                <i class="material-icons mdc-text-field__icon text-black switch-password">lock</i>
                <asp:TextBox ID="txtPassword" data-id="password" TextMode="Password" CssClass="mdc-text-field__input" required="required" runat="server" />
                <div class="mdc-notched-outline mdc-notched-outline--upgraded">
                    <div class="mdc-notched-outline__leading"></div>
                    <div class="mdc-notched-outline__notch" style="">
                        <label for="text-password" class="mdc-floating-label text-black" style="">Password</label>
                    </div>
                    <div class="mdc-notched-outline__trailing"></div>
                </div>
            </div>
        </div>


        <% if (CMS.CPanel.UseCaptcha) { %>
        <div class="mdc-layout-grid__cell stretch-card mdc-layout-grid__cell--span-12 mt-3 px-2">
            <div class="row">
                <div class="col d-none" id="captcha">
                    <BotDetect:WebFormsCaptcha CodeLength="6" ImageFormat="Png" ImageSize="160,40" CodeStyle="Alpha" ID="Captcha" ImageStyle="Chipped" SoundEnabled="false" UserInputID="txtCaptcha" runat="server" />
                </div>
                <div class="col">
                    <asp:TextBox CssClass="form-control form-control-sm text-primary" runat="server" ID="txtCaptcha" required="required" placeholder="Captcha" />
                </div>
            </div>
        </div>
        <% } %>

        <div class="mdc-layout-grid__cell stretch-card mdc-layout-grid__cell--span-12">
            <div class="mdc-form-field">
                <div class="mdc-checkbox mdc-checkbox--primary mdc-checkbox--upgraded mdc-ripple-upgraded mdc-ripple-upgraded--unbounded mdc-checkbox--selected">
                    <asp:CheckBox runat="server" ID="chkRemember" data-id="remember" CssClass="mdc-checkbox__native-control" />
                    <div class="mdc-checkbox__background">
                        <svg class="mdc-checkbox__checkmark" viewBox="0 0 24 24">
                            <path class="mdc-checkbox__checkmark-path" fill="none" d="M1.73,12.91 8.1,19.28 22.79,4.59"></path>
                        </svg>
                        <div class="mdc-checkbox__mixedmark"></div>
                    </div>
                    <div class="mdc-checkbox__ripple"></div>
                </div>
                <label for="chkRemember" class="text-normal m-0">Remember Me</label>
            </div>
        </div>

        <div class="mdc-layout-grid__cell stretch-card mdc-layout-grid__cell--span-12 mt-1 px-2">
            <% if (Session["error"] != null) { %>
            <div class="alert alert-rose w-100 m-0 p-2 text-center mb-3 bg-light text-danger" role="alert">
                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <i class="material-icons text-danger">close</i>
                </button>
                <%= Session["Error"] %>
            </div>
            <% } %>
        </div>

        <div id="progressbar" class="mdc-layout-grid__cell stretch-card mdc-layout-grid__cell--span-12 mt-1 px-2 d-none">
            <div role="progressbar" class="mdc-linear-progress mdc-linear-progress--indeterminate mt-1">
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
        
        <div class="mdc-layout-grid__cell stretch-card mdc-layout-grid__cell--span-12 px-2">
            <button id="login" class="btn btn-lg btn-block btn-success p-3">
                <i class="material-icons mdc-button__icon">exit_to_app</i>
                Login
            </button>
        </div>
        <% } %>

        <% if (Session["error"] != null && blocked) { %>
        <div class="mdc-layout-grid__cell stretch-card mdc-layout-grid__cell--span-12 mt-1 px-2">            
            <div class="alert alert-rose w-100 m-0 p-2 text-center mb-3 bg-light text-danger p-5" role="alert">
                <%= Session["Error"] %>
            </div>            
        </div>
        <% } %>

    </div>
</asp:Content>
