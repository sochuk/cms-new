$(document).ready(function () {
    var captcha = $("#captcha");
    var info = new Object();
    captcha.find("a").not(".BDC_ReloadLink").remove();
    captcha.removeClass("d-none");
    $(".switch-password").on('click', function (event) {
        alert("");
        event.preventDefault();
        var _this = $(this);
        var _input = _this.closest("div.input-group").find("input")
        if (_input.attr("type") == "text") {
            _input.attr('type', 'password');
            _this.find("i").addClass("fa-eye-slash");
            _this.find("i").removeClass("fa-eye text-danger");
        } else if (_input.attr("type") == "password") {
            _input.attr('type', 'text');
            _this.find("i").removeClass("fa-eye-slash");
            _this.find("i").addClass("fa-eye text-danger");
        }
    });

    $.get("https://www.cloudflare.com/cdn-cgi/trace", function (data) {
        var d = data.split("\n");
        var o = new Object();
        d.forEach(function (itm, idx) {
            var item = itm.split("=");
            o[item[0]] = item[1];
        });
        info = o;
        $("[name=ip]").val(o.ip);
        $("[name=ua]").val(o.uag);
        $("[name=loc]").val(o.loc);
    });

    $("button#login").on("click", function () {
        $("input[name=date]").val(new Date().toISOString());
        $("[name=ip]").val(info.ip);
        $("[name=ua]").val(info.uag);
        $("[name=loc]").val(info.loc);
    });
    $('form').on('submit', function () {        
        $("button#login").text("Signin...").attr("disabled", true);        
        $('#progressbar').show().removeClass("d-none");
        $('input[data-id=btnSubmit]').val("Validating...").attr("readonly", true);
    });

    $(".mdc-checkbox__native-control input").unwrap().addClass("mdc-checkbox__native-control");
    $(".mdc-radio__native-control input").unwrap().addClass("mdc-radio__native-control");

    $("#timeout").countdown(Date.parse($("#timeout").text()), function (event) {
        $(this).html(event.strftime('%H:%M:%S'));
    }).on("update.countdown", function () {
        $("button#login").attr("disabled", true).addClass("d-none");
        $("[data-id=username]").attr("disabled", true);
        $("[data-id=password]").attr("disabled", true);
        $("[role=alert]").find("button").remove();
    }).on("finish.countdown", function () {
        location.replace(window.location.href);
    });
});

var initialFocusedElement = null, $inputs = $('input[type="text"],input[type="password"]');
var removeAutofillStyle = function () {
    if ($(this).is(':-webkit-autofill')) {
        var val = this.value;

        // Remove change event, we won't need it until next "input" event.
        $(this).off('change');

        // Dispatch a text event on the input field to trick the browser
        this.focus();
        event = document.createEvent('TextEvent');
        event.initTextEvent('textInput', true, true, window, '*');
        this.dispatchEvent(event);

        // Now the value has an asterisk appended, so restore it to the original
        this.value = val;

        // Always turn focus back to the element that received 
        // input that caused autofill
        initialFocusedElement.focus();
    }
};

var onChange = function () {
    // Testing if element has been autofilled doesn't 
    // work directly on change event.
    var self = this;
    setTimeout(function () {
        removeAutofillStyle.call(self);
    }, 1);
};

$inputs.on('input', function () {
    if (this === document.activeElement) {
        initialFocusedElement = this;

        // Autofilling will cause "change" event to be 
        // fired, so look for it
        $inputs.on('change', onChange);
    }
});