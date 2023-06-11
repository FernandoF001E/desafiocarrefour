var nLimits = "10";

$("#txtSearchDate").datepicker({
    autoclose: true
});

$("#btnReport").click(function () {
    $.ajax({
        url: base_url + '/reports/financialrecordsreport',
        type: 'Post',
        data: {
            'searchDate': $("#txtSearchDate").val()
        },
        success: function (response) {
            var obj = response;
            debugger
            if (obj != null && parseInt(obj.code) == SUCCESS) {
                let pdfWindows = window.open("");
                pdfWindows.document.write("<iframe width=100%' height='100%' src='data:application/pdf;base64," + encodeURI(obj.xPath) + "'></iframe>")
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