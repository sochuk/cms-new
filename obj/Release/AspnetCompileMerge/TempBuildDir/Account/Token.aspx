<%@ Page MasterPageFile="~/Themes/Material/Login.Master" Language="C#" Async="true"  AutoEventWireup="true" CodeBehind="Token.aspx.cs" Inherits="CMS.Account.Token" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var viewsCount = 2;
        function ShowNextPage() {
            $("button#next").attr("disabled", true).text("Please wait..");
            ChangeActiveViewIndex(1);        
        }
        function ChangeActiveViewIndex(changeIndex) {
            hfView.Set("pageIndex", GetPageIndex() + changeIndex);
            cpToken.PerformCallback();
        }
        function GetPageIndex() {
            return hfView.Get("pageIndex");
        }

        function Submit() {
            $('[data-id=btnSubmit]').attr("disabled", true).text("Signing..");
        }
    </script>

    <dx:ASPxCallbackPanel runat="server" ID="cpToken" ClientInstanceName="cpToken" OnCallback="cpToken_Callback">
        <ClientSideEvents EndCallback="EndCallback" />
        <PanelCollection>
            <dx:PanelContent runat="server">
                <asp:MultiView runat="server" ActiveViewIndex="0" ID="mvToken">
                    <asp:View runat="server">
                        <div class="mdc-layout-grid mb-5">
                            <div class="text-center text-primary mdc-typography--overline mb-3">TOKEN OTP REQUIRED TO CONTINUE LOGIN</div>
                            <div class="mdc-layout-grid__cell stretch-card mdc-layout-grid__cell--span-12 px-2 mb-3">
                                <div id="method" class="w-100 mdc-select mdc-select--outlined mdc-select--required" data-mdc-auto-init="MDCSelect" aria-required="true">
                                    <input type="hidden" name="method" aria-required="true" />
                                    <input type="hidden" name="ip" />
                                    <input type="hidden" name="ua" />
                                    <span class="mdc-select__ripple"></span>
                                    <i class="mdc-select__dropdown-icon text-primary"></i>
                                    <div class="mdc-select__selected-text text-primary"></div>
                                    <div class="mdc-select__menu mdc-menu-surface" style="width: 300px" aria-required="true">
                                        <ul class="mdc-list" role="menu" aria-hidden="true" aria-orientation="vertical">
                                            <% int x = 0;
                                                foreach (DataRow row in CMS.Account.Token.Method.Rows)
                                                {
                                                    if (x == 0)
                                                    {
                                                        Response.Write("<li class=\"mdc-list-item mdc-list-item--selected\" aria-selected=\"true\" role=\"menuitem\" data-value=\"" + row["method"] + "\">");
                                                        switch (row["method"].ToString().ToUpper())
                                                        {
                                                            case "TELEGRAM":
                                                                Response.Write("<i class=\"fab fa-2x fa-telegram-plane text-primary\"></i>");
                                                                break;
                                                            case "WHATSAPP":
                                                                Response.Write("<i class=\"fab fa-2x fa-whatsapp text-primary\"></i>");
                                                                break;
                                                            case "SMS":
                                                                Response.Write("<i class=\"fas fa-2x fa-sms text-primary\"></i>");
                                                                break;
                                                            case "EMAIL":
                                                                Response.Write("<i class=\"fa fa-2x fa-envelope text-primary\"></i>");
                                                                break;
                                                            default:
                                                                break;
                                                        }
                                                        Response.Write("<div class=\"item-content d-flex align-items-start flex-column justify-content-center ml-3\">");
                                                        Response.Write("<a href=\"#\" class=\"mdc-typography mdc-theme--dark\" tabindex=\"-1\">");
                                                        Response.Write(row["method"]);
                                                        Response.Write("</a>");
                                                        Response.Write("</div>");
                                                        Response.Write("</li>");
                                                    }
                                                    else
                                                    {
                                                        Response.Write("<li class=\"mdc-list-item\" data-value=\"" + row["method"] + "\" role=\"menuitem\">");
                                                        switch (row["method"].ToString().ToUpper())
                                                        {
                                                            case "TELEGRAM":
                                                                Response.Write("<i class=\"fab fa-2x fa-telegram-plane text-primary\"></i>");
                                                                break;
                                                            case "WHATSAPP":
                                                                Response.Write("<i class=\"fab fa-2x fa-whatsapp text-primary\"></i>");
                                                                break;
                                                            case "SMS":
                                                                Response.Write("<i class=\"fas fa-2x fa-sms text-primary\"></i>");
                                                                break;
                                                            case "EMAIL":
                                                                Response.Write("<i class=\"fa fa-2x fa-envelope text-primary\"></i>");
                                                                break;
                                                            default:
                                                                break;
                                                        }
                                                        Response.Write("<div class=\"item-content d-flex align-items-start flex-column justify-content-center ml-3\">");
                                                        Response.Write("<a href=\"#\" class=\"mdc-typography mdc-theme--dark\" tabindex=\"-1\">");
                                                        Response.Write(row["method"]);
                                                        Response.Write("</a>");
                                                        Response.Write("</div>");
                                                        Response.Write("</li>");
                                                    }
                                                    x++;
                                                }
                                            %>
                                        </ul>
                                    </div>
                                    <span class="mdc-notched-outline">
                                        <span class="mdc-notched-outline__leading"></span>
                                        <span class="mdc-notched-outline__notch">
                                            <span id="outlined-select-label" class="mdc-floating-label text-black">Select method send Token</span>
                                        </span>
                                        <span class="mdc-notched-outline__trailing"></span>
                                    </span>
                                    <div class="mdc-line-ripple"></div>
                                </div>

                            </div>

                            <div class="mdc-layout-grid__cell stretch-card mdc-layout-grid__cell--span-12 px-2">
                                <button id="next" type="button" class="btn btn-lg btn-block btn-success p-3" onclick="ShowNextPage(); return false;">
                                    Next
                                    <i class="material-icons">keyboard_arrow_right</i>
                                </button>
                            </div>

                        </div>
                    </asp:View>

                    <asp:View runat="server">
                        <div class="mdc-layout-grid mb-5">
                            <div class="text-center text-primary mdc-typography--overline mb-3">
                                <asp:Label runat="server" ID="txtHeader" Text="No data sent" />
                            </div>
                            <div class="mdc-layout-grid__cell stretch-card mdc-layout-grid__cell--span-12 px-2">
                                <div class="mdc-text-field mdc-text-field--outlined mdc-text-field--with-trailing-icon">
                                    <i class="material-icons mdc-text-field__icon text-primary">lock</i>
                                    <asp:TextBox ID="txtCode" CssClass="mdc-text-field__input" required="required" runat="server" autofocus="autofocus" style="text-transform:uppercase" />
                                    <div class="mdc-notched-outline mdc-notched-outline--upgraded">
                                        <div class="mdc-notched-outline__leading"></div>
                                        <div class="mdc-notched-outline__notch"></div>
                                        <div class="mdc-notched-outline__trailing"></div>
                                    </div>
                                </div>
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

                            <div class="mdc-layout-grid__cell stretch-card mdc-layout-grid__cell--span-12 px-2 mt-3">
                                <asp:Button runat="server" data-id="btnSubmit" ID="btnSubmit" CssClass="btn btn-lg btn-block btn-success p-3" Text="Validate" 
                                    OnClick="btnSubmit_Click" />
                            </div>
                            <div class="mdc-layout-grid__cell stretch-card mdc-layout-grid__cell--span-12 px-2 mt-3">
                                <asp:Button runat="server" data-id="btnGoBack" ID="btnGoBack" CssClass="btn btn-lg btn-block btn-info p-3" Text="Go Back" 
                                    OnClick="btnGoBack_Click" Visible="false" />
                            </div>
                        </div>

                    </asp:View>
                </asp:MultiView>
                <dx:ASPxHiddenField runat="server" ClientInstanceName="hfView" ID="hfView">
                </dx:ASPxHiddenField>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
