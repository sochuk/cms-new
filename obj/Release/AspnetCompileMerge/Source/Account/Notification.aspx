<%@ Page Language="C#" MasterPageFile="~/CPanel.Master" AutoEventWireup="true" CodeBehind="Notification.aspx.cs" Inherits="CMS.Account.Notification" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="mdc-layout-grid bg-white">
        <div class="mdc-layout-grid__inner">
            <div class="mdc-layout-grid__cell stretch-card mdc-layout-grid__cell--span-12">
                <div class="mdc-card">
                    <div class="mdc-layout-grid__inner">
                        <div class="mdc-layout-grid__cell mdc-layout-grid__cell--span-12 mdc-layout-grid__cell mdc-layout-grid__cell--span-12-tablet email-right-wrapper">
                            <div class="d-flex align-items-center flex-wrap">
                                <button id="mark_all" class="mdc-button mdc-button--dense mdc-button--outlined mb-2" onclick="return false;">
                                    <i class="mdc-button__icon mdi mdi-bell-ring-outline"></i>
                                    Mark all as read
                                </button>
                            </div>
                            <ul id="__item_notification" class="mdc-list mdc-list--two-line mdc-list--avatar-list email-list" role="menu">
                                <%
                                    DataTable user = M_User.getNotification(0, 5).ToColumnLowerCase();
                                    if (user.Rows.Count > 0)
                                    {
                                        foreach (DataRow row in user.Rows)
                                        {
                                            string isread = row["isread"].ToString().ToBoolean() ? "mdi-bell-ring-outline" : "mdi-bell-ring";
                                            string unread = row["isread"].ToString().ToBoolean() ? "" : "unread";

                                            Response.Write("<li class=\"mdc-list-item pl-4 " + unread + "\" role=\"menuitem\" tabindex=\"-1\">");
                                            Response.Write("<span class=\"mdc-list-item__graphic\" role=\"presentation\">");
                                            Response.Write("<img src=\"" + row["photo"].ToString() + "\" class=\"img-sm rounded-circle\" data-initial=\"" + M_User.getInitial(row["fullname"].ToString()) + "\" />");
                                            Response.Write("</span>");
                                            Response.Write("<span class=\"mdc-list-item__text\">");
                                            Response.Write("<a data-id=\"" + row["notif_id"].ToString() + "\" onclick=\"javascript:setNotificationRead(this)\" class=\"text-black\" target=\"_blank\" href=\"" + row["url"].ToString() + "\">");
                                            Response.Write("<span>" + row["notif_title"].ToString() + "</span>");
                                            Response.Write("<span class=\"mdc-list-item__secondary-text\">" + row["notif_message"].ToString() + "</span>");
                                            Response.Write("</a>");
                                            Response.Write("</span>");
                                            Response.Write("<span class=\"mdc-list-item__meta text-right\"><a data-id=\"" + row["notif_id"].ToString() + "\" href=\"#\" onclick=\"javascript:setNotificationRead(this);event.preventDefault();\" class=\"material-icons mdi " + isread + "\"></a>");
                                            Response.Write("<span class=\"mdc-list-item__secondary-text d-none d-md-block\">" + row["fullname"].ToString() + " <i class=\"mdi mdi-timer\"></i> <span class=\"pretty\">" + PrettyDate.GetPrettyDate(Convert.ToDateTime(row["createdate"].ToString())) + "</span></span>");
                                            Response.Write("</span>");
                                            Response.Write("</li>");
                                            Response.Write("<li role=\"separator\" class=\"mdc-list-divider\"></li>");
                                        }
                                    }
                                    else
                                    {
                                        Response.Write("<li tabindex=\"0\">");
                                        Response.Write("<div class=\"item-content d-flex align-items-middle justify-content-center border p-4\">");
                                        Response.Write("<span class=\"mdc-list-item__secondary-text\">No data available</span>");
                                        Response.Write("</div>");
                                        Response.Write("</li>");
                                    }
                                %>
                            </ul>

                            <% if (user.Rows.Count > 0){
                                    Response.Write("<div class=\"item-content d-flex align-items-middle flex-column justify-content-center\">");
                                    Response.Write("<button type=\"button\" class=\"mdc-button\" id=\"__load_more\">");
                                    Response.Write("Load more...");
                                    Response.Write("</button>");
                                    Response.Write("</div>");
                               }
                            %>                                    
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="Script" runat="server">
    <script type="text/javascript">
        $.fn.dataList = function (options) {
            this.each(function () {
                var $table = $(this);
                if ($table.is('ul')) {
                    var $ul = $table;
                    $table = $ul.wrap('<table><tbody/></table>').closest('table');
                    $ul.find('li').wrap('<tr><td/></tr>').contents().unwrap();
                    $ul.contents().unwrap()
                    $table.prepend('<thead><tr><th>' + ($ul.data('heading') || '') + '</th></tr></thead>');
                }
                $table.dataTable(options);
            });
        }

        function setNotificationRead(object) {
            var id = isNaN(parseInt($(object).attr("data-id"))) ? 0 : parseInt($(object).attr("data-id"));
            $.connection.defaultHub.server.setNotificationRead(id, false);
            $(object).closest("li").removeClass("unread");
            $(object).closest("li").find("a.material-icons").removeClass("mdi-bell-ring").addClass("mdi-bell-ring-outline");
            var notif = isNaN(parseInt($("#__menu_notification .count-indicator .count").text())) ? 0 : parseInt($("#__menu_notification .count-indicator .count").text());
            $("#__menu_notification .count-indicator .count").text(notif - 1);
            if ((notif - 1) <= 0) $("#__menu_notification .count-indicator").fadeOut();
            setTotalCount();
        }

        function addItem(data) {
            var isread = data.isread === true ? "mdi-bell-ring-outline" : "mdi-bell-ring";
            var unread = data.isread === true ? "" : "unread";
            var item = '<li class="mdc-list-item pl-4 ' + unread + '" role="menuitem" tabindex="-1">';
            item += '<span class="mdc-list-item__graphic" role="presentation"><img src="'+ data.photo +'" class="img-sm rounded-circle" data-initial="'+ data.initial +'"></span>';
            item += '<span class="mdc-list-item__text"><a data-id="'+ data.notif_id +'" onclick="javascript:setNotificationRead(this)" class="text-black" href="'+ data.url +'"><span>'+ data.notif_title +'</span><span class="mdc-list-item__secondary-text">'+ data.notif_message +'</span></a></span>';
            item += '<span class="mdc-list-item__meta text-right"><a data-id="'+ data.notif_id +'" href="#" onclick="javascript:setNotificationRead(this);event.preventDefault();" class="material-icons mdi '+ isread +'"></a><span class="mdc-list-item__secondary-text d-none d-md-block">'+ data.fullname +' <i class="mdi mdi-timer"></i> <span class="pretty">'+ data.prettydate +'</span></span></span>';
            item += '</li>';
            item += '<li role="separator" class="mdc-list-divider"></li>';
            $("ul#__item_notification").append(item);
        }

        $.connection.defaultHub.client.getNotificationData = function (data) {            
            var notif = JSON.parse(data);            
            console.log(notif)
            if (notif.notification.length == 10) { //default load is 10 item
                notif.notification.forEach(function (item, index) {
                    addItem(item)
                })
            } else { //no data needed
                notif.notification.forEach(function (item, index) {
                    addItem(item)
                })
                $("#__load_more").fadeOut().remove();
            }

            $("#__load_more").html('Load more...');
        };

        $(document).ready(function () {
            $("#mark_all").on("click", function () {
                $.connection.defaultHub.server.setNotificationRead(0, true);
                $("#__item_notification li").removeClass("unread");
                $("#__item_notification li").find("a.material-icons").removeClass("mdi-bell-ring").addClass("mdi-bell-ring-outline");

                $("#__menu_notification .count-indicator .count").text(0);
                $("#__menu_notification .count-indicator").fadeOut();
                setTotalCount();
            });

            $("#__load_more").on("click", function () {
                var count = $("#__item_notification li.mdc-list-item").length;
                $(this).html('<div class="spinner-border spinner-border-sm mr-2" role="status"><span class="sr-only">Loading...</span></div> Loading...');
                $.connection.defaultHub.server.getNotificationData(count, 10);
            });
        });
    </script>
</asp:Content>
