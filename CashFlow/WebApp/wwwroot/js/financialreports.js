var nLimits = "10";

$("#btnReport").click(function () {
    $.ajax({
        url: base_url + '/reports/financialrecordsreport',
        type: 'Post',
        data: {
            'searchDate': $("#txtSearchDate").val()
        },
        success: function (response) {
            var obj = response;
            if (obj != null && parseInt(obj.code) == SUCCESS) {
                //Abrir new windows
            }
            else if (obj != null && parseInt(obj.code) == SESSION_EXPIRED) {
                $(window).scrollTop(0);
                fJS_ShowError(obj.message);
                setTimeout(function () { window.location = base_url_session_timeout; }, timeOutError);
            }
            else if (obj != null && parseInt(obj.code) == EXCEPTION) {
                $(window).scrollTop(0);
                fJS_ShowError(obj.message);
            }
            else if (obj != null && parseInt(obj.code) == RECORDALREADYEXISTS) {
                $(window).scrollTop(0);
                fJS_ShowWarningText(obj.message);
            }
            else if (obj != null && parseInt(obj.code) == RECORDSELECT) {
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