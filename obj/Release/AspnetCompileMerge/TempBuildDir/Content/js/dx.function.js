var focusedGrid = null;
var allow_edit = true;
var gridPerformingCallback = false;

function gridStartEditing(value) {
    console.log(value);
    allow_edit = value;
    if (value == true) {
        gridView.StartEditRow(key);
        gridView = true;
    }
}

function AddKeyboardNavigationTo(gridView) {
    gridView.BeginCallback.AddHandler(function (s, e) { gridPerformingCallback = true; });
    gridView.EndCallback.AddHandler(function (s, e) { gridPerformingCallback = false; });
    gridView.CallbackError.AddHandler(function (s, e) { console.log(e); });
    ASPxClientUtils.AttachEventToElement(document, "keydown",
        function (evt) {
            return OnDocumentKeyDown(gridView, evt);
        });
}

function OnDocumentKeyDown(gridView, evt) {
    gridView = focusedGrid;
    if (gridView !== null) {
        if (typeof (event) != "undefined" && event != null)
            evt = event;

        //F2                            
        if (event.keyCode == 113) {
            var key = gridView.GetFocusedRowIndex();
            //gridView.GetRowValues(key, "allow_edits", gridStartEditing);
            //console.log(status)
            gridView.StartEditRow(key);
            gridView = true;
        }

        //Alt+N
        if ((evt.altKey && evt.keyCode == 78) || (evt.altKey && evt.keyCode == 46))
            gridView.AddNewRow();
        //Alt+S
        if ((evt.altKey && evt.keyCode == 83) || (evt.altKey && evt.keyCode == 115))
            gridView.UpdateEdit();
        //Esc
        if ((evt.altKey && evt.keyCode == 81) || (evt.altKey && evt.keyCode == 27) || (evt.keyCode == 27))
            gridView.CancelEdit();

        if (evt.ctrlKey && NeedProcessDocumentKeyDown(evt) && !gridPerformingCallback) {
            var currentIndex = gridView.GetFocusedRowIndex();

            if (evt.keyCode == 40) //down
            {
                if (currentIndex == gridView.GetVisibleRowsOnPage() - 1) {
                    return ASPxClientUtils.PreventEvent(evt);
                }
                else {
                    gridView.SetFocusedRowIndex(currentIndex + 1);
                }
            }
            if (evt.keyCode == 38) {
                if (currentIndex == 0) {
                    return ASPxClientUtils.PreventEvent(evt);
                }
                else {
                    gridView.SetFocusedRowIndex(currentIndex - 1);
                }
            }

        }
    }
}

function NeedProcessDocumentKeyDown(evt) {
    var evtSrc = ASPxClientUtils.GetEventSource(evt);
    if (evtSrc.tagName == "INPUT")
        return evtSrc.type != "text" && evtSrc.type != "password";
    else
        return evtSrc.tagName != "TEXTAREA";
}

function addScrollBar() {
    const ps = document.querySelector('div.dxgvCSD');
    if (ps) {
        $(ps).css({ "position": "relative", "padding-bottom": "15px" });
        new PerfectScrollbar(ps);
    }
}

let mn = document.querySelector('div.mdc-menu');
if(mn !== null) new PerfectScrollbar(mn);

function hideExportOptions(s, e) {
    e.HideExportOptionsPanel();
}
function hideSearchOptions(s, e) {
    DevExpress.Reporting.Viewer.Settings.SearchAvailable(false);
}
function OnCallbackComplete(s, e) {
    if (e.result !== null) $('body').append(e.result);
}

function EndCallback(s, e) {
    if (s.cpError) {
        if ($('div.alert.notify-message.alert-danger.border-danger.shadow-lg').length == 0) {
            $('body').append(s.cpError.toString());
        }
        s.cpError = null;
    }
    if (s.cpSuccess) {
        if ($('div.alert.notify-message.alert-success.border-danger.shadow-lg').length == 0) {
            $('body').append(s.cpSuccess.toString());
        }
        s.cpSuccess = null;
    }
    if (s.cpWarning) {
        if ($('div.alert.notify-message.alert-warning.border-danger.shadow-lg').length == 0) {
            $('body').append(s.cpWarning.toString());
        }
        s.cpWarning = null;
    }
    if (s.cpOpenTab) {
        var win = window.open(s.cpOpenTab.toString(), '_blank');
        if (win) win.focus();
        s.cpOpenTab = null;
    }
    if (s.cpOpenUrl) {
        window.location.href = s.cpOpenUrl.toString();
        s.cpOpenUrl = null;
    }
    if (s.cpShowHystory) {
        s.cpShowHystory = false;
        DialogHystory.Show();
    }

    addScrollBar();
}

function ToolbarItemClick(s, e) {
    switch (e.item.name.toUpperCase()) {
        case "HYSTORY":
            e.processOnServer = true;
            break;
        case "PRINT":
            e.processOnServer = true;
            break;
        case "PRINTPREVIEW":
            e.processOnServer = true;
            break;
    }
}
