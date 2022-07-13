<%@ Page Language="C#" MasterPageFile="~/Themes/Material/Login.Master" AutoEventWireup="true" CodeBehind="Lock.aspx.cs" Inherits="CMS.Account.Lock" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="mdc-layout-grid text-center">
        <img src="<%: M_User.getPhoto() %>" class="lock-profile-img img-lg" data-initial="<%: M_User.getInitial() %>" />
        <div class="font-weight-bold text-primary h4 mb-4 mt-3 username"><%: User.Identity.Get_Fullname() %></div>
        <div class="mdc-layout-grid__cell stretch-card mdc-layout-grid__cell--span-8 mt-3 px-5">
            <div class="mdc-text-field mdc-text-field--outlined mdc-text-field--with-trailing-icon">
                <i class="material-icons mdc-text-field__icon text-black switch-password">lock</i>
                <asp:TextBox ID="txtPassword" data-id="password" TextMode="Password" CssClass="mdc-text-field__input" required="required" runat="server" autofocus="autofocus" />
                <div class="mdc-notched-outline mdc-notched-outline--upgraded">
                    <div class="mdc-notched-outline__leading"></div>
                    <div class="mdc-notched-outline__notch" style="">
                        <label for="text-password" class="mdc-floating-label text-black" style="">Password unlock</label>
                    </div>
                    <div class="mdc-notched-outline__trailing"></div>
                </div>
            </div>
        </div>
        <div id="progressbar" class="mdc-layout-grid__cell stretch-card mdc-layout-grid__cell--span-12 mt-1 px-5 d-none">
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
        <div class="mdc-layout-grid__cell stretch-card mdc-layout-grid__cell--span-12 mt-2 px-5">
            <% if (Session["error"] != null) { %>
            <div class="alert alert-rose w-100 m-0 p-2 text-center mb-3 bg-light text-danger" role="alert">
                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <i class="material-icons text-danger">close</i>
                </button>
                <%= Session["Error"] %>
            </div>
            <% } %>
        </div>
        <div class="mt-5">
            <a runat="server" href="~/account/logout" class="auth-link text-primary text-uppercase">Signout current account</a>
        </div>
    </div>    
</asp:Content>