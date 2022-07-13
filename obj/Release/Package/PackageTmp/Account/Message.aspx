<%@ Page Language="C#" MasterPageFile="~/CPanel.Master" AutoEventWireup="true" CodeBehind="Message.aspx.cs" Inherits="CMS.Account.Message" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <style>
        html,
        body {
            height: 100%;
            width: 100%;
            overflow: hidden;
            padding: 0;
            margin: 0;
            box-sizing: border-box;
        }

        #__user_target img {
            margin-right: 10px;
        }

        #__user_target .fullname {
            font-weight: bold;
        }

        #__chat_sidebar {
            height: 550px !important;
        }

        #__message_container {
            height: 550px;
            padding-bottom: 80px
        }
    </style>


    <div class="mdc-layout-grid bg-white">
        <div class="mdc-layout-grid__inner">
            <div class="mdc-layout-grid__cell stretch-card mdc-layout-grid__cell--span-4">
                <div id="__chat_sidebar" class="email-sidebar bg-white w-100 p-0 border-0">
                    <div class="pl-3 py-3 w-100">
                        <div class="mdc-text-field mdc-text-field--outlined mdc-text-field--with-leading-icon search-text-field d-none d-md-flex">
                            <i class="material-icons mdc-text-field__icon">search</i>
                            <input class="mdc-text-field__input" id="text-field-hero-input" autofocus="autofocus" />
                            <div class="mdc-notched-outline">
                                <div class="mdc-notched-outline__leading"></div>
                                <div class="mdc-notched-outline__notch">
                                    <label for="text-field-hero-input" class="mdc-floating-label">Search user</label>
                                </div>
                                <div class="mdc-notched-outline__trailing"></div>
                            </div>
                        </div>
                    </div>
                    <div class="mail-components pl-3 pt-1">
                        <div class="mdc-list-group">
                            <ul class="mdc-list mdc-list--two-line chat-list">
                                <%
                                    DataTable user = M_User.SelectAllExceptMe();
                                    foreach (DataRow row in user.Rows)
                                    {
                                        Response.Write("<li class=\"mdc-list-item mdc-ripple-upgraded px-1 border-bottom\" data-mdc-auto-init=\"MDCRipple\" data-mdc-auto-init-state=\"initialized\">");
                                        Response.Write("<span class=\"mdc-list-item__graphic\" role=\"presentation\">");
                                        Response.Write("<img src=\"" + row["photo"].ToString() + "\" class=\"img-sm rounded-circle\" data-initial=\"" + M_User.getInitial(row["fullname"].ToString()) + "\" />");
                                        Response.Write("</span>");
                                        Response.Write("<span class=\"mdc-list-item__text\">");
                                        Response.Write("<span class=\"fullname\" data-id=\"" + Crypto.Encode64Byte(row["user_id"].ToString()) + "\" data-username=\"" + Crypto.Encode64Byte(row["username"].ToString()) + "\">" + row["fullname"].ToString() + "</span>");
                                        Response.Write("<span class=\"mdc-list-item__secondary-text\">" + row["role_name"].ToString() + "</span>");
                                        Response.Write("</span>");
                                        Response.Write("</li>");
                                    }
                                %>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <div class="mdc-layout-grid__cell mdc-layout-grid__cell--span-8 mdc-layout-grid__cell mdc-layout-grid__cell--span-12-tablet email-right-wrapper border-left">
                <div id="__region_chat" class="position-relative d-none">
                    <div class="w-100 h-100 position-relative" style="background-color: #fafafa">
                        <div id="__user_target" class="bg-white px-3 pb-3 pt-3 border-bottom">
                            <img src="<%= M_User.getPhoto() %>" class="img-sm rounded-circle" />
                            <span class="fullname"><%= M_User.getFullname() %></span>
                        </div>
                        <div class="card-body p-0">
                            <div id="__message_container" class="pt-3 position-relative">
                            </div>
                            <div class="d-flex justify-content-start position-absolute pl-4 pr-5 py-3" style="left: 0; bottom: 0; right: 0; background-color: #ededed">
                                <textarea id="message" class="w-100 rounded border-0 p-2" cols="1" rows="1" placeholder="Type message"></textarea>
                                <button id="send_message" class="mdc-icon-button-sm mdc-top-app-bar__navigation-icon" onclick="return false;">
                                    <i class="fa fa-paper-plane"></i>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="Script" runat="server">
    <script type="text/javascript">
        const textarea = new PerfectScrollbar('#message');
        const chat_sidebar = new PerfectScrollbar('#__chat_sidebar');
        const __message_container = new PerfectScrollbar('#__message_container');

        $(document).ready(function () {
            $('.chat-list li.mdc-list-item').on('click', function (e) {
                e.preventDefault()
                var photo = $(this).find('img').clone();
                var username = $(this).find('.mdc-list-item__text .fullname').clone();
                $('div#__user_target').html(photo).append(username);
                $('div#__region_chat').removeClass("d-none").fadeIn();
                $('textarea#message').val('').focus();

                $.connection.defaultHub.server.getMessageData($(username).attr("data-id"));
                var menu_count = isNaN(parseInt($("#__menu_message").find("span#count .count").text())) ? 0 : parseInt($("#__menu_message").find("span#count .count").text());
                var this_count = isNaN(parseInt($(this).find("span#count .count").text())) ? 0 : parseInt($(this).find("span#count .count").text());

                $(this).find("span.count-indicator").fadeOut();
                $(this).find("span.count-indicator .count").text(0);
                $("#__menu_message, #__side_message").find("span#count .count").text(isNaN(parseInt(menu_count - this_count)) ? 0 : parseInt(menu_count - this_count));
                if ((menu_count - this_count) <= 0) $("#__menu_message, #__side_message").find("span#count").remove();
                setTotalCount();
            });

            $('textarea#message').on('keydown', function (e) {
                if (e.altKey && e.keyCode == 13) {
                    var val = $(this).val() + "\n";
                    $(this).val(val);
                }

                if ((e.keyCode || e.which) === 13) {
                    $("button#send_message").trigger("click");
                    return false;
                }

            });
        });       
        
        $.connection.defaultHub.client.getMessageUser = function (data) {
            $("div#__message_container").html(null);
            var message = JSON.parse(data);
            if (message !== null) {
                message.forEach(function (item, index) {
                    // Html encode display name and message. 
                    var encodedName = $('<div />').text(item.fullname).html();
                    var encodedMsg = $('<div />').text(item.message_content).html();
                    // Add the message to the page. 
                    //$('#discussion').append('<li><strong>' + encodedName + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>');
                    if (encodedName === $.connection.defaultHub.state.fullname) {
                        $("div#__message_container").append('<div class="d-flex justify-content-end px-4 py-1 position-relative"><span class="bg-lightgreen px-3 py-2 rounded">' + encodedMsg + '<span class="small ml-5 text-muted" style="right:10px;bottom:2px;">' + item.message_date + '</span></span></div>');
                    } else {
                        $("div#__message_container").append('<div class="d-flex justify-content-start px-4 py-1 position-relative"><span class="bg-lightblue px-3 py-2 rounded">' + encodedMsg + '<span class="small ml-5 text-muted" style="right:10px;bottom:2px;">' + item.message_date + '</span></span></div>');
                    }
                });
            }

            scrollMessageEnd();
        };        

        function webNotifyMessage(title, message) {
            if (Notification.permission !== "granted") {
                Notification.requestPermission();
            }
            else {
                if (!document.hasFocus()) {
                    var notification = new Notification(title, {
                        icon: host + '/favicon.png',
                        body: message,
                    });

                    notification.onclick = function (e) {
                        e.preventDefault();
                        parent.focus();
                        window.focus(); //just in case, older browsers
                        window.open(host + '/account/message', '_blank');
                        this.close();
                    };
                }
            }
        }

        

    </script>
</asp:Content>
