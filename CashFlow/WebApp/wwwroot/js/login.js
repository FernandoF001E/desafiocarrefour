base_form = "frmLogin";

$("#btnLogin").click(function () {
    var dataForm = $("#" + base_form + "").serialize();
    debugger
    var result = dataForm.split("&");
    var message = "";
    var data = "";

    $("label").css("color", "rgb(87,86,85)");
    $("#txtCity").css({ "border-color": "rgb(204,204,204)" });

    for (var i in result) {
        var data = result[i].split("=");
        $("#" + data[0]).css({ "border-color": "rgb(204,204,204)" });
    }

    for (var i in result) {
        var data = result[i].split("=");

        $("#" + data[0]).css({ "border-color": "rgb(204,204,204)" });

        switch (data[0]) {
            case "txtUser":
                var str = fJS_Remove_Space_String(data[1]);
                if (str == "") {
                    message = JSResource.MsgInsertDataField + " " + "<strong>Email</strong>";
                    fJS_ShowWarning(message, data);
                    return false;
                }
                break;
            case "txtPassword":
                var str = fJS_Remove_Space_String(data[1]);
                if (str == "") {
                    message = JSResource.MsgInsertDataField + " " + "<strong>Password</strong>";
                    fJS_ShowWarning(message, data);
                    return false;
                }
                break;
        }
    }

    $.ajax({
        url: base_url + '/authentication/validate',
        type: 'Post',
        data: dataForm,
        dataType: 'json',
        success: function (response) {
            debugger
            var obj = response;
            if (obj != null && parseInt(obj.code) == 0) {
                window.location = base_url + '/financialrecords/index';
            } else if (obj != null && parseInt(obj.code) == NOTFOUND) {
                $(window).scrollTop(0);
                fJS_ShowError(obj.message);
            } else if (obj != null && parseInt(obj.code) == SESSION_EXPIRED) {
                $(window).scrollTop(0);
                fJS_ShowError(obj.message);
                setTimeout(function () { window.location = base_url_session_timeout; }, timeOutError);
            } else if (obj != null && parseInt(obj.code) == -EXCEPTION) {
                $(window).scrollTop(0);
                fJS_ShowError(obj.message);
            } else if (obj != null && parseInt(obj.code) == RECORDALREADYEXISTS) {
                $(window).scrollTop(0);
                fJS_ShowWarningText(obj.message);
            } else if (obj != null && parseInt(obj.code) == RECORDSELECT) {
                $(window).scrollTop(0);
                fJS_ShowWarningText(obj.message);
            }
            else {
                $(window).scrollTop(0);
                fJS_ShowError(JSResource.ErrorRequest);
            }
            return false;
        },
        error: function (e) {
            $(window).scrollTop(0);
            fJS_ShowError(JSResource.ErrorRequest + " -- " + e.responseText);
            return false;
        }
    });
});

