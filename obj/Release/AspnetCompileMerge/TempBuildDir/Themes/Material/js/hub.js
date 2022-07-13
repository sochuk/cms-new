$(function () {
    // Declare a proxy to reference the hub. 
    let hub = $.connection.defaultHub; //hub
    var tryingToReconnect = false;
    var qw = "0c9e12731c3cd6df7de64d798a62afc8", er = "21ae63a5d0d4bdcdc1f9a07b3456ba7d", ty = "52d0c2d4b65c5e9650a4a1d088e1c36e"; //requiredLogin, requiredTitle, requiredMessage

    // Create a function that the hub can call to broadcast messages.
    hub.client.getBroadcastMessage = function (username, message) {
        // Html encode display name and message. 
        var encodedName = $('<div />').text(username).html();
        var encodedMsg = $('<div />').text(message).html();
        // Add the message to the page. 
        $('#discussion').append('<li><strong>' + encodedName + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>');
    };

    hub.client.getDuplicateLogin = function (ip, title, message) {
        if (ip !== $.connection.defaultHub.state.local_ipaddress){
            var encodedTitle = $('<div />').text(title).html();
            var encodedMsg = $('<div />').text(message).html();
            setQ(1 == 1, encodedTitle, encodedMsg);
            duplicateLogin(title, message);  
        }              
    };

    hub.client.logoutAnother = function (id) {
        if (id !== $.connection.defaultHub.state.local_ipaddress) {
            duplicateSignout();
        } else {
            clearQ();
            $(".notify").remove();
            $("body").removeClass("loader").addClass("loaded");
        }
    };

    hub.client.getMessage = function (data) {
        // Html encode display name and message. 
        var encodedName = $('<div />').text(data.message.user.fullname).html();
        var encodedMsg = $('<div />').text(data.message.message_content).html();
        // Add the message to the page. 
        //$('#discussion').append('<li><strong>' + encodedName + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>');

        if (encodedName === $.connection.defaultHub.state.fullname) {
            $("div#__message_container").append('<div class="d-flex justify-content-end px-4 py-1 position-relative"><span class="bg-lightgreen px-3 py-2 rounded">' + encodedMsg + '<span class="small ml-5 text-muted" style="right:10px;bottom:2px;">' + data.message.message_date + '</span></span></div>');
        } else {
            $("div#__message_container").append('<div class="d-flex justify-content-start px-4 py-1 position-relative"><span class="bg-lightblue px-3 py-2 rounded">' + encodedMsg + '<span class="small ml-5 text-muted" style="right:10px;bottom:2px;">' + data.message.message_date + '</span></span></div>');
            setMessageCount(data);
        }

        scrollMessageEnd();

        notifyMessage(data);
    };

    hub.client.getNotification = function (data) {
        setNotificationCount(data);
        notifyNotification(data);
    };

    // Start the connection.
    //$.connection.hub.logging = true;
    $.connection.hub.start(function () {
        $.get("https://www.cloudflare.com/cdn-cgi/trace", function (data) {
            var d = data.split("\n");
            var o = new Object();
            d.forEach(function (itm, idx) {
                var item = itm.split("=");
                o[item[0]] = item[1];
            });

            hub.server.join(o.ip, o.uag)
                .done(function (data) {
                    $('button#send_message').click(function () {
                        // Call the Send method on the hub. 
                        if ($('textarea#message').val().trim().length > 0) {
                            hub.server.sendTo(
                                $('div#__user_target span').attr("data-id"),
                                $('div#__user_target span').attr("data-username"),
                                $('textarea#message').val()
                            );
                        }
                        // Clear text box and reset focus for next comment. 
                        $('textarea#message').val('').focus();
                    });

                    setMessageCount(data);
                    setNotificationCount(data);
                    setTotalCount(data);
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

    $(document).ready(function () {
        if (Cookies.get(qw) !== undefined || Cookies.get(qw) === true) {
            var title = $("<div/>").html(Cookies.get(er)).text();
            var msg = $("<div/>").html(Cookies.get(ty)).text();
            duplicateLogin(title, msg);
        }        

        $("span.count").on('change', function () {
            var value = isNaN(parseInt($(this).text())) ? 0 : parseInt($(this).text());
            console.log("change event")
            if (value <= 0) {
                $(this).closest(".count-indicator").fadeOut();
            } else {
                $(this).closest(".count-indicator").fadeIn();
            }
        });
    });
});

function setMessageCount(data) {
    //Main menu
    var count = 0;
    var current_in_menu = $('#__menu_message').find('span.count-indicator .count');
    var current_in_side = $('#__side_message').find('span.count-indicator .count');
    var current_in_menu_selector = $('#__menu_message, #__side_message');
    if (current_in_menu.length === 0 && data.message.message_unread_count !== 0) {
        $(current_in_menu_selector).append('<span id="count" class="count-indicator"><span class="count">' + data.message.message_unread_count + '</span></span>');
    } else {
        count = parseInt($(current_in_menu).text()) + 1;
        $(current_in_menu).text(count);
        $(current_in_side).text(count);
    }

    if (data.message.message_unread_userdata !== null) {
        data.message.message_unread_userdata.forEach(function (item, i) {
            var current_in_message = $('ul.chat-list li span[data-username="' + item.username + '"]').closest('li').find('span.count-indicator .count');
            var current_in_message_selector = $('ul.chat-list li span[data-username="' + item.username + '"]').closest('li');
            if (current_in_message.length === 0) {
                $(current_in_message_selector).append('<span id="count" class="count-indicator"><span class="count">' + item.unread + '</span></span>');
                $(current_in_message_selector).trigger("change");
            } else {
                var count_child = parseInt($(current_in_message).text()) + 1;
                $(current_in_message).text(count_child).trigger("change");
            }
        });
    } else {
        var current_in_message = $('ul.chat-list li span[data-username="' + data.message.user.username + '"]').closest('li').find('span.count-indicator .count');
        var current_in_message_selector = $('ul.chat-list li span[data-username="' + data.message.user.username + '"]').closest('li');
        if (current_in_message.length === 0) {
            $(current_in_message_selector).append('<span id="count" class="count-indicator"><span class="count">1</span></span>');
        } else {
            var count_child = parseInt($(current_in_message).text()) + 1;
            $(current_in_message).text(count_child);
        }
    }

    setTotalCount();
}

function notifyMessage(data) {    
    if (window.location.href.toLowerCase() !== host + "/account/message") {
        $.notify({
            heading: '<i class="fa fa-comment mr-2"></i> ' + data.message.user.fullname,
            text: '<a href="' + host + "/account/message" + '">You have ' + $('a#__menu_message #count .count').text() +' unread message</a>',
            loader: false,
            hideAfter: 300000,
            stack: 5,
            position: 'bottom-right'
        });
        if (!document.hasFocus()) {
            parent.focus();
            window.focus();
        }
    }
}

function setNotificationCount(data) {
    //Main menu
    var count = 0;
    var current_in_menu = $('#__menu_notification').find('span.count-indicator .count');
    var current_in_side = $('#__side_notification').find('span.count-indicator .count');
    var current_in_menu_selector = $('#__menu_notification, #__side_notification');
    if (current_in_menu.length === 0 && data.notification.unread !== 0) {
        $(current_in_menu_selector).append('<span id="count" class="count-indicator"><span class="count">' + data.notification.unread + '</span></span>');
    } else {
        count = parseInt($(current_in_menu).text()) + 1;
        $(current_in_menu).text(count);
        $(current_in_side).text(count);
    }
    setTotalCount();
}

function notifyNotification(data) {
    if (window.location.href.toLowerCase() !== host + "/account/notification") {
        $.notify({
            heading: '<i class="mdi mdi-bell mr-2"></i> ' + data.user.fullname,
            text: '<a href="' + host + "/account/notification" + '">You have ' + $('a#__menu_notification #count .count').text() + ' unread notification</a>',
            loader: false,
            hideAfter: 300000,
            stack: 5,
            position: 'bottom-right'
        });
        if (!document.hasFocus()) {
            parent.focus();
            window.focus();
        }
    }
}

function setTotalCount() {
    var current_in_total = $('#__side_total').find('span.count-indicator .count');
    var current_in_total_selector = $('#__side_total');
    var notif = isNaN(parseInt($("#__menu_notification .count-indicator").text())) ? 0 : parseInt($("#__menu_notification .count-indicator").text());
    var msg = isNaN(parseInt($("#__menu_message .count-indicator").text())) ? 0 : parseInt($("#__menu_message .count-indicator").text());
    if (current_in_total.length === 0) {
        $(current_in_total_selector).append('<span id="count" class="count-indicator"><span class="count">' + (notif + msg) +'</span></span>');
        $(current_in_total_selector).trigger("change");
    } else {
        count = parseInt(notif + msg);
        $(current_in_total).text(count);
    }
}

function duplicateLogin(title, message) {
    $("body").removeClass("loaded").addClass("loader");
    $("body").find(".loader").fadeOut().remove();
    $.notify({
        heading: title,
        text: message,
        allowToastClose: false,
        hideAfter: false,
        position: 'top-center',
    });

    $("button#useHere").on("click", function (e) {
        e.preventDefault();
        hub.server.logoutAnother();
    });

    $("button#logout").on("click", function (e) {
        e.preventDefault();
        duplicateSignout();
    })
}

function duplicateSignout() {
    clearQ();
    window.location.replace(host + "/account/logout");
}

function setQ(a, b, c) {
    Cookies.set(qw, a);
    Cookies.set(er, b);
    Cookies.set(ty, c);
}

function clearQ() {
    Cookies.remove(qw);
    Cookies.remove(er);
    Cookies.remove(ty);
}

function scrollMessageEnd() {
    var target = $('#__message_container');
    if ($(target).length > 0) {
        $(target).stop().animate({
            scrollTop: $(target)[0].scrollHeight
        }, 0);
    }
}